namespace fut_muse_api.Models
{
	public class Title
	{
		public string Period { get; set; }
		public string? Entity { get; set; }

		public Title(
			string period,
			string? entity
		)
		{
			Period = period;
			Entity = entity;
		}
	}
}

