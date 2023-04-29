namespace fut_muse_api.Models
{
	public class Title
	{
		public string Period { get; set; }
		public string? Entity { get; set; }
		public string? EntityImageUrl { get; set; }

		public Title(
			string period,
			string? entity,
			string? entityImageUrl
		)
		{
			Period = period;
			Entity = entity;
			EntityImageUrl = entityImageUrl;
		}
	}
}

