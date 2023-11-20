using GraphQL.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQL
{
    public class DBContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Editor> Editors { get; set; }
        public DbSet<Studio> Studios { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasMany(g => g.Editors).WithMany(e => e.Games);
            modelBuilder.Entity<Game>().HasMany(g => g.Studios).WithMany(s => s.Games);
            modelBuilder.Entity<Game>().Property(g => g.Platforms).HasConversion(
                p => string.Join(";", p),
                p => p.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList()) ;
            modelBuilder.Entity<Game>().Property(g => g.Genres).HasConversion(
                g => string.Join(";", g),
                g => g.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }
}
