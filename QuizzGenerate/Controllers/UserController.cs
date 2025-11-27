using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizzGenerate.Dto.user;
using QuizzGenerate.Service;

namespace QuizzGenerate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ISupabaseService _supabaseService;

    public UserController(ISupabaseService service)
    {
        _supabaseService = service;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetUser()
    {
        var authenticatedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(authenticatedUserId))
        {
            return Unauthorized("Usuario no identificado, requiere inicio de sesion.");
        }

        var response = await _supabaseService.GetUser(authenticatedUserId);
        if (response.HasError)
        {
            return NotFound(response.Menssage);
        }

        return Ok(response.User);
    }
}