using AdministracionRH.Common;
using AdministracionRH.Models;
using AdministracionRH.Repositories;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;

using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using static AdministracionRH.Common.Enumeraciones;

namespace AdministracionRH.Services
{
    public class ControlAsistenciaService
    {
        ControlAsistenciaRepository _ControlAsistenciaService;
        private string _SpProcedureName { get; set; }
        public ControlAsistenciaSearch _model { get; set; }
        private List<EmpleadoJefe> LstEmpleadosJefe { get; set; }
        private List<EmployeeAsistencia> _LstEmpAsistencia { get; set; }
        public List<Empleado_Asistencia> ListaEmpAsistencia { get; set; }
        private string _FilePath { get; set; }
        private string _Path { get; set; }

        private string RenglonActual { get; set; }
        private string CurrentColumn { get; set; }

        public List<ResumenEmployeeAsistencia> LstResumenEmployeeAsistencia { get; set; }


        public ControlAsistenciaService()
        {
            _ControlAsistenciaService = new ControlAsistenciaRepository();
            LstResumenEmployeeAsistencia = new List<ResumenEmployeeAsistencia>();
        }


        public ControlAsistenciaService(ControlAsistenciaSearch model)
        {
            _ControlAsistenciaService = new ControlAsistenciaRepository();
            _model = model;
            _model.AreaId = model.AreaId;
            _model.BossId = model.BossId;
            _model.PositionId = model.PositionId;
            _model.LocalizationId = model.LocalizationId;
            _model.FechaInicial = model.FechaInicial;
            _model.FechaFinal = model.FechaFinal;
            _model.PayRollId = model.PayRollId;
            _model.NombreEmpleado = model.NombreEmpleado;
            _model.ListaEmpleado_Asistenica = model.ListaEmpleado_Asistenica;
            _model.Rol = model.Rol;
            LstResumenEmployeeAsistencia = new List<ResumenEmployeeAsistencia>();
        }

