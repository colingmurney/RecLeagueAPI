using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecLeagueAPI.Models.QueryResults
{
    [Keyless]
    public class Schedule
    {
        public DateTime Date { get; set; }
        public string Home { get; set; }
        public string Away { get; set; }
        public string VenueName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
