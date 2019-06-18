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
    public class CatalogosController : Controller
    {
        // GET: Catalogos
        [Route("Index/{origen}")]
        public ActionResult Index(Enumeraciones.enumCatalogos enumCatalogo)
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

            if (Session["ListaCatalogo"] != null)
                Session["ListaCatalogo"] = null;

            CatalogosModel model = new CatalogosModel(enumCatalogo);
            ViewBag.Description = model.HeaderDescription;
            ViewBag.Origen = model.Origen;
            Session["ListaCatalogo"] = model.lstCatalogos;
            return View("Index", model);
        }
        public JsonResult Create(Catalogo catalogo, Enumeraciones.enumCatalogos enumeracion)
        {

            CatalogoService service = new CatalogoService(enumeracion);
            var result = service.Create(catalogo);
            return new JsonResult()
            {
                Data = new
                {
                    success = result.Success,
                    message = result.Success ? "Nuevo registro exitoso" : string.Format("Hubo un problema con el registro: {0}, no se procesó exitosamente, vuelva a intentarlo", result.ErrorMessage),
                    Enumeracion = enumeracion
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult Edit(int Id, Enumeraciones.enumCatalogos enumeracion)
        {
            Catalogo catalogo = new Catalogo();
            if (Session["ListaCatalogo"]!=null)
            {
                var lista = (List<Catalogo>)Session["ListaCatalogo"];
                catalogo = lista.Where(i => i.ID == Id).FirstOrDefault();
            }
            return Json(catalogo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(Catalogo catalogo, Enumeraciones.enumCatalogos enumeracion)
        {
            CatalogoService service = new CatalogoService(enumeracion);
            var result= service.Update(catalogo);
         
            return new JsonResult()
            {
                Data = new
                {
                    success = result.Success,
                    message = result.Success ? "El registro se actualizó exitosamente" : string.Format("Hubo un problema con el registro: {0}, no se actualizó exitosamente, vuelva a intentarlo", result.ErrorMessage),
                    Enumeracion = enumeracion
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        
        public JsonResult Delete(int Id, Enumeraciones.enumCatalogos enumeracion)
        {
            CatalogoService service = new CatalogoService(enumeracion);
            var result =service.Delete(Id);
            return new JsonResult()
            {
                Data = new
                {
                    success = result.Success,
                    message = result.Success ? "El registro se eliminó exitosamente" : string.Format("Hubo un problema con el registro: {0}, no se eliminó exitosamente, vuelva a intentarlo", result.ErrorMessage),
                    Enumeracion = enumeracion
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }



    }
}