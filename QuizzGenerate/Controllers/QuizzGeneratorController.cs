using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuizzGenerate.Dto;
using QuizzGenerate.Models;
using QuizzGenerate.Service;

namespace QuizzGenerate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzGeneratorController : ControllerBase
{
    
    private readonly IGeminiService _apiService;

    public QuizzGeneratorController(IGeminiService apiService)
    {
        _apiService = apiService;
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateQuestion([FromBody] QuestionPrompt prompt)
    {
        if (string.IsNullOrEmpty(prompt.topic) || prompt.topic.Length < 3)
        {
            return BadRequest("Topic is empty or too short");
        }
        
        Response.ContentType = "text/plain; charset=utf-8";
        await foreach (var response in _apiService.GenerateQuestion(prompt))
        {
            await Response.WriteAsync(response);
            await Response.Body.FlushAsync();
        }
        
        return NoContent();
    }
}