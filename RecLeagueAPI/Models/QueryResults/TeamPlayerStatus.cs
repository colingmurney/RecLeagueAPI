using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecLeagueAPI.Models.QueryResults
{
    [Keyless]
    public class TeamPlayerStatus
    {
        public int PlayerId { get; set; }
        public DateTime Date { get; set; }
        public string TeamName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GameStatusName { get; set; }
    }
}
