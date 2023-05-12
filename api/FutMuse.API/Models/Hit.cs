using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FutMuse.API.Models
{
	public class Hit
	{
        public int TMId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string? Club { get; set; }
        public string? ClubImageUrl { get; set; }
        public string? MainNationality { get; set; }
        public string? MainNationalityImageUrl { get; set; }
        public string? Status { get; set; } // active, retired, deceased

        public Hit(
            int tmId,
            string name,
            string imageUrl,
            string? club,
            string? clubImageUrl,
            string? mainNationality,
            string? mainNationalityImageUrl,
            string? status
        )
        {
            TMId = tmId;
            Name = name;
            ImageUrl = imageUrl;
            Club = club;
            ClubImageUrl = clubImageUrl;
            MainNationality = mainNationality;
            MainNationalityImageUrl = mainNationalityImageUrl;
            Status = status;
        }
	}
}

