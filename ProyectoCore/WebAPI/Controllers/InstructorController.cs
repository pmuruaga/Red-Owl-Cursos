using Aplicacion.Instructores;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class InstructorController : MiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructores()
        {
            return await Mediator.Send(new Consulta.Lista());
        }
    }
}
