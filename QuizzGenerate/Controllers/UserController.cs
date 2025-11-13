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
    [HttpPost]
    public async Task<ActionResult> GetUser([FromBody] GetUserDto user)
    {
        if (user.Id.Length == 0 )
        {
            return BadRequest("Id is empty.");
        }
        var response = await _supabaseService.GetUser(user.Id);
        if (response.HasError)
        {
            return Conflict(response.Menssage);
        }

        return Ok(response.User);
    }
}