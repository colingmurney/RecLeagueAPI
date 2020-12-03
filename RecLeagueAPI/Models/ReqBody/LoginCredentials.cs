using System.ComponentModel.DataAnnotations;


namespace RecLeagueAPI.Models
{
    public class LoginCredentials
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool StaySignedIn { get; set; }
    }
}
