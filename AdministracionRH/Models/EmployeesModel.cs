using AdministracionRH.Common;
using AdministracionRH.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AdministracionRH.Common.Enumeraciones;

namespace AdministracionRH.Models
{
    public class EmployeesModel
    {
        public List<Employee> LstEmployees { get; set; }

        [Display(Name = "Nombre Empleado")]
        public string Nombre { get; set; }

        [Display(Name = "Area")]
        public string Area { get; set; }

        [Display(Name = "Puesto")]
        public string Posicion { get; set; }

        [Display(Name = "Jefe")]
        public string Jefe { get; set; }

        [Display(Name = "Fecha Ingreso Inicial")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaInicial { get; set; }

        [Display(Name = "Fecha Ingreso Final")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaFinal { get; set; }

        public int? SelectedEmployeeId { get; set; }
        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public int RecordCount { get; set; }

        [Display(Name = "Sin jefe")]
        public bool boolSinJefe { get; set; }

        public IEnumerable<SelectListItem> ListaAreas { get; set; }
        public int AreaId { get; set; }
        public IEnumerable<SelectListItem> ListaPuestos { get; set; }
        public int PositionId { get; set; }

        public IEnumerable<SelectListItem> ListaJefes { get; set; }
        public int BossId { get; set; }

        public List<SelectListItem> ListaSinOperacion { get; set; }
        public int OperacionId { get; set; }
        public enumRoles Rol { get; set; }
        public int EmployeeIdConected { get; set; }

        [Display(Name = "Con Huella")]
        public bool FingerPrint { get; set; }


        public EmployeesModel ()
        {
            EmployeeService service = new EmployeeService();

            LstEmployees = new List<Employee>();
            SelectedEmployeeId = null;
            Activo = true;
            RecordCount = -1;
            PageSize = 15;
            PageCount = 0;
            CurrentPageIndex = 1;
            SortField = "FirstName";
            SortDirection = "ascending";
            boolSinJefe = false;
            ListaAreas = service.GetCatalogos().Where(i => i.ORIGEN == "AREAS").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION}).ToList();
            ListaPuestos = service.GetCatalogos().Where(i => i.ORIGEN == "POSITIONS").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION }).ToList();
            ListaJefes =service.GetAllJefes().Select(i => new SelectListItem { Value = i.BossId.ToString(), Text = i.FullName }).ToList();
            List <SelectListItem> _ListaSinOperacion = new List<SelectListItem>();
            _ListaSinOperacion.Add(new SelectListItem() { Text = "Sin Jefe", Value = "1" });
            _ListaSinOperacion.Add(new SelectListItem() { Text = "Sin Ubicación", Value = "2" });
            _ListaSinOperacion.Add(new SelectListItem() { Text = "Sin Puesto", Value = "3" });
            _ListaSinOperacion.Add(new SelectListItem() { Text = "Sin Area", Value = "4" });
            _ListaSinOperacion.Add(new SelectListItem() { Text = "Sin Patrón", Value = "5" });
            ListaSinOperacion = _ListaSinOperacion;
            FingerPrint = true;

        }



    }
}