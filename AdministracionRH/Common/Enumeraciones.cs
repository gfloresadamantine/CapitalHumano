using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionRH.Common
{
    public static class Enumeraciones
    {
        public  enum enumCatalogos
        {
             Areas=1
            , Patrones = 2
            , Puestos = 3
            , Ubicaciones = 4
            , Compañias = 5
            , Nacionalidad = 6
        }

        public enum enumOperacion
        {
              Ninguna=0
            , Nuevo = 1
            , Editar = 2
            , Borra = 3
        }

        public enum enumRoles
        {
           AdministradorSistemas = 1
         , AdministradorRh = 2
         , DireccionGeneral = 3
         , DireccionArea = 4
         , SubDirector = 5
         , Gerente =6
         , Empleado =7

        }

        public enum enumPeriodoBusqueda
        {
            EstaSemana =1,
            LaSemanaPasada =2 ,
            HaceDosSemanas =3,
            DefinirPeriodo =4

        }

        public enum TipoIncidenciaHora
        {
            Ninguna = 0,
            EntradaOk=1,
            SalidaOk =2,
            Retardo =3,
            FaltaPorRetardo = 4,
            FaltaPorInasistencia =5,
            NoRegistroEntrada =6,
            NoRegistroSalida =7,
            RegistroSalidaAnticipada =8
        }


        public enum TipoOperacionLector
        {
            Entrada = 1,
            Salida = 2
        }

    }
}