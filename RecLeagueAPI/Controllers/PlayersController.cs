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

            var claimEmail = JwtAuthentication.GetClaim(accessToken, claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);
            if (user == null)
                return Unauthorized("user was null");

            if (!JwtAuthentication.ValidateCurrentToken(accessToken, tokenKey))
            {
                return Unauthorized("Didnt validate");
            }

            user.GameStatusId = statusId;

            await _context.SaveChangesAsync();

            //_context.Players.FromSqlRaw($"EXECUTE dbo.UpdatePlayerGameStatus {user.PlayerId}, {status}");

            var tokenString = JwtAuthentication.CreateJWT(tokenKey, user.Email);
            Response.Cookies.Append("X-Access-Token", tokenString, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok();
        }
    }
}
