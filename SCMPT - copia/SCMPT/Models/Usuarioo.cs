using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCMPT.Models
{
    public class Usuarioo
    {
        [Key]
        public int idUsuario { get; set; }
        public string aliasUsuario { get; set; }
        public string contraseña { get; set; }
        public bool Activo { get; set; }
    }
}