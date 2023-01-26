//using OSS.Common;
//using OSS.Common.Resp;
//using OSS.Core.Client.Http;

//namespace TM.WMS.Client;

//internal class MCodeHttpClient : IMCodeOpenService
//{
//    private const string apiEntityPath = "/MCode";

 

//    public Task<TokenListResp<MCodeMo> MList(long m_id)
//    {
//        var apiPath = string.Concat(apiEntityPath, "/MList?m_id=", m_id);

//        return new WMSRemoteReq(apiPath)
//            .GetAsync<TokenListResp<MCodeMo>>();
//    }

//    /// <inheritdoc />
//    public Task<Resp<MCodeMo>> Get(long id)
//    {
//        var apiPath = string.Concat(apiEntityPath, "/Get?id=", id);

//        return new WMSRemoteReq(apiPath).GetAsync<Resp<MCodeMo>>();
//    }
    
//    /// <inheritdoc />
//    public Task<Resp> SetUseable(string pass_token, ushort flag)
//    {
//        var apiPath = string.Concat(apiEntityPath, "/SetUseable?pass_token=", pass_token, "&flag=", flag);

//        return new WMSRemoteReq(apiPath).PostAsync<Resp>();
//    }

//    /// <inheritdoc />
//    public Task<LongResp> Add(AddMSkuReq req)
//    {
//        var apiPath = string.Concat(apiEntityPath, "/Add");

//        return new WMSRemoteReq(apiPath).PostAsync<LongResp>(req);
//    }
//}

