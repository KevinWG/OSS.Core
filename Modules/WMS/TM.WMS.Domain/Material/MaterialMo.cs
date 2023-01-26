
#region Copyright (C)  Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 实体对象
*
*　　	创建人： osscore
*    	创建日期：
*       
*****************************************************************************/

#endregion

using OSS.Core.Domain;
using System.Text.Json;

namespace TM.WMS;

/// <summary>
///  物料 对象实体 
/// </summary>
public class MaterialMo : BaseTenantOwnerAndStateMo<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///   物料编码
    /// </summary>
    public string code { get; set; } = string.Empty;

    /// <summary>
    ///  物料目录Id
    /// </summary>
    public long c_id { get; set; }

    /// <summary>
    ///  物料形态
    /// </summary>
    public MaterialType type { get; set; }

    /// <summary>
    ///  单位(基础)
    /// </summary>
    public string basic_unit { get; set; } = string.Empty;

    /// <summary>
    ///  多单位扩展信息
    /// </summary>
    public string multi_units { get; set; } = string.Empty;

    /// <summary>
    ///  技术规格
    /// </summary>
    public string? tec_spec { get; set; }

    /// <summary>
    ///  原厂型号
    /// </summary>
    public string? factory_serial { get; set; }


    /// <summary>
    ///  备注信息
    /// </summary>
    public string? remark { get; set; }
}

public static class MaterialMap
{
    public static MaterialView ToView(this MaterialMo mo)
    {
        var view = new MaterialView
        {
            id = mo.id,
            add_time = mo.add_time,
            owner_uid = mo.owner_uid,
            status = mo.status,
            c_id = mo.c_id,
             
            name = mo.name,
            code = mo.code,
            type = mo.type,

            basic_unit = mo.basic_unit,
            factory_serial = mo.factory_serial,
            tec_spec = mo.tec_spec,
            remark = mo.remark
        };

        if (!string.IsNullOrEmpty(mo.multi_units))
        {
            view.multi_units = JsonSerializer.Deserialize<List<MultiUnitItem>>(mo.multi_units);
        }
        return view;
    }

    /// <summary>
    ///  转化为物料对象实体
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static MaterialMo ToMo(this AddMaterialReq req)
    {
        var mo = new MaterialMo
        {
            code       = req.code,
            basic_unit = req.basic_unit
        };

        mo.FormatByUpdateReq(req);

        mo.FormatBaseByContext();

        return mo;
    }


    public static void FormatByUpdateReq(this MaterialMo mo, UpdateMaterialReq req)
    {
        mo.name           = req.name;
        mo.c_id           = req.c_id;
        mo.type           = req.type;
        mo.factory_serial = req.factory_serial;
        mo.tec_spec       = req.tec_spec;
        mo.remark         = req.remark;
        mo.multi_units    = req.multi_units == null ? string.Empty : JsonSerializer.Serialize(req.multi_units);
    }
}

