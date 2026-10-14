using Microsoft.EntityFrameworkCore;
using SmartBusinessProApp.Models;
using System.Configuration;

namespace SmartBusinessProApp.Data
{
    public class SmartBusinessContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SmartBusinessConnStr"]?.ConnectionString
                    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'SmartBusinessConnStr' en App.config.");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeo exacto según el script SQL de la clase
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("Categorias");
                entity.HasKey(c => c.CategoriaId);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Productos");
                entity.HasKey(p => p.ProductoId);
                entity.Property(p => p.Precio).HasColumnType("decimal(12, 2)");

                entity.HasOne(p => p.Categoria)
                    .WithMany(c => c.Productos)
                    .HasForeignKey(p => p.CategoriaId)
                    .HasConstraintName("FK_Prod_Cat");
            });
        }
    }
}