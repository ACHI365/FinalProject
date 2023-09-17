using System.Security.Claims;
using FinalProject.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AdminPanel.Config;

public class CustomJwtBearerEvents : JwtBearerEvents
{
    private readonly IUserService _userService;

    public CustomJwtBearerEvents(IUserService userService)
    {
        _userService = userService;
    }

    public override Task TokenValidated(TokenValidatedContext context)
    {
        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
        var userIdClaim = context.Principal.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null)
        {
            int userId = int.Parse(userIdClaim.Value);
            var user = userService.GetUserById(userId).Result;
            if (user == null)
            {
                context.Fail("User does not exist");
                return Task.CompletedTask;
            }
        }
        return Task.CompletedTask;
    }
}