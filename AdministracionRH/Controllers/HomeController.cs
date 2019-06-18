using AdministracionRH.Common;
using AdministracionRH.Models;
using AdministracionRH.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministracionRH.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {



            Employee employee= (Employee)System.Web.HttpContext.Current.Session["_SessionUser"];
            if (employee == null)
                return RedirectToAction("Index", "Login");
           
            ViewBag.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0} {1} {2}",  employee.FirstName.ToLower(), employee.LastName.ToLower(), employee.MiddleName.ToLower()));
            ViewBag.EmailUser = employee.CompanyEmail;
            ViewBag.ImageUser = employee.GoogleImage;
            ViewBag.AccessGroup = employee.AreaName;
            ViewBag.Rol = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.PositionName.ToLower());
            ViewBag.EnumRol = employee.Rol;
            ViewBag.CardNumber = employee.CardNumber;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}