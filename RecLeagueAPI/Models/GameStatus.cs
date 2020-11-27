using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecLeagueAPI.Models
{
    [Table("GameStatus")]
    public partial class GameStatus
    {
        [Key]
        public int GameStatusId { get; set; }
        [Required]
        [StringLength(50)]
        public string GameStatusName { get; set; }
    }
}
