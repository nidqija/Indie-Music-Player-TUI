using Microsoft.EntityFrameworkCore;
using Models;
using DotNetEnv;
using Microsoft.Extensions.Options;


namespace Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Song> Songs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Env.Load();
            string password = Env.GetString("db_password");
            optionsBuilder.UseNpgsql($"Host=localhost;Port=5432;Database=MusicConsole;Username=postgres;Password={password}");


        }
    }
}