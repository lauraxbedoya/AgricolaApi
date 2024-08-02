using AgricolaApi.Domain.Models;
using System.Linq;
using AgricolaApi.Infrastructure.Context;
using AgricolaApi.Application.Interfaces;
using AgricolaApi.Application;
using Microsoft.EntityFrameworkCore;
using AgricolaApi.Domain;

namespace AgricolaApi.Applications
{
    public class GroupServices : IGroupService
    {
        private readonly AppDbContext _context;

        public GroupServices(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Group> GetAll()
        {
            var groups = _context.Groups.ToList();
            return groups;
        }

        public IList<Group> GetGroupLotById(int lotId)
        {
            return _context.Groups.Include(l => l.Lot).Where(x => x.LotId == lotId).ToList();
        }

        public Group Create(GroupDto group)
        {
            var groupEntity = new Group()
            {
                LotId = group.LotId,
                Name = group.Name,
            };
            _context.Groups.Add(groupEntity);
            _context.SaveChanges();

            return groupEntity;
        }

        public async Task<Result> UpdateAsync(int groupId, UpdateGroupDto group)
        {
            var groupDB = await _context.Groups.FirstOrDefaultAsync((group) => group.Id == groupId);
            if (groupDB is null)
            {
                return Result.Failure(GroupErrors.NotGroupFound);
            }

            groupDB.Name = group.Name ?? groupDB.Name;

            await _context.SaveChangesAsync();

            return Result.Success(groupDB);
        }

        public async Task<Result> RemoveAsync(int id)
        {
            var group = await _context.Groups.FirstOrDefaultAsync((group) => group.Id == id);
            if (group is null)
            {
                return Result.Failure(GroupErrors.NotGroupFound);
            }
            _context.Remove(group);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}

public static class GroupErrors
{
    public static readonly Error NotGroupFound = new("Group Not Found");
}