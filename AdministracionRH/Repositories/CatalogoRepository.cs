using AdministracionRH.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdministracionRH.Repositories
{
    public class CatalogoRepository
    {
        private DAO _dao;
        string _SpProcedureName { get; set; }
        public Enumeraciones.enumCatalogos _enumCatalogo { get; set; }
        public CatalogoRepository()
        {
            _dao = new DAO();
        }

        public CatalogoRepository(Enumeraciones.enumCatalogos enumeracion)
        {
            _enumCatalogo = enumeracion;
            _dao = new DAO();
        }


        public Result Update(Catalogo catalogo)
        {
            _SpProcedureName = "[dbo].[Sp_UpdateItemCatalogo]";
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>{
                        new SqlParameter("@DESCRIPCION" ,catalogo.DESCRIPTION)
                       ,new SqlParameter("@Origen" ,_enumCatalogo)
                       ,new SqlParameter("@ID"     ,catalogo.ID)
                       };

                _dao.ExecuteProcedure(parametros, _SpProcedureName);
                resultado.Success = true;
            }
            catch (Exception e)
            {

                resultado.Success = false;
                resultado.ErrorMessage = String.Format("Se ha producido un error al procesar datos del catálogo {0}", e.Message);
            }

            return resultado;
        }

        public Result Create(Catalogo Catalogo)
        {
            _SpProcedureName = "[dbo].[Sp_CreateItemCatalogo]";
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>{
                         new SqlParameter("@DESCRIPCION" ,Catalogo.DESCRIPTION)
                        ,new SqlParameter("@Origen" ,_enumCatalogo)
                       };

                _dao.ExecuteProcedure(parametros, _SpProcedureName);
                resultado.Success = true;
            }
            catch (Exception e)
            {

                resultado.Success = false;
                resultado.ErrorMessage = String.Format("Se ha producido un error al procesar datos del catálogo {0}", e.Message);
            }

            return resultado;
        }

        public Result Delete(int Id)
        {
            _SpProcedureName = "[dbo].[Sp_DeletetemCatalogo]";
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>{
                        new SqlParameter("@Origen" ,_enumCatalogo)
                       ,new SqlParameter("@ID"     ,Id)

                       };

                _dao.ExecuteProcedure(parametros, _SpProcedureName);
                resultado.Success = true;
            }
            catch (Exception e)
            {

                resultado.Success = false;
                resultado.ErrorMessage = String.Format("Se ha producido un error al procesar datos del catálogo {0}", e.Message);
            }

            return resultado;
        }

        public Result GetCatalogo(out List<Catalogo>  lstCatalogo)
        {
            _SpProcedureName = "[dbo].[Sp_GetOneCatalogo]";
            lstCatalogo = new List<Catalogo>();
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>{
                        new SqlParameter("@Origen" ,Convert.ToInt32(_enumCatalogo))
                       };

                SqlDataReader reader = _dao.ExecuteReader(parametros, _SpProcedureName);
                if (reader.HasRows)
                {
                    lstCatalogo = new List<Catalogo>();
                    while (reader.Read())
                    {
                        Catalogo catalogo = new Catalogo();
                        catalogo.ID = (int)reader["ID"];
                        catalogo.DESCRIPTION = reader["DESCRIPTION"].ToString();
                        catalogo.ORIGEN = reader["ORIGEN"].ToString();
                        lstCatalogo.Add(catalogo);
                    }

                }
                reader.Close();
                resultado.Success = true;
            }
            catch (Exception e)
            {

                resultado.Success = false;
                resultado.ErrorMessage = String.Format("Se ha producido un error al procesar datos del catálogo {0}", e.Message);
            }

            return resultado;
        }


        public Result GetOneItemCatalogo(out Catalogo catalogo, int Id)
        {
            _SpProcedureName = "[dbo].[Sp_GetOneCatalogo]";
            catalogo = new Catalogo();
            Result resultado = new Result();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>{
                         new SqlParameter("@Origen" ,Convert.ToInt32(_enumCatalogo))
                        ,new SqlParameter("@ID", Id)
                       };

                SqlDataReader reader = _dao.ExecuteReader(parametros, _SpProcedureName);
                if (reader.HasRows)
                {
                    
                    while (reader.Read())
                    {
                        catalogo.ID = (int)reader["ID"];
                        catalogo.DESCRIPTION = reader["DESCRIPTION"].ToString();
                        catalogo.ORIGEN = reader["ORIGEN"].ToString();
                    }

                }
                reader.Close();
                resultado.Success = true;
            }
            catch (Exception e)
            {

                resultado.Success = false;
                resultado.ErrorMessage = String.Format("Se ha producido un error al procesar datos del catálogo {0}", e.Message);
            }

            return resultado;
        }



    }
}