using AdministracionRH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministracionRH.Controllers
{


    public class JerarquiasController : Controller
    {
        // GET: Jerarquias
        public static List<Student> Students = new List<Student>()
            {
                new Student
                { ID = 1, FirstName = "Jhon", LastName = "Smith",
                            Courses = new List<Course>()
                            {
                                new Course { ID = 1,
                            CourseName = "CS 101",
                            SemesterName = "Winter 2010"
                          },
                                new Course { ID = 2,
                            CourseName = "MATH 102",
                             SemesterName = "Fall 2010"
                          },
                                new Course { ID = 3,
                            CourseName = "ENG 103",
                            SemesterName = "Winter 2011"
                          },
                                new Course { ID = 4,
                            CourseName = "EE 104",
                            SemesterName = "Fall 2012"
                          }
                            }
                },
                new Student
                { ID = 2, FirstName = "Jorge", LastName = "Garcia",
                            Courses = new List<Course>()
                            {
                                new Course { ID = 5,
                            CourseName = "CS 205",
                            SemesterName = "Winter 2010"
                          },
                                new Course { ID = 6,
                            CourseName = "MATH 206",
                            SemesterName = "Fall 2010"
                          },
                                new Course { ID = 7,
                            CourseName = "ENG 207",
                            SemesterName = "Winter 2011"
                          },
                                new Course { ID = 8,
                            CourseName = "EE 208",
                            SemesterName = "Fall 2012"
                          }
                            }
                },
                new Student
                { ID = 3, FirstName = "Gorge", LastName = "Klene",
                            Courses = new List<Course>()
                            {
                                new Course { ID = 9,
                            CourseName = "CS 301",
                            SemesterName = "Winter 2010"
                          },
                                new Course { ID = 10,
                            CourseName = "MATH 302",
                            SemesterName = "Fall 2010"
                          },
                                new Course { ID = 11,
                            CourseName = "ENG 303",
                            SemesterName = "Winter 2011"
                          },
                                new Course { ID = 12,
                            CourseName = "EE 304",
                            SemesterName = "Fall 2012"
                          }
                            }
                }
};



        public ActionResult Index()
        {

            return View(Students);
        }
    }
}