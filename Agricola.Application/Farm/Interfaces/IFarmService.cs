using AgricolaApi.Domain;
using AgricolaApi.Domain.Models;

namespace AgricolaApi.Application.Interfaces;

public interface IFarmService
{
    IEnumerable<Farm> GetAllFarms();
    Task<Farm?> GetFarmByIdAsync(int id);
    Farm Create(FarmDto farm);
    Task<Result> RemoveAsync(int id);
    Task<Result> UpdateAsync(int farmId, UpdateFarmDto farm);
}