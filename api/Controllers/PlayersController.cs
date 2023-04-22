using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fut_muse_api.Models;
using fut_muse_api.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fut_muse_api.Controllers
{
    [Route("api/[controller]")]
    public class PlayersController : Controller
    {
        private readonly IPlayerRepository playerRepository;

        public PlayersController(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        // GET api/players/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> Get(int id)
        {
            try
            {
                Player player = await playerRepository.Get(id);

                if (player is null)
                {
                    return NotFound($"player with id {id} not found");
                }

                return Ok(player);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}

