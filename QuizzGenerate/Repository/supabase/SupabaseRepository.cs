using Microsoft.Extensions.Options;
using QuizzGenerate.Configuration;
using QuizzGenerate.Dto.register;
using QuizzGenerate.Models.supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;
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

    public async Task<Session?> SignInUser(string email, string password)
    {
        var response = await _client.Auth.SignInWithPassword(email, password);

        if (response is not null)
        {
            await _client.Auth.SetSession(response.AccessToken!, response.RefreshToken!, true);
            
        }
        return response;
    }

    public async Task<TblUsers?> InsertUser(TblUsers user)
    {
        try
        {
            var response = await _client
                .From<TblUsers>()
                .Insert(user);
            return response.Models.FirstOrDefault();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new TblUsers();
        }
    }

    public async Task<TblUsers?> GetUser(string uid)
    {
        var user = await _client
            .From<TblUsers>()
            .Where(u => u.IdAuth == uid)
            .Single();

        return user;
    }

    public async Task<Session?> RefreshToken(string refreshToken, string accessToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;
        
        try
        {
            var newSession = await _client.Auth.SetSession(
                accessToken,
                refreshToken,
                true);
            return newSession;
        }
        catch (GotrueException ex)
        {
            Console.WriteLine($"Error Gotrue al refrescar: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al refrescar el token: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> SignOutUser(string refreshToken, string accessToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken) || string.IsNullOrWhiteSpace(accessToken))
        {
            return false;
        }
        
        try
        {
            await _client.Auth.SetSession(accessToken, refreshToken, true);
            await _client.Auth.SignOut();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error al cerrar sesion: {e.Message}");
            return false;
        }
    }
}