using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace SCMPT.Models
{
    public class Proc_Usuarioo
    {
        public string id { get; set; }
        public string Primer_Nombre { get; set; }
        public string Segundo_Nombre { get; set; }
        public string Primer_Apellido { get; set; }
        public string Segundo_Apellido { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter Fecha")]
        public DateTime Fecha_Nacimiento { get; set; }
        public string Genero { get; set; }
        public string Cedula { get; set; }
        public int Telefono { get; set; }
        public string Email { get; set; }
        public string Especialidad { get; set; }
    }
}