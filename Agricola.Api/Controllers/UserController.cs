using AgricolaApi.Application;
using AgricolaApi.Application.Interfaces;
using AgricolaApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgricolaApi.Api;

[Authorize]
[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthServices _authService;

    public UsersController(IUserService userService, IAuthServices authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpGet]
    [Route("")]
    public IEnumerable<User> GetUsers() => _userService.GetAllUsers();

    // using attributes set out user, so the AuthenticatedUser Middleware can identify it and get the current logged in user
    [SetAuthUser]
    [HttpGet]
    [Route("me")]
    public ActionResult<User?> GetMe()
    {
        //get current logged in user from HttpContext that was set by AuthenticatedUser Middleware
        User? currentUser = HttpContext.Items["User"] as User;

        if (currentUser is null)
        {
            return NotFound();
        }

        currentUser.Password = null;

        return Ok(currentUser);
    }


    [HttpGet("{id}")]
    public ActionResult<User> GetUserById([FromRoute] int id)
    {
        User? user = _userService.GetUserById(id);
        if (user is null) return NotFound();

        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<object>> LoginUser(
        [FromBody] LoginDataDto LoginRequest)
    {
        string token = await _authService.AuthenticateAsync(LoginRequest.Email, LoginRequest.Password);

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(token);
        }

        return new { token };
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("")]
    public async Task<ActionResult<object>> CreateUser([FromBody] UserDto user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        if (await _userService.DoesUserExistByEmail(user))
        {
            return BadRequest("User aleready exist");
        }

        var token = _userService.Create(user);

        return Ok(new { token });
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<bool>> RemoveUser([FromRoute] int id)
    {
        var result = await _userService.RemoveAsync(id);

        if (result.IsFailure)
        {
            if (result.Error.Code == UserErrors.NotUserFound.Code)
            {
                return NotFound(result.Error.Description);
            }
        }

        return Ok(true);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<bool>> UpdateAsync([FromRoute] int id, [FromBody] UpdateUserDto user)
    {
        var result = await _userService.UpdateAsync(id, user);

        if (result.IsFailure)
        {
            if (result.Error.Code == UserErrors.NotUserFound.Code)
            {
                return NotFound(result.Error.Description);
            }
        }

        return Ok(true);
    }
}