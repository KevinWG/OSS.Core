
namespace OSS.Core.Context
{
    internal class CoreIdentities
    {
        public AppIdentity? AppIdentity { get; set; }
        public TenantIdentity? TenantIdentity { get; set; }
        public UserIdentity? MemberIdentity { get; set; }
    }


    internal static class ContextHelper
    {
        
        private static readonly AsyncLocal<CoreIdentities> _items = new AsyncLocal<CoreIdentities>();

        internal static void SetAppIdentity(AppIdentity identity)
        {
            var identityOwner = GetContext();

            identityOwner.AppIdentity = identity;
        }


        internal static void SetTenantIdentity(TenantIdentity identity)
        {
            var identityOwner = GetContext();

            identityOwner.TenantIdentity = identity;
        }

        internal static void SetMemberIdentity(UserIdentity identity)
        {
            var identityOwner = GetContext();

            identityOwner.MemberIdentity = identity;
        }


        internal static CoreIdentities GetContext()
        {
            return _items.Value ?? (_items.Value = new CoreIdentities());
        }


    }

}
