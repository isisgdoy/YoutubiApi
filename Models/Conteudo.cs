namespace YoutubiApi.Models
{
    public class Conteudo
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Tipo { get; set; }

        public int IdCriador { get; set; }
        public Criador Criador { get; set; }
        public List<ItemPlaylist> ItemPlaylist { get; set; }
    }
}
