namespace fut_muse_api.Models
{
	public class Title
	{
		public string? Entity { get; set; }
		public string? EntityImageUrl { get; set; }
		public List<string> Periods { get; set; }

		public Title(
			string? entity,
			string? entityImageUrl,
			List<string> periods
		)
		{
			Entity = entity;
			EntityImageUrl = entityImageUrl;
			Periods = periods;
		}
	}
}

