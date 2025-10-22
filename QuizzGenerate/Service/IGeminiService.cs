using QuizzGenerate.Dto;

namespace QuizzGenerate.Service;

public interface IGeminiService
{
    IAsyncEnumerable<string> GenerateQuestion(QuestionPrompt question);
}