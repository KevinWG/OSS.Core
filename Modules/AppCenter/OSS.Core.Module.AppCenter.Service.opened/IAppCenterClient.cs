namespace OSS.Core.Module.AppCenter
{
    /// <summary>
    ///  应用中心的客户端接口
    /// </summary>
    public interface IAppCenterClient
    {
        public IOpenedAccessService AccessService { get; set; }
    }
}
