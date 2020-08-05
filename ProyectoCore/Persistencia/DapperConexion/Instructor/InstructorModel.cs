using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorModel
    {
        //Deben matchear con los nombres que vienen del resultado del sp de la bd.
        public Guid InstructorId { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Grado { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
