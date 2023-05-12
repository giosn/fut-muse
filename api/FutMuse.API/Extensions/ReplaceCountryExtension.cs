namespace FutMuse.API.Extensions
{
    public static class ReplaceCountryExtension
    {
        static readonly Dictionary<string, string> countryList = new()
        {
            { "USSR", "Soviet Union" },
            { "UdSSR", "Soviet Union" },
            { "CSSR", "Czechoslovakia" },
            { "Jugoslawien (SFR)", "Yugoslavia" },
            { "Yugoslavia (Republic)", "Yugoslavia" },
            { "East Germany (GDR)", "East Germany" },
            { "Cookinseln", "Cook Islands" },
            { "DR Congo", "Democratic Republic of the Congo" },
            { "Hongkong", "Hong Kong" },
            { "Korea, North", "North Korea" },
            { "Korea, South", "South Korea" },
            { "Netherlands East India", "Dutch East Indies" },
            { "Neukaledonien", "New Caledonia" },
            { "People's republic of the Congo", "People's Republic of the Congo" },
            { "Turks- and Caicosinseln", "Turks and Caicos Islands" }
        };

        public static string ReplaceCountry(this string country)
        {
            string? newCountry = countryList.GetValueOrDefault(country);

            if (newCountry is not null)
            {
                country = country.Replace(country, newCountry);
            }

            return country;
        }
    }
}

