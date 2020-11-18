using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RecLeagueAPI.Models;
using RecLeagueAPI.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

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
            //if not valid, check if user has stay signed in checked in the Player table
            var tokenKey = _config.GetValue<string>("TokenKey");
            var claimType = "email";
            var jwtAuth = new JwtAuthentication();

            //check if claim == to a player in the database
            var claimEmail = jwtAuth.GetClaim(accessToken, claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);
            if (user == null)
                return Unauthorized("user was null");

            if (!jwtAuth.ValidateCurrentToken(accessToken, tokenKey))
            {
                if (user.StaySignedIn == false)
                    return Unauthorized("Didnt validate");
            }

            var tokenString = jwtAuth.createJWT(tokenKey, user.Email);
            Response.Cookies.Append("X-Access-Token", tokenString, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });


            return Ok(user);

            //if not, send unauthorized response
            //outside if statements make new token and send with ok

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginCredentials loginCredentials)
        {

            if (string.IsNullOrEmpty(loginCredentials.Email) || string.IsNullOrEmpty(loginCredentials.Password))
                return BadRequest();

            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == loginCredentials.Email);

            // check if username exists
            if (user == null)
                return BadRequest();

            // check if password is correct
            if (user.Password != loginCredentials.Password)
                return BadRequest();

            if (loginCredentials.StaySignedIn)
            {
                user.StaySignedIn = true;
                _context.SaveChanges();
            }
            
            var tokenKey = _config.GetValue<string>("TokenKey");
            var jwtAuth = new JwtAuthentication();
            var tokenString = jwtAuth.createJWT(tokenKey, user.Email);

            Response.Cookies.Append("X-Access-Token", tokenString, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok(user);

        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult> Create(CreatePlayer player)
        {
            if (string.IsNullOrEmpty(player.Email) || string.IsNullOrEmpty(player.Password) || string.IsNullOrEmpty(player.FirstName) || string.IsNullOrEmpty(player.LastName))
                return BadRequest();

            //check if the passwords match
            if (player.Password != player.ConfirmPassword)
                return BadRequest("Passwords do not match.");
            
            //check if user with this email already exists
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == player.Email);
            if (user != null)
                return BadRequest("Player with email already exists.");

            //create new Player instance to add to database
            Player newPlayer = new Player(player.FirstName, player.LastName, player.Email, player.Password, player.StaySignedIn);                

            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();

            var tokenKey = _config.GetValue<string>("TokenKey");
            var jwtAuth = new JwtAuthentication();
            var tokenString = jwtAuth.createJWT(tokenKey, newPlayer.Email);

            Response.Cookies.Append("X-Access-Token", tokenString, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok(player);
        }

        [HttpPut("logout")]
        public async Task<ActionResult> Logout()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var claimType = "email";
            var jwtAuth = new JwtAuthentication();

            //check if claim == to a player in the database
            var claimEmail = jwtAuth.GetClaim(accessToken, claimType);
            Player user = await _context.Players.SingleOrDefaultAsync(x => x.Email == claimEmail);

           if (user != null)
            {
                user.StaySignedIn = false;
                _context.SaveChanges();
            }
                    
            Response.Cookies.Delete("X-Access-Token");
            return Ok();
        }
    }
}
