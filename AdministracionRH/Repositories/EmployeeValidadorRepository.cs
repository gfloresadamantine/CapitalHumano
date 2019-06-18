using AdministracionRH.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static AdministracionRH.Common.Enumeraciones;

namespace AdministracionRH.Repositories
{
    public class Documentacion
    {
        public string TipoDocto { get; set; }
        public string NombreDocto { get; set; }
    }
    public class EmployeeValidadorRepository
    {

        private DAO _dao;
        string _SpProcedureName { get; set; }
        public Enumeraciones.enumCatalogos _enumCatalogo { get; set; }
        public EmployeeValidadorRepository()
        {
            _dao = new DAO();
        }
        public Result GetAllEmployessValidador(out List<EmployeeValidador> lstEmployees, string nombre, string no_empleado, string Rfc, string Nss)
        {
            lstEmployees = null;
            _SpProcedureName = "[dbo].[Sp_GetEmployeesValidadorByFilter]";
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>{
                                                             new SqlParameter("@Nombre", string.IsNullOrEmpty(nombre) ?null:nombre)
                                                            ,new SqlParameter("@NoEmpleado", string.IsNullOrEmpty(no_empleado) ?null:no_empleado)
                                                            ,new SqlParameter("@Rfc", string.IsNullOrEmpty(Rfc) ?null:Rfc)
                                                            ,new SqlParameter("@Nss", string.IsNullOrEmpty(Nss) ?null:Nss)
                                                            

                };
                SqlDataReader reader = _dao.ExecuteReader(parametros, _SpProcedureName);
                if (reader.HasRows)
                {
                    lstEmployees = new List<EmployeeValidador>();
                    while (reader.Read())
                    {
                        EmployeeValidador employee = new EmployeeValidador();
                         employee.EmployeeId = reader["PK_TEMP"].ToString();
                        //employee.CardId = reader["CardId"].ToString();
                        employee.CardNumber = reader["NO_EMPLEADO"].ToString();
                        employee.FirstName = reader["NOMBRE"].ToString();
                        employee.LastName = reader["PATERNO"].ToString();
                        employee.MiddleName = reader["MATERNO"].ToString();
                        employee.FullName = reader["FULLNAME"].ToString();
                        employee.Rfc = reader["Rfc"].ToString();
                        employee.Curp = reader["Curp"].ToString();
                        employee.NSS = reader["NSS"].ToString();
                        employee.BirthDay = reader["F_NACIMIENTO"] == DBNull.Value ? null : (DateTime?)reader["F_NACIMIENTO"];
                        //employee.AdmissionDate = reader["F_INGRESO"] == DBNull.Value ? null : (DateTime?)reader["F_INGRESO"];
                        //employee.EntryDate = reader["F_ANTIGUEDAD"] == DBNull.Value ? null : (DateTime?)reader["F_ANTIGUEDAD"];
                        //employee.LeavingDate = reader["F_BAJA"] == DBNull.Value ? null : (DateTime?)reader["F_BAJA"];
                        employee.StreetName = reader["CALLE"].ToString();
                        employee.NumberExt = reader["NUM EXT"].ToString();
                        employee.NumberInt = reader["NO_INT"].ToString();
                        employee.Delegation = reader["DELEGACION"].ToString();
                        employee.Colony = reader["COLONIA"].ToString();
                        employee.CP = reader["CP"].ToString();
                        employee.PhoneNumber = reader["TEL_CASA"].ToString();
                        employee.CellPhoneNumber = reader["CELULAR"].ToString();
                        //employee.OfficePhone = reader["OfficePhone"].ToString();
                        //employee.OfficeExt = reader["OfficeExt"].ToString();
                        employee.PersonalEmail = reader["EMAIL"].ToString();
                        //employee.CompanyEmail = reader["CompanyEmail"].ToString();
                        //employee.Enabled = (bool)reader["Enabled"];
                        //employee.LocalizationId = reader["LocalizationId"] == DBNull.Value ? null : (int?)reader["LocalizationId"];
                        //employee.PayRollId = reader["PayRollId"] == DBNull.Value ? null : (int?)reader["PayRollId"];
                        //employee.CompanyId = reader["CompanyId"] == DBNull.Value ? null : (int?)reader["CompanyId"];
                        //employee.NationalityId = reader["NationalityId"] == DBNull.Value ? null : (int?)reader["NationalityId"];
                        //employee.EmployeePositionId = reader["EmployeePositionId"] == DBNull.Value ? null : (int?)reader["EmployeePositionId"];
                        //employee.BossId = reader["BossId"] == DBNull.Value ? null : (int?)reader["BossId"];
                        //employee.BossName = reader["BossName"].ToString();
                        //employee.AreaName = reader["AreaName"].ToString();
                        //employee.PositionName = reader["PUESTO"].ToString();
                        //employee.FingerPrint = reader["FingerPrint"] == DBNull.Value ? false : Convert.ToBoolean(reader["FingerPrint"]) ? true : false;
                        //employee.ParkingSpace = reader["ParkingSpace"].ToString();
                        //employee.CardId = reader["CardId"].ToString();
                        //employee.Compania = reader["COMPANIA"].ToString();
                        employee.Banco = reader["BANCO"].ToString();
                        employee.Clabe = reader["CLABE"].ToString();
                        employee.CreditoInfonavit = reader["CREDITO_INFONAVIT"].ToString();
                        employee.LugarNacimiento = reader["LUGAR_NAC"].ToString();
                        employee.EdoCivil = reader["EDO_CIVIL"].ToString();
                        employee.EstadoEmpleado = reader["ESTADO"].ToString();
                        employee.LstDocumentos = new List<Documentacion>() {
                            new Documentacion() { TipoDocto = "Acta Nacimiento", NombreDocto = "ACTA_NACIMIENTO.pdf" }
                            , new Documentacion() { TipoDocto = "Curp", NombreDocto = "CURP.pdf" }
                            , new Documentacion() { TipoDocto = "Comprobante Domicilio", NombreDocto = "DOMICILIO.pdf" }
                            , new Documentacion() { TipoDocto = "Identificación", NombreDocto = "DOMICILIO.pdf" }
                            , new Documentacion() { TipoDocto = "Comprobante Domicilio", NombreDocto = "DOMICILIO.pdf" }
                            , new Documentacion() { TipoDocto = "Imss", NombreDocto = "DOMICILIO.pdf" }
                            , new Documentacion() { TipoDocto = "Rfc", NombreDocto = "DOMICILIO.pdf" }
                            , new Documentacion() { TipoDocto = "Carta de Recomendación", NombreDocto = "DOMICILIO.pdf" }
                            , new Documentacion() { TipoDocto = "Estado de cuenta", NombreDocto = "DOMICILIO.pdf" }
                        };

                        employee.Sexo = reader["SEXO"].ToString();
                        employee.TRAMITE_INF_SN = reader["TRAMITE_INF_SN"].ToString();
                        employee.Etapa = reader["ETAPA"].ToString();
                        employee.Estudios = reader["ESTUDIOS"].ToString();
                        employee.Nivel_Estudios = reader["NIVEL ESTUDIOS"].ToString();
                        employee.Universidad = reader["UNIVERSIDAD"].ToString();
                        employee.Id_Jefe = reader["ID JEFE"].ToString();
                        employee.Nombre_jefe = reader["NOMBRE JEFE"].ToString();
                        employee.Ultimo_GradoEstudios = reader["ULT_GRADO_ESTUDIOS"].ToString();
                        employee.Estatus = reader["ESTATUS"] == DBNull.Value ? false : Convert.ToBoolean(reader["ESTATUS"]);


                        lstEmployees.Add(employee);
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

        public Result UpdateEmployee(EmployeeValidador employeeValidador)
        {
            _SpProcedureName = "[dbo].[Sp_UpdateEmployeeValidador]";
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>{
                        new SqlParameter("@CardNumber"         ,employeeValidador.CardNumber)
                       ,new SqlParameter("@FirstName"          ,employeeValidador.FirstName)
                       ,new SqlParameter("@LastName"           ,employeeValidador.LastName)
                       ,new SqlParameter("@MiddleName"         ,employeeValidador.MiddleName)
                       ,new SqlParameter("@FullName"           ,employeeValidador.FullName)
                       ,new SqlParameter("@Rfc"                ,employeeValidador.Rfc)
                       ,new SqlParameter("@Curp"               ,employeeValidador.Curp)
                       ,new SqlParameter("@NSS"                ,employeeValidador.NSS)
                       ,new SqlParameter("@BirthDay"           ,employeeValidador.BirthDay)
                       ,new SqlParameter("@PhoneNumber"        ,employeeValidador.PhoneNumber)
                       ,new SqlParameter("@CellPhoneNumber"    ,employeeValidador.CellPhoneNumber)
                       ,new SqlParameter("@PersonalEmail"      ,employeeValidador.PersonalEmail)
                       ,new SqlParameter("@StreetName"         ,employeeValidador.StreetName)
                       ,new SqlParameter("@NumberExt"          ,employeeValidador.NumberExt)
                       ,new SqlParameter("@NumberInt"          ,employeeValidador.NumberInt)
                       ,new SqlParameter("@Delegation"         ,employeeValidador.Delegation)
                       ,new SqlParameter("@Colony"             ,employeeValidador.Colony)
                       ,new SqlParameter("@CP"                 ,employeeValidador.CP)
                       ,new SqlParameter("@Banco"              ,employeeValidador.Banco)
                       ,new SqlParameter("@Clabe"              ,employeeValidador.Clabe)
                       ,new SqlParameter("@EdoCivil"           ,employeeValidador.EdoCivil)
                       ,new SqlParameter("@LugarNacimiento"    ,employeeValidador.LugarNacimiento)
                       ,new SqlParameter("@CreditoInfonavit"   ,employeeValidador.CreditoInfonavit)
                       ,new SqlParameter("@EstadoEmpleado"     ,employeeValidador.EstadoEmpleado)
                       ,new SqlParameter("@Sexo" ,employeeValidador.Sexo)
                       ,new SqlParameter("@TramiteInfSn" ,employeeValidador.TRAMITE_INF_SN)         
                       ,new SqlParameter("@Etapa" ,employeeValidador.Etapa)                
                       ,new SqlParameter("@Estudios" ,employeeValidador.Estudios)             
                       ,new SqlParameter("@NivelEstudios" ,employeeValidador.Nivel_Estudios)        
                       ,new SqlParameter("@Universidad" ,employeeValidador.Universidad)          
                       ,new SqlParameter("@NoJefe" ,employeeValidador.Id_Jefe)               
                       ,new SqlParameter("@NombreJefe" ,employeeValidador.Nombre_jefe)
                       ,new SqlParameter("@Ult_GradoEstudios" ,employeeValidador.Ultimo_GradoEstudios)
                       ,new SqlParameter("@Estatus" ,employeeValidador.Estatus)
                       };

                _dao.ExecuteProcedure(parametros, _SpProcedureName);
                resultado.Success = true;
            }
            catch (Exception e)
            {

                resultado.Success = false;
                resultado.ErrorMessage = String.Format("Se ha producido un error al procesar datos de archivo Error {0}", e.Message);
            }

            return resultado;
        }

    }
}