using System;
namespace fut_muse_api.Models
{
	public class Player
	{
		public int TMId { get; set; }
		public string Name { get; set; }
		public string? FullName { get; set; }
		public DateTimeOffset DateOfBirth { get; set; }
		public string PlaceOfBirth { get; set; }
		public DateTimeOffset? DateOfDeath { get; set; }
		public List<string> Citizenship { get; set; }
		public int Age { get; set; }
		public int? Height { get; set; } 
		public string? PreferredFoot { get; set; }
		public string Position { get; set; }
		public string? CurrentClub { get; set; }
		public string Status { get; set; }
	}
}

