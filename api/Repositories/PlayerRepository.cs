using Fizzler.Systems.HtmlAgilityPack;
using fut_muse_api.Models;
using HtmlAgilityPack;

namespace fut_muse_api.Repositories
{
	public class PlayerRepository : IPlayerRepository
	{
		public PlayerRepository()
		{
		}

        public async Task<Player> Get(int id)
        {
            // retrieve html
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("user-agent", "*");
            string response = await client.GetStringAsync($"https://www.transfermarkt.com/_/profil/spieler/{id}");
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            HtmlNodeCollection nameNodes = htmlDoc
                .DocumentNode
                .SelectNodes("//header/div/h1");

            if (nameNodes is not null)
            {
                nameNodes = nameNodes.First().ChildNodes;

                HtmlNode strongNode = nameNodes
                    .Where(node => node.Name == "strong")
                    .First();
                int strongNodeIndex = nameNodes.IndexOf(strongNode);
                IEnumerable<string> actualNames = nameNodes
                    .Skip(strongNodeIndex - 1)
                    .Take(2)
                    .Select(node => node.InnerText);

                string name = string.Join("", actualNames).Trim();

                HtmlNode playerDataNode = htmlDoc
                    .DocumentNode
                    .QuerySelector(".spielerdatenundfakten .info-table.info-table--right-space");

                string fullName = name;
                string? dateOfBirth = null;
                string? placeOfBirth = null;
                string? countryOfBirth = null;
                string? dateOfDeath = null;
                int age = 0;
                int height = 0;
                string? currentClub = null;
                string? status = null;

                if (playerDataNode is not null)
                {
                    HtmlNodeCollection dataNodes = playerDataNode.ChildNodes;

                    var fullNameHeader = dataNodes
                        .Where(node => {
                            return node.InnerText.ToLower().Contains("name in home country") ||
                                   node.InnerText.ToLower().Contains("full name");
                         })
                        .FirstOrDefault();

                    if (fullNameHeader is not null)
                    {
                        int fullNameHeaderIndex = dataNodes.IndexOf(fullNameHeader);
                        fullName = dataNodes[fullNameHeaderIndex + 2]
                            .InnerText
                            .Replace("geb. ", "b. ") // geb. is german for born (b.)
                            .Replace("geb.", "b. ")  // for players who where born under a different name
                            .Trim();
                    }

                    var dateOfBirthHeader = dataNodes
                        .Where(node => node.InnerText.ToLower().Contains("date of birth"))
                        .FirstOrDefault();

                    if (dateOfBirthHeader is not null)
                    {
                        int dateOfBirthHeaderIndex = dataNodes.IndexOf(dateOfBirthHeader);
                        string dateOfBirthValue = dataNodes[dateOfBirthHeaderIndex + 2].InnerText.Trim();

                        if (dateOfBirthValue != "N/A")
                        {
                            dateOfBirth = DateOnly
                                .Parse(dataNodes[dateOfBirthHeaderIndex + 2].InnerText.Trim())
                                .ToShortDateString();
                        }
                    }

                    var placeOfBirthHeader = dataNodes
                        .Where(node => node.InnerText.ToLower().Contains("place of birth"))
                        .FirstOrDefault();

                    if (placeOfBirthHeader is not null)
                    {
                        int placeOfBirthHeaderIndex = dataNodes.IndexOf(placeOfBirthHeader);
                        placeOfBirth = dataNodes[placeOfBirthHeaderIndex + 2]
                            .InnerText
                            .Replace("&nbsp;", "")
                            .Replace("---, ", "")
                            .Trim();
                        countryOfBirth = dataNodes[placeOfBirthHeaderIndex + 2]
                            .Descendants("img")
                            .First()
                            .Attributes["title"]
                            .Value;
                    }

                    var dateOfDeathHeader = dataNodes
                        .Where(node => node.InnerText.ToLower().Contains("date of death"))
                        .FirstOrDefault();

                    if (dateOfDeathHeader is not null)
                    {
                        int dateOfDeathHeaderIndex = dataNodes.IndexOf(dateOfDeathHeader);
                        dateOfDeath = DateOnly
                            .ParseExact(
                                dataNodes[dateOfDeathHeaderIndex + 2]
                                    .InnerText
                                    .Trim()
                                    .Substring(0, 10),
                                "dd.MM.yyyy"
                            )
                            .ToShortDateString();

                        age = int.Parse(
                            dataNodes[dateOfDeathHeaderIndex + 2]
                                .InnerText
                                .Trim()
                                .Substring(12, 2)
                        );
                    }

                    if (age == 0)
                    {
                        var ageHeader = dataNodes
                            .Where(node => node.InnerText.ToLower().Contains("age:"))
                            .FirstOrDefault();

                        if (ageHeader is not null)
                        {
                            int ageHeaderIndex = dataNodes.IndexOf(ageHeader);
                            string ageValue = dataNodes[ageHeaderIndex + 2].InnerText.Trim();

                            if (!ageValue.Contains("N/A"))
                            {
                                age = int.Parse(ageValue);
                            }
                        }
                    }

                    if (height == 0)
                    {
                        var heightHeader = dataNodes
                            .Where(node => node.InnerText.ToLower().Contains("height:"))
                            .FirstOrDefault();

                        if (heightHeader is not null)
                        {
                            int heightHeaderIndex = dataNodes.IndexOf(heightHeader);
                            string heightValue = dataNodes[heightHeaderIndex + 2].InnerText;

                            if (heightValue != "N/A")
                            {
                                height = int.Parse(
                                    dataNodes[heightHeaderIndex + 2]
                                        .InnerText
                                        .Replace(",", "")
                                        .Replace("&nbsp;m", "")
                                        .Trim()
                                );
                            }
                        }
                    }

                    var currentClubHeader = dataNodes
                        .Where(node => node.InnerText.ToLower().Contains("current club"))
                        .FirstOrDefault();

                    if (currentClubHeader is not null)
                    {
                        int currentClubHeaderIndex = dataNodes.IndexOf(currentClubHeader);
                        string currentClubValue = dataNodes[currentClubHeaderIndex + 2]
                            .InnerText
                            .Trim();

                        if (currentClubValue.ToLower() != "retired" && currentClubValue != "---")
                        {
                            if (currentClubValue.ToLower() != "career break" &&
                                currentClubValue.ToLower() != "without club")
                            {
                                currentClub = currentClubValue;
                            }
                            status = "Active";
                        }
                        else
                        {
                            status = currentClubValue == "---" ? "Deceased" : currentClubValue;
                        }
                    }
                }

                IEnumerable<HtmlNode> headerDetailNodes = htmlDoc
                    .DocumentNode
                    .QuerySelector(".data-header__details")
                    .Descendants("li");

                if (countryOfBirth is null)
                {
                    string countryOfBirthValue = headerDetailNodes
                        .Where(node => node.InnerText.ToLower().Contains("citizenship"))
                        .First()
                        .ChildNodes[1]
                        .InnerText
                        .Trim();

                    if (!countryOfBirthValue.Contains("N/A"))
                    {
                        countryOfBirth = countryOfBirthValue;
                    }
                }

                string position = headerDetailNodes
                    .Where(node => node.InnerText.ToLower().Contains("position"))
                    .First()
                    .ChildNodes[1]
                    .InnerText
                    .ToLower()
                    .Replace("midfield", "midfielder")
                    .Trim();

                if (position == "attack")
                {
                    position = "attacker";
                }

                position = $"{position[0].ToString().ToUpper()}{position[1..]}";

                Player player = new(
                    id,
                    name,
                    fullName,
                    dateOfBirth,
                    placeOfBirth,
                    countryOfBirth,
                    dateOfDeath,
                    age,
                    height,
                    position,
                    currentClub,
                    status
                );

                return player;
            }

            return null;
        }
    }
}

