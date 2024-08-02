using AgricolaApi.Domain;
using AgricolaApi.Domain.Models;

namespace AgricolaApi.Application.Interfaces;

public interface IUserService
{
    IEnumerable<User> GetAllUsers();
    User? GetUserById(int id);
    Task<User?> GetUserByIdAsync(int id);
    string Create(UserDto user);
    Task<bool> DoesUserExistByEmail(UserDto user);
    Task<Result> RemoveAsync(int id);
    Task<Result> UpdateAsync(int userId, UpdateUserDto user);
}