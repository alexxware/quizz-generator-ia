using QuizzGenerate.Dto.register;

namespace QuizzGenerate.Service;

public interface ISupabaseService
{
    RegisterResponseDto RegisterUser(RegisterRequestDto user);
}