namespace AgricolaApi.Application;

public interface IAuthServices
{
    Task<string> AuthenticateAsync(string Email, string Password);
}
