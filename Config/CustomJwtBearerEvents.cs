using System.Security.Claims;
using FinalProject.Service.ServiceInterface;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FinalProject.Config;

public class CustomJwtBearerEvents : JwtBearerEvents
{
    public override Task TokenValidated(TokenValidatedContext context)
    {
        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
        var userIdClaim = context.Principal?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null)
           UserValidation(userIdClaim, userService, context);
        return Task.CompletedTask;
    }

    private void UserValidation(Claim userIdClaim, IUserService userService, TokenValidatedContext context)
    {
        int userId = int.Parse(userIdClaim.Value);
        var user = userService.GetUserById(userId).Result;
        if (user == null)
            context.Fail("User does not exist");
    }
}