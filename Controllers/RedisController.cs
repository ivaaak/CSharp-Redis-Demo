using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Redis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Foo()
        {
            var db = _redis.GetDatabase();
            var foo = await db.StringGetAsync("test");
            return Ok(foo.ToString());
        }
    }
}
