using Microsoft.EntityFrameworkCore;//PAQUETE PARA CONECTAR A LA BASE DE DATOS
using EntitiesLibrary.Entities;//TRAE LAS ENTIDADES
using EntitiesLibrary.Entities.Enum;//SE TRAE EL ENUM USERSTATUS PARA RECORRERLOS COMO SI FUERA UN BUCLE

namespace ApiTalking.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PublishedFile> Files { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }

        // //Para recorrer el enum UserStatus
        //  protected override void OnModelCreating(ModelBuilder modelBuilder)
        //  {
        //      modelBuilder.Entity<User>().Property(u => u.UserStatus)
        //          .HasConversion(
        //              v => v.ToString(),
        //              v => (UserStatus)Enum.Parse(typeof(UserStatus), v));
        //      modelBuilder.Entity<Post>().Property(p => p.PostStatus)
        //          .HasConversion(
        //              v => v.ToString(),
        //              v => (PostStatus)Enum.Parse(typeof(PostStatus), v));
        //      modelBuilder.Entity<Comment>().Property(c => c.CommentStatus)
        //          .HasConversion(
        //              v => v.ToString(),
        //              v => (CommentStatus)Enum.Parse(typeof(CommentStatus), v));
        //      modelBuilder.Entity<Reaction>().Property(r => r.ReactionStatus)
        //          .HasConversion(
        //              v => v.ToString(),
        //              v => (ReactionStatus)Enum.Parse(typeof(ReactionStatus), v));
        //  }
    }
}
