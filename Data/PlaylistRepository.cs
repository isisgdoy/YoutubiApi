using YoutubiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace YoutubiApi.Data
{
    public class PlaylistRepository
    {
        private readonly AppDbContext _context;

        public PlaylistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Playlist>> GetAllPlaylistsAsync()
        {
            return await _context.Playlists
                             .Include(p => p.ItemPlaylists)
                             .ThenInclude(ip => ip.Conteudo)
                             .ToListAsync(); 
        }
                
        public async Task<Playlist> GetPlaylistById(int id)
        {
            return await _context.Playlists
                             .Include(p => p.ItemPlaylists)
                             .ThenInclude(ip => ip.Conteudo)
                             .FirstOrDefaultAsync(p => p.PlaylistId == id);
        }

       
        public async Task AddPlaylist(Playlist playlist)
        {
            await _context.Playlists.AddAsync(playlist);
            await _context.SaveChangesAsync();
        }

       
        public async Task UpdatePlaylist(Playlist playlist)
        {
            _context.Playlists.Update(playlist); 
            await _context.SaveChangesAsync();  
        }

        
        public async Task DeletePlaylist(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id); // Localiza a playlist
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist); // Remove a playlist
                await _context.SaveChangesAsync();  
            }
        }
    }
}
