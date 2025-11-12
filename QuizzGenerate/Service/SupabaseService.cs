using AutoMapper;
using QuizzGenerate.Dto.register;
using QuizzGenerate.Models.supabase;
using QuizzGenerate.Repository.supabase;

namespace QuizzGenerate.Service;

public class SupabaseService: ISupabaseService
{
    private readonly ISupabaseRepository _respository;
    private readonly IMapper _mapper;

    public SupabaseService(ISupabaseRepository repository, IMapper mapper)
    {
        _respository = repository;
        _mapper = mapper;
    }
    public async Task<RegisterResponseDto> SignUpUser(RegisterRequestDto user)
    {
        var response = await _respository.SignUpUser(user);

        if (response.Length == 0)
        {
            return new RegisterResponseDto
            {
                HasError = true,
                ErrorMessage = "Ocurrio un error al intentar registrar al usuario"
            };
        }
        
        user.IdAuth = response;

        var mappedUser = _mapper.Map<TblUsers>(user);

        var res = await _respository.InsertUser(mappedUser);

        var resMapped = _mapper.Map<RegisterResponseDto>(res);
        return resMapped;
    }
}