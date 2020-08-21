using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OSS.CorePro.TAdminSite.Apis
{
    [AllowAnonymous]
    public class AnonymousModel : PageModel
    {
        public void OnGet()
        {
        }

    }
}
