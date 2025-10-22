namespace QuizzGenerate.Repository;

public interface IGeminiRepository
{
    Task<string> QuestionPrompt(string question);
}