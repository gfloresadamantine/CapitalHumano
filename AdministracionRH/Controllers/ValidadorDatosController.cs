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
    public class ValidadorDatosController : Controller
    {
        // GET: ValidadorDatos
        public ActionResult Index()
        {

            Employee employee = (Employee)System.Web.HttpContext.Current.Session["_SessionUser"];
            if (employee == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0} {1} {2}", employee.FirstName.ToLower(), employee.LastName.ToLower(), employee.MiddleName.ToLower()));
            ViewBag.EmailUser = employee.CompanyEmail;
            ViewBag.ImageUser = employee.GoogleImage;
            ViewBag.AccessGroup = employee.AreaName;
            ViewBag.Rol = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.PositionName.ToLower());
            //ViewBag.RecordCount = null;
            ViewBag.EnumRol = employee.Rol;

            ViewBag.CardNumber = employee.CardNumber;

            EmployeeValidadorServices service = new EmployeeValidadorServices(new EmployeeValidadorModel());
            service.Rol = employee.Rol;
            int CardNumberlongitud = employee.CardNumber.Length;
            service._NoEmpleado = CardNumberlongitud != 5 ? employee.CardNumber.Substring(3,5): employee.CardNumber;
            var _model = service.GetDataByFilter();
            if (!service.result.Success)
            {
                TempData["AlertMessage"] = service.result.ErrorMessage;
                return View(_model);
            }
            _model.Rol = employee.Rol;
            _model.EmployeeIdConected = Convert.ToInt32(employee.EmployeeId);

            TempData["PageCount"] = _model.PageCount;
            TempData["RecordCount"] = _model.RecordCount;
            TempData["CurrentPageIndex"] = 1;
            return View(_model);


        }
        [HttpPost]
        public ActionResult Index(EmployeeValidadorModel model)
        {

            Employee employee = (Employee)System.Web.HttpContext.Current.Session["_SessionUser"];
            if (employee == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0} {1} {2}", employee.FirstName.ToLower(), employee.LastName.ToLower(), employee.MiddleName.ToLower()));
            ViewBag.EmailUser = employee.CompanyEmail;
            ViewBag.ImageUser = employee.GoogleImage;
            ViewBag.AccessGroup = employee.AreaName;
            ViewBag.Rol = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.PositionName.ToLower());
            //ViewBag.RecordCount = null;
            ViewBag.EnumRol = employee.Rol;
            ViewBag.CardNumber = employee.CardNumber;

          

            EmployeeValidadorServices service = new EmployeeValidadorServices(model);
            service.Rol = employee.Rol;
            int CardNumberlongitud = employee.CardNumber.Length;
            service._NoEmpleado = CardNumberlongitud != 5 ? employee.CardNumber.Substring(3, 5) : employee.CardNumber;

            if (model.ClickOnButton == "DescargarReporte")
            {
                TempData["PageCount"] = model.PageCount;
                TempData["RecordCount"] = model.RecordCount;
                TempData["CurrentPageIndex"] = model.CurrentPageIndex;
                model.PageSize = 15;
                var FilePath = service.ExportarExcel();
                Response.ClearContent();
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", System.IO.Path.GetFileName(FilePath)));
                Response.BinaryWrite(System.IO.File.ReadAllBytes(FilePath));
                Response.Flush();
                Response.End();

                return View(model);
            }
            else
            {
                var   _model = service.GetDataByFilter();
                if (!service.result.Success)
                {
                    TempData["AlertMessage"] = service.result.ErrorMessage;
                    return View(_model);
                }
                _model.Rol = employee.Rol;
                _model.EmployeeIdConected = Convert.ToInt32(employee.EmployeeId);


                TempData["PageCount"] = _model.PageCount;
                TempData["RecordCount"] = _model.RecordCount;
                TempData["CurrentPageIndex"] = _model.CurrentPageIndex;
                _model.PageSize = 15;
                return View(_model);
            }


            
        }

        public ActionResult Editar(string NoEmpleado)
        {

            Employee employee = (Employee)System.Web.HttpContext.Current.Session["_SessionUser"];
            if (employee == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0} {1} {2}", employee.FirstName.ToLower(), employee.LastName.ToLower(), employee.MiddleName.ToLower()));
            ViewBag.EmailUser = employee.CompanyEmail;
            ViewBag.ImageUser = employee.GoogleImage;
            ViewBag.AccessGroup = employee.AreaName;
            ViewBag.Rol = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.PositionName.ToLower());
            //ViewBag.RecordCount = null;
            ViewBag.EnumRol = employee.Rol;
            ViewBag.CardNumber = employee.CardNumber;



            EmployeeValidadorModel model = new EmployeeValidadorModel();
            model.NoEmpleado = NoEmpleado;

            EmployeeValidador empleado = new EmployeeValidador();
            EmployeeValidadorServices service = new EmployeeValidadorServices(model);

            var _model = service.GetDataByFilter();
            if (!service.result.Success)
            {
                TempData["AlertMessage"] = service.result.ErrorMessage;
                return View(_model);
            }
            _model.Rol = employee.Rol;
            _model.EmployeeIdConected = Convert.ToInt32(employee.EmployeeId);
            _model.LstEmployeesValidador.ForEach(g => g.CertificatePath = "/Pdfs/");
            empleado = _model.LstEmployeesValidador.Where(i => i.CardNumber == model.NoEmpleado).FirstOrDefault();
            TempData["PageCount"] = _model.PageCount;
            TempData["RecordCount"] = _model.RecordCount;
            TempData["CurrentPageIndex"] = 1;
            return View("_Edicion",empleado);


        }

        [HttpPost]
        public ActionResult Editar(EmployeeValidador employeeValidador)
        {

            Employee employee = (Employee)System.Web.HttpContext.Current.Session["_SessionUser"];
            if (employee == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0} {1} {2}", employee.FirstName.ToLower(), employee.LastName.ToLower(), employee.MiddleName.ToLower()));
            ViewBag.EmailUser = employee.CompanyEmail;
            ViewBag.ImageUser = employee.GoogleImage;
            ViewBag.AccessGroup = employee.AreaName;
            ViewBag.Rol = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.PositionName.ToLower());
            //ViewBag.RecordCount = null;
            ViewBag.EnumRol = employee.Rol;
            ViewBag.CardNumber = employee.CardNumber;

            EmployeeValidadorServices service = new EmployeeValidadorServices();
            var result = service.UpdateEmployee(employeeValidador);
            if (!result.Success)
            {
                TempData["AlertMessage"] = "Hubo un problema al actualizar los datos  del empleado, intente de nuevo";
                return RedirectToAction("Index", "ValidadorDatos");
            }
            
            TempData["AlertMessage"] = "Los datos del empleado se actualizaron exitosamente"; 
            return RedirectToAction("Index", "ValidadorDatos");


        }
       

    }
}