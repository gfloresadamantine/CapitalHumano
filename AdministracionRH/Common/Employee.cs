using AdministracionRH.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AdministracionRH.Common.Enumeraciones;

namespace AdministracionRH.Common
{
    public class Employee
    {
        public string EmployeeId { get; set; }

        [Display(Name = "No. Gafete")]
        public string CardId { get; set; }

        [Required]
        [Display(Name = "No. Empleado")]
        public string CardNumber { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Apellido paterno")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Apellido materno")]
        public string MiddleName { get; set; }
        [Display(Name = "Nombre")]

        public string FullName { get; set; }
        public string Photo { get; set; }

        //[Required]
        [Display(Name = "Rfc")]
        public string Rfc { get; set; }

        //[Required]
        [Display(Name = "Curp")]
        public string Curp { get; set; }

        //[Required]
        [Display(Name = "NSS")]
        public string NSS { get; set; }


        //[Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha nacimiento")]
        public DateTime? BirthDay { get; set; }

        //[Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha ingreso")]
        public DateTime? AdmissionDate { get; set; }

        //[Required]
        [Display(Name = "Calle")]
        public string StreetName { get; set; }

        //[Required]
        [Display(Name = "No. Exterior")]
        public string NumberExt { get; set; }

        [Display(Name = "No. Interior")]
        public string NumberInt { get; set; }

        //[Required]
        [Display(Name = "Delegación")]
        public string Delegation { get; set; }

        //[Required]
        [Display(Name = "Colonia")]
        public string Colony { get; set; }

        //[Display(Name = "Cp")]
        public string CP { get; set; }

        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        //[Required]
        [Display(Name = "Celular")]
        public string CellPhoneNumber { get; set; }

        [Display(Name = "Tel. Oficinal")]
        public string OfficePhone { get; set; }

        [Display(Name = "Extensión")]
        public string OfficeExt { get; set; }

        [RegularExpression("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$", ErrorMessage = "Email erróneo")]
        [Display(Name = "Email personal")]
        public string PersonalEmail { get; set; }

        //[Required]
        [RegularExpression("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$", ErrorMessage = "Email erróneo")]
        [Display(Name = "Email empresa")]
        public string CompanyEmail { get; set; }

        [Display(Name = "Activo")]
        public bool Enabled { get; set; }

        //[Required]
        [Display(Name = "Ubicación")]
        public int? LocalizationId { get; set; }

        //[Required]
        [Display(Name = "Patrón")]
        public int? PayRollId { get; set; }

        //[Required]
        [Display(Name = "Compañia")]
        public int? CompanyId { get; set; }

        //[Required]
        [Display(Name = "Nacionalidad")]
        public int? NationalityId { get; set; }

        public int? EmployeePositionId { get; set; }

        [Required]
        [Display(Name = "Jefe")]
        public int? BossId { get; set; }

        //[Required]
        [Display(Name = "Puesto")]
        public int? PositionId { get; set; }

        //[Required]
        [Display(Name = "Area")]
        public int? AreaId { get; set; }


        [Display(Name = "Puesto")]
        public string PositionName { get; set; }
        [Display(Name = "Area")]
        public string AreaName { get; set; }
        [Display(Name = "Jefe")]
        public string BossName { get; set; }

        public List<Catalogo> lstNacionalidad { get; set; }
        public List<Catalogo> lstAreas { get; set; }
        public List<Catalogo> lstPosition { get; set; }
        public List<Catalogo> lstLocalizacion { get; set; }
        public List<Catalogo> lstPayRoll { get; set; }
        public List<Catalogo> lstCompanies { get; set; }

        
        public IEnumerable<SelectListItem> ListaNacionalidad { get; set; }
        public IEnumerable<SelectListItem> ListaAreas { get; set; }
        public IEnumerable<SelectListItem> ListaPosition { get; set; }
        public IEnumerable<SelectListItem> ListaLocalizacion { get; set; }
        public IEnumerable<SelectListItem> ListaPayRoll { get; set; }
        public IEnumerable<SelectListItem> ListaCompanies { get; set; }
        public IEnumerable<SelectListItem> ListaJefes { get; set; }
        public List<Jefe> lstJefes { get; set; }

        public enumRoles  Rol { get; set; }
        public string RolDescripcion { get; set; }

        [Display(Name = "Huella Registrada")]
        public bool FingerPrint { get; set; }

        [Display(Name = "Estacionamiento")]
        public string ParkingSpace { get; set; }

        public string GoogleImage{ get; set; }
        public Employee()
        {

        }
        public Employee(string Action)
        {

            if (Action =="C")

            {
                lstNacionalidad = new List<Catalogo>();
                lstAreas = new List<Catalogo>();
                lstPosition = new List<Catalogo>();
                lstLocalizacion = new List<Catalogo>();
                lstPayRoll = new List<Catalogo>();
                lstCompanies = new List<Catalogo>();
                lstJefes = new List<Jefe>();

                EmployeeService employeeService = new EmployeeService();
                List<Catalogo> lstCatalogos = employeeService.GetCatalogos();
                lstAreas = lstCatalogos.Where(i => i.ORIGEN == "AREAS").ToList();
                lstNacionalidad = lstCatalogos.Where(i => i.ORIGEN == "NACIONALIDAD").ToList();
                lstPosition = lstCatalogos.Where(i => i.ORIGEN == "POSITIONS").ToList();
                lstLocalizacion = lstCatalogos.Where(i => i.ORIGEN == "LOCALIZACION").ToList();
                lstPayRoll = lstCatalogos.Where(i => i.ORIGEN == "PAYROLL").ToList();
                lstCompanies = lstCatalogos.Where(i => i.ORIGEN == "COMPANIES").ToList();
                lstJefes = employeeService.GetAllJefes();
                Enabled = true;
                
                
            }
          

        }

    }
}