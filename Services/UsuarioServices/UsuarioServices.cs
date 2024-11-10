//a controller se conecta com a interface e a interface irá se conectar com os 'serviços' e os serviços que irá se
//conectar com o banco de dados e devolver essas informações para o controller 
using System.Text;
using YoutubiApi.Data;
using YoutubiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace YoutubiApi.Services.UsuarioServices
{
    public class UsuarioService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;
                
        public string HashPassword(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return passwordHash;
            }
        }

        public async Task<Usuario> CreateUser(Usuario usuario)
        {
            usuario.Password = HashPassword(usuario.Password);
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
