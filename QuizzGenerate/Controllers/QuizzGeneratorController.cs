using Microsoft.AspNetCore.Mvc;

namespace QuizzGenerate.Controllers;

public class QuizzGeneratorController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}