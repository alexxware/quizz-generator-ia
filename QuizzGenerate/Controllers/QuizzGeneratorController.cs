using Microsoft.AspNetCore.Mvc;
using QuizzGenerate.Dto;
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
        
        var question =  await _apiService.GenerateQuestion(prompt);
        if (question.HasError)
        {
            return StatusCode(500, question);
        }
        
        return Ok(question);
    }
}