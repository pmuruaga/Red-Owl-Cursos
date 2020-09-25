using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistencia;
using MediatR;
using Aplicacion.Cursos;
using FluentValidation.AspNetCore;
using WebAPI.Middleware;
using Dominio;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication;
using Aplicacion.Contratos;
using Seguridad.TokenSeguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.Instructor;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Persistencia.DapperConexion.Paginacion;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //This is for EFCore
            services.AddDbContext<CursosOnlineContext>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //This is for Dapper. Adding connection for dapper.
            services.AddOptions();
            services.Configure<ConexionConfiguracion>(Configuration.GetSection("ConnectionStrings"));

            services.AddMediatR(typeof(Consulta.Manejador).Assembly);

            //Agrego el AddFluentValidation y el using de la libreria para las validaciones.
            //Luego al addController le paso las politicas para filtrar y chequear que se acceda con token, con authorizacion.
            services.AddControllers( opt => {
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    opt.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddFluentValidation( cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            //Agrego un builder y un identityBuilder y le asigno un store... 
            var builder = services.AddIdentityCore<Usuario>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);

            //Agrego la siguiente linea para manejar el tema de roles. Instancio el servicio de RoleManager.
            identityBuilder.AddRoles<IdentityRole>();
            identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario, IdentityRole>>();
            
            identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
            //Le indico que quien va manejar el login va ser coreIdentity
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();

            //
            services.TryAddSingleton<ISystemClock, SystemClock>();

            //Agrego la seguridad, para que no me deje consumir los controllers si no tengo un token.
            //Ademas agregar en el configure que uso autentication
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
                //Aqui puedo indicar de donde acepto el token/las llamadas por ejemplo. Restricciones de url/dns.
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false, //Esta abierto para cualquiera, si es algo cerrado deberia poner el ip de donde pueden acceder
                    ValidateIssuer = false //Valida quien dio el token
                };
            });


            //Agrego el servicio del generador de token para jwt
            //Haciendo esta inyection de servicios webapi para poder ingresar a los servicios del generador de token.
            services.AddScoped<IJwtGenerador, JwtGenerador>();

            //Agrego el servicio que me devuelve el usuario actual.
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();

            services.AddAutoMapper(typeof(Consulta.Manejador));

            services.AddTransient<IFactoryConnection, FactoryConnection>();
            services.AddScoped<IInstructor, InstructorRepositorio>();
            services.AddScoped<IPaginacion, PaginacionRepositorio>();

            //Agregado para incluir swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Services para mantenimiento de cursos",
                    Version = "v1"
                });
                //Le indico que tome todo el nombre de las clases (con namespace), sino me va dar error porque tengo en aplicación varios similares-> Nuevo.Ejecuta por ejem.
                c.CustomSchemaIds( x => x.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ManejadorErrorMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            //Esto habilitarlo al deployar en un hosting con https. Para local o pruebas mantener comentado.
            //app.UseHttpsRedirection(); 

            app.UseAuthentication(); //Luego a los controles agregar el Authorize decorator.

            app.UseRouting();

            app.UseAuthorization();

            //Agrego para swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cursos Online V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });            
        }
    }
}
