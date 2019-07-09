using AuthLib.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    public class ElmahController : Controller
    {

        [Feature(FeatureType.Page, "Error Log", "QBCS", protectType: ProtectType.Authorized)]
        public RedirectResult Index()
        {
            var url = Url.Content("~/elmah.axd");
            return Redirect(url);
        }
    }
}