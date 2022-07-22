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
        
        private readonly IDistributedCache _cache;


        public RedisController(
            IConnectionMultiplexer redis, 
            IDistributedCache _cache)
        {
            _redis = redis;
            _cache = cache;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Foo()
        {
            var db = _redis.GetDatabase();
            var foo = await db.StringGetAsync("test");
            return Ok(foo.ToString());
        }
        
        [HttpGet("date")]
        public async Task<IActionResult> Date()
        {
            DateTime dateTime = DateTime.Now;
            var cachedData = await _cache.GetStringAsync("cachedTime");

            if (cachedData == null)
            {
                cachedData = dateTime.ToString();
                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromSeconds(20),
                    AbsoluteExpiration = DateTime.Now.AddSeconds(60)
                };

                await _cache.SetStringAsync("cachedTime", cachedData, cacheOptions);
            }

            return View(nameof(Index), cachedData);
        }
    }
}
