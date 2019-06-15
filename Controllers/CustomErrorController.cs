using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProjectLab.Controllers
{
    public class CustomErrorController : Controller
    {
        // GET: CustomError
        public ActionResult Error400s()
        {
            return View();
        }

        public ActionResult Error404s()
        {
            return View();
        }
        public ActionResult GeneralError()
        {
            return View();
        }
    }
}