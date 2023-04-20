using System;
using fut_muse_api.Models;

namespace fut_muse_api.Repositories
{
	public interface IPlayerRepository
	{
		Player Get(int id);
	}
}