        public Result CreaExcelReporteAsistenciasPorArea(string Path, out List<string> LstArchivos)
        {
            LstArchivos = new List<string>();
            _Path = Path;
            Result resultCreateExcelAsistencias = new Result();
            string OutPutFile = string.Empty;
            Result result = new Result();
            try
            {
                var ListaAreas = _model.ListaEmpleado_Asistenica
                                 .Where(ar => ar.AreaId != null)
                                 .Select(area => new { AreaId = area.AreaId, Area = area.Area })
                                 .Distinct().OrderBy(o => o.Area)
                                 .ToList();
                if (ListaAreas != null)
                {
                    foreach (var area in ListaAreas)
                    {
                        var lstJefesArea = _model.ListaEmpleado_Asistenica.Where(i => i.Es_Jefe == 1 && i.Children.Count() > 0 && i.AreaId == area.AreaId).OrderBy(g => g.Level).ToList();
                        if (lstJefesArea != null && lstJefesArea.Count() > 0)
                        {
                            resultCreateExcelAsistencias = CreaExcelArea(lstJefesArea, area.Area, out OutPutFile);
                            if (resultCreateExcelAsistencias.Success)
                            {
                                var listaCorreo = lstJefesArea
                                                   .Where(j => !string.IsNullOrEmpty(j.CompanyEmail) && IsValidEmail(j.CompanyEmail))
                                                   .Select(b => new { CompanyEmail = b.CompanyEmail, archivo = OutPutFile, Level = b.Level }).ToList();


                                var JefeAreaLevel = lstJefesArea.Where(j => !string.IsNullOrEmpty(j.CompanyEmail) && IsValidEmail(j.CompanyEmail)).Select(b => new { CompanyEmail = b.CompanyEmail, archivo = OutPutFile, Level = b.Level }).Min(g => g.Level);

                                if (listaCorreo.Where(i => i.Level == JefeAreaLevel) != null && listaCorreo.Where(i => i.Level == JefeAreaLevel).Count() > 0)
                                {
                                    Correo _correo = new Correo();
                                    string AttachFile = string.Empty;
                                    List<MailAddress> destinatarios = new List<MailAddress>();
                                    foreach (var item in listaCorreo.Where(i => i.Level == JefeAreaLevel))
                                    {
                                        AttachFile = item.archivo;
                                        if (IsValidEmail(item.CompanyEmail))
                                            destinatarios.Add(new MailAddress(item.CompanyEmail));
                                    }

                                    destinatarios.Add(new MailAddress("gflores@adamantine.com.mx"));
                                    Correo correo = new Correo();
                                    string Subject = string.Format("Reporte de Asistencia semanal de {0}", area.Area);
                                    DateTime fechaInicial = (DateTime)_model.FechaInicial;
                                    DateTime fechaFinal = (DateTime)_model.FechaFinal;

                                    string Body = string.Format("Se adjunta el reporte de semanal de asistencia del periodo {0} al {1}", fechaInicial.ToString("dd/MM/yyyy"), fechaFinal.ToString("dd/MM/yyyy"));
                                    try
                                    {
                                        //correo.SendMail(destinatarios, Subject, Body, correo.AttachFile(AttachFile));
                                    }
                                    catch (Exception ex)
                                    {

                                        result.Success = false;
                                        result.ErrorMessage = ex.Message;
                                    }


                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public Result DescargarExcelReporteAsistencia(string Path, out List<string> LstArchivos)
        {
            LstArchivos = new List<string>();
            _Path = Path;
            Result resultCreateExcelAsistencias = new Result();
            string OutPutFile = string.Empty;

            Result result = new Result();
            try
            {

                var ListaAreas = _model.ListaEmpleado_Asistenica
                               //  .Where(ar => ar.AreaId != null)
                                 .Select(area => new { AreaId = area.AreaId, Area = area.Area })
                                 .Distinct().OrderBy(o => o.Area)
                                 .ToList();
                if (ListaAreas != null)
                {
                    foreach (var area in ListaAreas)
                    {
                        //var lstJefesArea = _model.ListaEmpleado_Asistenica.Where(i => i.Es_Jefe == 1 && i.Children.Count() > 0 && i.AreaId == area.AreaId).OrderBy(g => g.Level).ToList();
                        var lstJefesArea = _model.ListaEmpleado_Asistenica.Where(i => i.Children.Count() > 0 && i.AreaId == area.AreaId).OrderBy(g => g.Level).ToList();
                        if (lstJefesArea != null && lstJefesArea.Count() > 0)
                        {
                            resultCreateExcelAsistencias = CreaExcelArea(lstJefesArea, area.Area, out OutPutFile);
                            if (resultCreateExcelAsistencias.Success)
                            {
                                LstArchivos.Add(OutPutFile);
                            }
                        }
                        //flujo sin jefes
                        else
                        {
                            var lstEmpleadosArea = _model.ListaEmpleado_Asistenica.Where(i => i.AreaId == area.AreaId).OrderBy(g => g.Level).ToList();
                            resultCreateExcelAsistencias = CreaExcelAreaSinJefes1(lstEmpleadosArea, area.Area, out OutPutFile);
                            if (resultCreateExcelAsistencias.Success)
                            {
                                LstArchivos.Add(OutPutFile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }


        public Result DescargarExcelReporteAsistenciaSinJefes(string Path, out List<string> LstArchivos)
        {
            LstArchivos = new List<string>();
            _Path = Path;
            Result resultCreateExcelAsistencias = new Result();
            string OutPutFile = string.Empty;

            Result result = new Result();
            try
            {

                var ListaAreas = _model.ListaEmpleado_Asistenica
                                 .Where(ar => ar.AreaId != null)
                                 .Select(area => new { AreaId = area.AreaId, Area = area.Area })
                                 .Distinct().OrderBy(o => o.Area)
                                 .ToList();
                if (ListaAreas != null)
                {
                    foreach (var area in ListaAreas)
                    {
                        var lstJefesArea = _model.ListaEmpleado_Asistenica.Where(i => i.Es_Jefe == 0 && i.AreaId == area.AreaId).OrderBy(g => g.Level).ToList();
                        if (lstJefesArea != null && lstJefesArea.Count() > 0)
                        {
                            resultCreateExcelAsistencias = CreaExcelArea(lstJefesArea, area.Area, out OutPutFile);
                            if (resultCreateExcelAsistencias.Success)
                            {
                                LstArchivos.Add(OutPutFile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public void BorrarArchivos(string ServerMapPath)
        {
            if (Directory.Exists(ServerMapPath))
            {
                foreach (string _file in Directory.GetFiles(ServerMapPath))
                {
                    File.Delete(_file);

                }
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        public Result Compress(string _ServerPath, string CompressDirectory, out string OutPutile)
        {
            Result result = new Result();
            OutPutile = string.Empty;
            _FilePath = _ServerPath;
            try
            {
                var archivo = String.Format("{0}_{1}.zip", "ReporteAsistencia", DateTime.Now.ToString("yyyyMMdd"));
                OutPutile = CompressDirectory + archivo;
                if (File.Exists(OutPutile)) File.Delete(OutPutile);
                ZipFile.CreateFromDirectory(_FilePath, OutPutile);
                if (result.Success)
                {
                    foreach (string file in Directory.GetFiles(_FilePath))
                    {
                        File.Delete(file);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private string[] GetColumnasExcel()
        {

            string[] Columnas = new string[] {
                     "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                    ,"AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ"
                    ,"BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ"
                    ,"CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ"
                    ,"DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ"
                    ,"EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ"
                    ,"FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX", "FY", "FZ"
                    ,"GA", "GB", "GC", "GD", "GE", "GF", "GG", "GH", "GI", "GJ", "GK", "GL", "GM", "GN", "GO", "GP", "GQ", "GR", "GS", "GT", "GU", "GV", "GW", "GX", "GY", "GZ"
                    ,"HA", "HB", "HC", "HD", "HE", "HF", "HG", "HH", "HI", "HJ", "HK", "HL", "HM", "HN", "HO", "HP", "HQ", "HR", "HS", "HT", "HU", "HV", "HW", "HX", "HY", "HZ"
                    ,"IA", "IB", "IC", "ID", "IE", "IF", "IG", "IH", "II", "IJ", "IK", "IL", "IM", "IN", "IO", "IP", "IQ", "IR", "IS", "IT", "IU", "IV", "IW", "IX", "IY", "IZ"
                    ,"JA", "JB", "JC", "JD", "JE", "JF", "JG", "JH", "JI", "JJ", "JK", "JL", "JM", "JN", "JO", "JP", "JQ", "JR", "JS", "JT", "JU", "JV", "JW", "JX", "JY", "JZ"
                    ,"KA", "KB", "KC", "KD", "KE", "KF", "KG", "KH", "KI", "KJ", "KK", "KL", "KM", "KN", "KO", "KP", "KQ", "KR", "KS", "KT", "KU", "KV", "KW", "KX", "KY", "KZ"
                    ,"LA", "LB", "LC", "LD", "LE", "LF", "LG", "LH", "LI", "LJ", "LK", "LL", "LM", "LN", "LO", "LP", "LQ", "LR", "LS", "LT", "LU", "LV", "LW", "LX", "LY", "LZ"
                    ,"MA", "MB", "MC", "MD", "ME", "MF", "MG", "MH", "MI", "MJ", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY", "MZ"
                    ,"NA", "NB", "NC", "ND", "NE", "NF", "NG", "NH", "NI", "NJ", "NK", "NL", "NM", "NN", "NO", "NP", "NQ", "NR", "NS", "NT", "NU", "NV", "NW", "NX", "NY", "NZ"
                    ,"OA", "OB", "OC", "OD", "OE", "OF", "OG", "OH", "OI", "OJ", "OK", "OL", "OM", "ON", "OO", "OP", "OQ", "OR", "OS", "OT", "OU", "OV", "OW", "OX", "OY", "OZ"
                    ,"PA", "PB", "PC", "PD", "PE", "PF", "PG", "PH", "PI", "PJ", "PK", "PL", "PM", "PN", "PO", "PP", "PQ", "PR", "PS", "PT", "PU", "PV", "PW", "PX", "PY", "PZ"
                    ,"QA", "QB", "QC", "QD", "QE", "QF", "QG", "QH", "QI", "QJ", "QK", "QL", "QM", "QN", "QO", "QP", "QQ", "QR", "QS", "QT", "QU", "QV", "QW", "QX", "QY", "QZ"
                    ,"RA", "RB", "RC", "RD", "RE", "RF", "RG", "RH", "RI", "RJ", "RK", "RL", "RM", "RN", "RO", "RP", "RQ", "RR", "RS", "RT", "RU", "RV", "RW", "RX", "RY", "RZ"
                    ,"SA", "SB", "SC", "SD", "SE", "SF", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SP", "SQ", "SR", "SS", "ST", "SU", "SV", "SW", "SX", "SY", "SZ"
                    ,"TA", "TB", "TC", "TD", "TE", "TF", "TG", "TH", "TI", "TJ", "TK", "TL", "TM", "TN", "TO", "TP", "TQ", "TR", "TS", "TT", "TU", "TV", "TW", "TX", "TY", "TZ"
                    ,"UA", "UB", "UC", "UD", "UE", "UF", "UG", "UH", "UI", "UJ", "UK", "UL", "UM", "UN", "UO", "UP", "UQ", "UR", "US", "UT", "UU", "UV", "UW", "UX", "UY", "UZ"
                    ,"VA", "VB", "VC", "VD", "VE", "VF", "VG", "VH", "VI", "VJ", "VK", "VL", "VM", "VN", "VO", "VP", "VQ", "VR", "VS", "VT", "VU", "VV", "VW", "VX", "VY", "VZ"
                    ,"XA", "XB", "XC", "XD", "XE", "XF", "XG", "XH", "XI", "XJ", "XK", "XL", "XM", "XN", "XO", "XP", "XQ", "XR", "XS", "XT", "XU", "XV", "XW", "XX", "XY", "XZ"
                    ,"YA", "YB", "YC", "YD", "YE", "YF", "YG", "YH", "YI", "YJ", "YK", "YL", "YM", "YN", "YO", "YP", "YQ", "YR", "YS", "YT", "YU", "YV", "YW", "YX", "YY", "YZ"
                    ,"ZA", "ZB", "ZC", "ZD", "ZE", "ZF", "ZG", "ZH", "ZI", "ZJ", "ZK", "ZL", "ZM", "ZN", "ZO", "ZP", "ZQ", "ZR", "ZS", "ZT", "ZU", "ZV", "ZW", "ZX", "ZY", "ZZ"
                };

            return Columnas;

        }

        private TimeSpan ConvertTimeSpan(string Cadena)
        {
            return DateTime.Parse(Cadena, System.Globalization.CultureInfo.CurrentCulture).TimeOfDay;
        }

        private XLColor ApplyFormat(object horaInicial, string horaFinal, string horasLaboradas)
        {
            
            if (horasLaboradas != "0")
            {
                //Si es retardo
                if (TimeSpan.Compare(ConvertTimeSpan(horaInicial.ToString()), ConvertTimeSpan("08:45:59")) == 1 && TimeSpan.Compare(ConvertTimeSpan(horaInicial.ToString()), ConvertTimeSpan("09:00:00")) == -1)
                    return XLColor.Red;

                //Si es falta
                if (ComparaFechas(horaInicial.ToString(), horaFinal) == 1)
                    return XLColor.Red;
            }
            return XLColor.White;
        }

        private int ComparaFechas(string horaInicial, string horaFinal)
        {

            return TimeSpan.Compare(ConvertTimeSpan(horaInicial), ConvertTimeSpan(horaFinal));
        }

        public XLColor FormatHorasAcumuladas(string horasAcumuladas)
        {
            int horas = 0;
            int minutos = 0;
            if (horasAcumuladas != "0")
            {

                if (horasAcumuladas.Split(':').Count() >1)
                {
                    horas = Convert.ToInt32(horasAcumuladas.Split(':')[0]);
                    minutos = Convert.ToInt32(horasAcumuladas.Split(':')[1]);
                }
                //horas = Convert.ToInt32(horasAcumuladas.Substring(0, 2));
                //minutos = Convert.ToInt32(horasAcumuladas.Substring(3, 2));
                if (horas >= 40)
                {
                    if (horas > 40)
                    {
                        return XLColor.Navy;
                    }
                    else
                    {
                        if (minutos < 30)
                        {
                            return XLColor.Red;
                        }
                        else
                        {
                            return XLColor.Navy;
                        }
                    }
                    
                }
                else
                {
                    return XLColor.Red;
                }
            }
            else
            {
                return XLColor.Red;
            }
           // return XLColor.Black;
        }


        private TipoIncidenciaHora GetTipoIncidencia(Asistencia asistencia, TipoOperacionLector tipoOperacionLector)
        {
            TipoIncidenciaHora tipoIncidenciaHora = TipoIncidenciaHora.Ninguna;
            if (asistencia == null)
                return TipoIncidenciaHora.FaltaPorInasistencia;

            if (tipoOperacionLector == TipoOperacionLector.Entrada)
            {
                if (string.IsNullOrEmpty(asistencia.Entrada))
                    return TipoIncidenciaHora.FaltaPorInasistencia;

                if (asistencia.Entrada != asistencia.Salida)
                {
                    if (ComparaFechas(asistencia.Entrada, "08:45:59") == 1 && ComparaFechas(asistencia.Entrada, "09:00:00") == -1)
                        return TipoIncidenciaHora.Retardo;

                    if (ComparaFechas(asistencia.Entrada, "09:00:00") == 1)
                        return TipoIncidenciaHora.FaltaPorRetardo;

                    return TipoIncidenciaHora.EntradaOk;

                }
                else
                {
                    if (ComparaFechas(asistencia.Entrada, "15:00:00") == 1)
                    {
                        return TipoIncidenciaHora.NoRegistroEntrada;
                    }
                    else
                    {
                        return TipoIncidenciaHora.EntradaOk;
                    }
                     
                  
                }

            }
            if (tipoOperacionLector == TipoOperacionLector.Salida)
            {
                if (string.IsNullOrEmpty(asistencia.Salida))
                    return TipoIncidenciaHora.FaltaPorInasistencia;

                if (ComparaFechas(asistencia.Salida, "15:00:00") == -1)
                {
                    if (asistencia.Entrada == asistencia.Salida)
                    {
                        return TipoIncidenciaHora.NoRegistroSalida;
                    }
                    return TipoIncidenciaHora.RegistroSalidaAnticipada;
                }
                else
                {

                    return TipoIncidenciaHora.SalidaOk;
                }
            }
            return tipoIncidenciaHora;
        }
            private Result CreaExcelArea(List<Empleado_Asistencia> lstJefesArea, string Area, out string OutPutFile)
        {
            Result result = new Result();
            OutPutFile = string.Empty;

            
            try
            {
                var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Reporte Asistencia");
                string[] Columnas = new string[] {
                     "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                    ,"AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ"
                    ,"BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ"
                    ,"CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ"
                    ,"DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ"
                    ,"EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ"
                    ,"FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX", "FY", "FZ"
                    ,"GA", "GB", "GC", "GD", "GE", "GF", "GG", "GH", "GI", "GJ", "GK", "GL", "GM", "GN", "GO", "GP", "GQ", "GR", "GS", "GT", "GU", "GV", "GW", "GX", "GY", "GZ"
                    ,"HA", "HB", "HC", "HD", "HE", "HF", "HG", "HH", "HI", "HJ", "HK", "HL", "HM", "HN", "HO", "HP", "HQ", "HR", "HS", "HT", "HU", "HV", "HW", "HX", "HY", "HZ"
                    ,"IA", "IB", "IC", "ID", "IE", "IF", "IG", "IH", "II", "IJ", "IK", "IL", "IM", "IN", "IO", "IP", "IQ", "IR", "IS", "IT", "IU", "IV", "IW", "IX", "IY", "IZ"
                    ,"JA", "JB", "JC", "JD", "JE", "JF", "JG", "JH", "JI", "JJ", "JK", "JL", "JM", "JN", "JO", "JP", "JQ", "JR", "JS", "JT", "JU", "JV", "JW", "JX", "JY", "JZ"
                    ,"KA", "KB", "KC", "KD", "KE", "KF", "KG", "KH", "KI", "KJ", "KK", "KL", "KM", "KN", "KO", "KP", "KQ", "KR", "KS", "KT", "KU", "KV", "KW", "KX", "KY", "KZ"
                    ,"LA", "LB", "LC", "LD", "LE", "LF", "LG", "LH", "LI", "LJ", "LK", "LL", "LM", "LN", "LO", "LP", "LQ", "LR", "LS", "LT", "LU", "LV", "LW", "LX", "LY", "LZ"
                    ,"MA", "MB", "MC", "MD", "ME", "MF", "MG", "MH", "MI", "MJ", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY", "MZ"
                    ,"NA", "NB", "NC", "ND", "NE", "NF", "NG", "NH", "NI", "NJ", "NK", "NL", "NM", "NN", "NO", "NP", "NQ", "NR", "NS", "NT", "NU", "NV", "NW", "NX", "NY", "NZ"
                    ,"OA", "OB", "OC", "OD", "OE", "OF", "OG", "OH", "OI", "OJ", "OK", "OL", "OM", "ON", "OO", "OP", "OQ", "OR", "OS", "OT", "OU", "OV", "OW", "OX", "OY", "OZ"
                    ,"PA", "PB", "PC", "PD", "PE", "PF", "PG", "PH", "PI", "PJ", "PK", "PL", "PM", "PN", "PO", "PP", "PQ", "PR", "PS", "PT", "PU", "PV", "PW", "PX", "PY", "PZ"
                    ,"QA", "QB", "QC", "QD", "QE", "QF", "QG", "QH", "QI", "QJ", "QK", "QL", "QM", "QN", "QO", "QP", "QQ", "QR", "QS", "QT", "QU", "QV", "QW", "QX", "QY", "QZ"
                    ,"RA", "RB", "RC", "RD", "RE", "RF", "RG", "RH", "RI", "RJ", "RK", "RL", "RM", "RN", "RO", "RP", "RQ", "RR", "RS", "RT", "RU", "RV", "RW", "RX", "RY", "RZ"
                    ,"SA", "SB", "SC", "SD", "SE", "SF", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SP", "SQ", "SR", "SS", "ST", "SU", "SV", "SW", "SX", "SY", "SZ"
                    ,"TA", "TB", "TC", "TD", "TE", "TF", "TG", "TH", "TI", "TJ", "TK", "TL", "TM", "TN", "TO", "TP", "TQ", "TR", "TS", "TT", "TU", "TV", "TW", "TX", "TY", "TZ"
                    ,"UA", "UB", "UC", "UD", "UE", "UF", "UG", "UH", "UI", "UJ", "UK", "UL", "UM", "UN", "UO", "UP", "UQ", "UR", "US", "UT", "UU", "UV", "UW", "UX", "UY", "UZ"
                    ,"VA", "VB", "VC", "VD", "VE", "VF", "VG", "VH", "VI", "VJ", "VK", "VL", "VM", "VN", "VO", "VP", "VQ", "VR", "VS", "VT", "VU", "VV", "VW", "VX", "VY", "VZ"
                    ,"WA", "WB", "WC", "WD", "WE", "WF", "WG", "WH", "WI", "WJ", "WK", "WL", "WM", "WN", "WO", "WP", "WQ", "WR", "WS", "WT", "WU", "WV", "WW", "WX", "WY", "WZ"
                    ,"XA", "XB", "XC", "XD", "XE", "XF", "XG", "XH", "XI", "XJ", "XK", "XL", "XM", "XN", "XO", "XP", "XQ", "XR", "XS", "XT", "XU", "XV", "XW", "XX", "XY", "XZ"
                    ,"YA", "YB", "YC", "YD", "YE", "YF", "YG", "YH", "YI", "YJ", "YK", "YL", "YM", "YN", "YO", "YP", "YQ", "YR", "YS", "YT", "YU", "YV", "YW", "YX", "YY", "YZ"
                    ,"ZA", "ZB", "ZC", "ZD", "ZE", "ZF", "ZG", "ZH", "ZI", "ZJ", "ZK", "ZL", "ZM", "ZN", "ZO", "ZP", "ZQ", "ZR", "ZS", "ZT", "ZU", "ZV", "ZW", "ZX", "ZY", "ZZ"
                    ,"AAA", "AAB", "AAC", "AAD", "AAE", "AAF", "AAG", "AAH", "AAI", "AAJ", "AAK", "AAL", "AAM", "AAN", "AAO", "AAP", "AAQ", "AAR", "AAS", "AAT", "AAU", "AAV", "AAW", "AAX", "AAY", "AAZ"
                    ,"ABA", "ABB", "ABC", "ABD", "ABE", "ABF", "ABG", "ABH", "ABI", "ABJ", "ABK", "ABL", "ABM", "ABN", "ABO", "ABP", "ABQ", "ABR", "ABS", "ABT", "ABU", "ABV", "ABW", "ABX", "ABY", "ABZ"
                    ,"ACA", "ACB", "ACC", "ACD", "ACE", "ACF", "ACG", "ACH", "ACI", "ACJ", "ACK", "ACL", "ACM", "ACN", "ACO", "ACP", "ACQ", "ACR", "ACS", "ACT", "ACU", "ACV", "ACW", "ACX", "ACY", "ACZ"
                    ,"ADA", "ADB", "ADC", "ADD", "ADE", "ADF", "ADG", "ADH", "ADI", "ADJ", "ADK", "ADL", "ADM", "ADN", "ADO", "ADP", "ADQ", "ADR", "ADS", "ADT", "ADU", "ADV", "ADW", "ADX", "ADY", "ADZ"
                    ,"AEA", "AEB", "AEC", "AED", "AEE", "AEF", "AEG", "AEH", "AEI", "AEJ", "AEK", "AEL", "AEM", "AEN", "AEO", "AEP", "AEQ", "AER", "AES", "AET", "AEU", "AEV", "AEW", "AEX", "AEY", "AEZ"
                    ,"AFA", "AFB", "AFC", "AFD", "AFE", "AFF", "AFG", "AFH", "AFI", "AFJ", "AFK", "AFL", "AFM", "AFN", "AFO", "AFP", "AFQ", "AFR", "AFS", "AFT", "AFU", "AFV", "AFW", "AFX", "AFY", "AFZ"
                    ,"AGA", "AGB", "AGC", "AGD", "AGE", "AGF", "AGG", "AGH", "AGI", "AGJ", "AGK", "AGL", "AGM", "AGN", "AGO", "AGP", "AGQ", "AGR", "AGS", "AGT", "AGU", "AGV", "AGW", "AGX", "AGY", "AGZ"
                    ,"AHA", "AHB", "AHC", "AHD", "AHE", "AHF", "AHG", "AHH", "AHI", "AHJ", "AHK", "AHL", "AHM", "AHN", "AHO", "AHP", "AHQ", "AHR", "AHS", "AHT", "AHU", "AHV", "AHW", "AHX", "AHY", "AHZ"
                    ,"AIA", "AIB", "AIC", "AID", "AIE", "AIF", "AIG", "AIH", "AII", "AIJ", "AIK", "AIL", "AIM", "AIN", "AIO", "AIP", "AIQ", "AIR", "AIS", "AIT", "AIU", "AIV", "AIW", "AIX", "AIY", "AIZ"
                    ,"AJA", "AJB", "AJC", "AJD", "AJE", "AJF", "AJG", "AJH", "AJI", "AJJ", "AJK", "AJL", "AJM", "AJN", "AJO", "AJP", "AJQ", "AJR", "AJS", "AJT", "AJU", "AJV", "AJW", "AJX", "AJY", "AJZ"
                    ,"AKA", "AKB", "AKC", "AKD", "AKE", "AKF", "AKG", "AKH", "AKI", "AKJ", "AKK", "AKL", "AKM", "AKN", "AKO", "AKP", "AKQ", "AKR", "AKS", "AKT", "AKU", "AKV", "AKW", "AKX", "AKY", "AKZ"
                    ,"ALA", "ALB", "ALC", "ALD", "ALE", "ALF", "ALG", "ALH", "ALI", "ALJ", "ALK", "ALL", "ALM", "ALN", "ALO", "ALP", "ALQ", "ALR", "ALS", "ALT", "ALU", "ALV", "ALW", "ALX", "ALY", "ALZ"
                    ,"AMA", "AMB", "AMC", "AMD", "AME", "AMF", "AMG", "AMH", "AMI", "AMJ", "AMK", "AML", "AMM", "AMN", "AMO", "AMP", "AMQ", "AMR", "AMS", "AMT", "AMU", "AMV", "AMW", "AMX", "AMY", "AMZ"
                    ,"ANA", "ANB", "ANC", "AND", "ANE", "ANF", "ANG", "ANH", "ANI", "ANJ", "ANK", "ANL", "ANM", "ANN", "ANO", "ANP", "ANQ", "ANR", "ANS", "ANT", "ANU", "ANV", "ANW", "ANX", "ANY", "ANZ"
                    ,"AOA", "AOB", "AOC", "AOD", "AOE", "AOF", "AOG", "AOH", "AOI", "AOJ", "AOK", "AOL", "AOM", "AON", "AOO", "AOP", "AOQ", "AOR", "AOS", "AOT", "AOU", "AOV", "AOW", "AOX", "AOY", "AOZ"
                    ,"APA", "APB", "APC", "APD", "APE", "APF", "APG", "APH", "API", "APJ", "APK", "APL", "APM", "APN", "APO", "APP", "APQ", "APR", "APS", "APT", "APU", "APV", "APW", "APX", "APY", "APZ"
                    ,"AQA", "AQB", "AQC", "AQD", "AQE", "AQF", "AQG", "AQH", "AQI", "AQJ", "AQK", "AQL", "AQM", "AQN", "AQO", "AQP", "AQQ", "AQR", "AQS", "AQT", "AQU", "AQV", "AQW", "AQX", "AQY", "AQZ"
                    ,"ARA", "ARB", "ARC", "ARD", "ARE", "ARF", "ARG", "ARH", "ARI", "ARJ", "ARK", "ARL", "ARM", "ARN", "ARO", "ARP", "ARQ", "ARR", "ARS", "ART", "ARU", "ARV", "ARW", "ARX", "ARY", "ARZ"
                    ,"ASA", "ASB", "ASC", "ASD", "ASE", "ASF", "ASG", "ASH", "ASI", "ASJ", "ASK", "ASL", "ASM", "ASN", "ASO", "ASP", "ASQ", "ASR", "ASS", "AST", "ASU", "ASV", "ASW", "ASX", "ASY", "ASZ"
                    ,"ATA", "ATB", "ATC", "ATD", "ATE", "ATF", "ATG", "ATH", "ATI", "ATJ", "ATK", "ATL", "ATM", "ATN", "ATO", "ATP", "ATQ", "ATR", "ATS", "ATT", "ATU", "ATV", "ATW", "ATX", "ATY", "ATZ"
                    ,"AUA", "AUB", "AUC", "AUD", "AUE", "AUF", "AUG", "AUH", "AUI", "AUJ", "AUK", "AUL", "AUM", "AUN", "AUO", "AUP", "AUQ", "AUR", "AUS", "AUT", "AUU", "AUV", "AUW", "AUX", "AUY", "AUZ"
                    ,"AWA", "AWB", "AWC", "AWD", "AWE", "AWF", "AWG", "AWH", "AWI", "AWJ", "AWK", "AWL", "AWM", "AWN", "AWO", "AWP", "AWQ", "AWR", "AWS", "AWT", "AWU", "AWV", "AWW", "AWX", "AWY", "AWZ"
                    ,"AVA", "AVB", "AVC", "AVD", "AVE", "AVF", "AVG", "AVH", "AVI", "AVJ", "AVK", "AVL", "AVM", "AVN", "AVO", "AVP", "AVQ", "AVR", "AVS", "AVT", "AVU", "AVV", "AVW", "AVX", "AVY", "AVZ"
                    ,"AXA", "AXB", "AXC", "AXD", "AXE", "AXF", "AXG", "AXH", "AXI", "AXJ", "AXK", "AXL", "AXM", "AXN", "AXO", "AXP", "AXQ", "AXR", "AXS", "AXT", "AXU", "AXV", "AXW", "AXX", "AXY", "AXZ"
                    ,"AYA", "AYB", "AYC", "AYD", "AYE", "AYF", "AYG", "AYH", "AYI", "AYJ", "AYK", "AYL", "AYM", "AYN", "AYO", "AYP", "AYQ", "AYR", "AYS", "AYT", "AYU", "AYV", "AYW", "AYX", "AYY", "AYZ"
                    ,"AZA", "AZB", "AZC", "AZD", "AZE", "AZF", "AZG", "AZH", "AZI", "AZJ", "AZK", "AZL", "AZM", "AZN", "AZO", "AZP", "AZQ", "AZR", "AZS", "AZT", "AZU", "AZV", "AZW", "AZX", "AZY", "AZZ"
                };

                string ColumnaInicial = "A";
                int RenglonInicial = 3;
                
                int Renglon = 0;
                string RenglonTitle = (Convert.ToInt32(RenglonInicial) - 1).ToString();
                string RenglonTitlefechas = (Convert.ToInt32(RenglonInicial) - 2).ToString();

                //string celdaInicial = string.Empty;
                //string celdaTitle = string.Empty;
                string CellRango = string.Empty;
                string[] ColumnasFinales = Columnas.Take(lstJefesArea.Count()).ToArray();
                int RowSpan = 0;

                Renglon = RenglonInicial;
                CurrentColumn = ColumnaInicial;

                //Por cada Jefe
                foreach (Empleado_Asistencia jefe in lstJefesArea)
                {

                    List<Empleado_Asistencia> lstEmpleadosACargo = new List<Empleado_Asistencia>();
                    lstEmpleadosACargo = jefe.Children.ToList();
                    if (Renglon == RenglonInicial)
                    {

                        SetCell(ws, CurrentColumn, RenglonTitle, "Jefe Directo", true, false);
                        GetNextColumn(CurrentColumn);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Nombre", true, false);
                        GetNextColumn(CurrentColumn);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Area", true, false);
                        GetNextColumn(CurrentColumn);

                        foreach (DateTime fecha in _model.ListaRangoFechas)
                        {
                            CultureInfo culture = new CultureInfo("es-MX");
                            string DiaSemana = culture.DateTimeFormat.GetDayName(fecha.DayOfWeek);
                            string fechaDia = string.Format("{0}- {1}", DiaSemana.ToString(), fecha.ToShortDateString());
                            SetCell(ws, CurrentColumn, RenglonTitlefechas, fechaDia, true, true);

                            CellRango = GetRango(CurrentColumn, RenglonTitlefechas, GetNextColumn(Columnas, CurrentColumn, 2), RenglonTitlefechas);
                            SetRangeMerge(ws, CellRango, false);

                            SetCell(ws, CurrentColumn, RenglonTitle, "Entrada", true, true);
                            CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                            SetCell(ws, CurrentColumn, RenglonTitle, "Salida", true, true);
                            CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                            SetCell(ws, CurrentColumn, RenglonTitle, "Horas laboradas", true, true, XLColor.FromHtml("#428bca"), XLColor.Navy);

                            CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1); 
                        }
                        CultureInfo ci = new CultureInfo("es-MX");
                        var DiaInicial = ci.DateTimeFormat.GetDayName(_model.ListaRangoFechas.Select(g => g).Min().DayOfWeek);
                        var DiaFinal = ci.DateTimeFormat.GetDayName(_model.ListaRangoFechas.Select(g => g).Max().DayOfWeek);

                        var periodo = string.Format("{0} {1} al {2} {3}", DiaInicial.ToString(), _model.ListaRangoFechas.Select(g => g).Min().ToShortDateString(), DiaFinal.ToString(), _model.ListaRangoFechas.Select(g => g).Max().ToShortDateString());

                        SetCell(ws, CurrentColumn, RenglonTitlefechas, "Periodo del " + periodo, true, true, XLColor.Navy, XLColor.White);
                        CellRango = GetRango(CurrentColumn, RenglonTitlefechas, GetNextColumn(Columnas, CurrentColumn, 4), RenglonTitlefechas);
                        SetRangeMerge(ws, CellRango, false);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Horas acumuladas", true, true, XLColor.Navy, XLColor.White);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Total retardos", true, true, XLColor.Navy, XLColor.White);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Faltas por retardos", true, true, XLColor.Navy, XLColor.White);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Faltas por inasistencia", true, true, XLColor.Navy, XLColor.White);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Total faltas", true, true, XLColor.Navy, XLColor.White);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        CurrentColumn = ColumnaInicial;
                    }

                    SetCell(ws, CurrentColumn, Renglon.ToString(), jefe.Nombre, false, false);
                    RowSpan = lstEmpleadosACargo.Count() == 1 ? Renglon : Renglon + (lstEmpleadosACargo.Count() - 1);
                    CellRango = GetRango(CurrentColumn, Renglon.ToString(), CurrentColumn, RowSpan.ToString());
                    SetRangeMerge(ws, CellRango, false);

                    foreach (Empleado_Asistencia empleado in lstEmpleadosACargo)
                    {
                        CurrentColumn = GetNextColumn(Columnas, ColumnaInicial, 1);

                        SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.Nombre, false, false);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.Area, false, true);

                        string valorCelda = string.Empty;

                        foreach (DateTime fecha in _model.ListaRangoFechas)
                        {

                            TipoIncidenciaHora tipoIncidenciaHora = TipoIncidenciaHora.Ninguna;
                            XLColor BackGroudColor = XLColor.White;
                            XLColor FontColor = XLColor.Black;
                            CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);
                            var valor = string.Empty;
                            var _asistencia = empleado.LstDetalleAsistencia.Where(emp => ((DateTime)emp.Fecha).ToShortDateString() == fecha.ToShortDateString()).FirstOrDefault();

                            
                            tipoIncidenciaHora = GetTipoIncidencia(_asistencia, TipoOperacionLector.Entrada);
                            valorCelda = tipoIncidenciaHora == TipoIncidenciaHora.EntradaOk || tipoIncidenciaHora == TipoIncidenciaHora.Retardo || tipoIncidenciaHora == TipoIncidenciaHora.FaltaPorRetardo ? string.Format("'{0}:{1}", _asistencia.Entrada.Split(':')[0].ToString(), _asistencia.Entrada.Split(':')[1].ToString()) : "Sin Registro";

                            BackGroudColor = tipoIncidenciaHora == TipoIncidenciaHora.Retardo ? XLColor.Yellow : tipoIncidenciaHora == TipoIncidenciaHora.FaltaPorRetardo ? XLColor.FromHtml("#ff6961") : tipoIncidenciaHora == TipoIncidenciaHora.FaltaPorInasistencia || tipoIncidenciaHora == TipoIncidenciaHora.NoRegistroEntrada ? XLColor.FromHtml("#98FB98") : XLColor.White;
                            SetCell(ws, CurrentColumn, Renglon.ToString(), valorCelda.ToString(), false, true, BackGroudColor, XLColor.Black);

                            CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                            tipoIncidenciaHora = GetTipoIncidencia(_asistencia, TipoOperacionLector.Salida);
                            //valorCelda = tipoIncidenciaHora == TipoIncidenciaHora.SalidaOk ? string.Format("'{0}:{1}", _asistencia.Salida.Split(':')[0].ToString(), _asistencia.Salida.Split(':')[1].ToString()):"Sin Registro";
                            //valorCelda = (tipoIncidenciaHora == TipoIncidenciaHora.SalidaOk || tipoIncidenciaHora == TipoIncidenciaHora.RegistroSalidaAnticipada)? string.Format("'{0}:{1}", _asistencia.Salida.Split(':')[0].ToString(), _asistencia.Salida.Split(':')[1].ToString()) : "Sin Registro";
                            valorCelda = tipoIncidenciaHora == TipoIncidenciaHora.NoRegistroSalida ? "Sin Registro" : (tipoIncidenciaHora == TipoIncidenciaHora.SalidaOk || tipoIncidenciaHora == TipoIncidenciaHora.RegistroSalidaAnticipada) ? string.Format("'{0}:{1}", _asistencia.Salida.Split(':')[0].ToString(), _asistencia.Salida.Split(':')[1].ToString()) : "Sin Registro";
                            BackGroudColor = tipoIncidenciaHora != TipoIncidenciaHora.SalidaOk || tipoIncidenciaHora == TipoIncidenciaHora.RegistroSalidaAnticipada ? XLColor.FromHtml("#98FB98") : XLColor.White;
                            SetCell(ws, CurrentColumn, Renglon.ToString(), valorCelda.ToString(), false, true, BackGroudColor, XLColor.Black);

                            CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                            valorCelda = _asistencia == null ? "0": _asistencia.HorasLaboradas != "0" ? string.Format("'{0}:{1}", _asistencia.HorasLaboradas.Split(':')[0].ToString(), _asistencia.HorasLaboradas.Split(':')[1].ToString()) : "0";

                            FontColor = _asistencia == null || _asistencia.HorasLaboradas =="0" ? XLColor.Red : XLColor.Navy;
                            SetCell(ws, CurrentColumn, Renglon.ToString(), valorCelda.ToString(), false, true, XLColor.White, FontColor);
                        }

                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);
                        
                        SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.HorasAcumuladas=="0" ? empleado.HorasAcumuladas: string.Format("'{0}:{1}", empleado.HorasAcumuladas.Split(':')[0].ToString(), empleado.HorasAcumuladas.Split(':')[1].ToString()), false, true, XLColor.White,  FormatHorasAcumuladas(empleado.HorasAcumuladas));
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);
                        
                        SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.TotalRetardos.ToString(), false, true, empleado.TotalRetardos > 0 ? XLColor.Yellow : XLColor.White,  XLColor.Black);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);
                        
                        SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.TotalFaltasPorRetardos.ToString(), false, true, empleado.TotalFaltasPorRetardos > 0 ? XLColor.FromHtml("#ff6961") : XLColor.White, XLColor.Black);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.TotlaFaltasInAsistencia.ToString(), false, true);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);
                        
                        SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.TotalFaltas.ToString(), false, true, empleado.TotalFaltas > 0 ? XLColor.FromHtml("#ff6961") : XLColor.White, XLColor.Black);

                        Renglon++;
                    }
                    CurrentColumn = ColumnaInicial;

                }

                //var columnaA = ws.Column("A");
                //columnaA.AdjustToContents();
                //ws.Column(2).AdjustToContents();
                //ws.Column(3).AdjustToContents();
                ws.Columns().AdjustToContents();

                var archivo = String.Format("{0}-{1}.xlsx", DateTime.Now.ToString("yyyyMMdd-HHmmss"), Area);
                OutPutFile = _Path + archivo;
                workbook.SaveAs(OutPutFile);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private void GetNextColumn(string ValueIndex)
        {
            string[] Columnas = GetColumnasExcel();
            CurrentColumn = Columnas[(Array.FindIndex(Columnas, row => row == ValueIndex)) + 1];
        }


        private void GetNextColumn1(string ValueIndex)
        {
            string[] Columnas = GetColumnasExcel();
            CurrentColumn = Columnas[(Array.FindIndex(Columnas, row => row == ValueIndex)) + 1];
        }

        private string GetNextColumn(string[] Columnas,string ValueIndex, int posiciones)
        {
            return Columnas[(Array.FindIndex(Columnas, row => row == ValueIndex)) + posiciones];
        }

        private void GetNextCurrentColumn(string[] Columnas, string ValueIndex, int posiciones)
        {

            CurrentColumn = Columnas[(Array.FindIndex(Columnas, row => row == ValueIndex)) + posiciones];
        }

        private string SetCell (IXLWorksheet ws, string Col, string Ren, string Value, bool boolTitle, bool boolCenter, XLColor BackGroudColor, XLColor FontColor)
        {

            string Celda = string.Format("{0}{1}", Col, Ren);
            ws.Cell(Celda).Value = Value;
            ws.Cell(Celda).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell(Celda).Style.Font.FontName = "Arial";
            ws.Cell(Celda).Style.Font.FontSize = 10;
            ws.Cell(Celda).Style.Fill.BackgroundColor = BackGroudColor;
            ws.Cell(Celda).Style.Font.FontColor = FontColor;
           // ws.Cell(Celda).Style.Font.Bold = true;
            if (boolCenter)
            {
                ws.Cell(Celda).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(Celda).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }
            return Celda;
        }

        private string SetCell(IXLWorksheet ws, string Col, string Ren, string Value, bool boolTitle, bool boolCenter)
        {

            string Celda = string.Format("{0}{1}", Col, Ren);
            ws.Cell(Celda).Value = Value;
            ws.Cell(Celda).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell(Celda).Style.Font.FontName = "Arial";
            ws.Cell(Celda).Style.Font.FontSize = 10;

            if (boolTitle)
            {
                ws.Cell(Celda).Style.Fill.BackgroundColor = XLColor.FromHtml("#428bca");
                ws.Cell(Celda).Style.Font.FontColor = XLColor.White;
            }
            if (boolCenter)
            {
                ws.Cell(Celda).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(Celda).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }
            return Celda;
        }

        private string SetCell(IXLWorksheet ws, string Col, string Ren, string Value, bool boolSinRegistro)
        {

            string Celda = string.Format("{0}{1}", Col, Ren);
            ws.Cell(Celda).Value = Value;
            ws.Cell(Celda).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell(Celda).Style.Font.FontName = "Arial";
            ws.Cell(Celda).Style.Font.FontSize = 10;

            if (boolSinRegistro)
                ws.Cell(Celda).Style.Font.FontColor = XLColor.Red;

            ws.Cell(Celda).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell(Celda).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            return Celda;
        }


        private void SetRange(IXLWorksheet ws, string Rango)
        {

            ws.Range(Rango).Style.Border.OutsideBorderColor = XLColor.Gray;
            ws.Range(Rango).Style.Font.FontName = "Arial";
            ws.Range(Rango).Style.Font.FontSize = 10;
            
        }

        private void SetRangeMerge(IXLWorksheet ws, string Rango , bool boolTitle)
        {

            ws.Range(Rango).Merge();
            ws.Range(Rango).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range(Rango).Style.Border.OutsideBorderColor = XLColor.Gray;
            if (boolTitle)
            {
                ws.Range(Rango).Style.Fill.BackgroundColor = XLColor.FromHtml("#428bca");
            }
            
            ws.Range(Rango).Style.Font.FontName = "Arial";
            ws.Range(Rango).Style.Font.FontSize = 10;
            ws.Range(Rango).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range(Rango).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        }


        private Result CreaExcelAreaSinJefes1(List<Empleado_Asistencia> lstEmpleadosArea, string Area, out string OutPutFile)
        {
            Result result = new Result();
            OutPutFile = string.Empty;


            try
            {
                var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Reporte Asistencia");
                string[] Columnas = new string[] {
                     "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                    ,"AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ"
                    ,"BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ"
                    ,"CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ"
                    ,"DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ"
                    ,"EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ"
                    ,"FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX", "FY", "FZ"
                    ,"GA", "GB", "GC", "GD", "GE", "GF", "GG", "GH", "GI", "GJ", "GK", "GL", "GM", "GN", "GO", "GP", "GQ", "GR", "GS", "GT", "GU", "GV", "GW", "GX", "GY", "GZ"
                    ,"HA", "HB", "HC", "HD", "HE", "HF", "HG", "HH", "HI", "HJ", "HK", "HL", "HM", "HN", "HO", "HP", "HQ", "HR", "HS", "HT", "HU", "HV", "HW", "HX", "HY", "HZ"
                    ,"IA", "IB", "IC", "ID", "IE", "IF", "IG", "IH", "II", "IJ", "IK", "IL", "IM", "IN", "IO", "IP", "IQ", "IR", "IS", "IT", "IU", "IV", "IW", "IX", "IY", "IZ"
                    ,"JA", "JB", "JC", "JD", "JE", "JF", "JG", "JH", "JI", "JJ", "JK", "JL", "JM", "JN", "JO", "JP", "JQ", "JR", "JS", "JT", "JU", "JV", "JW", "JX", "JY", "JZ"
                    ,"KA", "KB", "KC", "KD", "KE", "KF", "KG", "KH", "KI", "KJ", "KK", "KL", "KM", "KN", "KO", "KP", "KQ", "KR", "KS", "KT", "KU", "KV", "KW", "KX", "KY", "KZ"
                    ,"LA", "LB", "LC", "LD", "LE", "LF", "LG", "LH", "LI", "LJ", "LK", "LL", "LM", "LN", "LO", "LP", "LQ", "LR", "LS", "LT", "LU", "LV", "LW", "LX", "LY", "LZ"
                    ,"MA", "MB", "MC", "MD", "ME", "MF", "MG", "MH", "MI", "MJ", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY", "MZ"
                    ,"NA", "NB", "NC", "ND", "NE", "NF", "NG", "NH", "NI", "NJ", "NK", "NL", "NM", "NN", "NO", "NP", "NQ", "NR", "NS", "NT", "NU", "NV", "NW", "NX", "NY", "NZ"
                    ,"OA", "OB", "OC", "OD", "OE", "OF", "OG", "OH", "OI", "OJ", "OK", "OL", "OM", "ON", "OO", "OP", "OQ", "OR", "OS", "OT", "OU", "OV", "OW", "OX", "OY", "OZ"
                    ,"PA", "PB", "PC", "PD", "PE", "PF", "PG", "PH", "PI", "PJ", "PK", "PL", "PM", "PN", "PO", "PP", "PQ", "PR", "PS", "PT", "PU", "PV", "PW", "PX", "PY", "PZ"
                    ,"QA", "QB", "QC", "QD", "QE", "QF", "QG", "QH", "QI", "QJ", "QK", "QL", "QM", "QN", "QO", "QP", "QQ", "QR", "QS", "QT", "QU", "QV", "QW", "QX", "QY", "QZ"
                    ,"RA", "RB", "RC", "RD", "RE", "RF", "RG", "RH", "RI", "RJ", "RK", "RL", "RM", "RN", "RO", "RP", "RQ", "RR", "RS", "RT", "RU", "RV", "RW", "RX", "RY", "RZ"
                    ,"SA", "SB", "SC", "SD", "SE", "SF", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SP", "SQ", "SR", "SS", "ST", "SU", "SV", "SW", "SX", "SY", "SZ"
                    ,"TA", "TB", "TC", "TD", "TE", "TF", "TG", "TH", "TI", "TJ", "TK", "TL", "TM", "TN", "TO", "TP", "TQ", "TR", "TS", "TT", "TU", "TV", "TW", "TX", "TY", "TZ"
                    ,"UA", "UB", "UC", "UD", "UE", "UF", "UG", "UH", "UI", "UJ", "UK", "UL", "UM", "UN", "UO", "UP", "UQ", "UR", "US", "UT", "UU", "UV", "UW", "UX", "UY", "UZ"
                    ,"VA", "VB", "VC", "VD", "VE", "VF", "VG", "VH", "VI", "VJ", "VK", "VL", "VM", "VN", "VO", "VP", "VQ", "VR", "VS", "VT", "VU", "VV", "VW", "VX", "VY", "VZ"
                    ,"WA", "WB", "WC", "WD", "WE", "WF", "WG", "WH", "WI", "WJ", "WK", "WL", "WM", "WN", "WO", "WP", "WQ", "WR", "WS", "WT", "WU", "WV", "WW", "WX", "WY", "WZ"
                    ,"XA", "XB", "XC", "XD", "XE", "XF", "XG", "XH", "XI", "XJ", "XK", "XL", "XM", "XN", "XO", "XP", "XQ", "XR", "XS", "XT", "XU", "XV", "XW", "XX", "XY", "XZ"
                    ,"YA", "YB", "YC", "YD", "YE", "YF", "YG", "YH", "YI", "YJ", "YK", "YL", "YM", "YN", "YO", "YP", "YQ", "YR", "YS", "YT", "YU", "YV", "YW", "YX", "YY", "YZ"
                    ,"ZA", "ZB", "ZC", "ZD", "ZE", "ZF", "ZG", "ZH", "ZI", "ZJ", "ZK", "ZL", "ZM", "ZN", "ZO", "ZP", "ZQ", "ZR", "ZS", "ZT", "ZU", "ZV", "ZW", "ZX", "ZY", "ZZ"
                    ,"AAA", "AAB", "AAC", "AAD", "AAE", "AAF", "AAG", "AAH", "AAI", "AAJ", "AAK", "AAL", "AAM", "AAN", "AAO", "AAP", "AAQ", "AAR", "AAS", "AAT", "AAU", "AAV", "AAW", "AAX", "AAY", "AAZ"
                    ,"ABA", "ABB", "ABC", "ABD", "ABE", "ABF", "ABG", "ABH", "ABI", "ABJ", "ABK", "ABL", "ABM", "ABN", "ABO", "ABP", "ABQ", "ABR", "ABS", "ABT", "ABU", "ABV", "ABW", "ABX", "ABY", "ABZ"
                    ,"ACA", "ACB", "ACC", "ACD", "ACE", "ACF", "ACG", "ACH", "ACI", "ACJ", "ACK", "ACL", "ACM", "ACN", "ACO", "ACP", "ACQ", "ACR", "ACS", "ACT", "ACU", "ACV", "ACW", "ACX", "ACY", "ACZ"
                    ,"ADA", "ADB", "ADC", "ADD", "ADE", "ADF", "ADG", "ADH", "ADI", "ADJ", "ADK", "ADL", "ADM", "ADN", "ADO", "ADP", "ADQ", "ADR", "ADS", "ADT", "ADU", "ADV", "ADW", "ADX", "ADY", "ADZ"
                    ,"AEA", "AEB", "AEC", "AED", "AEE", "AEF", "AEG", "AEH", "AEI", "AEJ", "AEK", "AEL", "AEM", "AEN", "AEO", "AEP", "AEQ", "AER", "AES", "AET", "AEU", "AEV", "AEW", "AEX", "AEY", "AEZ"
                    ,"AFA", "AFB", "AFC", "AFD", "AFE", "AFF", "AFG", "AFH", "AFI", "AFJ", "AFK", "AFL", "AFM", "AFN", "AFO", "AFP", "AFQ", "AFR", "AFS", "AFT", "AFU", "AFV", "AFW", "AFX", "AFY", "AFZ"
                    ,"AGA", "AGB", "AGC", "AGD", "AGE", "AGF", "AGG", "AGH", "AGI", "AGJ", "AGK", "AGL", "AGM", "AGN", "AGO", "AGP", "AGQ", "AGR", "AGS", "AGT", "AGU", "AGV", "AGW", "AGX", "AGY", "AGZ"
                    ,"AHA", "AHB", "AHC", "AHD", "AHE", "AHF", "AHG", "AHH", "AHI", "AHJ", "AHK", "AHL", "AHM", "AHN", "AHO", "AHP", "AHQ", "AHR", "AHS", "AHT", "AHU", "AHV", "AHW", "AHX", "AHY", "AHZ"
                    ,"AIA", "AIB", "AIC", "AID", "AIE", "AIF", "AIG", "AIH", "AII", "AIJ", "AIK", "AIL", "AIM", "AIN", "AIO", "AIP", "AIQ", "AIR", "AIS", "AIT", "AIU", "AIV", "AIW", "AIX", "AIY", "AIZ"
                    ,"AJA", "AJB", "AJC", "AJD", "AJE", "AJF", "AJG", "AJH", "AJI", "AJJ", "AJK", "AJL", "AJM", "AJN", "AJO", "AJP", "AJQ", "AJR", "AJS", "AJT", "AJU", "AJV", "AJW", "AJX", "AJY", "AJZ"
                    ,"AKA", "AKB", "AKC", "AKD", "AKE", "AKF", "AKG", "AKH", "AKI", "AKJ", "AKK", "AKL", "AKM", "AKN", "AKO", "AKP", "AKQ", "AKR", "AKS", "AKT", "AKU", "AKV", "AKW", "AKX", "AKY", "AKZ"
                    ,"ALA", "ALB", "ALC", "ALD", "ALE", "ALF", "ALG", "ALH", "ALI", "ALJ", "ALK", "ALL", "ALM", "ALN", "ALO", "ALP", "ALQ", "ALR", "ALS", "ALT", "ALU", "ALV", "ALW", "ALX", "ALY", "ALZ"
                    ,"AMA", "AMB", "AMC", "AMD", "AME", "AMF", "AMG", "AMH", "AMI", "AMJ", "AMK", "AML", "AMM", "AMN", "AMO", "AMP", "AMQ", "AMR", "AMS", "AMT", "AMU", "AMV", "AMW", "AMX", "AMY", "AMZ"
                    ,"ANA", "ANB", "ANC", "AND", "ANE", "ANF", "ANG", "ANH", "ANI", "ANJ", "ANK", "ANL", "ANM", "ANN", "ANO", "ANP", "ANQ", "ANR", "ANS", "ANT", "ANU", "ANV", "ANW", "ANX", "ANY", "ANZ"
                    ,"AOA", "AOB", "AOC", "AOD", "AOE", "AOF", "AOG", "AOH", "AOI", "AOJ", "AOK", "AOL", "AOM", "AON", "AOO", "AOP", "AOQ", "AOR", "AOS", "AOT", "AOU", "AOV", "AOW", "AOX", "AOY", "AOZ"
                    ,"APA", "APB", "APC", "APD", "APE", "APF", "APG", "APH", "API", "APJ", "APK", "APL", "APM", "APN", "APO", "APP", "APQ", "APR", "APS", "APT", "APU", "APV", "APW", "APX", "APY", "APZ"
                    ,"AQA", "AQB", "AQC", "AQD", "AQE", "AQF", "AQG", "AQH", "AQI", "AQJ", "AQK", "AQL", "AQM", "AQN", "AQO", "AQP", "AQQ", "AQR", "AQS", "AQT", "AQU", "AQV", "AQW", "AQX", "AQY", "AQZ"
                    ,"ARA", "ARB", "ARC", "ARD", "ARE", "ARF", "ARG", "ARH", "ARI", "ARJ", "ARK", "ARL", "ARM", "ARN", "ARO", "ARP", "ARQ", "ARR", "ARS", "ART", "ARU", "ARV", "ARW", "ARX", "ARY", "ARZ"
                    ,"ASA", "ASB", "ASC", "ASD", "ASE", "ASF", "ASG", "ASH", "ASI", "ASJ", "ASK", "ASL", "ASM", "ASN", "ASO", "ASP", "ASQ", "ASR", "ASS", "AST", "ASU", "ASV", "ASW", "ASX", "ASY", "ASZ"
                    ,"ATA", "ATB", "ATC", "ATD", "ATE", "ATF", "ATG", "ATH", "ATI", "ATJ", "ATK", "ATL", "ATM", "ATN", "ATO", "ATP", "ATQ", "ATR", "ATS", "ATT", "ATU", "ATV", "ATW", "ATX", "ATY", "ATZ"
                    ,"AUA", "AUB", "AUC", "AUD", "AUE", "AUF", "AUG", "AUH", "AUI", "AUJ", "AUK", "AUL", "AUM", "AUN", "AUO", "AUP", "AUQ", "AUR", "AUS", "AUT", "AUU", "AUV", "AUW", "AUX", "AUY", "AUZ"
                    ,"AWA", "AWB", "AWC", "AWD", "AWE", "AWF", "AWG", "AWH", "AWI", "AWJ", "AWK", "AWL", "AWM", "AWN", "AWO", "AWP", "AWQ", "AWR", "AWS", "AWT", "AWU", "AWV", "AWW", "AWX", "AWY", "AWZ"
                    ,"AVA", "AVB", "AVC", "AVD", "AVE", "AVF", "AVG", "AVH", "AVI", "AVJ", "AVK", "AVL", "AVM", "AVN", "AVO", "AVP", "AVQ", "AVR", "AVS", "AVT", "AVU", "AVV", "AVW", "AVX", "AVY", "AVZ"
                    ,"AXA", "AXB", "AXC", "AXD", "AXE", "AXF", "AXG", "AXH", "AXI", "AXJ", "AXK", "AXL", "AXM", "AXN", "AXO", "AXP", "AXQ", "AXR", "AXS", "AXT", "AXU", "AXV", "AXW", "AXX", "AXY", "AXZ"
                    ,"AYA", "AYB", "AYC", "AYD", "AYE", "AYF", "AYG", "AYH", "AYI", "AYJ", "AYK", "AYL", "AYM", "AYN", "AYO", "AYP", "AYQ", "AYR", "AYS", "AYT", "AYU", "AYV", "AYW", "AYX", "AYY", "AYZ"
                    ,"AZA", "AZB", "AZC", "AZD", "AZE", "AZF", "AZG", "AZH", "AZI", "AZJ", "AZK", "AZL", "AZM", "AZN", "AZO", "AZP", "AZQ", "AZR", "AZS", "AZT", "AZU", "AZV", "AZW", "AZX", "AZY", "AZZ"
                };

                string ColumnaInicial = "A";
                int RenglonInicial = 3;

                int Renglon = 0;
                string RenglonTitle = (Convert.ToInt32(RenglonInicial) - 1).ToString();
                string RenglonTitlefechas = (Convert.ToInt32(RenglonInicial) - 2).ToString();

                //string celdaInicial = string.Empty;
                //string celdaTitle = string.Empty;
                string CellRango = string.Empty;

         
                Renglon = RenglonInicial;
                CurrentColumn = ColumnaInicial;

                //Por cada Jefe
                foreach (Empleado_Asistencia empleado in lstEmpleadosArea)
                {
                    if (Renglon == RenglonInicial)
                    {

                        SetCell(ws, CurrentColumn, RenglonTitle, "Nombre", true, false);
                        GetNextColumn(CurrentColumn);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Area", true, false);
                        GetNextColumn(CurrentColumn);

                        foreach (DateTime fecha in _model.ListaRangoFechas)
                        {
                            CultureInfo culture = new CultureInfo("es-MX");
                            string DiaSemana = culture.DateTimeFormat.GetDayName(fecha.DayOfWeek);
                            string fechaDia = string.Format("{0}- {1}", DiaSemana.ToString(), fecha.ToShortDateString());
                            SetCell(ws, CurrentColumn, RenglonTitlefechas, fechaDia, true, true);

                            CellRango = GetRango(CurrentColumn, RenglonTitlefechas, GetNextColumn(Columnas, CurrentColumn, 2), RenglonTitlefechas);
                            SetRangeMerge(ws, CellRango, false);

                            SetCell(ws, CurrentColumn, RenglonTitle, "Entrada", true, true);
                            CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                            SetCell(ws, CurrentColumn, RenglonTitle, "Salida", true, true);
                            CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                            SetCell(ws, CurrentColumn, RenglonTitle, "Horas laboradas", true, true, XLColor.FromHtml("#428bca"), XLColor.Navy);

                            CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);
                        }
                        CultureInfo ci = new CultureInfo("es-MX");
                        var DiaInicial = ci.DateTimeFormat.GetDayName(_model.ListaRangoFechas.Select(g => g).Min().DayOfWeek);
                        var DiaFinal = ci.DateTimeFormat.GetDayName(_model.ListaRangoFechas.Select(g => g).Max().DayOfWeek);

                        var periodo = string.Format("{0} {1} al {2} {3}", DiaInicial.ToString(), _model.ListaRangoFechas.Select(g => g).Min().ToShortDateString(), DiaFinal.ToString(), _model.ListaRangoFechas.Select(g => g).Max().ToShortDateString());

                        SetCell(ws, CurrentColumn, RenglonTitlefechas, "Periodo del " + periodo, true, true, XLColor.Navy, XLColor.White);
                        CellRango = GetRango(CurrentColumn, RenglonTitlefechas, GetNextColumn(Columnas, CurrentColumn, 4), RenglonTitlefechas);
                        SetRangeMerge(ws, CellRango, false);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Horas acumuladas", true, true, XLColor.Navy, XLColor.White);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Total retardos", true, true, XLColor.Navy, XLColor.White);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Faltas por retardos", true, true, XLColor.Navy, XLColor.White);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Faltas por inasistencia", true, true, XLColor.Navy, XLColor.White);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        SetCell(ws, CurrentColumn, RenglonTitle, "Total faltas", true, true, XLColor.Navy, XLColor.White);
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        CurrentColumn = ColumnaInicial;
                    }

                   
                    SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.Nombre, false, false);
                    CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.Area, false, true);

                    string valorCelda = string.Empty;

                    foreach (DateTime fecha in _model.ListaRangoFechas)
                    {

                        TipoIncidenciaHora tipoIncidenciaHora = TipoIncidenciaHora.Ninguna;
                        XLColor BackGroudColor = XLColor.White;
                        XLColor FontColor = XLColor.Black;
                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);
                        var valor = string.Empty;
                        var _asistencia = empleado.LstDetalleAsistencia.Where(emp => ((DateTime)emp.Fecha).ToShortDateString() == fecha.ToShortDateString()).FirstOrDefault();


                        tipoIncidenciaHora = GetTipoIncidencia(_asistencia, TipoOperacionLector.Entrada);
                        valorCelda = tipoIncidenciaHora == TipoIncidenciaHora.EntradaOk || tipoIncidenciaHora == TipoIncidenciaHora.Retardo || tipoIncidenciaHora == TipoIncidenciaHora.FaltaPorRetardo ? string.Format("'{0}:{1}", _asistencia.Entrada.Split(':')[0].ToString(), _asistencia.Entrada.Split(':')[1].ToString()) : "Sin Registro";

                        BackGroudColor = tipoIncidenciaHora == TipoIncidenciaHora.Retardo ? XLColor.Yellow : tipoIncidenciaHora == TipoIncidenciaHora.FaltaPorRetardo ? XLColor.FromHtml("#ff6961") : tipoIncidenciaHora == TipoIncidenciaHora.FaltaPorInasistencia || tipoIncidenciaHora == TipoIncidenciaHora.NoRegistroEntrada ? XLColor.FromHtml("#98FB98") : XLColor.White;
                        SetCell(ws, CurrentColumn, Renglon.ToString(), valorCelda.ToString(), false, true, BackGroudColor, XLColor.Black);

                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        tipoIncidenciaHora = GetTipoIncidencia(_asistencia, TipoOperacionLector.Salida);
                        valorCelda =  tipoIncidenciaHora == TipoIncidenciaHora.NoRegistroSalida ? "Sin Registro":(tipoIncidenciaHora == TipoIncidenciaHora.SalidaOk || tipoIncidenciaHora == TipoIncidenciaHora.RegistroSalidaAnticipada) ? string.Format("'{0}:{1}", _asistencia.Salida.Split(':')[0].ToString(), _asistencia.Salida.Split(':')[1].ToString()) : "Sin Registro";
                        BackGroudColor = tipoIncidenciaHora != TipoIncidenciaHora.SalidaOk || tipoIncidenciaHora == TipoIncidenciaHora.RegistroSalidaAnticipada ? XLColor.FromHtml("#98FB98") : XLColor.White;
                        SetCell(ws, CurrentColumn, Renglon.ToString(), valorCelda.ToString(), false, true, BackGroudColor, XLColor.Black);

                        CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                        valorCelda = _asistencia == null ? "0" : _asistencia.HorasLaboradas != "0" ? string.Format("'{0}:{1}", _asistencia.HorasLaboradas.Split(':')[0].ToString(), _asistencia.HorasLaboradas.Split(':')[1].ToString()) : "0";

                        FontColor = _asistencia == null || _asistencia.HorasLaboradas == "0" ? XLColor.Red : XLColor.Navy;
                        SetCell(ws, CurrentColumn, Renglon.ToString(), valorCelda.ToString(), false, true, XLColor.White, FontColor);
                    }

                    CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.HorasAcumuladas == "0" ? empleado.HorasAcumuladas : string.Format("'{0}:{1}", empleado.HorasAcumuladas.Split(':')[0].ToString(), empleado.HorasAcumuladas.Split(':')[1].ToString()), false, true, XLColor.White, FormatHorasAcumuladas(empleado.HorasAcumuladas));
                    CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.TotalRetardos.ToString(), false, true, empleado.TotalRetardos > 0 ? XLColor.Yellow : XLColor.White, XLColor.Black);
                    CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.TotalFaltasPorRetardos.ToString(), false, true, empleado.TotalFaltasPorRetardos > 0 ? XLColor.FromHtml("#ff6961") : XLColor.White, XLColor.Black);
                    CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.TotlaFaltasInAsistencia.ToString(), false, true);
                    CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.TotalFaltas.ToString(), false, true, empleado.TotalFaltas > 0 ? XLColor.FromHtml("#ff6961") : XLColor.White, XLColor.Black);

                    //foreach (DateTime fecha in _model.ListaRangoFechas)
                    //{
                    //    CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);
                    //    var valor = string.Empty;
                    //    var _asistencia = empleado.LstDetalleAsistencia.Where(emp => ((DateTime)emp.Fecha).ToShortDateString() == fecha.ToShortDateString()).FirstOrDefault();
                    //    //valorCelda = _asistencia == null || string.IsNullOrEmpty(_asistencia.Entrada) ? "Sin Registro" : _asistencia.Entrada;
                    //    valorCelda = _asistencia == null ? "Sin Registro" : _asistencia.Entrada == _asistencia.Salida && ComparaFechas(_asistencia.Entrada, "15:00") == -1 ? _asistencia.Entrada : "Sin Registro";

                    //    if (valorCelda == "Sin Registro")
                    //    {
                    //        SetCell(ws, CurrentColumn, Renglon.ToString(), valorCelda, true);
                    //    }
                    //    else
                    //    {
                    //        valor = string.Format("'{0}:{1}", valorCelda.Split(':')[0].ToString(), valorCelda.Split(':')[1].ToString());
                    //        SetCell(ws, CurrentColumn, Renglon.ToString(), valor.ToString(), false, true);
                    //    }

                    //    // SetCell(ws, CurrentColumn, Renglon.ToString(), valorCelda, false, true);
                    //    CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    //    //valorCelda = _asistencia == null || string.IsNullOrEmpty(_asistencia.Salida) || _asistencia.Entrada == _asistencia.Salida ? "Sin Registro" : _asistencia.Salida;
                    //    valorCelda = _asistencia == null ? "Sin Registro" : _asistencia.Salida;

                    //    if (valorCelda == "Sin Registro")
                    //    {
                    //        SetCell(ws, CurrentColumn, Renglon.ToString(), valorCelda, true);
                    //    }
                    //    else
                    //    {
                    //        valor = string.Format("'{0}:{1}", valorCelda.Split(':')[0].ToString(), valorCelda.Split(':')[1].ToString());
                    //        SetCell(ws, CurrentColumn, Renglon.ToString(), valor.ToString(), false, true);
                    //    }
                    //    CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    //    valorCelda = _asistencia != null ? _asistencia.HorasLaboradas : "0";

                    //    if (valorCelda == "0")
                    //    {
                    //        SetCell(ws, CurrentColumn, Renglon.ToString(), valorCelda, true);
                    //    }
                    //    else
                    //    {
                    //        valor = string.Format("'{0}:{1}", valorCelda.Split(':')[0].ToString(), valorCelda.Split(':')[1].ToString());
                    //        SetCell(ws, CurrentColumn, Renglon.ToString(), valor.ToString(), false, true);
                    //    }
                    //}

                    //CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    //SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.HorasAcumuladas == "0" ? empleado.HorasAcumuladas : string.Format("'{0}:{1}", empleado.HorasAcumuladas.Split(':')[0].ToString(), empleado.HorasAcumuladas.Split(':')[1].ToString()), false, true, XLColor.White, FormatHorasAcumuladas(empleado.HorasAcumuladas));
                    //CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    //SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.TotalRetardos.ToString(), false, true, XLColor.White, empleado.TotalRetardos > 1 ? XLColor.Red : XLColor.Black);
                    //CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    //SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.TotalFaltasPorRetardos.ToString(), false, true, XLColor.White, empleado.TotalFaltasPorRetardos > 1 ? XLColor.Red : XLColor.Black);
                    //CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    //SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.TotlaFaltasInAsistencia.ToString(), false, true);
                    //CurrentColumn = GetNextColumn(Columnas, CurrentColumn, 1);

                    //SetCell(ws, CurrentColumn, Renglon.ToString(), empleado.TotalFaltas.ToString(), false, true, XLColor.White, empleado.TotalFaltas > 1 ? XLColor.Red : XLColor.Black);

                    Renglon++;

                    CurrentColumn = ColumnaInicial;
                }


                //var columnaA = ws.Column("A");
                //columnaA.AdjustToContents();
                //ws.Column(2).AdjustToContents();
                //ws.Column(3).AdjustToContents();
                ws.Columns().AdjustToContents();
                var archivo = String.Format("{0}-{1}.xlsx", DateTime.Now.ToString("yyyyMMdd-HHmmss"), Area);
                OutPutFile = _Path + archivo;
                workbook.SaveAs(OutPutFile);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private Result CreaExcelAreaSinJefes(List<Empleado_Asistencia> lstEmpleadosArea, string Area, out string OutPutFile)
        {
            Result result = new Result();
            OutPutFile = string.Empty;
            try
            {
                var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Reporte Asistencia");
                string[] Columnas = new string[] {
                     "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                    ,"AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ"
                    ,"BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ"
                    ,"CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ"
                    ,"DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ"
                    ,"EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ"
                    ,"FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX", "FY", "FZ"
                    ,"GA", "GB", "GC", "GD", "GE", "GF", "GG", "GH", "GI", "GJ", "GK", "GL", "GM", "GN", "GO", "GP", "GQ", "GR", "GS", "GT", "GU", "GV", "GW", "GX", "GY", "GZ"
                    ,"HA", "HB", "HC", "HD", "HE", "HF", "HG", "HH", "HI", "HJ", "HK", "HL", "HM", "HN", "HO", "HP", "HQ", "HR", "HS", "HT", "HU", "HV", "HW", "HX", "HY", "HZ"
                    ,"IA", "IB", "IC", "ID", "IE", "IF", "IG", "IH", "II", "IJ", "IK", "IL", "IM", "IN", "IO", "IP", "IQ", "IR", "IS", "IT", "IU", "IV", "IW", "IX", "IY", "IZ"
                    ,"JA", "JB", "JC", "JD", "JE", "JF", "JG", "JH", "JI", "JJ", "JK", "JL", "JM", "JN", "JO", "JP", "JQ", "JR", "JS", "JT", "JU", "JV", "JW", "JX", "JY", "JZ"
                    ,"KA", "KB", "KC", "KD", "KE", "KF", "KG", "KH", "KI", "KJ", "KK", "KL", "KM", "KN", "KO", "KP", "KQ", "KR", "KS", "KT", "KU", "KV", "KW", "KX", "KY", "KZ"
                    ,"LA", "LB", "LC", "LD", "LE", "LF", "LG", "LH", "LI", "LJ", "LK", "LL", "LM", "LN", "LO", "LP", "LQ", "LR", "LS", "LT", "LU", "LV", "LW", "LX", "LY", "LZ"
                    ,"MA", "MB", "MC", "MD", "ME", "MF", "MG", "MH", "MI", "MJ", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY", "MZ"
                    ,"NA", "NB", "NC", "ND", "NE", "NF", "NG", "NH", "NI", "NJ", "NK", "NL", "NM", "NN", "NO", "NP", "NQ", "NR", "NS", "NT", "NU", "NV", "NW", "NX", "NY", "NZ"
                    ,"OA", "OB", "OC", "OD", "OE", "OF", "OG", "OH", "OI", "OJ", "OK", "OL", "OM", "ON", "OO", "OP", "OQ", "OR", "OS", "OT", "OU", "OV", "OW", "OX", "OY", "OZ"
                    ,"PA", "PB", "PC", "PD", "PE", "PF", "PG", "PH", "PI", "PJ", "PK", "PL", "PM", "PN", "PO", "PP", "PQ", "PR", "PS", "PT", "PU", "PV", "PW", "PX", "PY", "PZ"
                    ,"QA", "QB", "QC", "QD", "QE", "QF", "QG", "QH", "QI", "QJ", "QK", "QL", "QM", "QN", "QO", "QP", "QQ", "QR", "QS", "QT", "QU", "QV", "QW", "QX", "QY", "QZ"
                    ,"RA", "RB", "RC", "RD", "RE", "RF", "RG", "RH", "RI", "RJ", "RK", "RL", "RM", "RN", "RO", "RP", "RQ", "RR", "RS", "RT", "RU", "RV", "RW", "RX", "RY", "RZ"
                    ,"SA", "SB", "SC", "SD", "SE", "SF", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SP", "SQ", "SR", "SS", "ST", "SU", "SV", "SW", "SX", "SY", "SZ"
                    ,"TA", "TB", "TC", "TD", "TE", "TF", "TG", "TH", "TI", "TJ", "TK", "TL", "TM", "TN", "TO", "TP", "TQ", "TR", "TS", "TT", "TU", "TV", "TW", "TX", "TY", "TZ"
                    ,"UA", "UB", "UC", "UD", "UE", "UF", "UG", "UH", "UI", "UJ", "UK", "UL", "UM", "UN", "UO", "UP", "UQ", "UR", "US", "UT", "UU", "UV", "UW", "UX", "UY", "UZ"
                    ,"VA", "VB", "VC", "VD", "VE", "VF", "VG", "VH", "VI", "VJ", "VK", "VL", "VM", "VN", "VO", "VP", "VQ", "VR", "VS", "VT", "VU", "VV", "VW", "VX", "VY", "VZ"
                    ,"XA", "XB", "XC", "XD", "XE", "XF", "XG", "XH", "XI", "XJ", "XK", "XL", "XM", "XN", "XO", "XP", "XQ", "XR", "XS", "XT", "XU", "XV", "XW", "XX", "XY", "XZ"
                    ,"YA", "YB", "YC", "YD", "YE", "YF", "YG", "YH", "YI", "YJ", "YK", "YL", "YM", "YN", "YO", "YP", "YQ", "YR", "YS", "YT", "YU", "YV", "YW", "YX", "YY", "YZ"
                    ,"ZA", "ZB", "ZC", "ZD", "ZE", "ZF", "ZG", "ZH", "ZI", "ZJ", "ZK", "ZL", "ZM", "ZN", "ZO", "ZP", "ZQ", "ZR", "ZS", "ZT", "ZU", "ZV", "ZW", "ZX", "ZY", "ZZ"
                };

                string ColumnaInicial = "A";
                string RenglonInicial = "3";
                string RenglonActual = string.Empty;
                string Renglon = string.Empty;
                string RenglonTitle = (Convert.ToInt32(RenglonInicial) - 1).ToString();
                string RenglonTitlefechas = (Convert.ToInt32(RenglonInicial) - 2).ToString();

                string celdaInicial = string.Empty;
                string celdaTitle = string.Empty;
                string CellRango = string.Empty;
                string[] ColumnasFinales = Columnas.Take(lstEmpleadosArea.Count()).ToArray();
                int RowSpan = 0;
                //int EmpleadosAcargo = 0;
                Renglon = RenglonInicial;
                var NextColumn = string.Empty;
                foreach (Empleado_Asistencia empleado in lstEmpleadosArea)
                {


                    if (Renglon == RenglonInicial)
                    {
                        NextColumn = ColumnaInicial;
                    }


                    //    celdaInicial = GetCelda(ColumnaInicial, RenglonTitle);
                    //    ws.Cell(celdaInicial).Value = "Jefe Directo";
                    //    ws.Cell(celdaInicial).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //    ws.Cell(celdaInicial).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    //    ws.Cell(celdaInicial).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    //    ws.Cell(celdaInicial).Style.Fill.BackgroundColor = XLColor.FromArgb(0x293352);
                    //    ws.Cell(celdaInicial).Style.Font.FontColor = XLColor.White;
                    //    ws.Cell(celdaInicial).Style.Font.FontName = "arial";
                    //    ws.Cell(celdaInicial).Style.Font.FontSize = 10;
                    //}
                    //NOMBRE JEFE
                    //celdaInicial = GetCelda(ColumnaInicial, Renglon);
                    //ws.Cell(celdaInicial).Value = empleado.Nombre;
                    //ws.Cell(celdaInicial).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    //ws.Cell(celdaInicial).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    //ws.Cell(celdaInicial).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    //ws.Range(celdaInicial).Style.Border.OutsideBorderColor = XLColor.Gray;
                    //ws.Range(celdaInicial).Style.Font.FontName = "Arial";
                    //ws.Range(celdaInicial).Style.Font.FontSize = 10;

                    RowSpan = lstEmpleadosArea.Count() == 1 ? Convert.ToInt32(Renglon) : Convert.ToInt32(Renglon) + (lstEmpleadosArea.Count() - 1);
                    //CellRango = GetRango(ColumnaInicial, Renglon, ColumnaInicial, RowSpan.ToString());
                    //ws.Range(CellRango).Merge();
                    //ws.Range(CellRango).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    //ws.Range(CellRango).Style.Border.OutsideBorderColor = XLColor.Gray;
                    //ws.Range(CellRango).Style.Font.FontName = "Arial";
                    //ws.Range(CellRango).Style.Font.FontSize = 10;


                    int renglon = Convert.ToInt32(Renglon);

                    var NextColumnFechas = NextColumn;


                    if (Renglon == RenglonInicial)
                    {

                        celdaTitle = GetCelda(NextColumn, RenglonTitle);
                        ws.Cell(celdaTitle).Value = "Empleado";
                        ws.Cell(celdaTitle).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(celdaTitle).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(celdaTitle).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(celdaTitle).Style.Fill.BackgroundColor = XLColor.FromArgb(0x293352);
                        ws.Cell(celdaTitle).Style.Font.FontColor = XLColor.White;
                        ws.Cell(celdaTitle).Style.Font.FontName = "Arial";
                        ws.Cell(celdaTitle).Style.Font.FontSize = 10;

                        var NextColumnArea = Columnas[(Array.FindIndex(Columnas, row => row == NextColumn)) + 1];

                        celdaTitle = GetCelda(NextColumnArea, RenglonTitle);
                        ws.Cell(celdaTitle).Value = "Area";
                        ws.Cell(celdaTitle).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(celdaTitle).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(celdaTitle).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(celdaTitle).Style.Fill.BackgroundColor = XLColor.FromArgb(0x293352);
                        ws.Cell(celdaTitle).Style.Font.FontColor = XLColor.White;
                        ws.Cell(celdaTitle).Style.Font.FontName = "Arial";
                        ws.Cell(celdaTitle).Style.Font.FontSize = 10;
                        // NextColumn = Columnas[(Array.FindIndex(Columnas, row => row == NextColumn)) + 1];
                    }

                    var celda = GetCelda(NextColumn, renglon.ToString());
                    ws.Cell(celda).Value = empleado.Nombre;
                    ws.Cell(celda).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(celda).Style.Border.OutsideBorderColor = XLColor.Gray;
                    ws.Range(celda).Style.Font.FontName = "Arial";
                    ws.Range(celda).Style.Font.FontSize = 10;
                    NextColumnFechas = Columnas[(Array.FindIndex(Columnas, row => row == NextColumn)) + 1];


                    var celdaArea = GetCelda(NextColumnFechas, renglon.ToString());
                    ws.Cell(celdaArea).Value = empleado.Area;
                    ws.Cell(celdaArea).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(celdaArea).Style.Border.OutsideBorderColor = XLColor.Gray;
                    ws.Range(celdaArea).Style.Font.FontName = "Arial";
                    ws.Range(celdaArea).Style.Font.FontSize = 10;
                    NextColumnFechas = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnFechas)) + 1];

                    if (Renglon == RenglonInicial)
                    {
                        //titulos fecha
                        var NextColumnTitleFechas = NextColumnFechas;
                        var NextColumnEntradaSalida = NextColumnFechas;
                        var NextColumnHorasLaboradas = NextColumnFechas;

                        foreach (DateTime fecha in _model.ListaRangoFechas)
                        {
                            celdaTitle = GetCelda(NextColumnTitleFechas, RenglonTitlefechas);
                            ws.Cell(celdaTitle).Value = fecha.ToShortDateString();
                            ws.Cell(celdaTitle).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(celdaTitle).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            var NextColumnTitleFechas2 = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnTitleFechas)) + 2];

                            CellRango = GetRango(NextColumnTitleFechas, RenglonTitlefechas, NextColumnTitleFechas2, RenglonTitlefechas);
                            ws.Range(CellRango).Merge();
                            ws.Range(CellRango).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            ws.Range(CellRango).Style.Border.OutsideBorderColor = XLColor.Gray;
                            ws.Range(CellRango).Style.Fill.BackgroundColor = XLColor.FromArgb(0x293352);
                            ws.Range(CellRango).Style.Font.FontColor = XLColor.White;
                            ws.Range(CellRango).Style.Font.FontName = "Arial";
                            ws.Range(CellRango).Style.Font.FontSize = 10;


                            NextColumnTitleFechas = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnTitleFechas2)) + 1];

