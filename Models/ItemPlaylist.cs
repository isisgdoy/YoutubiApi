namespace YoutubiApi.Models
{
    public class ItemPlaylist
    {
        public int IdPlaylist { get; set; }
        public Playlist Playlist { get; set; }
        public int IdConteudo { get; set; }

        public Conteudo Conteudo { get; set; }
    }
}
