namespace QuizzGenerate.Dto.login;

public class RefreshResponseDto
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public bool HasError { get; set; }
    public string? Message { get; set; }
}