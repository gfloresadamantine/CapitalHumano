using AdministracionRH.Common;
using AdministracionRH.Models;
using AdministracionRH.Repositories;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AdministracionRH.Services
{
    public class EmployeeValidadorServices
    {
        EmployeeValidadorRepository _employeeValidadorRepository;

        EmployeeValidadorModel _model { get; set; }
        public string _NoEmpleado { get; set; }
        public Result result { get; set; }
        // private User _user { get; set; }

        public Enumeraciones.enumRoles Rol { get; set; }
        public EmployeeValidadorServices()
        {
            _employeeValidadorRepository = new EmployeeValidadorRepository();
        }

        public EmployeeValidadorServices(EmployeeValidadorModel model)
        {
            _model = new EmployeeValidadorModel();
            _employeeValidadorRepository = new EmployeeValidadorRepository();
           _model = model;
        }

        public EmployeeValidadorModel GetDataByFilter()
        {
            List<EmployeeValidador> lista = null;
            result = new Result();
            try
            {
                if (Rol == Enumeraciones.enumRoles.Empleado)
                {
                    _model.NoEmpleado = _NoEmpleado;
                    result = _employeeValidadorRepository.GetAllEmployessValidador(out lista, _model.Nombre, _model.NoEmpleado, _model.Rfc, _model.Nss);
                }
                else
                {
                    result = _employeeValidadorRepository.GetAllEmployessValidador(out lista, _model.Nombre, _model.NoEmpleado, _model.Rfc, _model.Nss);
                }
                
                _model.LstEmployeesValidador = lista;

                if (lista != null)
                {
                    _model.RecordCount = lista.Count();
                    var query = OrdenaDatos(lista);
                    _model.PageCount = query.Count() == 0 ? 1 : ((query.Count() - 1) / _model.PageSize) + 1;
                    _model.LstEmployeesValidadorPaged = query.Skip((_model.CurrentPageIndex - 1) * _model.PageSize).Take(_model.PageSize).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return _model;
        }

        private IEnumerable<EmployeeValidador> OrdenaDatos(List<EmployeeValidador> lista)
        {

            IQueryable<EmployeeValidador> query = null;
            query = lista.AsQueryable();
            switch (_model.SortField)
            {
                case "FirstName":
                    query = (_model.SortDirection == "ascending" ? query.OrderBy(c => c.FirstName).AsQueryable() : query.OrderByDescending(c => c.FirstName).AsQueryable());
                    break;
                case "LastName":
                    query = (_model.SortDirection == "ascending" ? query.OrderBy(c => c.LastName).AsQueryable() : query.OrderByDescending(c => c.LastName).AsQueryable());
                    break;

            }
            return query;
        }

        public Result UpdateEmployee(EmployeeValidador employeeValidador)
        {
           return  _employeeValidadorRepository.UpdateEmployee(employeeValidador);

        }

        public string  ExportarExcel()
        {
            string FilePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Download"), "Empleados.xlsx");
            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("Empleados");
                worksheet.Cell("A1").Value = "ID Adamantine";
                worksheet.Cell("B1").Value = "Nombre";
                worksheet.Cell("C1").Value = "Apellido Paterno";
                worksheet.Cell("D1").Value = "Apellido Materno";
                worksheet.Cell("E1").Value = "(Apellido Paterno, Materno, Nombre(s)";
                worksheet.Cell("F1").Value = "RFC: (Alfabético; Numérico, Homoclave)";
                worksheet.Cell("G1").Value = "CURP";
                worksheet.Cell("H1").Value = "No. Seguro Social";
                worksheet.Cell("I1").Value = "Estado Civil";
                worksheet.Cell("J1").Value = "Sexo";
                worksheet.Cell("K1").Value = "Fecha de nacimiento (Día/Mes/Año)";
                worksheet.Cell("L1").Value = "Calle";
                worksheet.Cell("M1").Value = "No. Exterior";
                worksheet.Cell("N1").Value = "No. Interior";
                worksheet.Cell("O1").Value = "Colonia";
                worksheet.Cell("P1").Value = "Delegación o Municipio";
                worksheet.Cell("Q1").Value = "Estado";
                worksheet.Cell("R1").Value = "Código Postal";
                worksheet.Cell("S1").Value = "Teléfono (particular)";
                worksheet.Cell("T1").Value = "Teléfono (Móvil)";
                worksheet.Cell("U1").Value = "Correo electrónico (Personal)";
                worksheet.Cell("V1").Value = "Lugar de Nacimiento";
                worksheet.Cell("W1").Value = "Clabe interbancaria";
                worksheet.Cell("X1").Value = "Banco";
                worksheet.Cell("Y1").Value = "Contrato Infonavit";
                worksheet.Cell("Z1").Value = "Trámite Infonavit (Sí-No)";
                worksheet.Cell("AA1").Value = "Etapa";
                worksheet.Cell("AB1").Value = "Estudios (Nombre de la carrera o Bachillerato)";
                worksheet.Cell("AC1").Value = "Nivel(Trunco, Pasante, Titulado, Posgrado)";
                worksheet.Cell("AD1").Value = "Universidad";
                worksheet.Cell("AE1").Value = "Ultimo grado de estudios";
                worksheet.Cell("AF1").Value = "Estatus";

                worksheet.Cell("A1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("A1").Style.Font.Bold = true;

                worksheet.Cell("A1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("A1").Style.Font.Bold = true;

                worksheet.Cell("B1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("B1").Style.Font.Bold = true;

                worksheet.Cell("C1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("C1").Style.Font.Bold = true;

                worksheet.Cell("D1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("D1").Style.Font.Bold = true;

                worksheet.Cell("E1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("E1").Style.Font.Bold = true;

                worksheet.Cell("F1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("F1").Style.Font.Bold = true;

                worksheet.Cell("G1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("G1").Style.Font.Bold = true;

                worksheet.Cell("H1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("H1").Style.Font.Bold = true;

                worksheet.Cell("I1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("I1").Style.Font.Bold = true;

                worksheet.Cell("J1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("J1").Style.Font.Bold = true;

                worksheet.Cell("K1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("K1").Style.Font.Bold = true;

                worksheet.Cell("L1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("L1").Style.Font.Bold = true;

                worksheet.Cell("M1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("M1").Style.Font.Bold = true;

                worksheet.Cell("N1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("N1").Style.Font.Bold = true;

                worksheet.Cell("O1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("O1").Style.Font.Bold = true;

                worksheet.Cell("P1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("P1").Style.Font.Bold = true;

                worksheet.Cell("Q1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("Q1").Style.Font.Bold = true;

                worksheet.Cell("R1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("R1").Style.Font.Bold = true;

                worksheet.Cell("S1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("S1").Style.Font.Bold = true;

                worksheet.Cell("T1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("T1").Style.Font.Bold = true;

                worksheet.Cell("U1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("U1").Style.Font.Bold = true;

                worksheet.Cell("V1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("V1").Style.Font.Bold = true;

                worksheet.Cell("W1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("W1").Style.Font.Bold = true;

                worksheet.Cell("X1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("X1").Style.Font.Bold = true;

                worksheet.Cell("Y1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("Y1").Style.Font.Bold = true;

                worksheet.Cell("Z1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("Z1").Style.Font.Bold = true;

                worksheet.Cell("AA1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("AA1").Style.Font.Bold = true;

                worksheet.Cell("AB1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("AB1").Style.Font.Bold = true;

                worksheet.Cell("AC1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("AC1").Style.Font.Bold = true;

                worksheet.Cell("AD1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("AD1").Style.Font.Bold = true;

                worksheet.Cell("AE1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("AE1").Style.Font.Bold = true;

                worksheet.Cell("AF1").Style.Fill.BackgroundColor = XLColor.OrangeColorWheel;
                worksheet.Cell("AF1").Style.Font.Bold = true;



                int index = 2;
                foreach (EmployeeValidador emp in _model.LstEmployeesValidador)
                {
                    
                    worksheet.Cell("A"+index.ToString()).Value = emp.CardNumber;
                    worksheet.Cell("A" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("B"+index.ToString()).Value = emp.FirstName;
                    worksheet.Cell("C"+index.ToString()).Value = emp.LastName;
                    worksheet.Cell("D"+index.ToString()).Value = emp.MiddleName;
                    worksheet.Cell("E"+index.ToString()).Value = emp.FullName;
                    worksheet.Cell("F"+index.ToString()).Value = emp.Rfc;
                    worksheet.Cell("F" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("G"+index.ToString()).Value = emp.Curp;
                    worksheet.Cell("G" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("H"+index.ToString()).Value = emp.NSS;
                    worksheet.Cell("H" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("I" + index.ToString()).Value = emp.EdoCivil;
                    worksheet.Cell("I" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell("J" + index.ToString()).Value = emp.Sexo;
                    worksheet.Cell("J" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("K"+index.ToString()).Value = emp.BirthDay;
                    worksheet.Cell("K" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("L"+index.ToString()).Value = emp.StreetName;
                    worksheet.Cell("M"+index.ToString()).Value = emp.NumberExt;
                    worksheet.Cell("M" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell("N"+index.ToString()).Value = emp.NumberInt;
                    worksheet.Cell("N" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell("O" + index.ToString()).Value = emp.Colony;
                    worksheet.Cell("P"+index.ToString()).Value = emp.Delegation;
                    worksheet.Cell("Q" + index.ToString()).Value = emp.EstadoEmpleado;
                    worksheet.Cell("R"+index.ToString()).Value = emp.CP;
                    worksheet.Cell("R" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("S"+index.ToString()).Value = emp.PhoneNumber;
                    worksheet.Cell("S" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("T"+index.ToString()).Value = emp.CellPhoneNumber;
                    worksheet.Cell("T" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("U"+index.ToString()).Value = emp.PersonalEmail;

                    worksheet.Cell("V" + index.ToString()).Value = emp.LugarNacimiento;
                    worksheet.Cell("V" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell("W" + index.ToString()).Value = string.Format("{0}{1}", "'", emp.Clabe ==null? string.Empty: emp.Clabe.ToString());
                    worksheet.Cell("W" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("X"+index.ToString()).Value = emp.Banco;
                    worksheet.Cell("X" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    worksheet.Cell("Y"+index.ToString()).Value = emp.CreditoInfonavit;
                    worksheet.Cell("Y" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("Z"+index.ToString()).Value = emp.TRAMITE_INF_SN;
                    worksheet.Cell("Z" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("AA" + index.ToString()).Value = emp.Etapa;
                    worksheet.Cell("AB" + index.ToString()).Value = emp.Estudios;
                    worksheet.Cell("AC" + index.ToString()).Value = emp.Nivel_Estudios;
                    worksheet.Cell("AD"+index.ToString()).Value = emp.Universidad;
                    worksheet.Cell("AE"+index.ToString()).Value = emp.Ultimo_GradoEstudios;
                    worksheet.Cell("AF"+index.ToString()).Value = emp.Estatus ?"A":"i";
                    worksheet.Cell("AF" + index.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    index++;

                }
                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(FilePath);
            }
            return FilePath;
        }
    }
}