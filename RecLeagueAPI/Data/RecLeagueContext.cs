using Microsoft.EntityFrameworkCore;
using RecLeagueAPI.Models.QueryResults;

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
        public DbSet<GameStatus> GameStatus { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<GameResult> GameResults { get; set; }
        public DbSet<PendingCaptainReport> PendingCaptainReports { get; set; }
        public DbSet<PlayerGameStatus> PlayerGameStatus { get; set; }
        public DbSet<TeamPlayerStatus> TeamPlayerStatuses { get; set; }
        public DbSet<SelectRegionName> RegionNames { get; set; }
        public DbSet<JoinTeamString> JoinTeamStrings { get; set; }
        public DbSet<JoinTeamByte> JoinTeamBytes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=COLIN;Initial Catalog=RecLeagueV2;Integrated Security=True;ConnectRetryCount=0");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //unique index of mulitple columns
            modelBuilder.Entity<League>()
                .HasIndex(p => new { p.SportId, p.RegionId, p.Tier })
                .IsUnique(true);

            //unique index for TeamName
            modelBuilder.Entity<Team>()
                .HasIndex(t => t.TeamName)
                .IsUnique(true);

            //Avoid table creation on migration for query result entities //this is currently not working properly and you must delete every migration
            modelBuilder.Entity<Schedule>().HasNoKey().ToView(null);
            modelBuilder.Entity<GameResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<PendingCaptainReport>().HasNoKey().ToView(null);
            modelBuilder.Entity<PlayerGameStatus>().HasNoKey().ToView(null);
            modelBuilder.Entity<TeamPlayerStatus>().HasNoKey().ToView(null);
            modelBuilder.Entity<SelectRegionName>().HasNoKey().ToView(null);
            modelBuilder.Entity<JoinTeamString>().HasNoKey().ToView(null);
            modelBuilder.Entity<JoinTeamByte>().HasNoKey().ToView(null);

        }
    }
}
