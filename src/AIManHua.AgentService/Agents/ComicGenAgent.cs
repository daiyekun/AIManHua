using Microsoft.SemanticKernel;

namespace AIManHua.AgentService.Agents;

public class ComicGenAgent
{
    private readonly Kernel _kernel;
    private readonly ILogger<ComicGenAgent> _logger;

    public ComicGenAgent(Kernel kernel, ILogger<ComicGenAgent> logger)
    {
        _kernel = kernel;
        _logger = logger;
    }

    // Agent methods will be implemented during business logic phase
}
