using Microsoft.EntityFrameworkCore;//PAQUETE PARA CONECTAR A LA BASE DE DATOS
using EntitiesLibrary.Entities.Enum;//SE TRAE LA ENTIDAD USER
using EntitiesLibrary.Entities;//SE TRAE EL ENUM USERSTATUS PARA RECORRERLOS COMO SI FUERA UN BUCLE

namespace ApiTalking.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        //Para recorrer el enum UserStatus
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.UserStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => (UserStatus)Enum.Parse(typeof(UserStatus), v));
        }
    }
}
