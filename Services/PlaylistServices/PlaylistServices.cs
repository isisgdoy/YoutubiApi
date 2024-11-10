using YoutubiApi.Data;
using YoutubiApi.Models;

namespace YoutubiApi.Services.PlaylistServices
{
    public class PlaylistServices(PlaylistRepository playlistRepository)
    {
        private readonly PlaylistRepository _playlistRepository = playlistRepository;


        public async Task<List<Playlist>> GetAllPlaylistsAsync()
        {
            return await _playlistRepository.GetAllPlaylistsAsync();
        }
                
        public async Task<Playlist> GetPlaylistByIdAsync(int id)
        {
            return await _playlistRepository.GetPlaylistById(id);
        }
                
        public async Task<Playlist> AddPlaylistAsync(Playlist playlist)
        {
            await _playlistRepository.AddPlaylist(playlist);
            return playlist;
        }
               
        public async Task<bool> UpdatePlaylistAsync(Playlist playlist)
        {
            var existingPlaylist = await _playlistRepository.GetPlaylistById(playlist.PlaylistId);
            if (existingPlaylist == null)
            {
                return false; 
            }

            await _playlistRepository.UpdatePlaylist(playlist);
            return true;
        }
                
        public async Task<bool> DeletePlaylistAsync(int id)
        {
            var existingPlaylist = await _playlistRepository.GetPlaylistById(id);
            if (existingPlaylist == null)
            {
                return false; 
            }

            await _playlistRepository.DeletePlaylist(id);
            return true;
        }
    }
}