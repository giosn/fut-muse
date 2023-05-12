using FutMuse.API.Models;
using FutMuse.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FutMuse.API.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private readonly ISearchRepository searchRepository;

        public SearchController(ISearchRepository searchRepository)
        {
            this.searchRepository = searchRepository;
        }

        // GET api/search/query?page=0
        [HttpGet("{query}")]
        public async Task<ActionResult<Search>> Get(string query, int page)
        {
            try
            {
                var search = await searchRepository.Get(query, page);
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

