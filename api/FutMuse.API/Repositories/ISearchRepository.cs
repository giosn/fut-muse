using System;
using FutMuse.API.Models;

namespace FutMuse.API.Repositories
{
	public interface ISearchRepository
	{
		Task<Search> Get(string query, int page);
	}
}

