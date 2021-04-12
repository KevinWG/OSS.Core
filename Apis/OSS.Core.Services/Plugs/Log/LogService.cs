using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Helpers;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Services.Plugs.Config.IProxies;
using OSS.Core.Services.Plugs.Log.IProxies;
using OSS.Core.Services.Plugs.Log.Mos;
using OSS.Core.Services.Plugs.Notify.IProxies;
using OSS.Core.Services.Plugs.Notify.Mos;
using OSS.Tools.Log;

namespace OSS.Core.Services.Plugs.Log
{
    public class LogService: ILogServiceProxy
    {
        private static readonly IToolLog _defaultFileLog = new DefaultToolLog();

        public async Task WriteLog(LogInfo log)
        {
            try
            {  
                if (log.source_name != CoreModuleNames.Log)
                {
                    await _defaultFileLog.WriteLogAsync(log);

                    var receiverRes =await GetLogReceivers(log.source_name, log.msg_key);
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
                            {"log_id", log.log_id},
                            {"msg_body", log.msg_body.ToString()}
                        },
                        msg_title = $"系统日志({log.source_name} 模块)",
                        t_code = "Email_Log_NotifyDetail"
                    };
                    await InsContainer<INotifyServiceProxy>.Instance.Send(notifyMsg,true);
                }
            }
            catch 
            {
                // 防止日志模块本身出错，再写日志，死循环
            }
        }



        private const string _receiverConfigKey = "plugs_log_receivers";

        public async Task<ListResp<string>> GetLogReceivers(string moduleName, string msgKey)
        {
            var configRes = await InsContainer<IDirConfigServiceProxy>.Instance.GetConfig<LogReceiverConfig>(_receiverConfigKey,true);
            if (!configRes.IsSuccess())
                return new ListResp<string>().WithResp(configRes);

            var config = configRes.data;
            string receivers = null;
            if (config.items!=null)
            {
                foreach (var item in config.items)
                {
                    if (item.module_name != moduleName)
                        continue;

                    if (string.IsNullOrEmpty(item.msg_key))
                    {
                        receivers = item.receivers;
                    }
                    else if (item.msg_key==msgKey)
                    {
                        receivers = item.receivers;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(receivers))
                receivers = config.default_receivers;
            
            if (string.IsNullOrEmpty(receivers))
                return new ListResp<string>().WithResp(RespTypes.ObjectNull,"未发现配置的可接收人！");
            
            return new ListResp<string>(receivers.Split(',').ToList());
        }

    }
}
