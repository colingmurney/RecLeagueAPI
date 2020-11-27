using System.ComponentModel.DataAnnotations;


namespace RecLeagueAPI.Models
{
    public class CreateTeam
    {
        [Required]
        public string TeamName { get; set; }

        [Required]
        public string RegionName { get; set; }

        [Required]
        public string SportName { get; set; }

        [Required]
        public byte Tier { get; set; }
    }
}
