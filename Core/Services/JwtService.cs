using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace OOP_Lab_1.Core.Interfaces;

public class JwtService
{
    private readonly string _secretKey = "secret-key";
    
    public string GenerateJwtToken(string email, int role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new System.Security.Claims.Claim("email", email),
            new System.Security.Claims.Claim("role", role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "your-issuer",  // You can put your app name or domain here
            audience: "your-audience",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public bool ValidateJwtToken(string token, out string email, out int role)
    {
        email = null;
        role = 0;

        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,  
                ValidIssuer = "your-issuer", 
                ValidAudience = "your-audience",  
                IssuerSigningKey = securityKey
            }, out var validatedToken);
            
            var jwtToken = validatedToken as JwtSecurityToken;

            if (jwtToken == null)
            {
                return false;
            }

            // Extract email and role from the token claims
            var emailClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var roleClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            if (emailClaim != null && roleClaim != null)
            {
                email = emailClaim;
                role = int.Parse(roleClaim);  
                return true;
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
}
