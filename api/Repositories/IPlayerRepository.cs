using System;
using fut_muse_api.Models;

namespace fut_muse_api.Repositories
{
	public interface IPlayerRepository
	{
		Task<Player?> GetProfile(int id);
		Task<IEnumerable<Achievement>?> GetAchivements(int id);
	}
}

