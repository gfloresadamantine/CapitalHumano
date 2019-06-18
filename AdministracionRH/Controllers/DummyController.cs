using AdministracionRH.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministracionRH.Controllers
{
    public class DummyController : Controller
    {
        // GET: Dummy
        public ActionResult Index()
        {

            ViewBag.EnumRol = Enumeraciones.enumRoles.AdministradorRh;
            return View();
        }
    }
}