using AgricolaApi.Domain.Models;
using System.Linq;
using AgricolaApi.Infrastructure.Context;
using AgricolaApi.Application.Interfaces;
using AgricolaApi.Application;
using Microsoft.EntityFrameworkCore;
using AgricolaApi.Domain;

namespace AgricolaApi.Applications
{
    public class UserServices : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IJwtProvider _jwtProvider;

        public UserServices(AppDbContext context, IJwtProvider jwtProvider)
        {
            _context = context;
            _jwtProvider = jwtProvider;
        }


        public IEnumerable<User> GetAllUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }

        public User? GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public string Create(UserDto user)
        {
            // encrypt password to store a hash in the db instead of the raw password
            string password = user.Password;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var userEntity = new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = passwordHash,
            };
            _context.Users.Add(userEntity);
            _context.SaveChanges();

            // if creation was successful, generate token
            return _jwtProvider.Generate(userEntity);
        }

        public async Task<Result> RemoveAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync((user) => user.Id == id);
            if (user is null)
            {
                return Result.Failure(UserErrors.NotUserFound);
            }
            _context.Remove(user);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(int userId, UpdateUserDto user)
        {
            var userDB = await _context.Users.FirstOrDefaultAsync((user) => user.Id == userId);
            if (userDB is null)
            {
                return Result.Failure(UserErrors.NotUserFound);
            }

            if (user.Email != default && user.Email != userDB.Email)
            {
                userDB.Email = user.Email;
            }

            if (user.Name != default && user.Name != userDB.Name)
            {
                userDB.Name = user.Name;
            }

            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<bool> DoesUserExistByEmail(UserDto user)
        {
            return await _context.Users.AnyAsync(x => x.Email == user.Email);
        }
    }
}

public static class UserErrors
{
    public static readonly Error NotUserFound = new("User Not Found");
}

//obtener usuario por id, si no deviuelve retorno un notfound, si si compaar propiedad x propiedad, ejemplo dsi el email de dto no es null y es iferente al de la bd entoncesa ctualizar el de la bd, 