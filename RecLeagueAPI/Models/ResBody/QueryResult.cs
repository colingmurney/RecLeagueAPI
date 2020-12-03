using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecLeagueAPI.Models.QueryResults;

namespace RecLeagueAPI.Models
{
    public class QueryResult
    {
        public List<Schedule> Schedule { get; set; }
        public List<GameResult> Results { get; set; }
        public List<PendingCaptainReport> PendingCaptainReports { get; set; }
        public List<TeamPlayerStatus> HomeTeamPlayerStatuses { get; set; }
        public List<TeamPlayerStatus> AwayTeamPlayerStatuses { get; set; }
        public PlayerGameStatus PlayerGameStatus { get; set; }
        public List<string> RegionNames { get; set; }

    }
}
