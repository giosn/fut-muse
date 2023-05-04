using System;
using fut_muse_api.Models;

namespace fut_muse_api.Repositories
{
	public interface ISearchRepository
	{
		Task<Search> Get(string query, int page);
	}
}

