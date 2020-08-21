using Microsoft.AspNetCore.Razor.TagHelpers;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Infrastructure.Web.Helpers;

namespace OSS.Core.Infrastructure.Web.TagHelpers
{
    [HtmlTargetElement("script", Attributes = "oss-version")]
    public class OssScriptTagHelper : TagHelper
    {
        [HtmlAttributeName("src")]
        public string Src { get; set; }

        [HtmlAttributeName("oss-version")]
        public bool Version { get; set; } = false;

        /// <summary>
        /// 加载前执行方法
        /// </summary>
        [HtmlAttributeName("oss-beforeload")]
        public string BeforLoad { get; set; }
        

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "script";
            var url = string.Concat(AppWebInfoHelper.CssDomain, Src.TrimStart('~'), "?v=",
                Version ? AppInfoHelper.AppVersion : string.Empty);

            output.Attributes.SetAttribute("src", url);
            output.Attributes.SetAttribute("beforeload", BeforLoad);
        }

    }

    [HtmlTargetElement("link", Attributes = "oss-version")]
    public class OssCssTagHelper : TagHelper
    {
        [HtmlAttributeName("href")]
        public string Href { get; set; }

        [HtmlAttributeName("rel")]
        public string Rel { get; set; }

        [HtmlAttributeName("oss-version")]
        public bool Version { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "link";
            output.Attributes.SetAttribute("rel", Rel);

            var url = string.Concat(AppWebInfoHelper.CssDomain, Href.TrimStart('~'), "?v=",
                Version ? AppInfoHelper.AppVersion : string.Empty);
            output.Attributes.SetAttribute("href", url);
        }

    }


}