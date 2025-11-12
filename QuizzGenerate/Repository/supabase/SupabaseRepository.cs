using QuizzGenerate.Dto.register;
using QuizzGenerate.Models.supabase;
using Supabase.Gotrue;
using Client = Supabase.Client;

namespace QuizzGenerate.Repository.supabase;

public class SupabaseRepository: ISupabaseRepository
{
    private readonly Client _client;

    public SupabaseRepository(Client client)
    {
        _client = client;
    }
    public async Task<string> SignUpUser(RegisterRequestDto users)
    {
        Session? response = await _client.Auth.SignUp(users.Email, users.Password);
        return response?.User?.Id ?? "";
    }

    public async Task<TblUsers?> InsertUser(TblUsers user)
    {
        try
        {
            var response = await _client.From<TblUsers>().Insert(user);
            return response.Models.FirstOrDefault();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new TblUsers();
        }
        
    }
}