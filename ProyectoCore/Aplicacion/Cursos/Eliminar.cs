using MediatR;
using System.Collections.Generic;
using Dominio;
using Persistencia;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System;
using Aplicacion.ManejadorError;
using System.Net;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest {
            public int Id{ get; set; }
        }   

        public class Manejador : IRequestHandler<Ejecuta>{
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.Id);
                if(curso == null){
                    //throw new Exception("El curso no existe");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" });
                }                

                _context.Curso.Remove(curso);
                var valor = await _context.SaveChangesAsync();

                if(valor > 0){
                    return Unit.Value;
                }

                throw new Exception("No se pudo borrar el curso");
            }
        }        
    }
}