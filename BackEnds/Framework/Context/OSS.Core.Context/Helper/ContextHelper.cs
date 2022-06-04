using System.Threading;

namespace OSS.Core.Context
{

    internal class CoreIdentities
    {
        public AppIdentity AppIdentity { get; set; }
        public TenantIdentity TenantIdentity { get; set; }
        public UserIdentity MemberIdentity { get; set; }
    }


    internal static class ContextHelper
    {



        private static readonly AsyncLocal<CoreIdentities> _items = new AsyncLocal<CoreIdentities>();

        public static void SetAppIdentity(AppIdentity identity)
        {
            if (_items.Value == null)
            {
                _items.Value = new CoreIdentities();
            }

            _items.Value.AppIdentity = identity;
        }
        public static void SetTenantIdentity(TenantIdentity identity)
        {
            if (_items.Value == null)
            {
                _items.Value = new CoreIdentities();
            }

            _items.Value.TenantIdentity = identity;
        }
        public static void SetMemberIdentity(UserIdentity identity)
        {
            if (_items.Value == null)
            {
                _items.Value = new CoreIdentities();
            }

            _items.Value.MemberIdentity = identity;
        }


        public static CoreIdentities GetContext()
        {
            return _items.Value;
        }

    }

}
