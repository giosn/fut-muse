namespace fut_muse_api.Models
{
	public class Achievement
	{
		public string Name { get; set; }
		public int NumberOfTitles { get; set; }
		public List<Title> Titles { get; set; }

		public Achievement(
			string name,
			int numberOfTitles,
			List<Title> titles
		)
		{
			Name = name;
			NumberOfTitles = numberOfTitles;
			Titles = titles;
		}
	}
}

