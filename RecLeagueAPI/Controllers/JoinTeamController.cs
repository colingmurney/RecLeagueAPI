using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecLeagueAPI.Helpers;
using RecLeagueAPI.Models;

namespace RecLeagueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JoinTeamController : Controller
    {
        private readonly RecLeagueContext _context;
        private readonly IConfiguration _config;

        public JoinTeamController(RecLeagueContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("sports/{region}")]
        public async Task<ActionResult> SelectAvailableSports(string region)
        {

            List<JoinTeamString> sports = await _context.JoinTeamStrings.FromSqlRaw("EXECUTE dbo.SelectAvailableSports {0}", region).ToListAsync();

            var sportList = new List<string>();

            foreach (JoinTeamString sport in sports)
            {
                sportList.Add(sport.Result);
            }
            return Ok(sportList);
        }

        [HttpGet("tiers/{region}/{sport}")]
        public async Task<ActionResult> SelectAvailableTiers(string region, string sport)
        {
            List<JoinTeamByte> tiers = await _context.JoinTeamBytes.FromSqlRaw($"EXECUTE dbo.SelectAvailableTiers @RegionName={region}, @SportName={sport}").ToListAsync();
            var tierList = new List<byte>();

            foreach (JoinTeamByte tier in tiers)
            {
                tierList.Add(tier.Result);
            }

            return Ok(tierList);
        }

        [HttpGet("teams/{region}/{sport}/{tier}")]
        public async Task<ActionResult> SelectAvailableTeams(string region, string sport, byte tier)
        {
            List<JoinTeamString> teams = await _context.JoinTeamStrings.FromSqlRaw($"EXECUTE dbo.SelectAvailableTeams @RegionName={region}, @SportName={sport}, @Tier={tier}").ToListAsync();
            var teamList = new List<string>();

            foreach (JoinTeamString team in teams)
            {
                teamList.Add(team.Result);
            }

            return Ok(teamList);
        }

        [HttpPut("update/{teamname}")]
        public async Task<ActionResult> UpdateTeam(string teamname)
        {
            //need to validate user and get player details. update their teamId where TeamName = the team passed
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (accessToken == null)
                return Unauthorized();

            var tokenKey = _config.GetValue<string>("TokenKey");
            var claimType = "email";
            var jwtAuth = new JwtAuthentication();

            //check if claim == to a player in the database
            var claimEmail = jwtAuth.GetClaim(accessToken, claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);
            if (user == null)
                return Unauthorized("user was null");

            //update user teamId where TeamName = the team passed
            Team team = await _context.Teams.SingleOrDefaultAsync(x => x.TeamName == teamname);
            if (team == null)
                return Unauthorized("team was null");
            user.TeamId = team.TeamId;

            await _context.SaveChangesAsync();
            return Ok("Team updated");
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create(CreateTeam team)
        {
            //need to validate user and get player details. update their teamId where TeamName = the team passed
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (accessToken == null)
                return Unauthorized();

            var tokenKey = _config.GetValue<string>("TokenKey");
            var claimType = "email";
            var jwtAuth = new JwtAuthentication();

            //check if claim == to a player in the database
            var claimEmail = jwtAuth.GetClaim(accessToken, claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);
            if (user == null)
                return Unauthorized("user was null");

            //The double quotes from the JSON in req wasn't working within SQL query when inputting
            //stored procedure args with formatted string
            var TeamName = new SqlParameter("@TeamName", team.TeamName);
            var RegionName = new SqlParameter("@RegionName", team.RegionName);
            var SportName = new SqlParameter("@SportName", team.SportName);
            var Tier = new SqlParameter("@Tier", team.Tier);

            List<Team> newTeam;
            try
            {
                newTeam = await _context.Teams.FromSqlRaw("EXECUTE dbo.CreateTeam @TeamName, @RegionName, @SportName, @Tier", parameters: new[] { TeamName, RegionName, SportName, Tier }).ToListAsync();
            
            }
            catch(Exception e)
            {
                return BadRequest("Team Name Already Exists or Invalid Name");
            }

            return Ok(newTeam);

        }
    }
}


