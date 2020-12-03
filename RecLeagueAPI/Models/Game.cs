using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RecLeagueAPI.Models
{
    [Table("Game")]
    public partial class Game
    {
        [Key]
        public int GameId { get; set; }
        public DateTime Date { get; set; }
        
        [ForeignKey("HomeTeam")]
        public int HomeTeamId { get; set; }
        
        [ForeignKey("AwayTeam")]
        public int AwayTeamId { get; set; }
       
        [ForeignKey("Venue")]
        public int VenueId { get; set; }

        public int? HomeTeamHomeScore { get; set; }

        public int? HomeTeamAwayScore { get; set; }

        public int? AwayTeamHomeScore { get; set; }
        public int? AwayTeamAwayScore { get; set; }
        [Required]
        public bool IsArchived { get; set; }

        public Venue Venue { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
    }
}
