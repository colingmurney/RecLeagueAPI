using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RecLeagueAPI.Models
{
    [Table("Venue")]
    public partial class Venue
    {
        [Key]
        public int VenueId { get; set; }
        [Required]
        
        [StringLength(50)]
        public string VenueName { get; set; }
        [Required]
        
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [ForeignKey("Region")]
        public int RegionId { get; set; }

        public Region Region { get; set; }
    }
}
