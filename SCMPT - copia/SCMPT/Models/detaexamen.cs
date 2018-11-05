using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCMPT.Models
{
    public class detaexamen
    {
        public Detalles_Cita DetallesCita { get; set; }
        public List<Resultado_examen> ResultadoExamens { get; set; }
    }
}