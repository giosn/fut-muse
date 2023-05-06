using System;
namespace fut_muse_api.Models
{
	public class Player : Hit
	{
		public string FullName { get; set; }
		public string? DateOfBirth { get; set; }
		public string? PlaceOfBirth { get; set; }
		public string? CountryOfBirth { get; set; }
		public string? CountryOfBirthImageUrl { get; set; }
		public string? DateOfDeath { get; set; }
		public int Age { get; set; }
		public int? Height { get; set; } // in cm
		public string Position { get; set; }

		public Player(
			int tmId,
			string name,
			string fullName,
			string imageUrl,
			string? mainNationality,
			string? mainNationalityImageUrl,
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
		) : base (
			tmId,
			name,
			imageUrl,
			mainNationality,
			mainNationalityImageUrl,
			club,
			clubImageUrl,
			status
		)
		{
			TMId = tmId;
			Name = name;
			FullName = fullName;
			ImageUrl = imageUrl;
			MainNationality = mainNationality;
			MainNationalityImageUrl = mainNationalityImageUrl;
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

