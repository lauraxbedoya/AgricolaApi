
using AgricolaApi.Domain.Models;

namespace AgricolaApi.Application.Interfaces;

public interface IJwtProvider
{
    string Generate(User user);
}
