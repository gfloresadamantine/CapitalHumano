using AdministracionRH.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionRH.Models
{
    public class EmployeeAsistencia
    {
        public int EmployeeId { get; set; }
        public string Nombre { get; set; }
        public int? ParentID { get; set; }
        public string CardNumber { get; set; }
        public int? PositionId { get; set; }
        public string Puesto { get; set; }
        public int? AreaId { get; set; }
        public string Area { get; set; }
        public int? LocalizationId { get; set; }
        public string Ubicacion { get; set; }
        public int? PayRollId { get; set; }
        public string Patron { get; set; }
        public bool boolSelected { get; set; }
        public int Es_Jefe { get; set; }
        public string CompanyEmail { get; set; }
        public int TotalRetardos { get; set; }
        public int TotalFaltasInAsistencia { get; set; }
        public int TotalFaltas { get; set; }
        public int TotalFaltasPorRetardos { get; set; }
        public int TotalFaltasDespuesDelas9 { get; set; }
        public string HorasAcumuladas { get; set; }
        public List<Asistencia> LstDetalleAsistencia { get; set; }

    }


    public class ResumenEmployeeAsistencia
    {
        public int EmployeeId { get; set; }
        public string Nombre { get; set; }
        public int? ParentID { get; set; }
        public string CardNumber { get; set; }
        public int? PositionId { get; set; }
        public string Puesto { get; set; }
        public int? AreaId { get; set; }
        public string Area { get; set; }
        public int? LocalizationId { get; set; }
        public string Ubicacion { get; set; }
        public int? PayRollId { get; set; }
        public string Patron { get; set; }
        public bool boolSelected { get; set; }
        public int Es_Jefe { get; set; }
        public DateTime? Fecha { get; set; }
        public string Entrada { get; set; }
        public string Salida { get; set; }
        public string HorasLaboradas { get; set; }
        public int Retardo { get; set; }
        public string CompanyEmail { get; set; }
        public string HorasAcumuladas { get; set; }


    }
}