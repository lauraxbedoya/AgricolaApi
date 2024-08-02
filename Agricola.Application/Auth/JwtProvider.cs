using System.Security.Claims;
using System.Text;
using AgricolaApi.Domain.Models;
using AgricolaApi.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace AgricolaApi.Application;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string Generate(User user)
    {
        // generate claims for user Id and email
        var claims = new Claim[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };
        //sign token credentials 
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SecretKey)
            ),
            SecurityAlgorithms.HmacSha256
        );
        //create jwt token object
        var token = new JwtSecurityToken
        (
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddDays(1),
            signingCredentials
        );

        // generate token hash with previous values
        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}
