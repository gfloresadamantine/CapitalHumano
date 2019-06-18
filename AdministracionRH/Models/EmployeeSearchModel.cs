using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionRH.Models
{
    public class EmployeeSearchModel
    {
        //public List<MenuItem> ListaEmployees { get; set; }
        public string NombreEmpleado { get; set; }

        public List<EmployeeAsistencia> LstemployeeAsistencias { get; set; }
        public List<DateTime> LstRangoFechas { get; set; }
    }
}