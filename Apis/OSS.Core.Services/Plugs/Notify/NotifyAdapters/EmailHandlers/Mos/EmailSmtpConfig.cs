namespace OSS.Core.Services.Plugs.Notify.NotifyAdapters.EmailHandlers.Mos
{
    public class EmailSmtpConfig
    {
        public string host { get; set; }

        public int port { get; set; }
        public int enable_ssl { get; set; }
        public string email { get; set; }
        public string email_name { get; set; }
        public string password { get; set; }
    }
}
