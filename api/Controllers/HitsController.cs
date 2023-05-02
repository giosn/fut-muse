using fut_muse_api.Models;
using fut_muse_api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace fut_muse_api.Controllers
{
    [Route("api/[controller]")]
    public class HitsController : Controller
    {
        private readonly IHitRepository hitRepository;

        public HitsController(IHitRepository hitRepository)
        {
            this.hitRepository = hitRepository;
        }

        // GET api/hits/"search_query"
        [HttpGet("{query}")]
        public async Task<ActionResult<Hit>> Get(string query)
        {
            try
            {
                var hits = await hitRepository.Get(query);
                return Ok(hits);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}

