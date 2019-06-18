using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionRH.Common
{
    public class Result
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string SomeResult { get; set; }

        public Result()
        {
            Success = true;
            ErrorMessage = null;
            SomeResult = null;
        }
    }
}