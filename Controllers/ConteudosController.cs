using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YoutubiApi.Data;
using YoutubiApi.Models;

namespace YoutubiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConteudosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConteudosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var conteudos = await _context.Conteudos.Include(c => c.Criador).ToListAsync();
            return Ok(conteudos); // Retorna todos os conteúdos em formato JSON
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            var conteudo = await _context.Conteudos.Include(c => c.Criador).FirstOrDefaultAsync(c => c.Id == id);

            if (conteudo == null)
            {
                return NotFound();
            }

            return Ok(conteudo);
        }

        [HttpPost]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem criar conteúdos
        public async Task<IActionResult> Create([FromBody] Conteudo conteudo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Mantendo validação
            }

            _context.Conteudos.Add(conteudo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = conteudo.Id }, conteudo);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem editar conteúdos
        public async Task<IActionResult> Edit(int id, [FromBody] Conteudo conteudo)
        {
            if (id != conteudo.Id)
            {
                return BadRequest();
            }

            _context.Entry(conteudo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Conteudos.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem excluir conteúdos
        public async Task<IActionResult> Delete(int id)
        {
            var conteudo = await _context.Conteudos.FindAsync(id);
            if (conteudo == null)
            {
                return NotFound();
            }

            _context.Conteudos.Remove(conteudo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
