using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Helpers;
using OSS.Tools.Cache;
using OSS.Tools.DirConfig;
using OSS.Tools.Log;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Services.Plugs.Log.Mos;
using OSS.Core.Services.Plugs.Notify.IProxies;
using OSS.Core.Services.Plugs.Notify.Mos;

namespace OSS.Core.Services.Sys_Global.Log
{
    public class EmailLogProvider : IToolLog
    {
        private static readonly IToolLog _defaultFileLog = new DefaultToolLog();

        public async Task WriteLogAsync(LogInfo log)
        {
            try
            {
                await _defaultFileLog.WriteLogAsync(log);

                await SendEmailLog(log);

            }
            catch
            {
                // 防止日志模块本身出错，再写日志，死循环        
            }
        }

        private static async Task SendEmailLog(LogInfo log)
        {
            var canSend = await CheckSendRate(log);
            if (!canSend)
            {
                //   当前靠此做熔断机制
                return;
            }

            var receiverRes = await GetLogReceivers(log.source_name, log.msg_key);
            if (!receiverRes.IsSuccess())
                return;

            var notifyMsg = new NotifyReq
            {
                targets = receiverRes.data,
                body_paras = new Dictionary<string, string>
                        {
                            {"module_name", log.source_name},
                            {"level", log.level.ToString()},
                            {"msg_key", log.msg_key},
                            {"log_id", log.trace_no},
                            {"msg_body", log.msg_body.ToString()}
                        },
                msg_title = $"系统日志({log.source_name} 模块)",
                t_code = CoreDirConfigKeys.plugs_notify_email_log_tcode
            };
            await InsContainer<INotifyServiceProxy>.Instance.Send(notifyMsg);
        }

        /// <summary>
        ///  检测五分钟内同一模块和key值不重复发送
        ///  熔断机制
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static async Task<bool> CheckSendRate(LogInfo info)
        {
            string cacheKey = string.Concat(CoreCacheKeys.System_Log_BySourceAndKey, info.source_name, info.msg_key);
            var cacheRes = await CacheHelper.GetAsync<int>(cacheKey);
            if (cacheRes > 0)
                return false;

            await CacheHelper.SetAbsoluteAsync(cacheKey, 1, TimeSpan.FromMinutes(5));
            return true;
        }

        private const string _receiverConfigKey = "plugs_log_receivers";

        private static async Task<ListResp<string>> GetLogReceivers(string moduleName, string msgKey)
        {
            var config = await DirConfigHelper.GetDirConfig<LogReceiverConfig>(_receiverConfigKey);
            if (config == null)
            {
                return new ListResp<string>().WithResp(RespTypes.ObjectNull, "未发现日志相关接收人配置信息!");
            }

            string receivers = null;
            if (config.items != null)
            {
                foreach (var item in config.items)
                {
                    if (item.module_name != moduleName)
                        continue;

                    if (string.IsNullOrEmpty(item.msg_key))
                    {
                        receivers = item.receivers;
                    }
                    else if (item.msg_key == msgKey)
                    {
                        receivers = item.receivers;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(receivers))
                receivers = config.default_receivers;

            if (string.IsNullOrEmpty(receivers))
                return new ListResp<string>().WithResp(RespTypes.ObjectNull, "未发现配置的可接收人！");

            return new ListResp<string>(receivers.Split(',').ToList());
        }

    }
}
