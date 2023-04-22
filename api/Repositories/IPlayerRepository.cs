using System;
using fut_muse_api.Models;

namespace fut_muse_api.Repositories
{
	public interface IPlayerRepository
	{
		Task<Player> Get(int id);
	}
}

