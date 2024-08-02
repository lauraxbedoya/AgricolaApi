using AgricolaApi.Domain.Models;
using System.Linq;
using AgricolaApi.Infrastructure.Context;
using AgricolaApi.Application.Interfaces;
using AgricolaApi.Application;
using Microsoft.EntityFrameworkCore;
using AgricolaApi.Domain;

namespace AgricolaApi.Applications
{
    public class FarmServices : IFarmService
    {
        private readonly AppDbContext _context;

        public FarmServices(AppDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Farm> GetAllFarms()
        {
            var farms = _context.Farms.ToList();
            return farms;
        }

        public async Task<Farm?> GetFarmByIdAsync(int id)
        {
            return await _context.Farms.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Farm Create(FarmDto farm)
        {
            var farmEntity = new Farm()
            {
                Name = farm.Name,
                Location = farm.Location,
                Hectares = farm.Hectares,
                Description = farm.Description,
            };
            _context.Farms.Add(farmEntity);
            _context.SaveChanges();

            return farmEntity;
        }

        public async Task<Result> UpdateAsync(int farmId, UpdateFarmDto farm)
        {
            var farmDB = await _context.Farms.FirstOrDefaultAsync((farm) => farm.Id == farmId);
            if (farmDB is null)
            {
                return Result.Failure(FarmErrors.NotFarmFound);
            }

            // let's update each updatable property
            farmDB.Name = farm.Name ?? farmDB.Name;
            farmDB.Location = farm.Location ?? farmDB.Location;
            farmDB.Hectares = farm.Hectares ?? farmDB.Hectares;
            farmDB.Description = farm.Description ?? farmDB.Description;

            await _context.SaveChangesAsync();

            return Result.Success(farmDB);
        }

        public async Task<Result> RemoveAsync(int id)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync((farm) => farm.Id == id);
            if (farm is null)
            {
                return Result.Failure(FarmErrors.NotFarmFound);
            }
            _context.Remove(farm);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}

public static class FarmErrors
{
    public static readonly Error NotFarmFound = new("Farm Not Found");
}

//obtener usuario por id, si no deviuelve retorno un notfound, si si compaar propiedad x propiedad, ejemplo dsi el email de dto no es null y es iferente al de la bd entoncesa ctualizar el de la bd, 