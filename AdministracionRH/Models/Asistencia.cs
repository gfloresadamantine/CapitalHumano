using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionRH.Models
{
    public class Asistencia
    {
        public int EmployeeId { get; set; }
        public DateTime? Fecha { get; set; }
        public string Entrada { get; set; }
        public string Salida { get; set; }
        public string HorasLaboradas { get; set; }
        public int Retardo { get; set; }
        public int? TotalRetardos { get; set; }
        public int? TotalFaltas { get; set; }
    }
}