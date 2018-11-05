using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCMPT.Models
{
    public class Pacienteo
    {
        public string paciente { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter Fecha")]
        public DateTime Fecha { get; set; }
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Enter Hora")]
        public DateTime Hora { get; set; }
    }
}