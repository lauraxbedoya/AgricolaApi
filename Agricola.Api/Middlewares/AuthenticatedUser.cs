using System.Security.Claims;
using AgricolaApi.Application.Interfaces;

namespace AgricolaApi.Api;

// .NET core middleware to be run before/after each endpoint
public class AuthenticatedUser : IMiddleware
{
  private readonly IUserService _userService;

  public AuthenticatedUser(IUserService userService)
  {
    _userService = userService;
  }

  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    // only endpoints with SetAuthUser attribute should be proccesed 
    if (ShouldSetUser(context))
    {
      context = await SetAuthUser(context);
    }

    await next(context);
  }

  // only endpoints with SetAuthUser attribute should be proccesed 
  private bool ShouldSetUser(HttpContext context)
  {
    var endpoint = context.GetEndpoint();
    if (endpoint != null)
    {
      var shouldSetUser = endpoint.Metadata.GetMetadata<SetAuthUser>();
      return shouldSetUser is not null;
    }
    return false;
  }

  // set on the HttpContext of the request the found logged in user by its token sent on the headers
  private async Task<HttpContext> SetAuthUser(HttpContext context)
  {
    // get User claims set by Microsoft.AspNetCore.Authentication when token verification was ok.
    var user = context.User;

    if (user.Identity?.IsAuthenticated == true)
    {
      // get user Id from token claims
      var userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
      if (userId != null)
      {
        // get user from database with the user id from token and set it on a new context Item called "User"
        context.Items["User"] = await _userService.GetUserByIdAsync(int.Parse(userId));
      }
    }
    return context;
  }
}
