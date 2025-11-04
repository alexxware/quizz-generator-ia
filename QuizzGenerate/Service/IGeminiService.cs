using QuizzGenerate.Dto;

namespace QuizzGenerate.Service;

public interface IGeminiService
{
    Task<QuestionResponse> GenerateQuestion(QuestionPrompt question);
}