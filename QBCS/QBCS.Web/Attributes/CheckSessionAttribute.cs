using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Attributes
{
    public class CheckSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = HttpContext.Current.Session["user"];
            if (user == null)
            {
                filterContext.Result = new RedirectResult("/QBCS.Web/Home");
            }
        }
    }
}