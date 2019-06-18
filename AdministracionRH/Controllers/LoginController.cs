using AdministracionRH.Common;
using AdministracionRH.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministracionRH.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (Session["_SessionUser"] != null)
            {
                System.Web.HttpContext.Current.Session["_SessionUser"] = null;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email, string imageUrl)
        {

            try
            {
                if (Session["_SessionUser"] == null)
                {
                    UserService user = new UserService(email, imageUrl);
                    if (user.IsValidUser())
                    {
                        System.Web.HttpContext.Current.Session["_SessionUser"] = user.employee;
                    }
                    else
                    {

                        return Json(new
                        {
                            success = false,
                            mensaje = "El empleado no tiene acceso al sistema"

                        });

                    }

                }

            }
            catch (Exception ex)
            {

                return Json(new
                {
                    success = false,
                    mensaje = ex.Message

                });
            }


            return Json(new
            {
                success = true,
                urlRedirect = Url.Action("Index", "Home"),

            });


            //return Json(new { success = true, urlRedirect = string.Format("{0}/Home/Index", Request.Url.GetLeftPart(UriPartial.Authority)) }, JsonRequestBehavior.AllowGet);

        }

    }
}