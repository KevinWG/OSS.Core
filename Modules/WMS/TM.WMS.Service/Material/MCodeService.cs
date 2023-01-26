//using OSS.Common;
//using OSS.Common.Resp;
//using OSS.Core.Domain;
//using OSS.Common.Extension;

//namespace TM.WMS;

///// <summary>
/////  物料库存单位 服务
///// </summary>
//public class MCodeService : IMSkuService
//{
//    private static readonly MCodeRep _mskuRep = new();


//    /// <inheritdoc />
//    public async Task<TokenListResp<MSkuMo>> MList(long m_id)
//    {
//        var list = await _mskuRep.GetByMId(m_id);
//        return list.ToTokenListRespWithIdToken();
//    }

//    /// <inheritdoc />
//    public async Task<Resp<MSkuMo>> Get(long id)
//    {
//        var getRes = await _mskuRep.GetById(id);
//        return getRes.IsSuccess() ? getRes : new Resp<MSkuMo>().WithResp(getRes, "未能找到物料库存单位信息");
//    }


//    /// <inheritdoc />
//    public Task<Resp> SetUseable(string pass_token, ushort flag)
//    {
//        var id = PassTokenHelper.GetData(pass_token).ToInt64();

//        return _mskuRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
//    }

//    /// <inheritdoc />
//    public async Task<LongResp> Add( AddMSkuReq req)
//    {
//        var mo = req.ToMo();

//        await _mskuRep.Add(mo);

//        return new LongResp(mo.id);
//    }



//    /// <inheritdoc />
//    public async Task<Resp> AddList(List<AddMSkuReq> reqList)
//    {
//        var mskuList = reqList.Select(req => req.ToMo()).ToList();

//        await _mskuRep.AddList(mskuList);

//        return new Resp();
//    }


//    /// <inheritdoc />
//    public async Task<Resp> Edit(string pass_token, AddMSkuReq req)
//    {
//        var id = PassTokenHelper.GetData(pass_token).ToInt64();
//        var itemRes = await Get(id);
//        if (!itemRes.IsSuccess())
//            return itemRes;

//        var item = itemRes.data;
//        if (!string.IsNullOrEmpty(req.bar_code) && string.IsNullOrEmpty(item.bar_code))
//        {
//            item.bar_code = req.bar_code;
//        }

//        return await _mskuRep.Edit(item);
//    }
//}
//internal interface IMSkuService : IMSkuOpenService
//{

//}