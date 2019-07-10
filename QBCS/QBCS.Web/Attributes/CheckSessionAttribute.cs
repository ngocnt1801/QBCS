using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
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
            IUserService userService = new UserService();

            var user = (UserViewModel)HttpContext.Current.Session["user"];
            if (user == null)
            {
                user = userService.GetUser(filterContext.HttpContext.User.Get(u => u.Code));
                HttpContext.Current.Session["user"] = user ?? user;
            }
        }
    }
}