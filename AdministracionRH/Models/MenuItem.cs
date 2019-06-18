﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionRH.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public virtual ICollection<MenuItem> Children { get; set; }
        public bool boolSelectd { get; set; }
    }
}