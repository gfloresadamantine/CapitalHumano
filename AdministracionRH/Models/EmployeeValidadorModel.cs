using AdministracionRH.Common;
using AdministracionRH.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static AdministracionRH.Common.Enumeraciones;

namespace AdministracionRH.Models
{
    public class EmployeeValidadorModel
    {

        public List<EmployeeValidador> LstEmployeesValidador { get; set; }

        public List<EmployeeValidador> LstEmployeesValidadorPaged { get; set; }

        [Display(Name = "Nombre Empleado")]
        public string NoEmpleado { get; set; }

        [Display(Name = "Nombre Empleado")]
        public string Nombre { get; set; }

        [Display(Name = "RFC")]
        public string Rfc { get; set; }

        [Display(Name = "NSS")]
        public string Nss { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public int RecordCount { get; set; }
        public string ClickOnButton { get; set; }




        public int OperacionId { get; set; }
        public enumRoles Rol { get; set; }
        public int EmployeeIdConected { get; set; }

        

        public EmployeeValidadorModel()
        {
            LstEmployeesValidador = new List<EmployeeValidador>();
            LstEmployeesValidadorPaged = new List<EmployeeValidador>();
            Nombre = null;
            Nss = null;
            NoEmpleado = null;
            Rfc = null;
            RecordCount = -1;
            PageSize = 15;
            PageCount = 0;
            CurrentPageIndex = 1;
            SortField = "LastName";
            SortDirection = "ascending";
            ClickOnButton = "Consulta";





        }
    }
}