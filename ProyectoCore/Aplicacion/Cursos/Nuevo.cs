using MediatR;
using System;
using Persistencia;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using System.Collections.Generic;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest {

            //[Required(ErrorMessage="Por favor ingrese el Titulo del Curso")] //Para agregar la validación
            public string Titulo {get; set;}
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }

            public List<Guid> ListaInstructor { get; set; }
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
                Guid _cursoId = Guid.NewGuid();
                var curso = new Curso {
                    CursoId = _cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };

                _context.Curso.Add(curso);

                if (request.ListaInstructor!=null) {                    
                    foreach(var id in request.ListaInstructor)
                    {
                        var cursoInstructor = new CursoInstructor
                        {
                            CursoId = _cursoId,
                            InstructorId = id
                        };

                        _context.CursoInstructor.Add(cursoInstructor);
                    }
                }

                var valor = await _context.SaveChangesAsync(); //Si es 0 no se inserto, hubo error. Si es 1 grabo 1...

                if(valor > 0){
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}