namespace YoutubiApi.Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        public string Name { get; set; }
        public int UuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public List<ItemPlaylist> ItemPlaylists { get; set; }
    }
}
