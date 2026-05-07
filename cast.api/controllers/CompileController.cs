using cast.api.core;
using Microsoft.AspNetCore.Mvc;

namespace cast.api.controllers;

[Route("api/[controller]")]
[ApiController]
public class CompileController : ControllerBase
{
    private readonly CompilationService _compilationService;
    public CompileController(CompilationService compilationService)
    {
        _compilationService = compilationService;
    }

    [HttpPost]
    [Produces("application/json")]
    public CompilationResult Compile(string input)
    {
        CompilationResult result = _compilationService.Compile(input);
        Console.WriteLine(result.Result);
        return result;
    }
}