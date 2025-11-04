using System.Text;
using System.Text.Json;
using QuizzGenerate.Dto;
using QuizzGenerate.Repository;

namespace QuizzGenerate.Service;

public class GeminiService: IGeminiService
{
    private readonly IGeminiRepository _repository;
    public GeminiService(IGeminiRepository repository)
    {
        _repository = repository;
    }
    public async Task<QuestionResponse> GenerateQuestion(QuestionPrompt question)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Sigue las siguientes instrucciones.");
        sb.Append("1. Unicamente debes generar la respuesta en texto plano pero formato JSON el cual contendra la siguiente estructura:");
        string json = """
                      {
                        mainQuestion: "pregunta generada",
                        answare1: "answare 1",
                        idAnsware1: 'A',
                        answare2: "answare 2",
                        idAnsware2: 'B',
                        answare3: "answare 3",
                        idAnsware3: 'C',
                        answare4: "answare 4",
                        idAnsware4: 'D',
                        idAnswareCorrect:'B',
                        explication: 'explicacion sobre la pregunta'
                      }
                      """;
        sb.Append(json);
        sb.Append("2. Debes generar el questionario en el lenguaje en el que venga la pregunta.");
        sb.Append("3. En caso de no entender la pregunta genera un objeto json con el error en true");
        sb.Append("4. Considera que no se deben repetir las preguntas, ya se realizaron las siguientes preguntas:");
        sb.Append("Nota: Si no hay preguntas extra indica que es la primera pregunta.");
        for (int i = 0; i < question.questions.Count; i++)
        {
            sb.Append(question.questions[i]);
        }

        sb.Append(
            "Con las consideraciones anteriores debes de generar 5 preguntas con sus respectivas respuestas en el formato JSON mencionado anteriormente, ten en cuenta que siempre debe tener formato de lista");
        sb.Append("El tema con el que debes generar la pregunta es: " + question.topic);

        string rawResponse = await _repository.QuestionPrompt(sb.ToString());
        string cleanedJson = rawResponse
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<QuestionDto>? result = JsonSerializer.Deserialize<List<QuestionDto>>(cleanedJson, options);
            if (result is null)
            {
                return new QuestionResponse
                {
                    HasError = true,
                    ErrorDescription = "No se generaron correctamente las preguntas",
                    Questions = new List<QuestionDto>()
                };
            }

            return new QuestionResponse
            {
                HasError = false,
                ErrorDescription = string.Empty,
                Questions = result
            };
        }
        catch (ArgumentNullException nu)
        {
            // Manejo de error si el string no pudo ser parseado como JSON
            Console.WriteLine($"Error de Deserialización JSON: {nu.Message}");
            Console.WriteLine($"JSON fallido: {cleanedJson}");
            return new QuestionResponse
            {
                HasError = true,
                ErrorDescription = nu.Message
            };
        }
        catch (JsonException e)
        {
            // Manejo de error si el string no pudo ser parseado como JSON
            Console.WriteLine($"Error de Deserialización JSON: {e.Message}");
            Console.WriteLine($"JSON fallido: {cleanedJson}");
            return new QuestionResponse
            {
                HasError = true,
                ErrorDescription = e.Message
            };
        }
    }
}