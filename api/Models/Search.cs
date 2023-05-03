namespace fut_muse_api.Models
{
	public class Search
	{
		public bool ExtendedSearchAvailable { get; set; }
		public int TotalHits { get; set; }
		public List<Hit> Hits { get; set; }

		public Search(
			bool extendedSearchAvailable,
			int totalHits,
			List<Hit> hits
		)
		{
			ExtendedSearchAvailable = extendedSearchAvailable;
			TotalHits = totalHits;
			Hits = hits;
		}
	}
}

