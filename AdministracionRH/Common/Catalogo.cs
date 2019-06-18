using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdministracionRH.Common
{
    public class Catalogo
    {
        public int ID { get; set; }
        [Required]
        public string DESCRIPTION { get; set; }
        public string ORIGEN { get; set; }
        public string OTRA_DESC { get; set; }
    }


}