                            celdaTitle = GetCelda(NextColumnEntradaSalida, RenglonTitle);
                            ws.Cell(celdaTitle).Value = "Entrada";
                            ws.Cell(celdaTitle).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(celdaTitle).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(celdaTitle).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            ws.Cell(celdaTitle).Style.Fill.BackgroundColor = XLColor.FromArgb(0x293352);
                            ws.Cell(celdaTitle).Style.Font.FontColor = XLColor.White;


                            NextColumnEntradaSalida = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnEntradaSalida)) + 1];

                            celdaTitle = GetCelda(NextColumnEntradaSalida, RenglonTitle);
                            ws.Cell(celdaTitle).Value = "Salida";
                            ws.Cell(celdaTitle).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(celdaTitle).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(celdaTitle).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            ws.Cell(celdaTitle).Style.Fill.BackgroundColor = XLColor.FromArgb(0x293352);
                            ws.Cell(celdaTitle).Style.Font.FontColor = XLColor.White;

                            NextColumnHorasLaboradas = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnEntradaSalida)) + 1];

                            // -------------------------------- HORAS LABORADAS------------------------------

                            celdaTitle = GetCelda(NextColumnHorasLaboradas, RenglonTitle);
                            ws.Cell(celdaTitle).Value = "Horas";
                            ws.Cell(celdaTitle).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(celdaTitle).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(celdaTitle).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            ws.Cell(celdaTitle).Style.Fill.BackgroundColor = XLColor.FromArgb(0x293352);
                            ws.Cell(celdaTitle).Style.Font.FontColor = XLColor.White;

                            NextColumnEntradaSalida = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnHorasLaboradas)) + 1];

                        }
                    }

                    foreach (DateTime fecha in _model.ListaRangoFechas)
                    {

                        var _asistencia = empleado.LstDetalleAsistencia.Where(emp => ((DateTime)emp.Fecha).ToShortDateString() == fecha.ToShortDateString()).FirstOrDefault();
                        var celdaEntrada = GetCelda(NextColumnFechas, renglon.ToString());
                        ws.Cell(celdaEntrada).Value = _asistencia == null || string.IsNullOrEmpty(_asistencia.Entrada) ? "Sin Registro" : _asistencia.Entrada;

                        ws.Cell(celdaEntrada).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(celdaEntrada).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(celdaEntrada).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Range(celdaEntrada).Style.Border.OutsideBorderColor = XLColor.Gray;
                        ws.Range(celdaEntrada).Style.Font.FontName = "Arial";
                        ws.Range(celdaEntrada).Style.Font.FontSize = 10;

                        NextColumnFechas = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnFechas)) + 1];

                        var celdaSalida = GetCelda(NextColumnFechas, renglon.ToString());
                        ws.Cell(celdaSalida).Value = _asistencia == null || string.IsNullOrEmpty(_asistencia.Salida) || _asistencia.Entrada == _asistencia.Salida ? "Sin Registro" : _asistencia.Salida;
                        ws.Cell(celdaSalida).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(celdaSalida).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(celdaSalida).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Range(celdaSalida).Style.Border.OutsideBorderColor = XLColor.Gray;
                        ws.Range(celdaSalida).Style.Font.FontName = "Arial";
                        ws.Range(celdaSalida).Style.Font.FontSize = 10;

                        NextColumnFechas = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnFechas)) + 1];

                        var celdaHoras = GetCelda(NextColumnFechas, renglon.ToString());
                        ws.Cell(celdaHoras).Value = _asistencia != null ? _asistencia.HorasLaboradas : "0";
                        ws.Cell(celdaHoras).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(celdaHoras).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(celdaHoras).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Range(celdaHoras).Style.Border.OutsideBorderColor = XLColor.Gray;
                        ws.Range(celdaHoras).Style.Font.FontName = "Arial";
                        ws.Range(celdaHoras).Style.Font.FontSize = 10;
                        ws.Range(celdaHoras).Style.Font.FontColor = XLColor.Orange;
                        ws.Column(NextColumnFechas).Width = 10;
                        NextColumnFechas = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnFechas)) + 1];

                    }
                    var NextColumnRetardosValue = string.Empty;
                    if (Renglon == RenglonInicial)
                    {

                        NextColumnRetardosValue = NextColumnFechas;
                        var celdaTituloRetardos = GetCelda(NextColumnFechas, RenglonTitle);
                        ws.Cell(celdaTituloRetardos).Value = "Total Retardos";
                        ws.Cell(celdaTituloRetardos).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(celdaTituloRetardos).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(celdaTituloRetardos).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(celdaTituloRetardos).Style.Fill.BackgroundColor = XLColor.FromArgb(0x293352);
                        ws.Cell(celdaTituloRetardos).Style.Font.FontColor = XLColor.White;
                        ws.Column(NextColumnRetardosValue).Width = 15;

                        NextColumnRetardosValue = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnRetardosValue)) + 1];

                        var celdaTituloFaltas = GetCelda(NextColumnRetardosValue, RenglonTitle);
                        ws.Cell(celdaTituloFaltas).Value = "Total Faltas";
                        ws.Cell(celdaTituloFaltas).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(celdaTituloFaltas).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(celdaTituloFaltas).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(celdaTituloFaltas).Style.Fill.BackgroundColor = XLColor.FromArgb(0x293352);
                        ws.Cell(celdaTituloFaltas).Style.Font.FontColor = XLColor.White;
                        ws.Column(NextColumnRetardosValue).Width = 15;


                        //--------------COLUMNA ---------------------


                        NextColumnRetardosValue = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnRetardosValue)) + 1];

                        var celdaTituloHorasAcumuladas = GetCelda(NextColumnRetardosValue, RenglonTitle);
                        ws.Cell(celdaTituloHorasAcumuladas).Value = "Horas Acumuladas";
                        ws.Cell(celdaTituloHorasAcumuladas).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(celdaTituloHorasAcumuladas).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(celdaTituloHorasAcumuladas).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(celdaTituloHorasAcumuladas).Style.Fill.BackgroundColor = XLColor.FromArgb(0x293352);
                        ws.Cell(celdaTituloHorasAcumuladas).Style.Font.FontColor = XLColor.White;
                        ws.Column(NextColumnRetardosValue).Width = 15;


                    }

                    var celdaRetardos = GetCelda(NextColumnFechas, renglon.ToString());
                    ws.Cell(celdaRetardos).Value = empleado.TotalRetardos.ToString();
                    ws.Cell(celdaRetardos).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(celdaRetardos).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(celdaRetardos).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(celdaRetardos).Style.Font.FontName = "Arial";
                    ws.Cell(celdaRetardos).Style.Font.FontSize = 10;

                    NextColumnFechas = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnFechas)) + 1];

                    var celdaFaltas = GetCelda(NextColumnFechas, renglon.ToString());
                    ws.Cell(celdaFaltas).Value = empleado.TotalFaltas.ToString();
                    ws.Cell(celdaFaltas).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(celdaFaltas).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(celdaFaltas).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(celdaFaltas).Style.Font.FontName = "Arial";
                    ws.Cell(celdaFaltas).Style.Font.FontSize = 10;

                    NextColumnFechas = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnFechas)) + 1];

                    var celdaHorasAcumuladas = GetCelda(NextColumnFechas, renglon.ToString());
                    ws.Cell(celdaHorasAcumuladas).Value = empleado.HorasAcumuladas.ToString();
                    ws.Cell(celdaHorasAcumuladas).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(celdaHorasAcumuladas).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(celdaHorasAcumuladas).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(celdaHorasAcumuladas).Style.Font.FontName = "Arial";
                    ws.Cell(celdaHorasAcumuladas).Style.Font.FontSize = 10;

                    NextColumnFechas = Columnas[(Array.FindIndex(Columnas, row => row == NextColumnFechas)) + 1];
                    renglon++;
                    //Renglon = (RowSpan + 1).ToString();
                    Renglon = renglon.ToString();

                }

                var columnaA = ws.Column("A");
                columnaA.Width = 40;
                ws.Column(2).AdjustToContents();
                ws.Column(3).AdjustToContents();
                var archivo = String.Format("{0}-{1}.xlsx", DateTime.Now.ToString("yyyyMMdd-HHmmss"), Area);
                OutPutFile = _Path + archivo;
                workbook.SaveAs(OutPutFile);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private static string GetCelda(string col, string ren)
        {
            return string.Format("{0}{1}", col, ren);
        }

        private static string GetRango(string col1, string ren1, string col2, string ren2)
        {
            return string.Format("{0}{1}:{2}{3}", col1, ren1, col2, ren2);
        }

        public Result GetDataByFilter()
        {

            Result result = new Result();

            GetFechasBusquedaPorPeriodo();

            List<DateTime> LstRangoFechas = new List<DateTime>();

            DateTime FechaInicial = (DateTime)_model.FechaInicial;
            DateTime FechaFinal = (DateTime)_model.FechaFinal;

            DateTime FechaInicialAux = (DateTime)FechaInicial;
            while (FechaInicialAux <= FechaFinal)
            {
                LstRangoFechas.Add(FechaInicialAux);
                FechaInicialAux = FechaInicialAux.AddDays(1);
            }

            _model.ListaRangoFechas = LstRangoFechas;

            result = GetRelacionEmpleadoJefe();
            if (result.Success)
            {

                _model.ListaEmpleadosJefe = LstEmpleadosJefe
                        .Select(e => new EmpleadoJefe
                        {
                            EmployeeId = e.EmployeeId,
                            NombreEmpleado = e.NombreEmpleado,
                            ParentID = e.ParentID,
                            Children = GetChildren(LstEmpleadosJefe, (int)e.EmployeeId) /* Recursively grab the children */
                        }).ToList();
            }
           
            result = GetControlAsistencaByFilter(_model.NombreEmpleado, _model.PositionId, _model.AreaId, _model.LocalizationId, _model.PayRollId, _model.FechaInicial, _model.FechaFinal, _model.BossId);

            //var lista = _model.ListaEmpleadoAsistencia.Where(i => i.EmployeeId == _model.EmployeeIdConnected || i.ParentID == _model.EmployeeIdConnected).ToList();
            List<Empleado_Asistencia> lst = new List<Empleado_Asistencia>();
            lst = _LstEmpAsistencia.Select(i =>
                                         new Empleado_Asistencia
                                         {
 
                                              EmployeeId = i.EmployeeId
                                            , Nombre = i.Nombre
                                            , ParentID = i.ParentID
                                            , CardNumber = i.CardNumber
                                            , PositionId = i.PositionId
                                            , Puesto = i.Puesto
                                            , AreaId = i.AreaId
                                            , Area = i.Area
                                            , LocalizationId = i.LocalizationId
                                            , Ubicacion = i.Ubicacion
                                            , PayRollId = i.PayRollId
                                            , Patron = i.Patron
                                            , boolSelected = i.boolSelected
                                            , Es_Jefe = i.Es_Jefe
                                             ,
                                             CompanyEmail = i.CompanyEmail,
                                             TotalRetardos = i.TotalRetardos,
                                             TotalFaltas = i.TotalFaltas,
                                             HorasAcumuladas = i.HorasAcumuladas,
                                             TotlaFaltasInAsistencia = i.TotalFaltasInAsistencia,
                                             TotalFaltasPorRetardos = i.TotalFaltasPorRetardos,

                                             LstDetalleAsistencia = i.LstDetalleAsistencia

                                         }).ToList();


            foreach (var x in LstEmpleadosJefe)
            {
                var itemToChange = lst.FirstOrDefault(d => d.EmployeeId == x.EmployeeId);
                if (itemToChange != null)
                    itemToChange.Level = x.Level;
            }

            if (_model.Rol == Enumeraciones.enumRoles.AdministradorRh || _model.Rol == Enumeraciones.enumRoles.AdministradorSistemas || _model.Rol == Enumeraciones.enumRoles.DireccionGeneral)
            {
                _model.ListaEmpleado_Asistenica = lst
                      .Select(i => new Empleado_Asistencia
                      {
                          EmployeeId = i.EmployeeId
       ,
                          Nombre = i.Nombre
       ,
                          ParentID = i.ParentID
       ,
                          CardNumber = i.CardNumber
       ,
                          PositionId = i.PositionId
       ,
                          Puesto = i.Puesto
       ,
                          AreaId = i.AreaId
       ,
                          Area = i.Area
       ,
                          LocalizationId = i.LocalizationId
       ,
                          Ubicacion = i.Ubicacion
       ,
                          PayRollId = i.PayRollId
       ,
                          Patron = i.Patron
       ,
                          boolSelected = i.boolSelected
       ,
                          Es_Jefe = i.Es_Jefe
                         ,
                          Level = i.Level
                         ,
                          CompanyEmail = i.CompanyEmail,
                          LstDetalleAsistencia = i.LstDetalleAsistencia
                          ,
                          TotalRetardos = i.TotalRetardos,
                          TotalFaltas = i.TotalFaltas,
                          HorasAcumuladas = i.HorasAcumuladas,
                          TotlaFaltasInAsistencia = i.TotlaFaltasInAsistencia,
                          TotalFaltasPorRetardos = i.TotalFaltasPorRetardos,
                          Children = GetChildren2(lst, (int)i.EmployeeId) /* Recursively grab the children */
                      }).ToList();
            }
            else
            {
                _model.ListaEmpleado_Asistenica = lst.Where(i => i.EmployeeId == _model.EmployeeIdConnected || i.ParentID == _model.EmployeeIdConnected)
                      .Select(i => new Empleado_Asistencia
                      {
                          EmployeeId = i.EmployeeId
       ,
                          Nombre = i.Nombre
       ,
                          ParentID = i.ParentID
       ,
                          CardNumber = i.CardNumber
       ,
                          PositionId = i.PositionId
       ,
                          Puesto = i.Puesto
       ,
                          AreaId = i.AreaId
       ,
                          Area = i.Area
       ,
                          LocalizationId = i.LocalizationId
       ,
                          Ubicacion = i.Ubicacion
       ,
                          PayRollId = i.PayRollId
       ,
                          Patron = i.Patron
       ,
                          boolSelected = i.boolSelected
       ,
                          Es_Jefe = i.Es_Jefe
                         ,
                          Level = i.Level
                         ,
                          CompanyEmail = i.CompanyEmail,
                          LstDetalleAsistencia = i.LstDetalleAsistencia
                          ,
                          TotalRetardos = i.TotalRetardos,
                          TotalFaltas = i.TotalFaltas,
                          HorasAcumuladas = i.HorasAcumuladas,
                          TotlaFaltasInAsistencia = i.TotlaFaltasInAsistencia,
                          TotalFaltasPorRetardos = i.TotalFaltasPorRetardos,
                          Children = GetChildren2(lst, (int)i.EmployeeId) /* Recursively grab the children */
                      }).ToList();
            }
            return result;

        }


        private static List<EmpleadoJefe> GetChildren(List<EmpleadoJefe> items, int? parentId)
        {
            {
                return items
                .Where(x => x.ParentID == parentId)
                .Select(e => new EmpleadoJefe
                {
                    EmployeeId = e.EmployeeId,
                    NombreEmpleado = e.NombreEmpleado,
                    ParentID = e.ParentID,
                    Children = GetChildren(items, (int?)e.EmployeeId)
                }).ToList();

            }

        }
        private static List<Empleado_Asistencia> GetChildren2(List<Empleado_Asistencia> items, int? parentId)
        {
            {
                return items
                .Where(x => x.ParentID == parentId)
                .Select(i => new Empleado_Asistencia
                {
                    EmployeeId = i.EmployeeId
        ,
                    Nombre = i.Nombre
        ,
                    ParentID = i.ParentID
        ,
                    CardNumber = i.CardNumber
        ,
                    PositionId = i.PositionId
        ,
                    Puesto = i.Puesto
        ,
                    AreaId = i.AreaId
        ,
                    Area = i.Area
        ,
                    LocalizationId = i.LocalizationId
        ,
                    Ubicacion = i.Ubicacion
        ,
                    PayRollId = i.PayRollId
        ,
                    Patron = i.Patron
        ,
                    boolSelected = i.boolSelected
        ,
                    Es_Jefe = i.Es_Jefe
                 ,
                    LstDetalleAsistencia = i.LstDetalleAsistencia
                    ,
                    TotalRetardos = i.TotalRetardos,
                    TotalFaltas = i.TotalFaltas,
                    HorasAcumuladas = i.HorasAcumuladas,
                    TotlaFaltasInAsistencia = i.TotlaFaltasInAsistencia,
                    TotalFaltasPorRetardos = i.TotalFaltasPorRetardos,
                    Children = GetChildren2(items, (int?)i.EmployeeId)
                }).ToList();

            }

        }


        public Result GetRelacionEmpleadoJefe()
        {
            Result result = new Result();
            List<EmpleadoJefe> lista = new List<EmpleadoJefe>();
            result = _ControlAsistenciaService.GetRelacionEmpleadoJefe(out lista);
            if (result.Success)
            {
                LstEmpleadosJefe = lista;
            }
            return result;
        }


        public Result GetControlAsistencaByFilter
            (
             string NombreEmpleado
           , int? PositionId
           , int? AreaId
           , int? LocalizationId
           , int? PayRollId
           , DateTime? FechaInicial
           , DateTime? FechaFinal
           , int? BossId
          )
        {
            Result result = new Result();

            List<EmployeeAsistencia> lstEmpleadoAsistencia = null;


            result = _ControlAsistenciaService.GetControlAsistencaByFilter(out lstEmpleadoAsistencia
                                                                           , NombreEmpleado
                                                                           , PositionId
                                                                           , AreaId
                                                                           , LocalizationId
                                                                           , PayRollId
                                                                           , FechaInicial
                                                                           , FechaFinal
                                                                           , BossId);

            if (!result.Success)
                return result;

            _LstEmpAsistencia = lstEmpleadoAsistencia.OrderBy(o => o.Es_Jefe).ThenBy(g => g.Nombre).ToList();
            _model.ListaEmpleadoAsistencia = lstEmpleadoAsistencia.OrderByDescending(o => o.Es_Jefe).ThenBy(g => g.Nombre).ToList();
            

            return result;
        }

        public Result EnviaReporteSemanalPorArea(string ServerMapPath)
        {
            Result result = new Result();
            Result resultEnviarExcel = new Result();

            try
            {
                GetDataReporteSemanal();

                List<string> LstArchivosPorArea = new List<string>();
                resultEnviarExcel = CreaExcelReporteAsistenciasPorArea(ServerMapPath, out LstArchivosPorArea);

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public Result GetDataReporteSemanal()
        {
            Result result = new Result();
            try
            {


                var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

                var sunday = DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek);

                List<DateTime> LstRangoFechas = new List<DateTime>();

                DateTime FechaInicial = (DateTime)_model.FechaInicial; //(DateTime)monday;
                DateTime FechaFinal = (DateTime)_model.FechaFinal; //(DateTime)sunday;

                DateTime FechaInicialAux = (DateTime)_model.FechaInicial;//monday;
                while (FechaInicialAux <= FechaFinal)
                {
                    LstRangoFechas.Add(FechaInicialAux);
                    FechaInicialAux = FechaInicialAux.AddDays(1);
                }

                _model.ListaRangoFechas = LstRangoFechas;

                result = GetRelacionEmpleadoJefe();
                if (result.Success)
                {

                    _model.ListaEmpleadosJefe = LstEmpleadosJefe
                            .Select(e => new EmpleadoJefe
                            {
                                EmployeeId = e.EmployeeId,
                                NombreEmpleado = e.NombreEmpleado,
                                ParentID = e.ParentID,
                                Children = GetChildren(LstEmpleadosJefe, (int)e.EmployeeId) /* Recursively grab the children */
                            }).ToList();
                }
                result = GetControlAsistencaByFilter(_model.NombreEmpleado, _model.PositionId, _model.AreaId, _model.LocalizationId, _model.PayRollId, _model.FechaInicial, _model.FechaFinal, _model.BossId);


                List<Empleado_Asistencia> lst = new List<Empleado_Asistencia>();
                lst = _LstEmpAsistencia.Select(i =>
                 new Empleado_Asistencia
                 {

                     EmployeeId = i.EmployeeId
            ,
                     Nombre = i.Nombre
            ,
                     ParentID = i.ParentID
            ,
                     CardNumber = i.CardNumber
            ,
                     PositionId = i.PositionId
            ,
                     Puesto = i.Puesto
            ,
                     AreaId = i.AreaId
            ,
                     Area = i.Area
            ,
                     LocalizationId = i.LocalizationId
            ,
                     Ubicacion = i.Ubicacion
            ,
                     PayRollId = i.PayRollId
            ,
                     Patron = i.Patron
            ,
                     boolSelected = i.boolSelected
            ,
                     Es_Jefe = i.Es_Jefe
                     ,
                     CompanyEmail = i.CompanyEmail,
                     LstDetalleAsistencia = i.LstDetalleAsistencia

                 }).ToList();


                foreach (var x in LstEmpleadosJefe)
                {
                    var itemToChange = lst.FirstOrDefault(d => d.EmployeeId == x.EmployeeId);
                    if (itemToChange != null)
                        itemToChange.Level = x.Level;
                }


                _model.ListaEmpleado_Asistenica = lst
                           .Select(i => new Empleado_Asistencia
                           {
                               EmployeeId = i.EmployeeId
            ,
                               Nombre = i.Nombre
            ,
                               ParentID = i.ParentID
            ,
                               CardNumber = i.CardNumber
            ,
                               PositionId = i.PositionId
            ,
                               Puesto = i.Puesto
            ,
                               AreaId = i.AreaId
            ,
                               Area = i.Area
            ,
                               LocalizationId = i.LocalizationId
            ,
                               Ubicacion = i.Ubicacion
            ,
                               PayRollId = i.PayRollId
            ,
                               Patron = i.Patron
            ,
                               boolSelected = i.boolSelected
            ,
                               Es_Jefe = i.Es_Jefe
                              ,
                               Level = i.Level
                              ,
                               CompanyEmail = i.CompanyEmail,
                               LstDetalleAsistencia = i.LstDetalleAsistencia
                               ,
                               Children = GetChildren2(lst, (int)i.EmployeeId) /* Recursively grab the children */
                           }).ToList();

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        private bool GetFechasBusquedaPorPeriodo()
        {

            DateTime hoy = DateTime.Now;

            //DateTime hoy = new DateTime(2018, 12, 7);
            
            DateTime EsteLunes = hoy.AddDays(-(int)hoy.DayOfWeek + 1);
            DateTime LunesSemanaPasada = hoy.AddDays(-(int)hoy.DayOfWeek - 6);
            DateTime DomingoSemanaPasada = LunesSemanaPasada.AddDays(6);

            DateTime LunesDoSemanasAtras = hoy.AddDays(-(int)hoy.DayOfWeek - 13);
            DateTime DomingoDoSemanasAtras = LunesDoSemanasAtras.AddDays(6);

            if (_model.enumPeriodoBusqueda == enumPeriodoBusqueda.EstaSemana)
            {
                _model.FechaInicial = EsteLunes;
                _model.FechaFinal = hoy;
            }

            if (_model.enumPeriodoBusqueda == enumPeriodoBusqueda.LaSemanaPasada)
            {
                _model.FechaInicial = LunesSemanaPasada;
                _model.FechaFinal = DomingoSemanaPasada;
            }

            if (_model.enumPeriodoBusqueda == enumPeriodoBusqueda.HaceDosSemanas)
            {
                _model.FechaInicial = LunesDoSemanasAtras;
                _model.FechaFinal = DomingoDoSemanasAtras;
            }

            if (_model.enumPeriodoBusqueda == enumPeriodoBusqueda.DefinirPeriodo)
            {
                _model.FechaInicial = _model.FechaInicial;
                _model.FechaFinal = _model.FechaFinal;
            }

            return true;

        }


       
    }
}