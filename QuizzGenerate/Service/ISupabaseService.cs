using Microsoft.AspNetCore.Mvc;
using QuizzGenerate.Dto.login;
using QuizzGenerate.Dto.register;
using QuizzGenerate.Dto.user;

namespace QuizzGenerate.Service;

public interface ISupabaseService
{
    Task<RegisterResponseDto> SignUpUser(RegisterRequestDto user);
    Task<LoginResponseDto> SignInUser(LoginRequestDto user);
    Task<ResponseUserDto> GetUser(string uid);
}