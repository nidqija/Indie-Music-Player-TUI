using Microsoft.EntityFrameworkCore;
using Models;
using DotNetEnv;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Song> Songs { get; set; }
        public DbSet<Collection> Collections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // optional — EF Core can infer this automatically
            modelBuilder.Entity<Song>()
                .HasMany(s => s.Collections)
                .WithMany(c => c.Songs)
                .UsingEntity(j => j.ToTable("SongCollections")); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Env.Load();
            string password = Env.GetString("db_password");
            optionsBuilder.UseNpgsql($"Host=localhost;Port=5432;Database=MusicConsole;Username=postgres;Password={password}");
        }
    }
}
