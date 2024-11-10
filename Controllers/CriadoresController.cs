using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YoutubiApi.Data;
using YoutubiApi.Models;
using System.Text;

namespace YoutubiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CriadoresController :  ControllerBase
    {
        private readonly AppDbContext _context;

        public CriadoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem listar criadores
        public async Task<IActionResult> Index()
        {
            var criadores = await _context.Criadores.ToListAsync();
            return Ok(criadores);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem acessar detalhes
        public async Task<IActionResult> Details(int id)
        {
            var criador = await _context.Criadores.FindAsync(id);

            if (criador == null)
            {
                return NotFound();
            }

            return Ok(criador);
        }

        [HttpPost]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem criar novos criadores
        public async Task<IActionResult> Create(Criador criador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            criador.Senha = 
            criador.Senha = HashPassword(criador.Senha); // Hash de senha adicionado
            _context.Criadores.Add(criador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = criador.Id }, criador);
        }
        private string HashPassword(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return passwordHash;
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem editar dados de criadores
        public async Task<IActionResult> Edit(int id, Criador criador)
        {
            if (id != criador.Id)
            {
                return BadRequest();
            }

            _context.Entry(criador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Criadores.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem excluir criadores
        public async Task<IActionResult> Delete(int id)
        {
            var criador = await _context.Criadores.FindAsync(id);
            if (criador == null)
            {
                return NotFound();
            }

            _context.Criadores.Remove(criador);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
