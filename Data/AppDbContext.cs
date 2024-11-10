// Aqui onde ocorre a comunicação entre o banco e restante do código, atráves desse contexto
// Para isso é preciso baixar alguns pacotes para que essa comunicação ocorra > Microsoft.EntityFrameworkCore | SqlServer | Designer | Tools

using Microsoft.EntityFrameworkCore;
using YoutubiApi.Models;

namespace YoutubiApi.Data
{
    public class AppDbContext : DbContext // DbContext vem do EntityFrameworkCore
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        // Cada propriedade será criada uma tabela 
        // O Entity Framework utiliza esses DbSets para executar consultas e operações no banco de dados
        public DbSet<Criador> Criadores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ItemPlaylist> ItemsPlaylists { get; set; }
        public DbSet<Conteudo> Conteudos { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Configuração da chave composta para ItemPlaylist (tabela associativa)
        modelBuilder.Entity<ItemPlaylist>()
            .HasKey(ip => new { ip.IdPlaylist, ip.IdConteudo });

        /* 
         * Configuração da relação entre Playlist e ItemPlaylist:
         * Uma playlist pode ter muitos itens (ItemPlaylists), e cada ItemPlaylist 
         * está associado a uma playlist específica (chave estrangeira PlaylistId).
         */
        modelBuilder.Entity<ItemPlaylist>()
            .HasOne(ip => ip.Playlist)
            .WithMany(p => p.ItemPlaylists)
            .HasForeignKey(ip => ip.IdPlaylist);

        /* 
         * Configuração da relação entre Conteudo e ItemPlaylist:
         * Um conteúdo pode estar associado a muitos itens (ItemPlaylists), e cada 
         * ItemPlaylist está associado a um conteúdo específico (chave estrangeira ConteudoId).
         */
        modelBuilder.Entity<ItemPlaylist>()
            .HasOne(ip => ip.Conteudo)
            .WithMany(c => c.ItemPlaylist)
            .HasForeignKey(ip => ip.IdConteudo);

    }
    }
}
