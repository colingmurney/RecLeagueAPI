using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecLeagueAPI.Models
{
    [Keyless]
    public class JoinTeamByte
    {
        public byte Result { get; set; }
    }
}
