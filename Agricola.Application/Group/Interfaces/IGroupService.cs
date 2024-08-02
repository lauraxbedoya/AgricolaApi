using AgricolaApi.Domain;
using AgricolaApi.Domain.Models;

namespace AgricolaApi.Application.Interfaces;

public interface IGroupService
{
    IEnumerable<Group> GetAll();
    IList<Group> GetGroupLotById(int lotId);
    Group Create(GroupDto group);
    Task<Result> RemoveAsync(int id);
    Task<Result> UpdateAsync(int groupId, UpdateGroupDto group);
}