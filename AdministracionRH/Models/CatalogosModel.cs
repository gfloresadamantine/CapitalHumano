using AdministracionRH.Common;
using AdministracionRH.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionRH.Models
{
    public class CatalogosModel
    {
        public List<Catalogo> lstCatalogos { get; set; }
        Enumeraciones.enumCatalogos _enumCatalogo { get; set; }
        CatalogoService catalogoService = null;
        public string HeaderDescription { get; set; }
        public Catalogo CatalogoItem { get; set; }
        public string Origen { get; set; }
        public Enumeraciones.enumCatalogos EnumCatalogo { get; set; }
        public Enumeraciones.enumOperacion EnumOperacion { get; set; }
        public int ID { get; set; }


        public CatalogosModel()
        {
            lstCatalogos = new List<Catalogo>();
        }


        public CatalogosModel(Enumeraciones.enumCatalogos enumCatalogo)
        {
            lstCatalogos = new List<Catalogo>();
            catalogoService = new CatalogoService(enumCatalogo);
            HeaderDescription = catalogoService.HeaderDescription;
            lstCatalogos = catalogoService.GetCatalogo();
            Origen = lstCatalogos.Select(i => i.ORIGEN).FirstOrDefault();
            CatalogoItem = new Catalogo();
            EnumCatalogo = enumCatalogo;
            EnumOperacion = Enumeraciones.enumOperacion.Ninguna;
            ID = 0;




        }
    }
}