namespace QuizzGenerate.Repository;

public interface IGeminiRepository
{
    IAsyncEnumerable<string> QuestionPrompt(string question);
}