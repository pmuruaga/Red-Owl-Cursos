using MediatR;
using System.Collections.Generic;
using Dominio;
using Persistencia;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System;
using FluentValidation;
using Aplicacion.ManejadorError;
using System.Net;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest {
            public int CursoId{ get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
        }        

        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>{

            private readonly CursosOnlineContext _context;

            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.CursoId);
                if (curso == null)
                {                    
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" });
                }

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                var resultado = await _context.SaveChangesAsync();

                if(resultado > 0){
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}