using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionRH.Models
{
    public class EmpleadoJefe
    {
        public int? EmployeeId { get; set; }
        public string NombreEmpleado { get; set; }
        public int? ParentID { get; set; }
        public int Level { get; set; }
        public virtual ICollection<EmpleadoJefe> Children { get; set; }
    }
}