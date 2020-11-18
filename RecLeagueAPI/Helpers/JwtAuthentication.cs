using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecLeagueAPI.Helpers
{
    public class JwtAuthentication
    {
		public bool ValidateCurrentToken(string token, string secret)
		{
			
			try
			{
				//var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
				var key = Encoding.ASCII.GetBytes(secret);

				var tokenHandler = new JwtSecurityTokenHandler();
				SecurityToken validatedToken = null;

				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
                    ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					// set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
					ClockSkew = TimeSpan.Zero
				}, out validatedToken) ;
			}
			catch (SecurityTokenException)
			{
				return false;
			}
			catch (Exception e)
			{
                Console.WriteLine(e.ToString()); ; //something else happened
				throw;
			}
			return true;
		}

		public string GetClaim(string token, string claimType)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

			var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
			return stringClaimValue;
		}

		public string createJWT(string tokenKey, string email)
        {
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(tokenKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Email, email)
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);

			//if (token == null)
			//	return Unauthorized();

			return tokenHandler.WriteToken(token);
		}

	}
}
