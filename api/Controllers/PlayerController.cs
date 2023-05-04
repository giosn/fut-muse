using fut_muse_api.Models;
using fut_muse_api.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fut_muse_api.Controllers
{
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private readonly IPlayerRepository playerRepository;

        public PlayerController(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        // GET api/players/profile/1
        [HttpGet("profile/{id}")]
        public async Task<ActionResult<Player>> GetProfile(int id)
        {
            try
            {
                var player = await playerRepository.GetProfile(id);
                return player is not null ? Ok(player) : NotFound($"player with id {id} not found");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        // GET api/players/achivements/1
        [HttpGet("achievements/{id}")]
        public async Task<ActionResult<IEnumerable<Achievement>>> GetAchievements(int id)
        {
            try
            {
                var achievements = await playerRepository.GetAchivements(id);
                return achievements is not null ? Ok(achievements) : NotFound($"player with id {id} not found");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}

