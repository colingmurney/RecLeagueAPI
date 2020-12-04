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
        private readonly string _tokenKey;

        public PlayersController(RecLeagueContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _tokenKey = config.GetValue<string>("TokenKey");
        }

        [HttpPut("{status}")]
        public async Task<IActionResult> ChangePlayerGameStatus(string status)
        {
            // update player game status

            // check if status passed is valid and create corresponding statusId
            status = status.ToLower();
            int statusId;
            if (status == "unknown")
                statusId = 1;
            else if (status == "absent")
                statusId = 2;
            else if (status == "attending")
                statusId = 3;
            else
                return BadRequest("Incorrect status.");

            // check if access token was passed
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (accessToken == null) return Unauthorized();

            // get key from appsettings.json
            //var tokenKey = _config.GetValue<string>("TokenKey");

            // check if claim == player in the database
            var claimEmail = JwtAuthentication.GetClaim(accessToken, JwtAuthentication.claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);
            if (user == null) return Unauthorized("Player was not found.");

            // check for wrong token key or expired token
            if (!JwtAuthentication.ValidateCurrentToken(accessToken, _tokenKey)) return Unauthorized("Incorrect access token.");
            
            // update player status
            user.GameStatusId = statusId;
            await _context.SaveChangesAsync();

            // create new jwt and attach to response
            var tokenString = JwtAuthentication.CreateJWT(_tokenKey, user.Email);
            Response.Cookies.Append("X-Access-Token", tokenString, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok();
        }
    }
}
