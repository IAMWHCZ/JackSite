namespace JackSite.Identity.Server.Entities.Clients
{
    public class ClientCertificationCancel
    {
        public bool IsFrontChannelCancel { get; set; }
        public string? FrontChannelCancel { get; set; }
        public bool IsBackChannelCancel { get; set; }
        public string? BackChannelCancel { get; set; }
        public bool IsLocalLogin { get; set; }
        public string? CanceledRedirectUri { get; set; }
        public string? ProvideLimit { get; set; }
        public int UserSSOLifeCycle{ get; set; }

        public void Update(
            bool isFrontChannelCancel,
            string? frontChannelCancel,
            bool isBackChannelCancel,
            string? backChannelCancel,
            bool isLocalLogin,
            string? canceledRedirectUri,
            string? provideLimit,
            int userSSOLifeCycle)
        {
            IsFrontChannelCancel = isFrontChannelCancel;
            FrontChannelCancel = frontChannelCancel;
            IsBackChannelCancel = isBackChannelCancel;
            BackChannelCancel = backChannelCancel;
            IsLocalLogin = isLocalLogin;
            CanceledRedirectUri = canceledRedirectUri;
            ProvideLimit = provideLimit;
            UserSSOLifeCycle = userSSOLifeCycle;
        }
    }
}
