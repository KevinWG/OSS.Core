using System.Threading;
using OSS.Core.Context.Mos;

namespace OSS.Core.Context.Helper
{
    internal class CoreContext
    {
        public AppIdentity AppIdentity { get; set; }
        public TenantIdentity TenantIdentity { get; set; }
        public UserIdentity MemberIdentity { get; set; }
    }

    internal static class ContextHelper
    {
        private static readonly AsyncLocal<CoreContext> _items = new AsyncLocal<CoreContext>();

        public static void SetAppIdentity(AppIdentity identity)
        {
            if (_items.Value == null)
            {
                _items.Value = new CoreContext();
            }

            _items.Value.AppIdentity = identity;
        }
        public static void SetTenantIdentity(TenantIdentity identity)
        {
            if (_items.Value == null)
            {
                _items.Value = new CoreContext();
            }

            _items.Value.TenantIdentity = identity;
        }
        public static void SetMemberIdentity(UserIdentity identity)
        {
            if (_items.Value == null)
            {
                _items.Value = new CoreContext();
            }

            _items.Value.MemberIdentity = identity;
        }


        public static CoreContext GetContext()
        {
            return _items.Value;
        }

    }

}
