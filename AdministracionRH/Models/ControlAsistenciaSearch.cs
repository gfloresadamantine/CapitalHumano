using AdministracionRH.Common;
using AdministracionRH.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdministracionRH.Models
{
    public class ControlAsistenciaSearch
    {

        public List<EmpleadoJefe> ListaEmpleadosJefe { get; set; }
        public List<Empleado_Asistencia> ListaEmpleado_Asistenica { get; set; }

        private List<EmpleadoJefe> _ListaEmpleadosJefe { get; set; }
        public List<EmployeeAsistencia> ListaEmpleadoAsistencia { get; set; }
        public List<DateTime> ListaRangoFechas { get; set; }


        [Display(Name = "Nombre Empleado")]
        public string NombreEmpleado { get; set; }
        [Display(Name = "Puesto")]
        public int? PositionId { get; set; }
        [Display(Name = "Area")]
        public int? AreaId { get; set; }

        [Display(Name = "Area")]
        public int? LocalizationId { get; set; }

        [Display(Name = "Patrón")]
        public int? PayRollId { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [Display(Name = "Desde")]
        public DateTime? FechaInicial { get; set; }

        [Display(Name = "Hasta")]
        [Required(ErrorMessage = "Requerido")]
        public DateTime? FechaFinal { get; set; }

        private DateTime FechaInicialAux { get; set; }

        [Display(Name = "Jefe")]
        public int? BossId { get; set; }

        public string ClickOnButton { get; set; }

        public Enumeraciones.enumRoles Rol { get; set; }
        public int EmployeeIdConnected { get; set; }
        public Enumeraciones.enumPeriodoBusqueda enumPeriodoBusqueda {get;set;}

        public List<ResumenEmployeeAsistencia> LstResumenEmployeeAsistencia { get; set; }

        public ControlAsistenciaSearch()
        {

            FechaInicial = null;//= new DateTime(2018, 12, 3);
            FechaFinal = null;// new DateTime(2018, 12, 7);
            NombreEmpleado = null;
            PositionId = null;
            AreaId = null;
            LocalizationId = null;
            PayRollId = null;
            BossId = null;
            ListaEmpleadoAsistencia = new List<EmployeeAsistencia>();
            ListaEmpleadosJefe = new List<EmpleadoJefe>();
            ListaRangoFechas = new List<DateTime>();
            ListaEmpleado_Asistenica = new List<Empleado_Asistencia>();
            ClickOnButton = string.Empty;
            enumPeriodoBusqueda = Enumeraciones.enumPeriodoBusqueda.EstaSemana;
            LstResumenEmployeeAsistencia = new List<ResumenEmployeeAsistencia>();

        }
      
    }
}