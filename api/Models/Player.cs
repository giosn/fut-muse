using System;
namespace fut_muse_api.Models
{
	public class Player
	{
		public int TMId { get; set; }
		public string Name { get; set; }
		public string FullName { get; set; }
		public string ImageUrl { get; set; }
		public string? DateOfBirth { get; set; }
		public string? PlaceOfBirth { get; set; }
		public string? CountryOfBirth { get; set; }
		public string? CountryOfBirthImageUrl { get; set; }
		public string? DateOfDeath { get; set; }
		public int Age { get; set; }
		public int? Height { get; set; } // in cm
		public string Position { get; set; }
		public string? Club { get; set; }
		public string? ClubImageUrl { get; set; }
		public string? Status { get; set; } // active, retired, deceased

		public Player(
			int tmId,
			string name,
			string fullName,
			string imageUrl,
			string? dateOfBirth,
			string? placeOfBirth,
			string? countryOfBirth,
			string? countryOfBirthImageUrl,
			string? dateOfDeath,
			int age,
			int? height,
			string position,
			string? club,
			string? clubImageUrl,
			string? status
		)
		{
			TMId = tmId;
			Name = name;
			FullName = fullName;
			ImageUrl = imageUrl;
			DateOfBirth = dateOfBirth;
			PlaceOfBirth = placeOfBirth;
			CountryOfBirth = countryOfBirth;
			CountryOfBirthImageUrl = countryOfBirthImageUrl;
			DateOfDeath = dateOfDeath;
			Age = age;
			Height = height;
			Position = position;
			Club = club;
			ClubImageUrl = clubImageUrl;
			Status = status;
		}
	}
}

