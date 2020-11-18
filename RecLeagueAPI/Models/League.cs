using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RecLeagueAPI.Models
{
    [Table("League")]
    public partial class League
    {
        [Key]
        public int LeagueId { get; set; }
        
        [ForeignKey("Sport")]
        public int SportId { get; set; }
        
        [ForeignKey("Region")]
        public int RegionId { get; set; }
       
        public byte Tier { get; set; }
        
        [StringLength(50)]
        public string Nickname { get; set; }

        public Sport Sport { get; set; }
        public Region Region { get; set; }
    }
}
