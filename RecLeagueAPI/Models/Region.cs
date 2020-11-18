using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RecLeagueAPI.Models
{
    [Table("Region")]
    public partial class Region
    {
        [Key]
        public int RegionId { get; set; }
        [Required]
        
        [StringLength(50)]
        public string RegionName { get; set; }
        [Required]
        
        [StringLength(2)]
        public string Province { get; set; }  
    }
}
