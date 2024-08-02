using AgricolaApi.Application;
using AgricolaApi.Application.Interfaces;
using AgricolaApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgricolaApi.Api;

[Authorize]
[ApiController]
[Route("groups")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet]
    [Route("")]
    public IEnumerable<Group> GetGroups() => _groupService.GetAll();

    [HttpGet("{lotId}")]
    public ActionResult<IList<Group>> GetLotFarmById([FromRoute] int lotId)
    {
        IList<Group>? groupLot = _groupService.GetGroupLotById(lotId);
        if (groupLot is null) return NotFound();

        return Ok(groupLot);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("")]
    public ActionResult<object> CreateGroup([FromBody] GroupDto group)
    {
        return Ok(_groupService.Create(group));
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<bool>> UpdateAsync([FromRoute] int id, [FromBody] UpdateGroupDto group)
    {
        var result = await _groupService.UpdateAsync(id, group);

        if (result.IsFailure)
        {
            if (result.Error.Code == GroupErrors.NotGroupFound.Code)
            {
                return NotFound(result.Error.Description);
            }
        }

        return Ok(true);
    }


    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<bool>> RemoveGroup([FromRoute] int id)
    {
        var result = await _groupService.RemoveAsync(id);

        if (result.IsFailure)
        {
            if (result.Error.Code == GroupErrors.NotGroupFound.Code)
            {
                return NotFound(result.Error.Description);
            }
        }

        return Ok(true);
    }
}