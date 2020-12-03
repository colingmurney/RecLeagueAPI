using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecLeagueAPI.Models.QueryResults
{
    [Keyless]
    public class GameResult
    {
        public DateTime Date { get; set; }
        public string Home { get; set; }
        public int HomeScore { get; set; }
        public string Away { get; set; }
        public int AwayScore { get; set; }
    }
}
