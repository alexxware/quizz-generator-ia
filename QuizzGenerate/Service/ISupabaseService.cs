using QuizzGenerate.Dto.register;

namespace QuizzGenerate.Service;

public interface ISupabaseService
{
    Task<RegisterResponseDto> SignUpUser(RegisterRequestDto user);
}