using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YoutubiApi.Data;
using YoutubiApi.Models;

namespace YoutubiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistControler : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlaylistControler(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Usuario")]  // Somente usuários autenticados podem acessar playlists
        public async Task<IActionResult> Index()
        {
            var playlists = await _context.Playlists.Include(p => p.ItemPlaylists).ThenInclude(ip => ip.Conteudo).ToListAsync();
            return Ok(playlists);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Usuario")]  // Somente usuários autenticados podem acessar playlists individuais
        public async Task<IActionResult> Details(int id)
        {
            var playlist = await _context.Playlists.Include(
                p => p.ItemPlaylists).ThenInclude(
                ip => ip.Conteudo).FirstOrDefaultAsync(
                p => p.PlaylistId == id);

            if (playlist == null)
            {
                return NotFound();
            }

            return Ok(playlist);
        }
        [HttpPost]
        [Authorize(Roles = "Usuario")]  // Somente usuários autenticados podem criar playlists
        public async Task<IActionResult> Create([FromBody] Playlist playlist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = playlist.PlaylistId }, playlist);
        }
         
        [HttpPut("{id}")]
        [Authorize(Roles = "Usuario")]  // Somente usuários autenticados podem editar playlists
        public async Task<IActionResult> Edit(int id, [FromBody] Playlist playlist)
        {
            if (id != playlist.PlaylistId)
            {
                return BadRequest();
            }

            _context.Entry(playlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Playlists.Any(e => e.PlaylistId == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Usuario")]  // Somente usuários autenticados podem excluir playlists
        public async Task<IActionResult> Delete(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
