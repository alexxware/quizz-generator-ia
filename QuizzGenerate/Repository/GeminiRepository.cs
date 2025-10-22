using System.Text;
using Microsoft.Extensions.Options;
using Mscc.GenerativeAI;
using QuizzGenerate.Models;

namespace QuizzGenerate.Repository;

public class GeminiRepository: IGeminiRepository
{
    private readonly string _geminiApiKey;

    public GeminiRepository(IOptions<ApiKeysOptions> options)
    {
        _geminiApiKey = options.Value.ApiGen;
    }
    
    public async Task<string> QuestionPrompt(string question)
    {
        var googleApi = new GoogleAI(apiKey: _geminiApiKey);
        var model = googleApi.GenerativeModel(model: Model.Gemini25Flash);
        
        var jsonResponseBuilder = new StringBuilder();
        
        await foreach (var response in model.GenerateContentStream(question))
        {
            foreach (var candidate in response.Candidates)
            {
                foreach (var part in candidate.Content.Parts)
                {
                    if (!string.IsNullOrEmpty(part.Text))
                    {
                        jsonResponseBuilder.Append(part.Text);
                    }
                }
            }
        }
        
        return jsonResponseBuilder.ToString();
    }
}