﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Cursos
{
    public class ComentarioDto
    {
        public Guid ComentarioId { get; set; }
        public string Alumno { get; set; }
        public int Puntaje { get; set; }
        public string ComentarioTexto { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Guid CursoId { get; set; }
    }
}
