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
    public class EmployeesController : Controller
    {
        // GET: Employees
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

            EmployeesModel model = new EmployeesModel();

            model.Rol = employee.Rol;
            model.EmployeeIdConected = Convert.ToInt32(employee.EmployeeId);

            TempData["RecordCount"] = 0;
            TempData["CurrentPageIndex"] = 1;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(EmployeesModel model)
        {
            Employee employee = (Employee)System.Web.HttpContext.Current.Session["_SessionUser"];
            if (employee == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0} {1} {2}", employee.FirstName.ToLower(), employee.LastName.ToLower(), employee.MiddleName.ToLower()));
            ViewBag.EmailUser = employee.CompanyEmail;
            ViewBag.ImageUser = employee.GoogleImage;
            ViewBag.AccessGroup = employee.AreaName;
            ViewBag.Rol = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.PositionName.ToLower());
            ViewBag.EnumRol = employee.Rol;

            EmployeeService employeeService = new EmployeeService(model);

            model.Rol = employee.Rol;
            model.EmployeeIdConected = Convert.ToInt32(employee.EmployeeId);

            model.LstEmployees = employeeService.GetAllEmployees();

            TempData["PageCount"] = model.PageCount;
            TempData["RecordCount"] = model.RecordCount;
            TempData["CurrentPageIndex"] = model.CurrentPageIndex;

            return View(model);
            
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            Employee employee = (Employee)System.Web.HttpContext.Current.Session["_SessionUser"];
            if (employee == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0} {1} {2}", employee.FirstName.ToLower(), employee.LastName.ToLower(), employee.MiddleName.ToLower()));
            ViewBag.EmailUser = employee.CompanyEmail;
            ViewBag.ImageUser = employee.GoogleImage;
            ViewBag.AccessGroup = employee.AreaName;
            ViewBag.Rol = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.PositionName.ToLower());
            ViewBag.EnumRol = employee.Rol;

            EmployeeService employeeService = new EmployeeService();
            Employee _employee = employeeService.GetAOneEmployee(id);
            ViewBag.ListaAreas = _employee.ListaAreas;
            ViewBag.ListaNacionalidad = _employee.ListaNacionalidad;
            ViewBag.ListaPosition = _employee.ListaPosition;
            ViewBag.ListaLocalizacion = _employee.ListaLocalizacion;
            ViewBag.ListaPayRoll = _employee.ListaPayRoll;
            ViewBag.ListaCompanies = _employee.ListaCompanies;
            ViewBag.ListaJefes = _employee.ListaJefes;


            return View("Editar", _employee);
        }

        [HttpPost]
        public ActionResult Editar(Employee employee)
        {

            bool success = false;
            Employee _employee = (Employee)System.Web.HttpContext.Current.Session["_SessionUser"];
            if (_employee == null)
                return RedirectToAction("Index", "Login");
            ViewBag.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0} {1} {2}", _employee.FirstName.ToLower(), _employee.LastName.ToLower(), _employee.MiddleName.ToLower()));
            ViewBag.EmailUser = _employee.CompanyEmail;
            ViewBag.ImageUser = _employee.GoogleImage;
            ViewBag.AccessGroup = _employee.AreaName;
            ViewBag.Rol = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_employee.PositionName.ToLower());
            ViewBag.EnumRol = employee.Rol;

            EmployeeService employeeService = new EmployeeService();

            List<Catalogo> ListaCatalogos = employeeService.GetCatalogos();
            ViewBag.ListaAreas = ListaCatalogos.Where(i => i.ORIGEN == "AREAS").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION, Selected = i.ID == employee.AreaId }).ToList();
            ViewBag.ListaNacionalidad = ListaCatalogos.Where(i => i.ORIGEN == "NACIONALIDAD").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION, Selected = i.ID == employee.AreaId }).ToList();
            ViewBag.ListaPosition = ListaCatalogos.Where(i => i.ORIGEN == "POSITIONS").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION, Selected = i.ID == employee.AreaId }).ToList();
            ViewBag.ListaLocalizacion = ListaCatalogos.Where(i => i.ORIGEN == "LOCALIZACION").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION, Selected = i.ID == employee.AreaId }).ToList();
            ViewBag.ListaPayRoll = ListaCatalogos.Where(i => i.ORIGEN == "PAYROLL").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION, Selected = i.ID == employee.AreaId }).ToList();
            ViewBag.ListaCompanies = ListaCatalogos.Where(i => i.ORIGEN == "COMPANIES").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION, Selected = i.ID == employee.AreaId }).ToList();
            ViewBag.ListaJefes = employeeService.GetAllJefes().Select(i => new SelectListItem { Value = i.BossId.ToString(), Text = i.FullName, Selected = i.BossId == employee.BossId }).ToList();


            if (ModelState.IsValid)
            {
                
                success = employeeService.UpdateEmployee(employee);
                TempData["AlertMessage"] = success == true ? "Los datos del empleado se actualizaron exitosamente" : "Hubo un problema al actualizar los datos  del empleado, intente de nuevo";
                return RedirectToAction("Index", "Employees");
            }
            return View("Editar", employee);

        }
        public ActionResult Crear()
        {
            Employee employee = (Employee)System.Web.HttpContext.Current.Session["_SessionUser"];
            if (employee == null)
                return RedirectToAction("Index", "Login");
            

            ViewBag.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0} {1} {2}", employee.FirstName.ToLower(), employee.LastName.ToLower(), employee.MiddleName.ToLower()));
            ViewBag.EmailUser = employee.CompanyEmail;
            ViewBag.ImageUser = employee.GoogleImage;
            ViewBag.AccessGroup = employee.AreaName;
            ViewBag.Rol = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.PositionName.ToLower());
            ViewBag.EnumRol = employee.Rol;
            Employee _employee = new Employee("C");
            EmployeeService employeeService = new EmployeeService();

            return View("Nuevo", _employee);
        }

        [HttpPost]
        public ActionResult Crear(Employee _employee)
        {
            bool success = false;

            Employee employee = (Employee)System.Web.HttpContext.Current.Session["_SessionUser"];
            if (employee == null)
                return RedirectToAction("Index", "Login");
            ViewBag.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0} {1} {2}", employee.FirstName.ToLower(), employee.LastName.ToLower(), employee.MiddleName.ToLower()));
            ViewBag.EmailUser = employee.CompanyEmail;
            ViewBag.ImageUser = employee.GoogleImage;
            ViewBag.AccessGroup = employee.AreaName;
            ViewBag.Rol = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.PositionName.ToLower());
            ViewBag.EnumRol = employee.Rol;
            if (ModelState.IsValid)
            {
                EmployeeService employeeService = new EmployeeService();
                success = employeeService.CreateEmployee(_employee);
                TempData["AlertMessage"] = success == true ? "El nuevo empleado se creo exitosamente" : "Hubo un problema al registrar el empleado, intente de nuevo";
                return RedirectToAction("Index", "Employees");
            }
            return View("Nuevo", _employee);
        }

    }
}