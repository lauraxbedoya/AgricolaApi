using AgricolaApi.Application;
using AgricolaApi.Application.Interfaces;
using AgricolaApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgricolaApi.Api;

// [Authorize]
[AllowAnonymous]
[ApiController]
[Route("farms")]
public class FarmsController : ControllerBase
{
    private readonly IFarmService _farmService;

    public FarmsController(IFarmService farmService)
    {
        _farmService = farmService;
    }

    [HttpGet]
    [Route("")]
    public IEnumerable<Farm> GetFarms() => _farmService.GetAllFarms();


    [AllowAnonymous]
    [HttpPost]
    [Route("")]
    public ActionResult<object> CreateFarm([FromBody] FarmDto farm)
    {
        return Ok(_farmService.Create(farm));
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<bool>> UpdateAsync([FromRoute] int id, [FromBody] UpdateFarmDto farm)
    {
        var result = await _farmService.UpdateAsync(id, farm);

        if (result.IsFailure)
        {
            if (result.Error.Code == FarmErrors.NotFarmFound.Code)
            {
                return NotFound(result.Error.Description);
            }
        }

        return Ok(true);
    }


    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<bool>> RemoveFarm([FromRoute] int id)
    {
        var result = await _farmService.RemoveAsync(id);

        if (result.IsFailure)
        {
            if (result.Error.Code == FarmErrors.NotFarmFound.Code)
            {
                return NotFound(result.Error.Description);
            }
        }

        return Ok(true);
    }
}