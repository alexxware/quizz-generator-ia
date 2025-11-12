using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using QuizzGenerate.Dto.register;
using QuizzGenerate.Service;

namespace QuizzGenerate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegisterController : ControllerBase
{
    private IValidator<RegisterRequestDto> validatorRequest;
    private ISupabaseService supabaseService;

    public RegisterController(
        IValidator<RegisterRequestDto> validatorRequest,
        ISupabaseService supabaseService)
    {
        this.validatorRequest = validatorRequest;
        this.supabaseService = supabaseService;
    }
    
    [HttpPost]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterRequestDto user)
    {
        var validationResult = await validatorRequest.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var response = supabaseService.RegisterUser(user);
        if (response.Name == string.Empty)
        {
            return Conflict();
        }

        return Ok(response);
    }
}