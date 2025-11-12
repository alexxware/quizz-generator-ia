using AutoMapper;
using QuizzGenerate.Dto.register;
using QuizzGenerate.Models.supabase;

namespace QuizzGenerate.Mappers;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<TblUsers, RegisterResponseDto>();
        CreateMap<RegisterRequestDto, TblUsers>();
    }
    
}