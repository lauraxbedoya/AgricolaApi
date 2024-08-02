using AgricolaApi.Domain.Models;
using System.Linq;
using AgricolaApi.Infrastructure.Context;
using AgricolaApi.Application.Interfaces;
using AgricolaApi.Application;
using Microsoft.EntityFrameworkCore;
using AgricolaApi.Domain;

namespace AgricolaApi.Applications
{
    public class LotServices : ILotService
    {
        private readonly AppDbContext _context;

        public LotServices(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Lot> GetAll()
        {
            var lots = _context.Lots.ToList();
            return lots;
        }

        public IList<Lot> GetLotsByFarmId(int farmId)
        {
            return _context.Lots.Include(f => f.Farm).Where(x => x.FarmId == farmId).ToList();
        }

        public Lot Create(LotDto lot)
        {
            var lotEntity = new Lot()
            {
                FarmId = lot.FarmId,
                Name = lot.Name,
                Trees = lot.Trees,
                Stage = lot.Stage,
            };
            _context.Lots.Add(lotEntity);
            _context.SaveChanges();

            return lotEntity;
        }

        public async Task<Result> UpdateAsync(int lotId, UpdateLotDto lot)
        {
            var lotDB = await _context.Lots.FirstOrDefaultAsync((lot) => lot.Id == lotId);
            if (lotDB is null)
            {
                return Result.Failure(LotErrors.NotLotFound);
            }

            // let's update each updatable property
            lotDB.Name = lot.Name ?? lotDB.Name;
            lotDB.Trees = lot.Trees;
            lotDB.Stage = lot.Stage ?? lotDB.Stage;

            await _context.SaveChangesAsync();

            return Result.Success(lotDB);
        }

        public async Task<Result> RemoveAsync(int id)
        {
            var lot = await _context.Lots.FirstOrDefaultAsync((lot) => lot.Id == id);
            if (lot is null)
            {
                return Result.Failure(LotErrors.NotLotFound);
            }
            _context.Remove(lot);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}

public static class LotErrors
{
    public static readonly Error NotLotFound = new("Lot Not Found");
}