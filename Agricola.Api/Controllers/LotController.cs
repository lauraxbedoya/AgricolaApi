using AgricolaApi.Application;
using AgricolaApi.Application.Interfaces;
using AgricolaApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgricolaApi.Api;

// [Authorize]
[AllowAnonymous]
[ApiController]
[Route("lots")]
public class LotsController : ControllerBase
{
    private readonly ILotService _lotService;

    public LotsController(ILotService lotService)
    {
        _lotService = lotService;
    }

    [HttpGet]
    [Route("")]
    public IEnumerable<Lot> GetLots() => _lotService.GetAll();

    [HttpGet("{farmId}")]
    public ActionResult<IList<Lot>> GetLotFarmById([FromRoute] int farmId)
    {
        IList<Lot>? lotFarm = _lotService.GetLotsByFarmId(farmId);
        if (lotFarm is null) return NotFound();

        return Ok(lotFarm);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("")]
    public ActionResult<object> CreateLot([FromBody] LotDto lot)
    {
        return Ok(_lotService.Create(lot));
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<bool>> UpdateAsync([FromRoute] int id, [FromBody] UpdateLotDto lot)
    {
        var result = await _lotService.UpdateAsync(id, lot);

        if (result.IsFailure)
        {
            if (result.Error.Code == LotErrors.NotLotFound.Code)
            {
                return NotFound(result.Error.Description);
            }
        }

        return Ok(true);
    }


    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<bool>> RemoveLot([FromRoute] int id)
    {
        var result = await _lotService.RemoveAsync(id);

        if (result.IsFailure)
        {
            if (result.Error.Code == LotErrors.NotLotFound.Code)
            {
                return NotFound(result.Error.Description);
            }
        }

        return Ok(true);
    }
}