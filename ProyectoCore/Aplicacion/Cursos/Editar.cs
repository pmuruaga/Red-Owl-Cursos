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
using System.Linq;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest {
            public Guid CursoId{ get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }

            public List<Guid> ListaInstructor { get; set; }

            public decimal? Precio { get; set; }
            public decimal? PrecioPromocion { get; set; }
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

                var precioEntidad = _context.Precio.Where(x => x.CursoId == curso.CursoId).FirstOrDefault();
                if(precioEntidad != null)
                {
                    precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
                    precioEntidad.Promocion = request.PrecioPromocion ?? precioEntidad.Promocion;
                }
                else
                {
                    precioEntidad = new Precio
                    {
                        CursoId = curso.CursoId,
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.PrecioPromocion ?? 0,
                        PrecioId = Guid.NewGuid()
                    };
                    await _context.Precio.AddAsync(precioEntidad);
                }

                if (request.ListaInstructor != null)
                {
                    if(request.ListaInstructor.Count > 0)
                    {
                        //Elimino los que ya tiene...
                        var instructoresDB = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();
                        foreach(var instructor in instructoresDB)
                        {
                            _context.CursoInstructor.Remove(instructor);
                        }

                        //Agrego instructores que manda el cliente...
                        foreach(var id in request.ListaInstructor)
                        {
                            var nuevoInstructor = new CursoInstructor
                            {
                                CursoId = request.CursoId,
                                InstructorId = id
                            };
                            _context.CursoInstructor.Add(nuevoInstructor);
                        }
                    }
                }

                var resultado = await _context.SaveChangesAsync();

                if(resultado > 0){
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}