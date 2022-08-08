using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace OSS.Core.Infrastructure.Web.Attributes.Page
{
    //public class PageETagAttribute : IPageFilter
    //{
    //    public PageETagAttribute()
    //    {
    //    }

    //    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    //    {
    //    }


    //    private string _oldTag;
    //    private bool _isPjax;

    //    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    //    {
    //        _oldTag = context.HttpContext.Request.Headers[HeaderNames.IfNoneMatch].FirstOrDefault();
    //        if (string.IsNullOrEmpty(_oldTag)) return;

    //        _isPjax = context.HttpContext.Request.IsPjax();
    //        if (_isPjax && _oldTag == AppWebInfoHelper.GetPjaxTagWithTenant())
    //            context.Result = new StatusCodeResult(304);

    //    }

    //    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    //    {
    //        //var newTag = AppSysContext.Identity?.OTag;
    //        //if (string.IsNullOrEmpty(newTag))
    //        //    return;

    //        //if (newTag == _oldTag)
    //        //    context.Result = new StatusCodeResult(304);        

    //        //context.HttpContext.Response.OnStarting(() =>
    //        //{
    //        //    context.HttpContext.Response.Headers[HeaderNames.ETag] = newTag;
    //        //    return Task.CompletedTask;
    //        //});
    //    }
    //}
}
