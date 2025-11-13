using Microsoft.AspNetCore.Mvc;
using QuizzGenerate.Dto.login;
using QuizzGenerate.Dto.register;

namespace QuizzGenerate.Service;

public interface ISupabaseService
{
    Task<RegisterResponseDto> SignUpUser(RegisterRequestDto user);
    Task<LoginResponseDto> SignInUser(LoginRequestDto user);
}