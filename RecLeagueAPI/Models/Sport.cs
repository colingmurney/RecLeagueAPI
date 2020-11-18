using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RecLeagueAPI.Models
{
    [Table("Sport")]
    public partial class Sport
    {
        [Key]
        public int SportId { get; set; }
        [Required]     
        [StringLength(50)]
        public string SportName { get; set; }

    }
}
