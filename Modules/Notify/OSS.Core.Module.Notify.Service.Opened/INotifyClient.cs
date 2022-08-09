namespace OSS.Core.Module.Notify
{
    public interface INotifyClient
    {
        /// <summary>
        ///  通知服务
        /// </summary>
        public IOpenedNotifyService NotifyService { get;  }
    }
}
