using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace SCMPT.Models
{
    public class Personao
    {
        [Key]
        public int idPersona { get; set; }
        public int idUsuario { get; set; }
        public string Primer_Nombre { get; set; }
        public string Segundo_Nombre { get; set; }
        public string Primer_Apellido { get; set; }
        public string Segundo_Apellido { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        public string Genero { get; set; }
        public string Cedula { get; set; }
        public int Telefono { get; set; }
        public string Email { get; set; }
        public Image Foto { get; set; }
        public bool Activo { get; set; }
    }
}