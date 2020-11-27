using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecLeagueAPI.Models.QueryResults
{
    [Keyless]
    public class SelectRegionName
    {
        public string RegionName { get; set; }
    }
}
