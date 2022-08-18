
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    public class RoleMo : BaseOwnerAndStateMo<long>
    {
        public string name { get; set; }
    }
}
