using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RecLeagueAPI.Models
{
    [Table("Player")]
    public partial class Player
    {
        public Player(string firstName, string lastName, string email, string password, bool staySignedIn)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            StaySignedIn = staySignedIn;
            GameStatusId = 1; //Manually set new player to unknown game status
        }

        [Key]
        public int PlayerId { get; set; }
        [Required]
        
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        
        [StringLength(255)]
        public string Password { get; set; }
        
        [ForeignKey("Team")]
        public int? TeamId { get; set; }

        public bool IsCaptain { get; set; }
        [ForeignKey("GameStatus")]
        public int GameStatusId { get; set; }
        public bool StaySignedIn { get; set; }
        public Team Team { get; set; }
        public GameStatus GameStatus { get; set; }
    }
}
