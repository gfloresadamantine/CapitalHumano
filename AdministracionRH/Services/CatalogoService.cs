using AdministracionRH.Common;
using AdministracionRH.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministracionRH.Services
{
    public class CatalogoService
    {

        CatalogoRepository _catalogoRepository;
        Enumeraciones.enumCatalogos _enumCatalogo { get; set; }
        public string HeaderDescription { get; set; }
        public JsonResult jsonResult { get; set; }
       
         public CatalogoService()
        {
            _catalogoRepository = new CatalogoRepository();
        }

        public CatalogoService(Enumeraciones.enumCatalogos enumCatalogo)
        {
            _enumCatalogo = enumCatalogo;
            _catalogoRepository = new CatalogoRepository();
            _catalogoRepository._enumCatalogo = enumCatalogo;
            HeaderDescription = GetDescription();
        }

        private string GetDescription()

        {
            string desc = string.Empty;
            switch (_enumCatalogo)
            {
                case Enumeraciones.enumCatalogos.Areas:
                    {
                        desc = "Área"; break;
                    }
                case Enumeraciones.enumCatalogos.Compañias:
                    {
                        desc = "Compañía"; break;
                    }
                case Enumeraciones.enumCatalogos.Nacionalidad:
                    {
                        desc = "Nacionalidad"; break;
                    }
                case Enumeraciones.enumCatalogos.Patrones:
                    {
                        desc = "Patrón"; break;
                    }
                case Enumeraciones.enumCatalogos.Puestos:
                    {
                        desc = "Puesto"; break;
                    }
                case Enumeraciones.enumCatalogos.Ubicaciones:
                    {
                        desc = "Ubicación"; break;
                    }
              
                default:return desc;
            }
            return desc;

        }

        public Result Create(Catalogo catalogo)
        {
           return  _catalogoRepository.Create(catalogo);
        }

        public Result Update(Catalogo catalogo)
        {
            return _catalogoRepository.Update(catalogo);
        }

        public Result Delete(int Id)
        {
            return _catalogoRepository.Delete(Id);
        }

        public List<Catalogo> GetCatalogo()
        {
            Result result = new Result();
            List<Catalogo> lista = null;
            result = _catalogoRepository.GetCatalogo(out lista);
            return lista;

        }


        public Catalogo GetOneItemCatalogo(int Id)

        {
            Catalogo catalogo = null;
            Result resultado = new Result();
            resultado = _catalogoRepository.GetOneItemCatalogo(out catalogo, Id);
            return catalogo;
        }



    }
}