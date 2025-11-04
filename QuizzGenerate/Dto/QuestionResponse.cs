namespace QuizzGenerate.Dto;

public struct QuestionResponse
{
    public List<QuestionDto> Questions {
        get;
        set;
    }

    public bool HasError { get; set; }
    public string ErrorDescription { get; set; }
}