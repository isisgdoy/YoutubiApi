using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YoutubiApi.Data;
using YoutubiApi.Models;

namespace YoutubiApi.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase 
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Incluir e-mail e senha!");
            }

            // Tenta autenticar como Usuário
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (usuario != null && VerificarSenhaHash(request.Password, usuario.Password))
            {
                var token = GerarTokenJWT(usuario.Email, "Usuario");
                return Ok(new { Token = token });
            }

            // Tenta autenticar como Criador
            var criador = await _context.Criadores.FirstOrDefaultAsync(c => c.Email == request.Email);
            if (criador != null && VerificarSenhaHash(request.Password, criador.Senha))
            {
                var token = GerarTokenJWT(criador.Email, "Criador");
                return Ok(new { Token = token });
            }

            return Unauthorized("Credenciais inválidas.");
        }
        private string GerarTokenJWT(string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool VerificarSenhaHash (string password, string storedHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return storedHash == computedHash;
            }
        }



    }
}
