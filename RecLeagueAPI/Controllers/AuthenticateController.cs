using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecLeagueAPI.Models;
using RecLeagueAPI.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using RecLeagueAPI.Models.QueryResults;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RecLeagueAPI.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticateController : ControllerBase
    {
        private readonly RecLeagueContext _context;
        private readonly IConfiguration _config;

        public AuthenticateController(RecLeagueContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult> Authenticate()
        {
            //Check access token passed through cookie
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (accessToken == null)
                return Unauthorized();
            
            var tokenKey = _config.GetValue<string>("TokenKey");
            var claimType = "email";
            

            //check if claim == to a player in the database
            var claimEmail = JwtAuthentication.GetClaim(accessToken, claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);
            if (user == null)
                return Unauthorized("user was null");

            if (!JwtAuthentication.ValidateCurrentToken(accessToken, tokenKey))
            {
                //if not valid, check if user has stay signed in checked in the Player table
                if (user.StaySignedIn == false)
                    return Unauthorized("Didnt validate");
            }

            var tokenString = JwtAuthentication.CreateJWT(tokenKey, user.Email);
            Response.Cookies.Append("X-Access-Token", tokenString, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            var queryResults = new QueryResult();
            
            queryResults.Schedule = _context.Schedules.FromSqlRaw("EXECUTE dbo.SelectScheduledGames {0}", user.PlayerId).ToList();
            queryResults.Results = _context.GameResults.FromSqlRaw("EXECUTE dbo.SelectGameResults {0}", user.PlayerId).ToList();
            queryResults.HomeTeamPlayerStatuses = _context.TeamPlayerStatuses.FromSqlRaw("EXECUTE dbo.HomeTeamPlayerStatuses {0}", user.PlayerId).ToList();
            queryResults.AwayTeamPlayerStatuses = _context.TeamPlayerStatuses.FromSqlRaw("EXECUTE dbo.AwayTeamPlayerStatuses {0}", user.PlayerId).ToList();
            queryResults.PlayerGameStatus = _context.PlayerGameStatus.FromSqlRaw("EXECUTE dbo.PlayerGameStatus {0}", user.PlayerId).ToList()[0]; // explicitly take first object in list since it only returns 1 item
            //queryResults.RegionNames = _context.RegionNames.FromSqlRaw("SELECT RegionName FROM Region").ToList();
            if (user.IsCaptain)
            {
                queryResults.PendingCaptainReports = _context.PendingCaptainReports.FromSqlRaw("EXECUTE dbo.PendingCaptainReports {0}", user.PlayerId).ToList();
            }

            List<SelectRegionName> regions = _context.RegionNames.FromSqlRaw("SELECT RegionName FROM Region").ToList();
            queryResults.RegionNames = new List<string>();
            foreach (SelectRegionName region in regions)
            {
                queryResults.RegionNames.Add(region.RegionName);
            }

            return Ok(queryResults);         
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginCredentials loginCredentials)
        {

            loginCredentials.Email = loginCredentials.Email.ToLower();
            ///validatation goes here

            if (string.IsNullOrEmpty(loginCredentials.Email) || string.IsNullOrEmpty(loginCredentials.Password))
                return BadRequest("email or password is empty");

            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == loginCredentials.Email);

            // check if username exists
            if (user == null)
                return BadRequest("user is null");

            //hash password with salt
           
            
            byte[] salt = Convert.FromBase64String(user.Salt);
            var check = Convert.ToBase64String(salt);
            var hashed = HashPassword(salt, loginCredentials.Password);

            // check if password is correct
            if (hashed != user.Password)
                return BadRequest();

            if (loginCredentials.StaySignedIn)
            {
                user.StaySignedIn = true;
                _context.SaveChanges();
            }
            
            var tokenKey = _config.GetValue<string>("TokenKey");
            var tokenString = JwtAuthentication.CreateJWT(tokenKey, user.Email);

            Response.Cookies.Append("X-Access-Token", tokenString, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok();

        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult> Create(CreatePlayer player)
        {
            player.Email = player.Email.ToLower();

            //validation goes here

            if (string.IsNullOrEmpty(player.Email) || string.IsNullOrEmpty(player.Password) || string.IsNullOrEmpty(player.FirstName) || string.IsNullOrEmpty(player.LastName))
                return BadRequest();

            //check if the passwords match
            if (player.Password != player.ConfirmPassword)
                return BadRequest("Passwords do not match.");
            
            //check if user with this email already exists
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == player.Email);
            if (user != null)
                return BadRequest("Player with email already exists.");

            
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            
            var hashed = HashPassword(salt, player.Password);

            //use the salt and returned hash to enter the database

            //create new Player instance to add to database
            Player newPlayer = new Player(player.FirstName, player.LastName, player.Email, hashed, player.StaySignedIn, Convert.ToBase64String(salt));                

            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();

            var tokenKey = _config.GetValue<string>("TokenKey");
            var tokenString = JwtAuthentication.CreateJWT(tokenKey, newPlayer.Email);

            Response.Cookies.Append("X-Access-Token", tokenString, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok(player);
        }

        [HttpPut("logout")]
        public async Task<ActionResult> Logout()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var claimType = "email";

            //check if claim == to a player in the database
            var claimEmail = JwtAuthentication.GetClaim(accessToken, claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);

           if (user != null)
            {
                user.StaySignedIn = false;
                _context.SaveChanges();
            }
                    
            Response.Cookies.Delete("X-Access-Token");
            return Ok();
        }

        public string HashPassword(byte[] salt, string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
