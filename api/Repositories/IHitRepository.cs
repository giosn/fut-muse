using System;
using fut_muse_api.Models;

namespace fut_muse_api.Repositories
{
	public interface IHitRepository
	{
		Task<Search> Get(string query, int page);
	}
}

