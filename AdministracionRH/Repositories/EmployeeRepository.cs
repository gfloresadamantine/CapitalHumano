using AdministracionRH.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static AdministracionRH.Common.Enumeraciones;

namespace AdministracionRH.Repositories
{
    public class EmployeeRepository
    {
        private string _SpProcedureName { get; set; }
        private DAO _dao;
        //public LogSistema _logUser { get; set; }

        public EmployeeRepository()
        {
            _dao = new DAO();

        }

        //public EmployeeRepository(string SpProcedureName)
        //{
        //    _dao = new DAO();
        //    _SpProcedureName = SpProcedureName;
        //}


        public Result GetAllEmployess(out List<Employee> lstEmployees, string nombre, int? PositionId, int? AreaId, DateTime? fecha_incial, DateTime? fecha_final, int? BossId, bool activo, int? EmployeeId, int? OperacionId, enumRoles Rol, int EmployeeIdConected, bool FingerPrint)
        {
            lstEmployees = null;
            _SpProcedureName = "[dbo].[Sp_GetEmployeesByFilter]";
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>{
                                                             new SqlParameter("@Nombre", nombre)
                                                            ,new SqlParameter("@PosicionId", PositionId==0 ? null:PositionId)
                                                            ,new SqlParameter("@AreaId", AreaId==0 ?null:AreaId)
                                                            ,new SqlParameter("@FechaInicial", fecha_incial)
                                                            ,new SqlParameter("@FechaFinal", fecha_final)
                                                            ,new SqlParameter("@BossId", BossId ==0?null :BossId)
                                                            ,new SqlParameter("@Activo", activo)
                                                            ,new SqlParameter("@EmployeeId", EmployeeId)
                                                            ,new SqlParameter("@OperacionId", OperacionId ==0?null:OperacionId)
                                                            ,new SqlParameter("@FingerPrint", FingerPrint)
                                                            //,new SqlParameter("@Rol", Convert.ToInt32(Rol))
                                                            //,new SqlParameter("@EmployeeIdConected", EmployeeIdConected)
                                                            //,new SqlParameter("@Rol", null)
                                                            //,new SqlParameter("@EmployeeIdConected", null)


                };
                SqlDataReader reader = _dao.ExecuteReader(parametros, _SpProcedureName);
                if (reader.HasRows)
                {
                    lstEmployees = new List<Employee>();
                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.EmployeeId = reader["EmployeeId"].ToString();
                        //employee.CardId = reader["CardId"].ToString();
                        employee.CardNumber = reader["CardNumber"].ToString();
                        employee.FirstName = reader["FirstName"].ToString();
                        employee.LastName = reader["LastName"].ToString();
                        employee.MiddleName = reader["MiddleName"].ToString();
                        employee.FullName = reader["FullName"].ToString();
                        //employee.Photo = reader["Photo"].ToString();
                        employee.Rfc = reader["Rfc"].ToString();
                        employee.Curp = reader["Curp"].ToString();
                        employee.NSS = reader["NSS"].ToString();
                        employee.BirthDay = reader["BirthDay"]==DBNull.Value?null: (DateTime?) reader["BirthDay"];
                        employee.AdmissionDate =reader["AdmissionDate"] == DBNull.Value ? null : (DateTime?)reader["AdmissionDate"];
                        employee.StreetName = reader["StreetName"].ToString();
                        employee.NumberExt = reader["NumberExt"].ToString();
                        employee.NumberInt = reader["NumberInt"].ToString();
                        employee.Delegation = reader["Delegation"].ToString();
                        employee.Colony = reader["Colony"].ToString();
                        employee.CP = reader["CP"].ToString();
                        employee.PhoneNumber = reader["PhoneNumber"].ToString();
                        employee.CellPhoneNumber =reader["CellPhoneNumber"].ToString();
                        employee.OfficePhone = reader["OfficePhone"].ToString();
                        employee.OfficeExt = reader["OfficeExt"].ToString();
                        employee.PersonalEmail = reader["PersonalEmail"].ToString();
                        employee.CompanyEmail = reader["CompanyEmail"].ToString();
                        employee.Enabled = (bool) reader["Enabled"];
                        employee.LocalizationId = reader["LocalizationId"]==DBNull.Value ?null:  (int?)reader["LocalizationId"];
                        employee.PayRollId = reader["PayRollId"] == DBNull.Value ? null : (int?)reader["PayRollId"];
                        employee.CompanyId = reader["CompanyId"] == DBNull.Value ? null : (int?)reader["CompanyId"];
                        employee.NationalityId = reader["NationalityId"] == DBNull.Value ? null : (int?)reader["NationalityId"];
                        employee.EmployeePositionId = reader["EmployeePositionId"] == DBNull.Value ? null : (int?)reader["EmployeePositionId"];
                        employee.BossId = reader["BossId"] == DBNull.Value ? null : (int?)reader["BossId"];
                        employee.BossName = reader["BossName"].ToString();
                        employee.AreaName = reader["AreaName"].ToString();
                        employee.PositionName = reader["PositionName"].ToString();
                        employee.FingerPrint = reader["FingerPrint"]==DBNull.Value?false : Convert.ToBoolean(reader["FingerPrint"])? true:false;
                        employee.ParkingSpace = reader["ParkingSpace"].ToString();
                        employee.CardId = reader["CardId"].ToString();

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


        public Result GetOneEmployee(out Employee employee, int EmployeeId)
        {
            _SpProcedureName = "[dbo].[Sp_GetOneEmployee]";
            Result resultado = new Result();
            employee = null;
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>{
                                                             new SqlParameter("@EmployeeId", EmployeeId)
                };
                SqlDataReader reader = _dao.ExecuteReader(parametros, _SpProcedureName);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employee = new Employee();
                        
                        employee.EmployeeId = reader["EmployeeId"].ToString();
                        //employee.CardId = reader["CardId"].ToString();
                        employee.CardNumber = reader["CardNumber"].ToString();
                        employee.FirstName = reader["FirstName"].ToString();
                        employee.LastName = reader["LastName"].ToString();
                        employee.MiddleName = reader["MiddleName"].ToString();
                        employee.FullName = reader["FullName"].ToString();
                        //employee.Photo = reader["Photo"].ToString();
                        employee.Rfc = reader["Rfc"].ToString();
                        employee.Curp = reader["Curp"].ToString();
                        employee.NSS = reader["NSS"].ToString();
                        employee.BirthDay = reader["BirthDay"] == DBNull.Value ? null : (DateTime?)reader["BirthDay"];
                        employee.AdmissionDate = reader["AdmissionDate"] == DBNull.Value ? null : (DateTime?)reader["AdmissionDate"];
                        employee.StreetName = reader["StreetName"].ToString();
                        employee.NumberExt = reader["NumberExt"].ToString();
                        employee.NumberInt = reader["NumberInt"].ToString();
                        employee.Delegation = reader["Delegation"].ToString();
                        employee.Colony = reader["Colony"].ToString();
                        employee.CP = reader["CP"].ToString();
                        employee.PhoneNumber = reader["PhoneNumber"].ToString();
                        employee.CellPhoneNumber = reader["CellPhoneNumber"].ToString();
                        employee.OfficePhone = reader["OfficePhone"].ToString();
                        employee.OfficeExt = reader["OfficeExt"].ToString();
                        employee.PersonalEmail = reader["PersonalEmail"].ToString();
                        employee.CompanyEmail = reader["CompanyEmail"].ToString();
                        employee.Enabled = (bool)reader["Enabled"];
                        employee.LocalizationId = reader["LocalizationId"] == DBNull.Value ? null : (int?)reader["LocalizationId"];
                        employee.PayRollId = reader["PayRollId"] == DBNull.Value ? null : (int?)reader["PayRollId"];
                        employee.CompanyId = reader["CompanyId"] == DBNull.Value ? null : (int?)reader["CompanyId"];
                        employee.NationalityId = reader["NationalityId"] == DBNull.Value ? null : (int?)reader["NationalityId"];
                        employee.EmployeePositionId = reader["EmployeePositionId"] == DBNull.Value ? null : (int?)reader["EmployeePositionId"];
                        employee.BossId = reader["BossId"] == DBNull.Value ? null : (int?)reader["BossId"];
                        employee.BossName = reader["BossName"].ToString();
                        employee.AreaName = reader["AreaName"].ToString();
                        employee.PositionId = reader["PositionId"] == DBNull.Value ? null : (int?)reader["PositionId"];
                        employee.PositionName = reader["PositionName"].ToString();
                        employee.AreaId = reader["AreaId"] == DBNull.Value ? null : (int?)reader["AreaId"];
                        employee.FingerPrint = reader["FingerPrint"] == DBNull.Value ? false : Convert.ToBoolean(reader["FingerPrint"]) ? true : false;
                        employee.ParkingSpace = reader["ParkingSpace"].ToString();
                        employee.CardId = reader["CardId"].ToString();
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


        public Result GetOneEmployeeSystem(out Employee employee, string CompanyEmail)
        {
            _SpProcedureName = "[dbo].[Sp_GetOneEmployeeSystem]";
            Result resultado = new Result();
            employee = null;
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>{
                                                             new SqlParameter("@CompanyEmail", CompanyEmail)
                };
                SqlDataReader reader = _dao.ExecuteReader(parametros, _SpProcedureName);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employee = new Employee();

                        employee.EmployeeId = reader["EmployeeId"].ToString();
                        //employee.CardId = reader["CardId"].ToString();
                        employee.CardNumber = reader["CardNumber"].ToString();
                        employee.FirstName = reader["FirstName"].ToString();
                        employee.LastName = reader["LastName"].ToString();
                        employee.MiddleName = reader["MiddleName"].ToString();
                        employee.FullName = reader["FullName"].ToString();
                        //employee.Photo = reader["Photo"].ToString();
                        employee.Rfc = reader["Rfc"].ToString();
                        employee.Curp = reader["Curp"].ToString();
                        employee.NSS = reader["NSS"].ToString();
                        employee.BirthDay = reader["BirthDay"] == DBNull.Value ? null : (DateTime?)reader["BirthDay"];
                        employee.AdmissionDate = reader["AdmissionDate"] == DBNull.Value ? null : (DateTime?)reader["AdmissionDate"];
                        employee.StreetName = reader["StreetName"].ToString();
                        employee.NumberExt = reader["NumberExt"].ToString();
                        employee.NumberInt = reader["NumberInt"].ToString();
                        employee.Delegation = reader["Delegation"].ToString();
                        employee.Colony = reader["Colony"].ToString();
                        employee.CP = reader["CP"].ToString();
                        employee.PhoneNumber = reader["PhoneNumber"].ToString();
                        employee.CellPhoneNumber = reader["CellPhoneNumber"].ToString();
                        employee.OfficePhone = reader["OfficePhone"].ToString();
                        employee.OfficeExt = reader["OfficeExt"].ToString();
                        employee.PersonalEmail = reader["PersonalEmail"].ToString();
                        employee.CompanyEmail = reader["CompanyEmail"].ToString();
                        employee.Enabled = (bool)reader["Enabled"];
                        employee.LocalizationId = reader["LocalizationId"] == DBNull.Value ? null : (int?)reader["LocalizationId"];
                        employee.PayRollId = reader["PayRollId"] == DBNull.Value ? null : (int?)reader["PayRollId"];
                        employee.CompanyId = reader["CompanyId"] == DBNull.Value ? null : (int?)reader["CompanyId"];
                        employee.NationalityId = reader["NationalityId"] == DBNull.Value ? null : (int?)reader["NationalityId"];
                        employee.EmployeePositionId = reader["EmployeePositionId"] == DBNull.Value ? null : (int?)reader["EmployeePositionId"];
                        employee.BossId = reader["BossId"] == DBNull.Value ? null : (int?)reader["BossId"];
                        employee.BossName = reader["BossName"].ToString();
                        employee.AreaName = reader["AreaName"].ToString();
                        employee.PositionName = reader["PositionName"].ToString();
                        employee.AreaId = reader["AreaId"] == DBNull.Value ? null : (int?)reader["AreaId"];
                        employee.Rol = reader["RolId"] ==DBNull.Value ? Enumeraciones.enumRoles.Empleado: (Enumeraciones.enumRoles)reader["RolId"];
                        employee.RolDescripcion = reader["Rol"].ToString();
                        

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



        public Result UpdateEmployee(Employee employee)
        {
            _SpProcedureName = "[dbo].[Sp_UpdateEmployee]";
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>{
                        new SqlParameter("@EmployeeId"          ,employee.EmployeeId)
                       ,new SqlParameter("@CardNumber"         ,employee.CardNumber)
                       ,new SqlParameter("@FirstName"          ,employee.FirstName)
                       ,new SqlParameter("@LastName"           ,employee.LastName)
                       ,new SqlParameter("@MiddleName"         ,employee.MiddleName)
                       ,new SqlParameter("@FullName"           ,employee.FullName)
                       ,new SqlParameter("@Rfc"                ,employee.Rfc)
                       ,new SqlParameter("@Curp"               ,employee.Curp)
                       ,new SqlParameter("@NSS"                ,employee.NSS)
                       ,new SqlParameter("@BirthDay"           ,employee.BirthDay)
                       ,new SqlParameter("@AdmissionDate"      ,employee.AdmissionDate)
                       ,new SqlParameter("@PhoneNumber"        ,employee.PhoneNumber)
                       ,new SqlParameter("@CellPhoneNumber"    ,employee.CellPhoneNumber)
                       ,new SqlParameter("@OfficePhone"        ,employee.OfficePhone)
                       ,new SqlParameter("@OfficeExt"          ,employee.OfficeExt)
                       ,new SqlParameter("@PersonalEmail"      ,employee.PersonalEmail)
                       ,new SqlParameter("@CompanyEmail"       ,employee.CompanyEmail)
                       ,new SqlParameter("@Activo"             ,employee.Enabled)
                       ,new SqlParameter("@StreetName"         ,employee.StreetName)
                       ,new SqlParameter("@NumberExt"          ,employee.NumberExt)
                       ,new SqlParameter("@NumberInt"          ,employee.NumberInt)
                       ,new SqlParameter("@Delegation"         ,employee.Delegation)
                       ,new SqlParameter("@Colony"             ,employee.Colony)
                       ,new SqlParameter("@CP"                 ,employee.CP)
                       ,new SqlParameter("@LocalizationId"     ,employee.LocalizationId)
                       ,new SqlParameter("@PayRollId"          ,employee.PayRollId)
                       ,new SqlParameter("@CompanyId"          ,employee.CompanyId)
                       ,new SqlParameter("@NationalityId"      ,employee.NationalityId)
                       ,new SqlParameter("@EmployeePositionId" ,employee.EmployeePositionId)
                       ,new SqlParameter("@BossId"             ,employee.BossId)
                       ,new SqlParameter("@PositionId"         ,employee.PositionId)
                       ,new SqlParameter("@AreaId"             ,employee.AreaId)
                       ,new SqlParameter("@ParkingSpace"       ,employee.ParkingSpace)
                       ,new SqlParameter("@FingerPrint"       ,employee.FingerPrint)
                        ,new SqlParameter("@CardId"             ,employee.CardId)

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


        public Result CreateEmployee(Employee employee)
        {
            _SpProcedureName = "[dbo].[Sp_CreateEmployee]";
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>{
                        new SqlParameter("@CardNumber"         ,employee.CardNumber)
                       ,new SqlParameter("@FirstName"          ,employee.FirstName)
                       ,new SqlParameter("@LastName"           ,employee.LastName)
                       ,new SqlParameter("@MiddleName"         ,employee.MiddleName)
                       ,new SqlParameter("@FullName"           ,employee.FullName)
                       ,new SqlParameter("@Rfc"                ,employee.Rfc)
                       ,new SqlParameter("@Curp"               ,employee.Curp)
                       ,new SqlParameter("@NSS"                ,employee.NSS)
                       ,new SqlParameter("@BirthDay"           ,employee.BirthDay)
                       ,new SqlParameter("@AdmissionDate"      ,employee.AdmissionDate)
                       ,new SqlParameter("@PhoneNumber"        ,employee.PhoneNumber)
                       ,new SqlParameter("@CellPhoneNumber"    ,employee.CellPhoneNumber)
                       ,new SqlParameter("@OfficePhone"        ,employee.OfficePhone)
                       ,new SqlParameter("@OfficeExt"          ,employee.OfficeExt)
                       ,new SqlParameter("@PersonalEmail"      ,employee.PersonalEmail)
                       ,new SqlParameter("@CompanyEmail"       ,employee.CompanyEmail)
                       ,new SqlParameter("@Activo"             ,employee.Enabled)
                       ,new SqlParameter("@StreetName"         ,employee.StreetName)
                       ,new SqlParameter("@NumberExt"          ,employee.NumberExt)
                       ,new SqlParameter("@NumberInt"          ,employee.NumberInt)
                       ,new SqlParameter("@Delegation"         ,employee.Delegation)
                       ,new SqlParameter("@Colony"             ,employee.Colony)
                       ,new SqlParameter("@CP"                 ,employee.CP)
                       ,new SqlParameter("@LocalizationId"     ,employee.LocalizationId)
                       ,new SqlParameter("@PayRollId"          ,employee.PayRollId)
                       ,new SqlParameter("@CompanyId"          ,employee.CompanyId)
                       ,new SqlParameter("@NationalityId"      ,employee.NationalityId)
                       ,new SqlParameter("@PositionId"         ,employee.PositionId)
                       ,new SqlParameter("@BossId"             ,employee.BossId)
                       ,new SqlParameter("@AreaId"             ,employee.AreaId)
                       ,new SqlParameter("@ParkingSpace"       ,employee.ParkingSpace)
                       ,new SqlParameter("@FingerPrint"        ,employee.FingerPrint)
                       ,new SqlParameter("@CardId"             ,employee.CardId)
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

        public Result GetCatalogos(out List<Catalogo> lstCatalogos)
        {
            lstCatalogos = null;
            _SpProcedureName = "[dbo].[Sp_GetCatalogos]";
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();

                SqlDataReader reader = _dao.ExecuteReader(parametros, _SpProcedureName);
                if (reader.HasRows)
                {
                    lstCatalogos = new List<Catalogo>();
                    while (reader.Read())
                    {
                        Catalogo catalogo = new Catalogo();
                        catalogo.ID = (int)reader["ID"];
                        catalogo.DESCRIPTION = reader["DESCRIPTION"].ToString();
                        catalogo.ORIGEN = reader["ORIGEN"].ToString();
                        lstCatalogos.Add(catalogo);
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


        public Result GetAllJefes(out List<Jefe> lstJefes)
        {
            lstJefes = null;
            _SpProcedureName = "[dbo].[Sp_GetAllJefes]";
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();

                SqlDataReader reader = _dao.ExecuteReader(parametros, _SpProcedureName);
                if (reader.HasRows)
                {
                    lstJefes = new List<Jefe>();
                    while (reader.Read())
                    {
                        Jefe jefe = new Jefe();
                        jefe.BossId = (int)reader["BossId"];
                        jefe.FullName = reader["FullName"].ToString();
                        
                        lstJefes.Add(jefe);
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




    }
}