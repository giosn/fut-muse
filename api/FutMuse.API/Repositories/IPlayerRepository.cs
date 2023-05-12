using System;
using FutMuse.API.Models;

namespace FutMuse.API.Repositories
{
	public interface IPlayerRepository
	{
		Task<Player?> GetProfile(int id);
		Task<IEnumerable<Achievement>?> GetAchivements(int id);
	}
}

