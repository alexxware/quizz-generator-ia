using QuizzGenerate.Dto.register;
using QuizzGenerate.Models.supabase;

namespace QuizzGenerate.Repository.supabase;

public interface ISupabaseRepository
{
    Task<string> SignUpUser(RegisterRequestDto users);
    Task<object?> SignInUser(string email, string password);
    Task<TblUsers?> InsertUser(TblUsers user);

}