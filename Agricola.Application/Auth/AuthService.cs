using AgricolaApi.Application.Interfaces;
using AgricolaApi.Domain;
using AgricolaApi.Domain.Models;
using AgricolaApi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AgricolaApi.Application;

public class AuthService : IAuthServices
{
    private readonly AppDbContext _context;
    private readonly IJwtProvider _jwtProvider;

    public AuthService(AppDbContext context, IJwtProvider jwtProvider)
    {
        _context = context;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> AuthenticateAsync(string Email, string Password)
    {
        //try to find user by email
        User? userDB = await _context.Users.SingleOrDefaultAsync(user => user.Email == Email);
        if (userDB is null)
        {
            throw new InvalidCredentialException("Invalid Credentials");
        }
        //if user was found, verify password is correct
        bool verified = BCrypt.Net.BCrypt.Verify(Password, userDB.Password);
        if (!verified)
        {
            throw new InvalidCredentialException("Invalid Credentials");
        }

        //if email and password is correct, generate token
        string token = _jwtProvider.Generate(userDB);
        return token;
    }
}
