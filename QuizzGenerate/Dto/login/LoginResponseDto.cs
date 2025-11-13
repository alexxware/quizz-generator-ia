namespace QuizzGenerate.Dto.login;

public class LoginResponseDto
{
    public string Uid { get; set; }
    public bool HasError { get; set; }
    public string Message { get; set; }
}