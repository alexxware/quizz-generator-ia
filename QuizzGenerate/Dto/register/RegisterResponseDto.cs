namespace QuizzGenerate.Dto.register;

public class RegisterResponseDto
{
    public long Id { get; set; }
    public string IdAuth { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool HasError { get; set; }
    public string ErrorMessage { get; set; }
}