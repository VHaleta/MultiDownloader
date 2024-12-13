using Microsoft.AspNetCore.Mvc;
using MultiDownloader.DatabaseApi.Host.Models;

namespace MultiDownloader.DatabaseApi.Host.Controllers
{
    [ApiController]
    [Route("Database")]
    public class GraphQlController : ControllerBase
    {
        //private readonly ILogger<GraphQlController> _logger;

        //public GraphQlController(ILogger<GraphQlController> logger)
        //{
        //    _logger = logger;
        //}

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody]GraphQlQuery query)
        //{
        //    if (query == null)
        //    {
        //        throw new ArgumentNullException(nameof(query));
        //    }

        //    var inputs = query.Variables?.ToInputs();
        //}
    }
}
