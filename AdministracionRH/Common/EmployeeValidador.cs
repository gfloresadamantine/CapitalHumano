using AdministracionRH.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static AdministracionRH.Common.Enumeraciones;

namespace AdministracionRH.Common
{
    public class EmployeeValidador
    {

       
        public string EmployeeId { get; set; }

        [Display(Name = "No. Gafete")]
        public string CardId { get; set; }

        [Required]
        [Display(Name = "ID Adamantine")]
        public string CardNumber { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Apellido Paterno")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Apellido Materno")]
        public string MiddleName { get; set; }

        [Display(Name = "Apellido Paterno, Materno, Nombre(s)")]
        public string FullName { get; set; }
        public string Photo { get; set; }

        //[Required]
        [Display(Name = "RFC: Alfabético, Numérico, Homoclave")]
        public string Rfc { get; set; }

        //[Required]
        [Display(Name = "Curp")]
        public string Curp { get; set; }

        //[Required]
        [Display(Name = "No. Seguro Social")]
        public string NSS { get; set; }


        //[Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha Nacimiento (Día/Mes/Año)")]
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

        [Display(Name = "Código Postal")]
        public string CP { get; set; }

        [Display(Name = "Teléfono Particular")]
        public string PhoneNumber { get; set; }

        //[Required]
        [Display(Name = "Teléfono Móvil")]
        public string CellPhoneNumber { get; set; }

        [Display(Name = "Tel. Oficinal")]
        public string OfficePhone { get; set; }

        [Display(Name = "Extensión")]
        public string OfficeExt { get; set; }

        [RegularExpression("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$", ErrorMessage = "Email erróneo")]
        [Display(Name = "Correo Electrónico (Personal)")]
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

        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; }

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

        public enumRoles Rol { get; set; }
        public string RolDescripcion { get; set; }

        [Display(Name = "Huella Registrada")]
        public bool FingerPrint { get; set; }

        [Display(Name = "Estacionamiento")]
        public string ParkingSpace { get; set; }

        public string GoogleImage { get; set; }

        [Display(Name = "Fecha Baja")]
        public DateTime? LeavingDate { get; set; }

        [Display(Name = "Fecha antigüedad")]
        public DateTime? EntryDate { get; set; }

        [Display(Name = "Compañia")]
        public string Compania { get; set; }

        [Display(Name = "Banco")]
        public string Banco { get; set; }

        [Display(Name = "Clabe Interbancaria")]
        public string Clabe { get; set; }

        [Display(Name = "Estado Civil")]
        public string EdoCivil { get; set; }

        [Display(Name = "Lugar Nacimiento")]
        public string LugarNacimiento { get; set; }

        [Display(Name = "Contrato Infonavit")]
        public string CreditoInfonavit { get; set; }

        [Display(Name = "Estado")]
        public string EstadoEmpleado { get; set; }

        public string CertificatePath { get; set; }

        public List<Documentacion> LstDocumentos { get; set; }

        [Display(Name = "Sexo")]
        public string Sexo { get; set; }

        [Display(Name = "Trámite Infonavit (Sí-No)")]
        public string TRAMITE_INF_SN { get; set; }

        [Display(Name = "Etapa")]
        public string Etapa { get; set; }

        [Display(Name = "Estudios (Nombre de la Carrera o Bachillerato)")]
        public string Estudios { get; set; }

        [Display(Name = "Nivel (Trunco, Pasante, Titulado, Posgrado)")]
        public string Nivel_Estudios { get; set; }

        [Display(Name = "Universidad")]
        public string Universidad { get; set; }

        [Display(Name = "No. Empleado Jefe")]
        public string Id_Jefe { get; set; }

        [Display(Name = "Nombre Jefe")]
        public string Nombre_jefe { get; set; }

        [Display(Name = "Último grado de estudios")]
        public string Ultimo_GradoEstudios { get; set; }

        [Display(Name = "Activo/Inactivo")]
        public bool Estatus { get; set; }


    }
}