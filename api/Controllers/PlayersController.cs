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
        public Player Get(int id)
        {
            return playerRepository.Get(id);
        }
    }
}

