using AdministracionRH.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionRH.Services
{
    public class UserService
    {
        public Employee employee { get; set; }
        private string _Email { get; set; }
        private string _googleImage { get; set; }

        public UserService(string email, string googleImage)
        {
            _Email = email;
            _googleImage = googleImage;
        }

        public bool IsValidUser()
        {

            EmployeeService employeeService = new EmployeeService();
            employee = employeeService.GetAOneEmployeeSystem(_Email);
            if (employee != null && employee.Enabled ==true)
                employee.GoogleImage = _googleImage;

            return (employee != null && employee.Enabled == true);
        }
    }



    }
