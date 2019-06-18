using AdministracionRH.Common;
using AdministracionRH.Models;
using AdministracionRH.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministracionRH.Controllers
{
    public class ControlAsistenciaController : Controller
    {
        // GET: ControlAsistencia
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
            ViewBag.EnumRol = employee.Rol;

            EmployeeService employeeService = new EmployeeService();
            List<Catalogo> ListaCatalogos = employeeService.GetCatalogos();
            ViewBag.ListaAreas = ListaCatalogos.Where(i => i.ORIGEN == "AREAS").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION }).ToList();
            ViewBag.ListaPuestos = ListaCatalogos.Where(i => i.ORIGEN == "POSITIONS").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION }).ToList();
            ViewBag.ListaUblicacion = ListaCatalogos.Where(i => i.ORIGEN == "LOCALIZACION").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION }).ToList();
            ViewBag.ListaPatrones = ListaCatalogos.Where(i => i.ORIGEN == "PAYROLL").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION }).ToList();
            ViewBag.ListaJefes = employeeService.GetAllJefes().Select(i => new SelectListItem { Value = i.BossId.ToString(), Text = i.FullName }).ToList();
            ControlAsistenciaSearch model = new ControlAsistenciaSearch();
            model.Rol = employee.Rol;
            model.EmployeeIdConnected = Convert.ToInt32(employee.EmployeeId);
            ViewBag.enumPeriodoBusqueda = Convert.ToInt32(model.enumPeriodoBusqueda);

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(ControlAsistenciaSearch model)
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
            model.Rol = employee.Rol;
            model.EmployeeIdConnected =Convert.ToInt32(employee.EmployeeId);


            Result resultDownloadExcel = new Result();

            EmployeeService employeeService = new EmployeeService();
            List<Catalogo> ListaCatalogos = employeeService.GetCatalogos();
            ViewBag.ListaAreas = ListaCatalogos.Where(i => i.ORIGEN == "AREAS").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION }).ToList();
            ViewBag.ListaPuestos = ListaCatalogos.Where(i => i.ORIGEN == "POSITIONS").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION }).ToList();
            ViewBag.ListaUblicacion = ListaCatalogos.Where(i => i.ORIGEN == "LOCALIZACION").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION }).ToList();
            ViewBag.ListaPatrones = ListaCatalogos.Where(i => i.ORIGEN == "PAYROLL").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION }).ToList();
            ViewBag.ListaJefes = employeeService.GetAllJefes().Select(i => new SelectListItem { Value = i.BossId.ToString(), Text = i.FullName }).ToList();
            ViewBag.enumPeriodoBusqueda = Convert.ToInt32(model.enumPeriodoBusqueda);

            if (model.enumPeriodoBusqueda != Enumeraciones.enumPeriodoBusqueda.DefinirPeriodo && ModelState.IsValid == false)
            {
                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }
            }


            if (ModelState.IsValid)
            {
                ControlAsistenciaService service = new ControlAsistenciaService(model);
                Result result = service.GetDataByFilter();
                if (result.Success)
                {
                        var level = service._model.ListaEmpleado_Asistenica.Where(g => g.Level > 0).Min(i => i.Level);
                        var dataSoure = service._model.ListaEmpleado_Asistenica.Where(i => i.Level == level).OrderBy(o => o.Area).ThenBy(h => h.Nombre).ToList();
                        if (dataSoure.Count() == 0)
                            TempData["AlertMessage"] = "No se encontraron registros";
                        ViewBag.menusList = dataSoure;

                    if (model.ClickOnButton == "EnviarReporteSemanal")
                    {
                        service.EnviaReporteSemanalPorArea(Server.MapPath("~/EnviarReportes/"));
                    }
                    if (model.ClickOnButton == "DescargarReporte")
                    {
                        List<string> LstArchivosPorArea = new List<string>();
                        string ServerPath = Server.MapPath("~/ReporteAsistencia/");
                        string OutputFile = string.Empty;
                        service.BorrarArchivos(ServerPath);
                        resultDownloadExcel =  service.DescargarExcelReporteAsistencia(ServerPath, out LstArchivosPorArea);
                        if (resultDownloadExcel.Success)
                        {
                            if (LstArchivosPorArea.Count()>0)
                            {
                                if (LstArchivosPorArea.Count() == 1)
                                {
                                    foreach (string archivo in LstArchivosPorArea)
                                    {
                                        string filePath = ServerPath;
                                        string targetFileName = Path.GetFileName(archivo);
                                        Response.ClearContent();
                                        Response.Clear();
                                        Response.ContentType = "application/vnd.ms-excel";
                                        Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", targetFileName));
                                        Response.BinaryWrite(System.IO.File.ReadAllBytes(System.IO.Path.Combine(filePath, targetFileName)));
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                                else
                                {
                                    string CompressDirectory = Server.MapPath("~/Comprimidos/");
                                    //service.BorrarArchivos(ServerPath);
                                    var resultZip = service.Compress(ServerPath, CompressDirectory, out OutputFile);
                                    if (resultZip.Success)
                                    {
                                        string filename = Path.GetFileName(OutputFile);
                                        string serverpath = Server.MapPath($"~/Comprimidos/{filename}");
                                        FileInfo file = new FileInfo(serverpath);
                                        Response.Clear();
                                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                                        Response.AddHeader("Content-Length", file.Length.ToString());
                                        Response.ContentType = "application/zip";
                                        Response.WriteFile(file.FullName);
                                        Response.Flush();
                                        Response.End();
                                       

                                    }
                                }
                            }
                            return View(model);
                        }
                    }

                }
                return View(service._model);

            }
            else
            {
                return View(model);
            }


        }

      


    }
}