using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace RecLeagueAPI.Models
{
    public partial class RecLeagueContext : DbContext
    {
        public RecLeagueContext(DbContextOptions<RecLeagueContext> options)
            :base(options)
        {}
        public DbSet<Game> Games { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Venue> Venues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=COLIN;Initial Catalog=RecLeagueV2;Integrated Security=True;ConnectRetryCount=0");
        }
   
    }
}
