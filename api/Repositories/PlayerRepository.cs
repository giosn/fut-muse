using System;
using fut_muse_api.Models;

namespace fut_muse_api.Repositories
{
	public class PlayerRepository : IPlayerRepository
	{
		public PlayerRepository()
		{
		}

        public Player Get(int id)
        {
            return new Player();
        }
    }
}

