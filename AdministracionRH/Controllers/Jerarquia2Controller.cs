using AdministracionRH.Common;
using AdministracionRH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministracionRH.Controllers
{
    public class Jerarquia2Controller : Controller
    {
        // GET: Jerarquia2
        public ActionResult Index()
        {
            EmployeeSearchModel model = new EmployeeSearchModel();
            model.LstRangoFechas = new List<DateTime>
            {
                 //new DateTime(2019, 2, 4),
                 //new DateTime(2019, 2, 5),
                 //new DateTime(2019, 2, 6),
                 //new DateTime(2019, 2, 7),
                 //new DateTime(2019, 2, 8),
                 new DateTime(2019, 2, 11),
                 new DateTime(2019, 2, 12),
                 new DateTime(2019, 2, 13),
                 new DateTime(2019, 2, 14),
                 new DateTime(2019, 2, 15)
            };
            model.LstemployeeAsistencias = new List<EmployeeAsistencia>
            {
                new EmployeeAsistencia
                { EmployeeId =26
                ,Nombre="GERARDO ESTEBAN FLORES CASTRO"
                ,boolSelected =false
               // ,BossId =364
                ,LstDetalleAsistencia= new List<Asistencia> {  new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="08:31:49",Salida ="18:05:14"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="08:31:49",Salida ="18:05:14"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="08:03:20",Salida ="18:18:47"},

                }

                 },
                
                  new EmployeeAsistencia
                { EmployeeId =48
                ,Nombre="BRAIN DORANTES"
                ,boolSelected =false
                ,LstDetalleAsistencia= new List<Asistencia> {  new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada ="09:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="09:31:49",Salida ="18:05:14"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="09:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="09:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="09:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="08:31:49",Salida ="18:05:14"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="08:03:20",Salida ="18:18:47"},
                }

                 },
                   new EmployeeAsistencia
                { EmployeeId =364
                ,Nombre="ANDRES MEDINA AGUILAR"
                ,boolSelected =false
                ,LstDetalleAsistencia= new List<Asistencia> { new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada =" 10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="10:31:49",Salida ="18:05:14"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="08:31:49",Salida ="18:05:14"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="08:03:20",Salida ="18:18:47"},
                }

                 }
                   ,
                               new EmployeeAsistencia
                { EmployeeId =428
                ,Nombre="CARRERA DAVILA SERGIO"
                ,boolSelected =false
                ,LstDetalleAsistencia= new List<Asistencia> { new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada =" 10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="10:31:49",Salida ="18:05:14"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="08:31:49",Salida ="18:05:14"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="08:03:20",Salida ="18:18:47"},
                }

                 },            new EmployeeAsistencia
                { EmployeeId =219
                ,Nombre="DEL RIO HERNANDEZ EDUARDO"
                ,boolSelected =false
                ,LstDetalleAsistencia= new List<Asistencia> { new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada =" 10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="10:31:49",Salida ="18:05:14"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="08:31:49",Salida ="18:05:14"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="08:03:20",Salida ="18:18:47"},
                }

                 },            new EmployeeAsistencia
                { EmployeeId =118
                ,Nombre="PEREZ ROMERO LUIS EMMANUEL"
                ,boolSelected =false
                ,LstDetalleAsistencia= new List<Asistencia> { new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada =" 10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="10:31:49",Salida ="18:05:14"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="08:31:49",Salida ="18:05:14"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="08:03:20",Salida ="18:18:47"},
                }

                 },
                new EmployeeAsistencia
                { EmployeeId =82
                ,Nombre="NERIA PEÑA JIMENA"
                ,boolSelected =false
                ,LstDetalleAsistencia= new List<Asistencia> { new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada =" 10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="10:31:49",Salida ="18:05:14"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="08:31:49",Salida ="18:05:14"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="08:03:20",Salida ="18:18:47"},
                }

                 },
                 new EmployeeAsistencia
                { EmployeeId =64
                ,Nombre="ALEGRIA MONREAL ARELI"
                ,boolSelected =false
                ,LstDetalleAsistencia= new List<Asistencia> { new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada =" 10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="10:31:49",Salida ="18:05:14"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="08:31:49",Salida ="18:05:14"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="08:03:20",Salida ="18:18:47"},
                }

                 }
                 ,
                  new EmployeeAsistencia
                { EmployeeId =1
                ,Nombre="MORENO SANTANA DULCE MARIA"
                ,boolSelected =false
                ,LstDetalleAsistencia= new List<Asistencia> { new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada =" 10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="10:31:49",Salida ="18:05:14"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="10:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,11),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,12),Entrada ="08:31:49",Salida ="18:05:14"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,13),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,14),Entrada ="08:03:20",Salida ="18:18:47"},
                                                               //new Asistencia { EmployeeId =26,Fecha = new DateTime(2019,2,15),Entrada ="08:03:20",Salida ="18:18:47"},
                }

                 }






            };

            List<MenuItem> allMenu = getData();
            //List<MenuItem> LstSelected = allMenu.Where(i => i.Id ==26 || i.Id == 48).ToList();
            List<MenuItem> LstSelected = allMenu.Where(i => i.Id == 364 || i.Id == 219).ToList();
            List<MenuItem> mi = allMenu
            .Where(e => LstSelected.Any(p2 => p2.ParentId == e.ParentId && e.Id == p2.Id)) /* grab only the root parent nodes */
            .Select(e => new MenuItem
            {
                Id = e.Id,
                Name = e.Name,
                ParentId = e.ParentId,
                Children = GetChildren(allMenu, e.Id) /* Recursively grab the children */
            }).ToList();
            ViewBag.menusList = mi;


            return View(model);
        }
        [HttpPost]
        public ActionResult Index(EmployeeSearchModel model)
        {
            List<MenuItem> allMenu = getData();
            List<MenuItem> LstSelected = allMenu.Where(i => i.Name.ToUpper().Contains(model.NombreEmpleado)).ToList();
            List<MenuItem> mi = allMenu
            .Where(e => LstSelected.Any(p2 => p2.ParentId == e.ParentId && e.Id == p2.Id)) /* grab only the root parent nodes */
            .Select(e => new MenuItem
            {
                Id = e.Id,
                Name = e.Name,
                ParentId = e.ParentId,
                Children = GetChildren(allMenu, e.Id) /* Recursively grab the children */
            }).ToList();
            ViewBag.menusList = mi;

            return View(model);
        }
        /// <summary>
        /// Recursively grabs the children from the list of items for the provided parentId
        /// </summary>
        /// <param name="items">List of all items</param>
        /// <param name="parentId">Id of parent item</param>
        /// <returns>List of children of parentId</returns>
        private static List<MenuItem> GetChildren(List<MenuItem> items, int parentId)
        {
            {
                return items
                .Where(x => x.ParentId == parentId)
                .Select(e => new MenuItem
                {
                    Id = e.Id,
                    Name = e.Name,
                    ParentId = e.ParentId,
                    Children = GetChildren(items, e.Id)
                }).ToList();

            }
               
        }
        private List<MenuItem> getData()
        {
            List<MenuItem> allMenu1 = new List<MenuItem>
            {
                    new MenuItem {boolSelectd=false,Id=364,Name="MEDINA AGUILAR ANDRES", ParentId=428},
                    new MenuItem {boolSelectd=false,Id=48,Name="DORANTES ARGÜELLO BRIAN", ParentId=364},
                    new MenuItem {boolSelectd=false,Id=26,Name="FLORES CASTRO GERARDO ESTEBAN", ParentId=364},
                    new MenuItem {boolSelectd=false,Id=219,Name="DEL RIO HERNANDEZ EDUARDO", ParentId=428},
                    new MenuItem {boolSelectd=false,Id=428,Name="CARRERA DAVILA SERGIO", ParentId=423},
                    new MenuItem {boolSelectd=false,Id=64,Name="ALEGRIA MONREAL ARELI", ParentId=219},
                    new MenuItem {boolSelectd=false,Id=1,Name="MORENO SANTANA DULCE MARIA", ParentId=64},
                    new MenuItem {boolSelectd=false,Id=82,Name="NERIA PEÑA JIMENA", ParentId=1},
                    new MenuItem {boolSelectd=false,Id=118,Name="PEREZ ROMERO LUIS EMMANUEL", ParentId=1},
            };


            List <MenuItem> allMenu = new List<MenuItem>
                        {
                            new MenuItem {boolSelectd=false,Id=6,Name="HERNANDEZ AMATITLA DANIEL", ParentId=0},
                            new MenuItem {boolSelectd=false,Id=111,Name="AGUILAR MORALES JULIO CESAR", ParentId=0},
                            new MenuItem {boolSelectd=false,Id=150,Name="AVILES OROZCO BRAULIO MARIN", ParentId=0},
                            new MenuItem {boolSelectd=false,Id=162,Name="BARRON FRANCO HECTOR", ParentId=0},
                            new MenuItem {boolSelectd=false,Id=238,Name="LOPEZ OROZCO FRANCISCO MARTIN", ParentId=0},
                            new MenuItem {boolSelectd=false,Id=322,Name="SALGADO FERNANDEZ SERGIO", ParentId=0},
                            new MenuItem {boolSelectd=false,Id=331,Name="MARTINEZ AURIOLES HUGO PASTOR", ParentId=0},
                            new MenuItem {boolSelectd=false,Id=405,Name="PIÑA MONROY JAQUELINE", ParentId=0},
                            new MenuItem {boolSelectd=false,Id=423,Name="DAVILA BARBERENA ANTONIO MANUEL", ParentId=0},
                            new MenuItem {boolSelectd=false,Id=429,Name="DAVILA URIBE ANTONIO", ParentId=0},
                            new MenuItem {boolSelectd=false,Id=428,Name="CARRERA DAVILA SERGIO", ParentId=423},
                            new MenuItem {boolSelectd=false,Id=431,Name="RENDON OBERHAUSER JOSE MANUEL", ParentId=423},
                            new MenuItem {boolSelectd=false,Id=16,Name="AVILA MORALES FRANCISCO JAVIER", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=19,Name="ALMAGUER RIVERA MIGUEL ANGEL", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=38,Name="MENDOZA JUAREZ JOSUE WEBSTER", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=62,Name="VEGA HERNANDEZ BEATRIZ", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=192,Name="DAVILA BECERRIL JOSE LUIS", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=219,Name="DEL RIO HERNANDEZ EDUARDO", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=233,Name="REYES OSORNIO ANA MARIA", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=325,Name="MARTINEZ VILLEGAS BRITANY VANESA", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=334,Name="REBOLLAR JIMENEZ ANGELINA", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=388,Name="OTEO BARREIRA CRISTINA CONCEPCION", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=422,Name="MARTINEZ IVANOV IVAN", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=424,Name="GARRIDO CORDERO MANUEL", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=425,Name="FERNANDEZ RUBIO JOSE MANUEL", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=426,Name="LOPEZ SAINZ MIGUEL ANGEL", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=430,Name="PEREZ DAVILA FERNANDO", ParentId=428},
                            new MenuItem {boolSelectd=false,Id=387,Name="CRISPIN XOLALPA RUBEN DARIO", ParentId=430},
                            new MenuItem {boolSelectd=false,Id=7,Name="LOPEZ MARTINEZ MARCO VINICIO", ParentId=426},
                            new MenuItem {boolSelectd=false,Id=13,Name="BARRANCO TEJE CESAR", ParentId=426},
                            new MenuItem {boolSelectd=false,Id=20,Name="VALDIVIA ORTIZ SUSANA JAQUELINE", ParentId=426},
                            new MenuItem {boolSelectd=false,Id=53,Name="LUNA GONZALEZ LUIS ALBERTO", ParentId=426},
                            new MenuItem {boolSelectd=false,Id=120,Name="AGUILAR ARIZAGA HECTOR", ParentId=426},
                            new MenuItem {boolSelectd=false,Id=360,Name="ATILANO LEAL JOSE ALBERTO", ParentId=426},
                            new MenuItem {boolSelectd=false,Id=382,Name="PEREZ MORALES ULISES ALEJANDRO", ParentId=426},
                            new MenuItem {boolSelectd=false,Id=17,Name="JIMENEZ TORRES IVAN CHRISTOPHER", ParentId=425},
                            new MenuItem {boolSelectd=false,Id=21,Name="OCHOA JAIMES HUGO", ParentId=425},
                            new MenuItem {boolSelectd=false,Id=23,Name="GALICIA MORALES EDGAR", ParentId=425},
                            new MenuItem {boolSelectd=false,Id=168,Name="DUARTE LEONIDES VERONICA", ParentId=425},
                            new MenuItem {boolSelectd=false,Id=18,Name="ANAYA LUNA GABRIEL IVAN", ParentId=424},
                            new MenuItem {boolSelectd=false,Id=14,Name="TORRES SALAS GABRIELA", ParentId=422},
                            new MenuItem {boolSelectd=false,Id=51,Name="LOPEZ CASILLAS REBECA OFELIA", ParentId=422},
                            new MenuItem {boolSelectd=false,Id=364,Name="MEDINA AGUILAR ANDRES", ParentId=422},
                            new MenuItem {boolSelectd=false,Id=40,Name="AQUINO CARMONA CARLOS", ParentId=219},
                            new MenuItem {boolSelectd=false,Id=43,Name="AVILA PEREZ KARINA", ParentId=219},
                            new MenuItem {boolSelectd=false,Id=45,Name="SANCHEZ REGLA MARIA ANTONIETA", ParentId=219},
                            new MenuItem {boolSelectd=false,Id=64,Name="ALEGRIA MONREAL ARELI", ParentId=219},
                            new MenuItem {boolSelectd=false,Id=80,Name="MONTOYA RIOS PABLO ANGEL", ParentId=219},
                            new MenuItem {boolSelectd=false,Id=337,Name="PIMENTEL LORIA GENARO", ParentId=219},
                            new MenuItem {boolSelectd=false,Id=143,Name="MARTINEZ GONZALEZ JULIO CESAR", ParentId=192},
                            new MenuItem {boolSelectd=false,Id=202,Name="VELAZQUEZ PEREZ DIANA KARINA", ParentId=192},
                            new MenuItem {boolSelectd=false,Id=253,Name="RAMIREZ RAMIREZ VICTOR JOSE", ParentId=192},
                            new MenuItem {boolSelectd=false,Id=318,Name="MORALES MARTINEZ JEOVANI SALVADOR", ParentId=192},
                            new MenuItem {boolSelectd=false,Id=44,Name="MEJIA ZAMITIZ RUTH EVELYN", ParentId=38},
                            new MenuItem {boolSelectd=false,Id=148,Name="CASTRO DE JESUS GUILLERMO", ParentId=38},
                            new MenuItem {boolSelectd=false,Id=295,Name="VALLEJO CARDONA HIRAM", ParentId=38},
                            new MenuItem {boolSelectd=false,Id=25,Name="SANCHEZ GOMEZ MABEL", ParentId=19},
                            new MenuItem {boolSelectd=false,Id=247,Name="OSORIO CAMPUZANO JOSE ARMANDO", ParentId=16},
                            new MenuItem {boolSelectd=false,Id=235,Name="MORALES MENDEZ JAVIER", ParentId=247},
                            new MenuItem {boolSelectd=false,Id=371,Name="PALMA AGUILAR DIANA KARIN", ParentId=247},
                            new MenuItem {boolSelectd=false,Id=482,Name="MAYOLA SOTO HERNANDEZ ", ParentId=13},
                            new MenuItem {boolSelectd=false,Id=108,Name="MIRANDA SEVILLA JULIO CESAR", ParentId=25},
                            new MenuItem {boolSelectd=false,Id=114,Name="TENORIO VIGNATI HECTOR MIGUEL", ParentId=25},
                            new MenuItem {boolSelectd=false,Id=182,Name="RAMOS NORIEGA IVONNE", ParentId=25},
                            new MenuItem {boolSelectd=false,Id=361,Name="PINEDA CELIS MILAGROS MARGARITA", ParentId=25},
                            new MenuItem {boolSelectd=false,Id=500,Name="PALMA GISELE CARLA", ParentId=25},
                            new MenuItem {boolSelectd=false,Id=10,Name="RUIZ JESUS", ParentId=143},
                            new MenuItem {boolSelectd=false,Id=144,Name="PEREZ AVELAR JOSE PABLO", ParentId=143},
                            new MenuItem {boolSelectd=false,Id=205,Name="NAJERA MARQUEZ ADAN ALBERTO", ParentId=143},
                            new MenuItem {boolSelectd=false,Id=211,Name="GONZALEZ OLIVARES FANNY", ParentId=143},
                            new MenuItem {boolSelectd=false,Id=226,Name="ROMERO GUZMAN ABRAHAM ROBERTO", ParentId=143},
                            new MenuItem {boolSelectd=false,Id=60,Name="SANTANA CABRERA TRINIDAD", ParentId=51},
                            new MenuItem {boolSelectd=false,Id=126,Name="LANDEROS JUAREZ DAVID RICARDO", ParentId=51},
                            new MenuItem {boolSelectd=false,Id=359,Name="AVALOS BALDERAS MARTIN", ParentId=51},
                            new MenuItem {boolSelectd=false,Id=146,Name="BUENTELLO SANCHEZ CLAUDIA GUILLERMINA", ParentId=14},
                            new MenuItem {boolSelectd=false,Id=248,Name="PIOQUINTO SEGURA FRANCISCO EDUARDO", ParentId=45},
                            new MenuItem {boolSelectd=false,Id=502,Name="RUIZ VALENZUELA MARIA DE LOS ANGELES", ParentId=45},
                            new MenuItem {boolSelectd=false,Id=154,Name="DORADO SANCHEZ ISRAEL", ParentId=43},
                            new MenuItem {boolSelectd=false,Id=417,Name="SANTIAGO HUERTA NICOLAS", ParentId=43},
                            new MenuItem {boolSelectd=false,Id=475,Name="CINTYA ROMERO GAONA ESTHER", ParentId=43},
                            new MenuItem {boolSelectd=false,Id=477,Name="CINTHIA MARTINEZ CRUZ VERENICE", ParentId=43},
                            new MenuItem {boolSelectd=false,Id=11,Name="VELAZQUEZ MORENO ROSA MARIA", ParentId=80},
                            new MenuItem {boolSelectd=false,Id=310,Name="PERALES ESCOBEDO JESUS JAVIER", ParentId=80},
                            new MenuItem {boolSelectd=false,Id=357,Name="VALDEZ POLANCO FERNANDO ISAAC", ParentId=80},
                            new MenuItem {boolSelectd=false,Id=439,Name="VELAZQUEZ MORENO ROSA MARIA", ParentId=80},
                            new MenuItem {boolSelectd=false,Id=473,Name="PAMELA MARIN SANCHEZ  ESTEFANIA", ParentId=80},
                            new MenuItem {boolSelectd=false,Id=1,Name="MORENO SANTANA DULCE MARIA", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=41,Name="OCOTE RAMIREZ LIDIA OLIVIA", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=54,Name="MURRIETA RAMIREZ ANTONIO DE JESUS", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=73,Name="JIMENEZ ALVAREZ BERENICE GUADALUPE", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=134,Name="GONZALEZ MARTINEZ LUIS SALVADOR", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=151,Name="GARCIA CABRERA DULCE NAYELI", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=155,Name="OLASCOAGA VEGA SHARON", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=157,Name="CENDEJAS RAYA JORGE LUIS", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=305,Name="ARELLANES QUITERIO IDALIA YANET", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=328,Name="LOPEZ ORTIZ ESPERANZA", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=356,Name="QUINTANA MANJARREZ CRISTINA PAOLA", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=363,Name="RAMIREZ TZONTECOMANI JAZMIN SELENE", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=368,Name="VAZQUEZ GARCIA EDGAR PAULINO", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=384,Name="GALICIA RAMIREZ REYNA NATHALY", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=456,Name="ALMA DELIA PAZ MARTINEZ ", ParentId=64},
                            new MenuItem {boolSelectd=false,Id=26,Name="FLORES CASTRO GERARDO ESTEBAN", ParentId=364},
                            new MenuItem {boolSelectd=false,Id=37,Name="ROCHA REYES SILVIA EURIDICE", ParentId=364},
                            new MenuItem {boolSelectd=false,Id=48,Name="DORANTES ARGÜELLO BRIAN", ParentId=364},
                            new MenuItem {boolSelectd=false,Id=59,Name="RUEDA SANTOS VICTOR ADOLFO", ParentId=364},
                            new MenuItem {boolSelectd=false,Id=448,Name="GARDUÑO RAMIREZ NORBERO", ParentId=364},
                            new MenuItem {boolSelectd=false,Id=479,Name="DIEGO YOUSHIMATZ NAVARRO KOYSHI", ParentId=364},
                            new MenuItem {boolSelectd=false,Id=34,Name="GARCIA PEREZ FRANS", ParentId=18},
                            new MenuItem {boolSelectd=false,Id=35,Name="VILLEDA CASTILLO AZUCENA", ParentId=18},
                            new MenuItem {boolSelectd=false,Id=47,Name="BENITEZ ARELLANO NAYELI SINAI", ParentId=18},
                            new MenuItem {boolSelectd=false,Id=29,Name="RAMOS CRUZ VICTOR MARTIN", ParentId=23},
                            new MenuItem {boolSelectd=false,Id=33,Name="HIDALGO CHAVEZ JOSE MARTIN", ParentId=23},
                            new MenuItem {boolSelectd=false,Id=250,Name="MONTAÑO ANAYA JOSE LUIS", ParentId=23},
                            new MenuItem {boolSelectd=false,Id=121,Name="ALARCON CHAVEZ DANIEL", ParentId=21},
                            new MenuItem {boolSelectd=false,Id=324,Name="VALENZUELA CAMACHO SARA LUZ", ParentId=17},
                            new MenuItem {boolSelectd=false,Id=397,Name="JIMENEZ ESPINO LESLY NOEMI", ParentId=360},
                            new MenuItem {boolSelectd=false,Id=149,Name="VELASCO RUIZ GUADALUPE", ParentId=120},
                            new MenuItem {boolSelectd=false,Id=332,Name="LANDERO CRUZ EDI", ParentId=120},
                            new MenuItem {boolSelectd=false,Id=338,Name="ABANERO JIMENEZ GICELA", ParentId=120},
                            new MenuItem {boolSelectd=false,Id=222,Name="CESAR GUERRA JAIME ROBERTO", ParentId=20},
                            new MenuItem {boolSelectd=false,Id=317,Name="GONZALEZ VENEGAS MARISOL", ParentId=20},
                            new MenuItem {boolSelectd=false,Id=28,Name="ESPINDOLA VAZQUEZ KARINA", ParentId=387},
                            new MenuItem {boolSelectd=false,Id=113,Name="MIRANDA SEVILLA LUCERO", ParentId=387},
                            new MenuItem {boolSelectd=false,Id=141,Name="JUAREZ PEÑA ELIZABETH", ParentId=387},
                            new MenuItem {boolSelectd=false,Id=234,Name="GIL OSORIO OLGA EDITH", ParentId=387},
                            new MenuItem {boolSelectd=false,Id=354,Name="SANCHEZ URIBE FRANCISCO JAVIER", ParentId=387},
                            new MenuItem {boolSelectd=false,Id=373,Name="ORTIZ GONZALEZ MIRIAM ARACELI", ParentId=387},
                            new MenuItem {boolSelectd=false,Id=418,Name="CRUZ MARTINEZ LUIS ANTONIO", ParentId=387},
                            new MenuItem {boolSelectd=false,Id=117,Name="GARCIA SANTES PILAR", ParentId=317},
                            new MenuItem {boolSelectd=false,Id=193,Name="BLANDO TORRES DANIELA", ParentId=47},
                            new MenuItem {boolSelectd=false,Id=116,Name="MEDINA TORRES GABRIEL", ParentId=35},
                            new MenuItem {boolSelectd=false,Id=55,Name="PEÑA ZARATE RAYMUNDO", ParentId=34},
                            new MenuItem {boolSelectd=false,Id=107,Name="TINOCO GARCIA CARLOS VICENTE", ParentId=34},
                            new MenuItem {boolSelectd=false,Id=189,Name="JAIME GARDUÑO JEREMIAS", ParentId=34},
                            new MenuItem {boolSelectd=false,Id=241,Name="DUFFLART HERNANDEZ JHEANNARY PAOLA", ParentId=37},
                            new MenuItem {boolSelectd=false,Id=115,Name="ORTEGA SANTOS ESMERALDA", ParentId=368},
                            new MenuItem {boolSelectd=false,Id=127,Name="GONZALEZ HERRERA RAFAEL", ParentId=368},
                            new MenuItem {boolSelectd=false,Id=132,Name="BARRIGA MENDOZA GISELE", ParentId=368},
                            new MenuItem {boolSelectd=false,Id=179,Name="VAZQUEZ MARTINEZ VIRGINIA", ParentId=368},
                            new MenuItem {boolSelectd=false,Id=259,Name="PALMA BERNAL ALFONSO BETHUEL", ParentId=368},
                            new MenuItem {boolSelectd=false,Id=401,Name="GONZALEZ DE GABRIEL DANIA ITZEL", ParentId=368},
                            new MenuItem {boolSelectd=false,Id=402,Name="ISLAS CONDE MARLENE", ParentId=368},
                            new MenuItem {boolSelectd=false,Id=69,Name="GARCIA BUENDIA MARIA ALEJANDRA", ParentId=363},
                            new MenuItem {boolSelectd=false,Id=216,Name="PEREZ PEREZ AURORA", ParentId=363},
                            new MenuItem {boolSelectd=false,Id=242,Name="SIERRA ALBARRAN VERONICA", ParentId=363},
                            new MenuItem {boolSelectd=false,Id=261,Name="PALACIOS RIOS JOSE LUIS", ParentId=363},
                            new MenuItem {boolSelectd=false,Id=290,Name="MIGUEL ACEVEDO BEIBY KATHY", ParentId=363},
                            new MenuItem {boolSelectd=false,Id=327,Name="GOMEZ GODOY NORMA KARINA", ParentId=363},
                            new MenuItem {boolSelectd=false,Id=343,Name="GOMEZ SALGADO BEATRIZ", ParentId=363},
                            new MenuItem {boolSelectd=false,Id=375,Name="HERNANDEZ RAYGOZA PAOLA THALIA", ParentId=363},
                            new MenuItem {boolSelectd=false,Id=376,Name="ROJAS OCAÑA ALEJANDRA", ParentId=363},
                            new MenuItem {boolSelectd=false,Id=406,Name="RODRIGUEZ HUITZITL MARTHA KAREN", ParentId=363},
                            new MenuItem {boolSelectd=false,Id=407,Name="SALAZAR MORALES ELSA LIBIER", ParentId=363},
                            new MenuItem {boolSelectd=false,Id=125,Name="SANCHEZ RAMOS VERONICA", ParentId=356},
                            new MenuItem {boolSelectd=false,Id=130,Name="MONTAÑO CASTRO MANUEL", ParentId=356},
                            new MenuItem {boolSelectd=false,Id=217,Name="OLIVERA FABIAN ALEXANDER UBALDO", ParentId=356},
                            new MenuItem {boolSelectd=false,Id=236,Name="VARGAS NAJERA DALIA", ParentId=356},
                            new MenuItem {boolSelectd=false,Id=270,Name="HERNANDEZ FLORES BIANETH ELISA", ParentId=356},
                            new MenuItem {boolSelectd=false,Id=355,Name="ESPINOZA NIEVES DAYANIRA YVETTE", ParentId=356},
                            new MenuItem {boolSelectd=false,Id=381,Name="MARTINEZ RODRIGUEZ MARIO AURELIO", ParentId=356},
                            new MenuItem {boolSelectd=false,Id=413,Name="CASTILLO GRANADOS JESSICA", ParentId=356},
                            new MenuItem {boolSelectd=false,Id=66,Name="BELTRAN ROMERO YADIRA", ParentId=328},
                            new MenuItem {boolSelectd=false,Id=74,Name="JIMENEZ LOPEZ CARLOS ALBERTO", ParentId=328},
                            new MenuItem {boolSelectd=false,Id=81,Name="MUNGUIA ALVAREZ DENISE", ParentId=328},
                            new MenuItem {boolSelectd=false,Id=119,Name="MEDINA DIAZ GABRIELA", ParentId=328},
                            new MenuItem {boolSelectd=false,Id=181,Name="LUCIANO OROZCO MARGARITA BERENICE", ParentId=328},
                            new MenuItem {boolSelectd=false,Id=306,Name="LORENZANA MARTINEZ LUIS FERNANDO", ParentId=328},
                            new MenuItem {boolSelectd=false,Id=321,Name="RODRIGUEZ JUAREZ MANUEL", ParentId=328},
                            new MenuItem {boolSelectd=false,Id=367,Name="LOPEZ CIGARROA NAYELI", ParentId=328},
                            new MenuItem {boolSelectd=false,Id=408,Name="RODRIGUEZ LAZARO BLANCA BRENDA", ParentId=328},
                            new MenuItem {boolSelectd=false,Id=409,Name="PEÑA CRUZ JOSELYN GUADALUPE", ParentId=328},
                            new MenuItem {boolSelectd=false,Id=75,Name="LEON DE SANTIAGO KARINA", ParentId=155},
                            new MenuItem {boolSelectd=false,Id=172,Name="LOZADA FLORES EDNA PRISCYLA", ParentId=155},
                            new MenuItem {boolSelectd=false,Id=190,Name="GAYTÁN CHACÓN RENÉ", ParentId=155},
                            new MenuItem {boolSelectd=false,Id=390,Name="CERVANTES OROZCO ANTONIO ARMANDO", ParentId=155},
                            new MenuItem {boolSelectd=false,Id=491,Name="OROZCO VALDEZ FERNANDA", ParentId=155},
                            new MenuItem {boolSelectd=false,Id=380,Name="IBAÑEZ CORTEZ MARCO ANTONIO", ParentId=134},
                            new MenuItem {boolSelectd=false,Id=68,Name="GALVEZ ORTIZ MARIA FERNANDA", ParentId=73},
                            new MenuItem {boolSelectd=false,Id=90,Name="RUIZ TREJO LAURA ELENA", ParentId=73},
                            new MenuItem {boolSelectd=false,Id=170,Name="TORRES NAVARRETE VIANEY DASMIN", ParentId=73},
                            new MenuItem {boolSelectd=false,Id=243,Name="QUINTERO RAMIREZ ANA YAZMIN", ParentId=73},
                            new MenuItem {boolSelectd=false,Id=266,Name="NERI SALAZAR JUAN FRANCISCO", ParentId=73},
                            new MenuItem {boolSelectd=false,Id=307,Name="QUINTANA BELLO CLAUDIA", ParentId=73},
                            new MenuItem {boolSelectd=false,Id=341,Name="REYES COLIN JUAN PABLO", ParentId=73},
                            new MenuItem {boolSelectd=false,Id=345,Name="HERNANDEZ ESQUIVEL ERICK ISRAEL", ParentId=73},
                            new MenuItem {boolSelectd=false,Id=383,Name="ZUÑIGA RAMIREZ ANAI ARIANA", ParentId=73},
                            new MenuItem {boolSelectd=false,Id=399,Name="CAMPOS AVILA LUCIA", ParentId=73},
                            new MenuItem {boolSelectd=false,Id=400,Name="CHAVEZ CASTILLO JESSICA AMAIRANI", ParentId=73},
                            new MenuItem {boolSelectd=false,Id=440,Name="QUINTO VEGA ANNA LAURA", ParentId=73},
                            new MenuItem {boolSelectd=false,Id=340,Name="MARTINEZ FLORES LIZETTE", ParentId=41},
                            new MenuItem {boolSelectd=false,Id=362,Name="CEDEÑO MONDRAGON LEONARDO", ParentId=41},
                            new MenuItem {boolSelectd=false,Id=372,Name="TIRADO DEL PINO BERENICE", ParentId=41},
                            new MenuItem {boolSelectd=false,Id=442,Name="PINEDA ORTEGA KEVIN", ParentId=41},
                            new MenuItem {boolSelectd=false,Id=488,Name="VILLEGAS CHAVARRIA LIZBETH", ParentId=41},
                            new MenuItem {boolSelectd=false,Id=490,Name="BARRIENTOS REYNA MARIANA GUADALUPE", ParentId=41},
                            new MenuItem {boolSelectd=false,Id=82,Name="NERIA PEÑA JIMENA", ParentId=1},
                            new MenuItem {boolSelectd=false,Id=118,Name="PEREZ ROMERO LUIS EMMANUEL", ParentId=1},
                            new MenuItem {boolSelectd=false,Id=133,Name="GARCIA ALARCON PABLO", ParentId=1},
                            new MenuItem {boolSelectd=false,Id=302,Name="BAUTISTA RODRIGUEZ SANDRA LUZ", ParentId=1},
                            new MenuItem {boolSelectd=false,Id=309,Name="MENDOZA CARRERA CLAUDIA IVETTE", ParentId=1},
                            new MenuItem {boolSelectd=false,Id=398,Name="ALMANZA GONZALEZ MARLEN", ParentId=1},
                            new MenuItem {boolSelectd=false,Id=411,Name="PLATA GUERRERO LIZBETH", ParentId=1},
                            new MenuItem {boolSelectd=false,Id=414,Name="SANCHEZ  GARCIA  EDITH", ParentId=1},
                            new MenuItem {boolSelectd=false,Id=449,Name="GARCIA SANCHEZ KARLA GUADALUPE", ParentId=1},
                            new MenuItem {boolSelectd=false,Id=458,Name="RAMIREZ CRUZ VICTOR", ParentId=1},
                        };
            return allMenu1;

        }
    }
}