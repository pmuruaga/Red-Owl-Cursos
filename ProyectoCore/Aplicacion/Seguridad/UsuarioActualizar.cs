﻿using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    /*La idea es agregar en esta clase y las del mismo commit, lo que haga falta para hacer un mantenimiento de usuarios. Actualizar contraseña por ejemplo.*/
    public class UsuarioActualizar
    {
        public class Ejecuta : IRequest<UsuarioData> { 
            public string NombreCompleto { get; set; }            
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta> {
            public EjecutaValidador()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }    
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IPasswordHasher<Usuario> _passwordHasher;
            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IPasswordHasher<Usuario> passwordHasher)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _passwordHasher = passwordHasher;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuarioIden = await _userManager.FindByNameAsync(request.Username);
                if (usuarioIden == null) {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No existe un usuario con este username" });
                }

                //Chequea si ya existe un usuario con el mail que estoy tratando de actualizar.
                var resultado = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.Username).AnyAsync();
                if (resultado) {
                    throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, new { mensaje = "Este email pertenece a otro usuario" });
                }

                usuarioIden.NombreCompleto = request.NombreCompleto;
                usuarioIden.PasswordHash = _passwordHasher.HashPassword(usuarioIden, request.Password);
                usuarioIden.Email = request.Email;

                var resultadoUpdate = await _userManager.UpdateAsync(usuarioIden);

                var resultadoRoles = await _userManager.GetRolesAsync(usuarioIden);
                var listRoles = new List<string>(resultadoRoles);

                if (resultadoUpdate.Succeeded) {
                    return new UsuarioData
                    {
                        NombreCompleto = usuarioIden.NombreCompleto,
                        Username = usuarioIden.UserName,
                        Email = usuarioIden.Email,
                        Token = _jwtGenerador.CrearToken(usuarioIden, listRoles)
                    };
                }

                throw new Exception("No se pudo actualizar el usuario");
            }
        }
    }
}
