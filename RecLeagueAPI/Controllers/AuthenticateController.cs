using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecLeagueAPI.Helpers;
using RecLeagueAPI.Models;
using RecLeagueAPI.Models.QueryResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RecLeagueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticateController : ControllerBase
    {
        private readonly RecLeagueContext _context;
        private readonly IConfiguration _config;
        private readonly string _tokenKey;

        public AuthenticateController(RecLeagueContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _tokenKey = config.GetValue<string>("TokenKey");
        }

        [HttpGet]
        public async Task<ActionResult> Authenticate()
        {
            // authenticate jwt from cookie on page load
            // if successful returns all user data formatted for frontend use and a new cookie

            // check if access token was passed
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (accessToken == null) return Unauthorized("No access token received.");

            // get key from appsettings.json
            //var tokenKey = _config.GetValue<string>("TokenKey");

            // check if claim == player in the database
            var claimEmail = JwtAuthentication.GetClaim(accessToken, JwtAuthentication.claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);
            if (user == null) return Unauthorized("Player was not found.");

            // check for wrong token key or expired token
            if (!JwtAuthentication.ValidateCurrentToken(accessToken, _tokenKey))
            {
                //if not valid, check if player has staySignedIn checked in the Player table
                if (user.StaySignedIn == false)
                    return Unauthorized("Invalid access token.");
            }

            //create new jwt and attach to response
            var tokenString = JwtAuthentication.CreateJWT(_tokenKey, user.Email);
            Response.Cookies.Append("X-Access-Token", tokenString, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            //create response data object and set attributes
            var queryResults = new QueryResult();
            queryResults.Schedule = _context.Schedules.FromSqlRaw("EXECUTE dbo.SelectScheduledGames {0}", user.PlayerId).ToList();
            queryResults.Results = _context.GameResults.FromSqlRaw("EXECUTE dbo.SelectGameResults {0}", user.PlayerId).ToList();
            queryResults.HomeTeamPlayerStatuses = _context.TeamPlayerStatuses.FromSqlRaw("EXECUTE dbo.HomeTeamPlayerStatuses {0}", user.PlayerId).ToList();
            queryResults.AwayTeamPlayerStatuses = _context.TeamPlayerStatuses.FromSqlRaw("EXECUTE dbo.AwayTeamPlayerStatuses {0}", user.PlayerId).ToList();
            queryResults.PlayerGameStatus = _context.PlayerGameStatus.FromSqlRaw("EXECUTE dbo.PlayerGameStatus {0}", user.PlayerId).ToList()[0]; // explicitly take first object in list since it only returns 1 item
            if (user.IsCaptain)
            {
                queryResults.PendingCaptainReports = _context.PendingCaptainReports.FromSqlRaw("EXECUTE dbo.PendingCaptainReports {0}", user.PlayerId).ToList();
            }

            //Explicitly turn list of objects into list of strings. Unable to create DbSet of string type in DbContext
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
            // check player credentials and return jwt if successful

            // emails are stored in lowecase and case insensitive
            loginCredentials.Email = loginCredentials.Email.ToLower();
            
            // validate credentials before autheniticating player ***** REPLACE WITH REAL VALIDATION
            if (string.IsNullOrEmpty(loginCredentials.Email) || string.IsNullOrEmpty(loginCredentials.Password))
                return BadRequest("Email or password is empty.");

            // check if player with provided email exists
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == loginCredentials.Email);
            if (user == null) return BadRequest("Incorrect username/password.");


            // hash provided password with salt
            byte[] salt = Convert.FromBase64String(user.Salt);
            var hashed = HashPassword(salt, loginCredentials.Password);

            // check if password providec is correct
            if (hashed != user.Password) return BadRequest("Incorrect username/password.");

            // update Player isSignedIn attribute if box checked on login
            if (loginCredentials.StaySignedIn)
            {
                user.StaySignedIn = true;
                _context.SaveChanges();
            }
            
            // create jwt and attach to response
            //var tokenKey = _config.GetValue<string>("TokenKey");
            var tokenString = JwtAuthentication.CreateJWT(_tokenKey, user.Email);
            Response.Cookies.Append("X-Access-Token", tokenString, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok("Login successful.");

        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult> Create(CreatePlayer player)
        {
            // check player provided crendentials and create a new player
            //if successful, return jwt


            // emails are stored in lowecase and case insensitive
            player.Email = player.Email.ToLower();

            // validate credentials before autheniticating player ***** REPLACE WITH REAL VALIDATION
            if (string.IsNullOrEmpty(player.Email) || string.IsNullOrEmpty(player.Password) || string.IsNullOrEmpty(player.FirstName) || string.IsNullOrEmpty(player.LastName))
                return BadRequest();

            // check if the passwords provided match
            if (player.Password != player.ConfirmPassword) return BadRequest("Passwords do not match.");
            
            // check if a player with the provided email already exists
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == player.Email);
            if (user != null) return BadRequest("Player with email already exists.");

            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create()) { rng.GetBytes(salt); }

            // hash password and add new player to DB
            var hashed = HashPassword(salt, player.Password);
            Player newPlayer = new Player(player.FirstName, player.LastName, player.Email, hashed, player.StaySignedIn, Convert.ToBase64String(salt));                
            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();

            // create jwt and attach to response
            //var tokenKey = _config.GetValue<string>("TokenKey");
            var tokenString = JwtAuthentication.CreateJWT(_tokenKey, newPlayer.Email);
            Response.Cookies.Append("X-Access-Token", tokenString, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok("New player successfully created.");
        }

        [HttpPut("logout")]
        public async Task<ActionResult> Logout()
        {
            // logout user by removing jwt in their browser

            //check if jwt claim == to a Player in the DB
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var claimEmail = JwtAuthentication.GetClaim(accessToken, JwtAuthentication.claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);
            if (user == null) return Unauthorized("Could not logout player.");
            if (!JwtAuthentication.ValidateCurrentToken(accessToken, _tokenKey)) return Unauthorized("Invalid access token.");

            // reset Player staySignedIn attribute in DB and delete cookie
            user.StaySignedIn = false;
            _context.SaveChanges();
            Response.Cookies.Delete("X-Access-Token");

            return Ok("Player successfully logged out.");
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
