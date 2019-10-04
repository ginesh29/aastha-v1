using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AASTHA.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.Cookies["User"] == null)
            {
                var returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectResult(String.Concat("~/Account/Login", "?ReturnUrl=", returnUrl));
                return;
            }
        }

        public class DoctorActionFilter : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (System.Web.HttpContext.Current.Request.Cookies["Role"].Value != "Doctor")
                {
                    var returnUrl = filterContext.HttpContext.Request.RawUrl;
                    filterContext.Result = new RedirectResult(String.Concat("~/Account/Login", "?ReturnUrl=", returnUrl));
                    return;
                }
                base.OnActionExecuting(filterContext);
            }
        }

    }
}
