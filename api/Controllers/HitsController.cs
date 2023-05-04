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
        public async Task<ActionResult<Search>> Get(string query, int page)
        {
            try
            {
                var search = await hitRepository.Get(query, page);
                return Ok(search);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}

