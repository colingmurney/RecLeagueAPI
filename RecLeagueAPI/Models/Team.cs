using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RecLeagueAPI.Models
{
    [Table("Team")]
    public partial class Team
    {
        [Key]
        public int TeamId { get; set; }
        [Required]
        
        [StringLength(50)]
        public string TeamName { get; set; }
        [ForeignKey("League")]
        public int LeagueId { get; set; }

        public League League { get; set; }
    }
}
