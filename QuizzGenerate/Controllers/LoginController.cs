using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using QuizzGenerate.Dto.login;
using QuizzGenerate.Service;

namespace QuizzGenerate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private ISupabaseService _supabaseService;
    private IValidator<LoginRequestDto> _validator;
    public LoginController(ISupabaseService service, IValidator<LoginRequestDto> validator)
    {
        _supabaseService = service;
        _validator = validator;
    }

    [HttpPost]
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

        return Ok(new LoginResponseDto
        {
            Uid = response.Uid,
            Message = "Inicio de sesion exitoso"
        });
    }
}