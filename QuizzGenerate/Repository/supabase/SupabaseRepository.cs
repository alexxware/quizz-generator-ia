using Microsoft.Extensions.Options;
using QuizzGenerate.Configuration;
using QuizzGenerate.Dto.register;
using QuizzGenerate.Models.supabase;
using Supabase.Gotrue;
using Client = Supabase.Client;

namespace QuizzGenerate.Repository.supabase;

public class SupabaseRepository: ISupabaseRepository
{
    private readonly Client _client;
    private readonly SupabaseSettings _settings;

    public SupabaseRepository(Client client, IOptions<SupabaseSettings> options)
    {
        _client = client;
        _settings = options.Value;
    }
    public async Task<string> SignUpUser(RegisterRequestDto users)
    {
        Session? response = await _client.Auth.SignUp(users.Email, users.Password);
        return response?.User?.Id ?? "";
    }

    public async Task<object?> SignInUser(string email, string password)
    {
        var response = await _client.Auth.SignIn(email, password);
        return response;
    }

    public async Task<TblUsers?> InsertUser(TblUsers user)
    {
        try
        {
            var options = new Supabase.SupabaseOptions
            {
                AutoRefreshToken = true,
            };
            var client = new Supabase.Client(_settings.Url,
                _settings.ServiceRoleKey,
                options);
            var response = await client.From<TblUsers>().Insert(user);
            return response.Models.FirstOrDefault();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new TblUsers();
        }
        
    }
}