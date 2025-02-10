using Microsoft.AspNetCore.Mvc;
using sharding.service;

namespace sharding.Controllers;

[ApiController]
[Route("[controller]")]
public class ShardingController : ControllerBase
{
    private Repository repository;
    public ShardingController(Repository repo)
    {
       repository = repo;
    }

    [HttpGet]
    [Route("url/{urlid}")]
    public async Task<IActionResult> Get([FromRoute] string urlid)
    {
        var result = await repository.getUrl(urlid);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> write()
    {
        for (int i = 0; i < 100; i++)
        {
            string url = $"https://www.sharding.com/wikipedia{i}";
            var result = await repository.write(url);
        }
        return Ok("done");
    }
}
