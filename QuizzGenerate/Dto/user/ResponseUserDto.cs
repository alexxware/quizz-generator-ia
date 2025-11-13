namespace QuizzGenerate.Dto.user;

public class ResponseUserDto
{
    public UserDto User { get; set; }
    public bool HasError { get; set; }
    public string Menssage { get; set; }
}