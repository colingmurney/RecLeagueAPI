using System;
using System.Collections.Generic;
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
        private readonly string _tokenKey;

        public JoinTeamController(RecLeagueContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _tokenKey = config.GetValue<string>("TokenKey");
        }

        [HttpGet("sports")]
        public async Task<ActionResult> SelectAvailableSports([FromQuery(Name = "region")] string region)
        {
            // create list of all available sports in req region
            List<JoinTeamString> sports = await _context.JoinTeamStrings.FromSqlRaw("EXECUTE dbo.SelectAvailableSports {0}", region).ToListAsync();
            var sportList = new List<string>();

            foreach (JoinTeamString sport in sports)
            {
                sportList.Add(sport.Result);
            }

            return Ok(sportList);
        }

        [HttpGet("tiers")]
        public async Task<ActionResult> SelectAvailableTiers([FromQuery(Name = "region")] string region, [FromQuery(Name = "sport")] string sport)
        {
            // create list of all available tiers for region & sport in request
            List<JoinTeamByte> tiers = await _context.JoinTeamBytes.FromSqlRaw($"EXECUTE dbo.SelectAvailableTiers @RegionName={region}, @SportName={sport}").ToListAsync();
            var tierList = new List<byte>();

            foreach (JoinTeamByte tier in tiers)
            {
                tierList.Add(tier.Result);
            }

            return Ok(tierList);
        }

        [HttpGet("teams")]
        public async Task<ActionResult> SelectAvailableTeams([FromQuery(Name = "region")] string region, [FromQuery(Name = "sport")] string sport, [FromQuery(Name = "tier")] byte tier)
        {
            // create list of all available teams for region, sport & tier in request
            List<JoinTeamString> teams = await _context.JoinTeamStrings.FromSqlRaw($"EXECUTE dbo.SelectAvailableTeams @RegionName={region}, @SportName={sport}, @Tier={tier}").ToListAsync();
            var teamList = new List<string>();

            foreach (JoinTeamString team in teams)
            {
                teamList.Add(team.Result);
            }

            return Ok(teamList);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateTeam([FromQuery(Name = "teamname")] string teamname)
        {
            // update player's team in DB

            // check if access token was passed
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (accessToken == null) return Unauthorized("No access token received.");
            
            // check if claim == to a player in the database
            var claimEmail = JwtAuthentication.GetClaim(accessToken, JwtAuthentication.claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);
            if (user == null) return Unauthorized ("Player was not found.");
            if (!JwtAuthentication.ValidateCurrentToken(accessToken, _tokenKey)) return Unauthorized("Invalid access token.");

            // update user teamId where TeamName = the team passed
            Team team = await _context.Teams.SingleOrDefaultAsync(x => x.TeamName == teamname);
            if (team == null) return Unauthorized("Team was not found.");
            user.TeamId = team.TeamId;
            await _context.SaveChangesAsync();

            return Ok("Team updated");
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create(CreateTeam team)
        {
            // create new team with req body params

            // check if access token was passed
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (accessToken == null) return Unauthorized("No access token received.");

            // check if claim == to a player in the database
            var claimEmail = JwtAuthentication.GetClaim(accessToken, JwtAuthentication.claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);
            if (user == null) return Unauthorized("Player was not found.");
            if (!JwtAuthentication.ValidateCurrentToken(accessToken, _tokenKey)) return Unauthorized("Invalid access token.");

            //create SqlParamters to pass to sql raw stored procedure
            var TeamName = new SqlParameter("@TeamName", team.TeamName);
            var RegionName = new SqlParameter("@RegionName", team.RegionName);
            var SportName = new SqlParameter("@SportName", team.SportName);
            var Tier = new SqlParameter("@Tier", team.Tier);

            //FromSQLRaw returns List of objs - declare new list of Team
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


