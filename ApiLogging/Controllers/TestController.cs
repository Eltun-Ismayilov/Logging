using ApiLogging.Data;
using ApiLogging.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiLogging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestDbContext db;

        public TestController(TestDbContext db)
        {
            this.db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Test demo)
        {
            db.Tests.Add(demo);
            await db.SaveChangesAsync();
            return Ok(demo.Id);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await db.Tests.ToListAsync();

            return Ok(data);
        }
    }
}
