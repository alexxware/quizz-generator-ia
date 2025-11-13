using QuizzGenerate.Dto.register;
using QuizzGenerate.Models.supabase;
using Supabase.Gotrue;

namespace QuizzGenerate.Repository.supabase;

public interface ISupabaseRepository
{
    Task<string> SignUpUser(RegisterRequestDto users);
    Task<Session?> SignInUser(string email, string password);
    Task<TblUsers?> InsertUser(TblUsers user);

}