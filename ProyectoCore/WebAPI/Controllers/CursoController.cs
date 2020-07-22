using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dominio;
using Aplicacion.Cursos;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    //http://localhost:5000/api/Cursos
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : MiControllerBase
    {        

        [HttpGet]
        //[Authorize] Quito el authorize, lo pondré a nivel de Startup.cs a nivel de controllers genericos.
        public async Task<ActionResult<List<CursoDto>>> Get(){
            return await Mediator.Send(new Consulta.ListaCursos());
        }

        //http://localhost:5000/api/Cursos/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> Detalle(int id){
            return await Mediator.Send(new ConsultaId.CursoUnico{ Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data){
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(int id, Editar.Ejecuta data){
            data.CursoId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id){
            return await Mediator.Send(new Eliminar.Ejecuta{Id = id});
        }
    }
}