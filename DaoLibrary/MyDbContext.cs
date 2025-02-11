using Microsoft.EntityFrameworkCore;//PAQUETE PARA CONECTAR A LA BASE DE DATOS
using EntitiesLibrary.Comment;
using EntitiesLibrary.File;
using EntitiesLibrary.Post;
using EntitiesLibrary.Reaction;
using EntitiesLibrary.User;
using EntitiesLibrary.Course;

namespace DaoLibrary;
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<EntitiesLibrary.File.File> Files { get; set; }
        public DbSet<FileType> FilesTypes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Course> Course { get; set; }

      
    }
