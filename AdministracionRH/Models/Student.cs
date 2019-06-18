using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionRH.Models
{
    public class Student
    {
        public int ID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Address { get; set; }
        public String email { get; set; }
        public String Department { get; set; }
        public List<Course> Courses = new List<Course>();
    }
}