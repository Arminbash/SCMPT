using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCMPT.Models
{
    public class imagen
    {
        [Key]
        public string id { get; set; }
        public byte[] imagenes { get; set; }
    }
}