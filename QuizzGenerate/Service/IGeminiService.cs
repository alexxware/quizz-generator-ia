using QuizzGenerate.Dto;

namespace QuizzGenerate.Service;

public interface IGeminiService
{
    Task<QuestionDto> GenerateQuestion(QuestionPrompt question);
}