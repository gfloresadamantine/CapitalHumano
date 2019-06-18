using AdministracionRH.Common;
using AdministracionRH.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdministracionRH.Repositories
{
    public class ControlAsistenciaRepository
    {
        private DAO _dao;
        string _SpProcedureName { get; set; }
        public Enumeraciones.enumCatalogos _enumCatalogo { get; set; }
        public List<ResumenEmployeeAsistencia> LstResumenEmployeeAsistencia { get; set; }

        public ControlAsistenciaRepository()
        {
            _dao = new DAO();
            LstResumenEmployeeAsistencia = new List<ResumenEmployeeAsistencia>();
        }


        public Result GetRelacionEmpleadoJefe(out List<EmpleadoJefe> lstEmpleadoJefe)
        {
            lstEmpleadoJefe = null;
            _SpProcedureName = "dbo.Sp_GetRelacionJefeEmpleado";
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>() { };
                SqlDataReader reader = _dao.ExecuteReader(parametros, _SpProcedureName);
                if (reader.HasRows)
                {
                    lstEmpleadoJefe = new List<EmpleadoJefe>();
                    while (reader.Read())
                    {
                        EmpleadoJefe empleadoJefe = new EmpleadoJefe();
                        empleadoJefe.EmployeeId = reader["EmployeeId"] == DBNull.Value ? null : (int?)reader["EmployeeId"];
                        empleadoJefe.NombreEmpleado = reader["Nombre"].ToString();
                        empleadoJefe.ParentID = reader["ParentID"] == DBNull.Value ? null : (int?)reader["ParentID"];
                        empleadoJefe.Level = (int)reader["Lvl"];
                        lstEmpleadoJefe.Add(empleadoJefe);
                    }
                }
                reader.Close();
                resultado.Success = true;
            }
            catch (Exception e)
            {

                resultado.Success = false;
                resultado.ErrorMessage = String.Format("Se ha producido un error al procesar datos de archivo Error {0}", e.Message);
            }

            return resultado;
        }

        public Result GetControlAsistencaByFilter(
            out List<EmployeeAsistencia> lstEmpleadoAsistencia
             , string NombreEmpleado
             , int? PositionId
             , int? AreaId
             , int? LocalizationId
             , int? PayRollId
             , DateTime? FechaInicial
             , DateTime? FechaFinal
             , int? BossId

            )
        {

            lstEmpleadoAsistencia = null;
            List<ResumenEmployeeAsistencia> LstResumenAsistencia = new List<ResumenEmployeeAsistencia>();
            _SpProcedureName = "dbo.Sp_GetEmpleadoAsistenciaFilter";
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>() {

                        new SqlParameter("@NombreEmpleado" ,NombreEmpleado)
                       ,new SqlParameter("@PositionId" ,PositionId)
                       ,new SqlParameter("@AreaId"  ,AreaId)
                       ,new SqlParameter("@LocalizationId" ,LocalizationId)
                       ,new SqlParameter("@PayRollId",PayRollId)
                       ,new SqlParameter("@FechaInicial" ,FechaInicial)
                       ,new SqlParameter("@FechaFinal",FechaFinal)
                       ,new SqlParameter("@BossId",BossId)
                };
                SqlDataReader reader = _dao.ExecuteReader(parametros, _SpProcedureName);
                if (reader.HasRows)
                {
                    LstResumenAsistencia = new List<ResumenEmployeeAsistencia>();
                    while (reader.Read())
                    {
                        ResumenEmployeeAsistencia resumenAsistencia = new ResumenEmployeeAsistencia();
                        resumenAsistencia.EmployeeId = (int)reader["EmployeeId"];
                        resumenAsistencia.Nombre = reader["Nombre"].ToString();
                        resumenAsistencia.PositionId = reader["PositionId"] == DBNull.Value ? null : (int?)reader["PositionId"];
                        resumenAsistencia.Puesto = reader["Puesto"].ToString();
                        resumenAsistencia.AreaId = reader["AreaId"] == DBNull.Value ? null : (int?)reader["AreaId"];
                        resumenAsistencia.Area = reader["Area"].ToString();
                        resumenAsistencia.LocalizationId = reader["LocalizationId"] == DBNull.Value ? null : (int?)reader["LocalizationId"];
                        resumenAsistencia.Ubicacion = reader["Ubicacion"].ToString();
                        resumenAsistencia.PayRollId = reader["PayRollId"] == DBNull.Value ? null : (int?)reader["PayRollId"];
                        resumenAsistencia.Patron = reader["Patron"].ToString();
                        resumenAsistencia.boolSelected = false;
                        resumenAsistencia.Fecha = reader["Fecha"] == DBNull.Value ? null : (DateTime?)reader["Fecha"];
                        resumenAsistencia.Entrada = reader["Entrada"].ToString();
                        resumenAsistencia.Salida = reader["Salida"].ToString();
                        resumenAsistencia.Es_Jefe = (int)reader["Es_Jefe"];
                        resumenAsistencia.ParentID = reader["ParentID"] == DBNull.Value ? null : (int?)reader["ParentID"];
                        resumenAsistencia.CompanyEmail = reader["CompanyEmail"].ToString();
                        LstResumenAsistencia.Add(resumenAsistencia);
                    }
                }
                reader.Close();

                LstResumenEmployeeAsistencia = LstResumenAsistencia;
                List<DateTime> lstRangoFechas = new List<DateTime>();
                var FechaAux = (DateTime)FechaInicial;
                while (FechaAux <= FechaFinal)
                {
                    lstRangoFechas.Add(FechaAux);
                    FechaAux = FechaAux.AddDays(1);

                }

                if (LstResumenAsistencia != null)
                {
                    foreach (ResumenEmployeeAsistencia itemResumen in LstResumenAsistencia)
                    {
                        if (IsValidTimeFormat(itemResumen.Entrada) && IsValidTimeFormat(itemResumen.Salida))
                        {

                            if (DateTime.Parse(itemResumen.Entrada, System.Globalization.CultureInfo.CurrentCulture).TimeOfDay.ToString() == DateTime.Parse(itemResumen.Salida, System.Globalization.CultureInfo.CurrentCulture).TimeOfDay.ToString())
                            {
                                itemResumen.HorasLaboradas = "0";
                            }
                            else
                            {
                                itemResumen.HorasLaboradas = DateTime.Parse(itemResumen.Salida, System.Globalization.CultureInfo.CurrentCulture).Subtract(DateTime.Parse(itemResumen.Entrada, System.Globalization.CultureInfo.CurrentCulture)).ToString();
                                itemResumen.HorasLaboradas = DateTime.Parse(itemResumen.HorasLaboradas, System.Globalization.CultureInfo.CurrentCulture).AddHours(-1).TimeOfDay.ToString();
                            }
                        }
                        else
                        {
                            itemResumen.HorasLaboradas = "0";
                            itemResumen.Retardo = 1;
                        }
                    }

                    lstEmpleadoAsistencia = LstResumenAsistencia
                        .GroupBy(ac => new
                        {
                            ac.EmployeeId,
                            ac.Nombre,
                            ac.PositionId,
                            ac.Puesto,
                            ac.AreaId,
                            ac.Area,
                            ac.LocalizationId,
                            ac.Ubicacion,
                            ac.PayRollId,
                            ac.Patron,
                            ac.boolSelected,
                            ac.Es_Jefe,
                            ac.ParentID,
                            ac.CompanyEmail
                            
                        })
                        .Select(ac => new EmployeeAsistencia
                        {
                            EmployeeId = ac.Key.EmployeeId,
                            Nombre = ac.Key.Nombre,
                            PositionId = ac.Key.PositionId,
                            Puesto = ac.Key.Puesto,
                            AreaId = ac.Key.AreaId,
                            Area = ac.Key.Area,
                            LocalizationId = ac.Key.LocalizationId,
                            Ubicacion = ac.Key.Ubicacion,
                            PayRollId = ac.Key.PayRollId,
                            Patron = ac.Key.Patron,
                            boolSelected = ac.Key.boolSelected,
                            Es_Jefe = ac.Key.Es_Jefe,
                            ParentID =ac.Key.ParentID,
                            CompanyEmail = ac.Key.CompanyEmail,
                            LstDetalleAsistencia = ac.Select(g => new Asistencia
                            {
                                EmployeeId = g.EmployeeId
                                                                                  ,
                                Fecha = g.Fecha
                                                                                  ,
                                Entrada = g.Entrada
                                                                                  ,
                                Salida = g.Salida,
                                HorasLaboradas = g.HorasLaboradas,
                                Retardo = g.Retardo
                            }).ToList()
                        }).ToList();
                }

                //var compare845 = TimeSpan.Compare(ConvertTimeSpan("08:51"), ConvertTimeSpan("08:45:59"));
                //var compare900 = TimeSpan.Compare(ConvertTimeSpan("08:51"), ConvertTimeSpan("09:00:00"));

                foreach (var empleado in lstEmpleadoAsistencia)
                {

                    empleado.TotalRetardos = empleado.LstDetalleAsistencia
                                                     .Where(itemResumen =>
                                                              itemResumen.HorasLaboradas != "0"
                                                            && TimeSpan.Compare(ConvertTimeSpan(itemResumen.Entrada), ConvertTimeSpan("08:45:59")) == 1
                                                            && TimeSpan.Compare(ConvertTimeSpan(itemResumen.Entrada), ConvertTimeSpan("09:00:00")) == -1).Count();

                    var fechasEmpleado = empleado.LstDetalleAsistencia
                                                 .Where(d => ((DateTime)d.Fecha).DayOfWeek != DayOfWeek.Sunday && ((DateTime)d.Fecha).DayOfWeek != DayOfWeek.Saturday)
                                                 .Select(f => (DateTime)f.Fecha).ToList();

                    var fechasRestantes = lstRangoFechas
                                          .Where(d => d.DayOfWeek != DayOfWeek.Sunday && d.DayOfWeek != DayOfWeek.Saturday)
                                          .Select(g => g.ToShortDateString()).ToList()
                                          .Except
                                          (
                                          fechasEmpleado.Select(m => m.ToShortDateString())
                                          ).ToList();

                    var faltasPorHorasNoLaboradas = 0;
                    faltasPorHorasNoLaboradas = empleado.LstDetalleAsistencia.Where(g => g.HorasLaboradas == "0" && ((DateTime)g.Fecha).DayOfWeek != DayOfWeek.Sunday && ((DateTime)g.Fecha).DayOfWeek != DayOfWeek.Saturday).Count();

                    empleado.TotalFaltasDespuesDelas9 = empleado.LstDetalleAsistencia
                                                    .Where(itemResumen => !string.IsNullOrEmpty(itemResumen.Entrada) 
                                                                      && itemResumen.HorasLaboradas !="0"
                                                                      && ((DateTime)itemResumen.Fecha).DayOfWeek != DayOfWeek.Sunday && ((DateTime)itemResumen.Fecha).DayOfWeek != DayOfWeek.Saturday
                                                                      && TimeSpan.Compare(ConvertTimeSpan(itemResumen.Entrada) ,ConvertTimeSpan("09:00:00")) == 1).Count();


                    empleado.TotalFaltasInAsistencia = faltasPorHorasNoLaboradas + fechasRestantes.Count() + empleado.TotalFaltasDespuesDelas9;
                    if (empleado.TotalRetardos >= 3 )
                        empleado.TotalFaltasPorRetardos = (empleado.TotalRetardos) / 3;
                  
                    empleado.TotalFaltas = empleado.TotalFaltasPorRetardos + empleado.TotalFaltasInAsistencia;
                    empleado.HorasAcumuladas = CalculaHorasAcumuladasEmpleado(empleado.LstDetalleAsistencia.Select(g => g.HorasLaboradas).ToArray());
                    
                }

                resultado.Success = true;
            }
            catch (Exception e)
            {

                resultado.Success = false;
                resultado.ErrorMessage = String.Format("Se ha producido un error al procesar datos de archivo Error {0}", e.Message);
            }

            return resultado;
        }
        private TimeSpan ConvertTimeSpan(string Cadena)
        {
            return DateTime.Parse(Cadena, System.Globalization.CultureInfo.CurrentCulture).TimeOfDay;
        }

        public bool IsValidTimeFormat(string input)
        {
            TimeSpan dummyOutput;
            return TimeSpan.TryParse(input, out dummyOutput);
        }


        private string CalculaHorasAcumuladasEmpleado(string[] horasLaboras)
        {

            // string[] times = { "09:05", "10:14", "12:49" };

            int countHoras = 0;
            int countHorasTotal = 0;
            int countMinutesTotal = 0;
            int countMinutes = 0;
            string countHorasAux = string.Empty;
            string countMinutesAux = string.Empty;

            var count = horasLaboras.Where(i => i != "0").Count();

            if (count>0)
            {

                foreach (string hora in horasLaboras.Where(i => i != "0"))
                {
                    var auxHora = DateTime.Parse(hora, System.Globalization.CultureInfo.CurrentCulture).TimeOfDay.ToString();

                    countHorasAux = auxHora.Substring(0, 2);
                    if (Int32.TryParse(countHorasAux, out countHoras))
                    {
                        countHorasTotal += countHoras;
                    }

                    countMinutesAux = auxHora.Substring(3, 2);
                    if (Int32.TryParse(countMinutesAux, out countMinutes))
                    {
                        countMinutesTotal += countMinutes;
                    }

                }

                int minutosAcumuladosHoras = countMinutesTotal / 60;
                int minutosAcumuladosREstantes = countMinutesTotal % 60;
                int HorasAcumuladas = 0;
                int MinutosAcumulados = 0;
                HorasAcumuladas = countHorasTotal + (minutosAcumuladosHoras > 0 ? minutosAcumuladosHoras : 0);
                var horas = HorasAcumuladas < 10 ? "0" + HorasAcumuladas.ToString() : HorasAcumuladas.ToString();
                MinutosAcumulados = minutosAcumuladosREstantes > 0 ? minutosAcumuladosREstantes : 0;
                var minutos = MinutosAcumulados < 10 ? "0" + MinutosAcumulados.ToString() : MinutosAcumulados.ToString();
                return string.Format("{0}:{1}", horas, minutos);
            }

            return "0";


        }

    }
}