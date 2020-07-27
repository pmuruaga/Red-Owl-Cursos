using MediatR;
using Dominio;
using Persistencia;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using System.Net;
using System;
using AutoMapper;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDto>{
            public Guid Id { get; set; }
        }

        public class Manejador: IRequestHandler<CursoUnico,CursoDto>{
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<CursoDto> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.InstructorLink)
                    .ThenInclude(y => y.Instructor)
                    .FirstOrDefaultAsync(a => a.CursoId == request.Id);

                if (curso == null)
                {                    
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" });
                }

                var cursoDto = _mapper.Map<Curso, CursoDto>(curso);

                return cursoDto;
            }
        }
    }
}