using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using QuizzGenerate.Dto.login;
using QuizzGenerate.Service;

namespace QuizzGenerate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    private const string RefreshTokenConst = "refresh_token";
    private const string AccessTokenConst = "access_token_client";
    private readonly ISupabaseService _supabaseService;
    private readonly IValidator<LoginRequestDto> _validator;
    public SessionController(ISupabaseService service, IValidator<LoginRequestDto> validator)
    {
        _supabaseService = service;
        _validator = validator;
    }

    [HttpPost("LogIn")]
    public async Task<ActionResult> LogIn([FromBody] LoginRequestDto user)
    {
        var validRes = await _validator.ValidateAsync(user);
        if (!validRes.IsValid)
        {
            return BadRequest(validRes.Errors);
        }

        var response = await _supabaseService.SignInUser(user);
        if (response.HasError)
        {
            return Unauthorized(response.Message);
        }
        
        SetCookieConfigure(RefreshTokenConst, response.RefreshToken!, 12);
        SetCookieConfigure(AccessTokenConst, response.AccessToken!, 5);
        
        return Ok(response);
    }

    [HttpPost("Refresh")]
    public async Task<ActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refresh_token"];
        var expiredAccessToken = Request.Cookies["access_token_client"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized("Se requiere iniciar sesion nuevamente");
        }
        if (string.IsNullOrEmpty(expiredAccessToken))
        {
            expiredAccessToken = "INVALID_TOKEN_FOR_SET_SESSION"; 
        }

        var session = await _supabaseService.RefreshToken(refreshToken, expiredAccessToken);
        if (session.HasError)
        {
            HttpContext.Response.Cookies.Delete("refresh_token");
            HttpContext.Response.Cookies.Delete("access_token_client");
            return Unauthorized(session.Message);
        }

        SetCookieConfigure(RefreshTokenConst, session.RefreshToken!, 12);
        SetCookieConfigure(AccessTokenConst, session.AccessToken!, 5);

        return Ok(new 
        {
            AccessToken = session.AccessToken
        });
    }

    [HttpPost("LogOut")]
    public async Task<ActionResult> LogOut()
    {
        await _supabaseService.SignOutUser();
        HttpContext.Response.Cookies.Delete("refresh_token");
        HttpContext.Response.Cookies.Delete("access_token_client");
        return Ok(new { Message = "Sesión cerrada exitosamente." });
    }

    private void SetCookieConfigure(string name, string sessionToken, int hours)
    {
        HttpContext.Response.Cookies.Append(name, sessionToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddHours(hours)
        });
    }
}