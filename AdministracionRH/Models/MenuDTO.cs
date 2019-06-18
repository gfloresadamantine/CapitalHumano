using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionRH.Models
{
    public class MenuDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public virtual ICollection<MenuItem> Children { get; set; }
    }
}