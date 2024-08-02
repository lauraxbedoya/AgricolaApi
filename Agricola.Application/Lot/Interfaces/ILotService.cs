using AgricolaApi.Domain;
using AgricolaApi.Domain.Models;

namespace AgricolaApi.Application.Interfaces;

public interface ILotService
{
    IEnumerable<Lot> GetAll();
    IList<Lot> GetLotsByFarmId(int farmId);
    Lot Create(LotDto lot);
    Task<Result> RemoveAsync(int id);
    Task<Result> UpdateAsync(int lotId, UpdateLotDto lot);
}