using System.ComponentModel.DataAnnotations;
using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
///   添加批次号请求
/// </summary>
public class AddBatchCodeReq
{
    /// <summary>
    /// 编码
    /// </summary>
    [StringLength(60,ErrorMessage = "编号长度不能超过60个字符!")]
    public string code { get; set; } = string.Empty;

    /// <summary>
    ///  物料Id
    /// </summary>
    [Range(1,long.MaxValue)]
    public long material_id { get; set; }

    /// <summary>
    ///  过期时间
    /// </summary>
    public long expire_date { get; set; }

    /// <summary>
    ///  备注信息
    /// </summary>
    public string remark { get; set; } = string.Empty;
}

/// <summary>
///  批次号转化映射
/// </summary>
public static class AddBatchCodeReqMap
{
    /// <summary>
    ///  转化为批次号对象实体
    /// </summary>
    /// <param name="req"></param>
    /// <param name="mv"></param>
    /// <returns></returns>
    public static BatchMo ToMo(this AddBatchCodeReq req,MaterialView mv)
    {
        var mo = new BatchMo
        {
            code   = req.code,
            remark = req.remark,

            expire_date = req.expire_date,

            material_id   = req.material_id,
            material_code = mv.code,
        };

        mo.FormatBaseByContext();
        return mo;
    }
}
