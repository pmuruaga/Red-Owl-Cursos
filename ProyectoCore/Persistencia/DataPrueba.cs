using Dominio;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia
{
    public class DataPrueba
    {
        public static async Task InsertarData(CursosOnlineContext context, UserManager<Usuario> usuarioManager)
        {
            if (!usuarioManager.Users.Any())
            {
                var usuario = new Usuario { NombreCompleto = "Pablo Mur", UserName = "pablomur", Email = "pmuruaga@gmail.com" };
                await usuarioManager.CreateAsync(usuario, "Password123$");
            }
        }
    }
}
