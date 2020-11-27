using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecLeagueAPI.Helpers;
using RecLeagueAPI.Models;

namespace RecLeagueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly RecLeagueContext _context;
        private readonly IConfiguration _config;

        public PlayersController(RecLeagueContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{status}")]
        public async Task<IActionResult> ChangePlayerGameStatus(string status)
        {
            //Check if status passed is valid
            status = status.ToLower();
            int statusId;
            if (status == "unknown")
                statusId = 1;
            else if (status == "absent")
                statusId = 2;
            else if (status == "attending")
                statusId = 3;
            else
                return BadRequest("Incorrect status");

            //Check access token passed through cookie
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (accessToken == null)
                return Unauthorized();

            var tokenKey = _config.GetValue<string>("TokenKey");
            var claimType = "email";
            var jwtAuth = new JwtAuthentication();

            var claimEmail = jwtAuth.GetClaim(accessToken, claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);
            if (user == null)
                return Unauthorized("user was null");

            if (!jwtAuth.ValidateCurrentToken(accessToken, tokenKey))
            {
                return Unauthorized("Didnt validate");
            }

            user.GameStatusId = statusId;

            await _context.SaveChangesAsync();

            //_context.Players.FromSqlRaw($"EXECUTE dbo.UpdatePlayerGameStatus {user.PlayerId}, {status}");

            var tokenString = jwtAuth.createJWT(tokenKey, user.Email);
            Response.Cookies.Append("X-Access-Token", tokenString, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok();
        }

        // POST: api/Players
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { id = player.PlayerId }, player);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Player>> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return player;
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}
