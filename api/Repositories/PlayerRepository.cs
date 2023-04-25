using Fizzler.Systems.HtmlAgilityPack;
using fut_muse_api.Extensions;
using fut_muse_api.Models;
using HtmlAgilityPack;

namespace fut_muse_api.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        public async Task<Player?> GetProfile(int id)
        {
            // retrieve html
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("user-agent", "*");
            string response = await client.GetStringAsync($"https://www.transfermarkt.com/_/profil/spieler/{id}");
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(response);

            HtmlNodeCollection nameNodes = htmlDoc
                .DocumentNode
                .SelectNodes("//header/div/h1");

            if (nameNodes is not null)
            {
                nameNodes = nameNodes.First().ChildNodes;

                HtmlNode strongNode = nameNodes.First(node => node.Name == "strong");
                int strongNodeIndex = nameNodes.IndexOf(strongNode);
                var actualNames = nameNodes
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
                        .FirstOrDefault(node =>
                        {
                            return node.InnerText.ToLower().Contains("name in home country") ||
                                   node.InnerText.ToLower().Contains("full name");
                        });

                    if (fullNameHeader is not null)
                    {
                        int fullNameHeaderIndex = dataNodes.IndexOf(fullNameHeader);
                        fullName = dataNodes[fullNameHeaderIndex + 2]
                            .InnerText
                            .Replace("geb. ", "b. ") // geb. is german for born (b.)
                            .Replace("geb.", "b. ")  // for players who where born under a different name
                            .Trim();
                    }

                    var dateOfBirthHeader = dataNodes.FirstOrDefault(node => node.InnerText.ToLower().Contains("date of birth"));

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

                    var placeOfBirthHeader = dataNodes.FirstOrDefault(node => node.InnerText.ToLower().Contains("place of birth"));

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
                            .Value
                            .ReplaceCountry();
                    }

                    var dateOfDeathHeader = dataNodes.FirstOrDefault(node => node.InnerText.ToLower().Contains("date of death"));

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
                        var ageHeader = dataNodes.FirstOrDefault(node => node.InnerText.ToLower().Contains("age:"));

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
                        var heightHeader = dataNodes.FirstOrDefault(node => node.InnerText.ToLower().Contains("height:"));
                            

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

                    var currentClubHeader = dataNodes.FirstOrDefault(node => node.InnerText.ToLower().Contains("current club"));

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

                var headerDetailNodes = htmlDoc
                    .DocumentNode
                    .QuerySelector(".data-header__details")
                    .Descendants("li");

                if (countryOfBirth is null)
                {
                    string countryOfBirthValue = headerDetailNodes
                        .First(node => node.InnerText.ToLower().Contains("citizenship"))
                        .ChildNodes[1]
                        .InnerText
                        .Trim();

                    if (!countryOfBirthValue.Contains("N/A"))
                    {
                        countryOfBirth = countryOfBirthValue.ReplaceCountry();
                    }
                }

                string position = headerDetailNodes
                    .First(node => node.InnerText.ToLower().Contains("position"))
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

                string imageUrl = htmlDoc
                    .DocumentNode
                    .QuerySelector(".data-header__profile-image")
                    .Attributes["src"]
                    .Value;

                Player player = new(
                    id,
                    name,
                    fullName,
                    imageUrl,
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

        public async Task<IEnumerable<Achievement>?> GetAchivements(int id)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("user-agent", "*");
            string response = await client.GetStringAsync($"https://www.transfermarkt.com/_/erfolge/spieler/{id}");
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(response);

            HtmlNode? allTitlesHeader = htmlDoc
                .DocumentNode
                .Descendants("h2")
                .FirstOrDefault(node => node.InnerText.ToLower().Trim() == "all titles");

            if (allTitlesHeader is not null)
            {
                var tableBodyNodes = allTitlesHeader
                    .ParentNode
                    .Descendants("tbody")
                    .First()
                    .Descendants("tr");

                List<Achievement> achievements = new();
                string name = "";
                int numberOfTitles = 0;
                List<Title> titles = new();

                for (int i = 0; i < tableBodyNodes.Count(); i++)
                {
                    var currentNode = tableBodyNodes.ElementAt(i);
                    bool nodeIsHeader = currentNode.HasClass("bg_Sturm");

                    if (nodeIsHeader)
                    {
                        if (i > 0)
                        {
                            achievements.Add(new Achievement(
                                name,
                                numberOfTitles,
                                titles
                            ));
                            titles = new();
                        }

                        string titleNameRowValue = currentNode.InnerText.Trim();
                        int xIndex = titleNameRowValue.IndexOf("x");
                        bool isParticipantTitle = titleNameRowValue.ToLower().Contains("participant");
                        name = titleNameRowValue
                            .Substring(xIndex + 2)
                            .ReplaceCountry()
                            .ReplaceEntity();
                        numberOfTitles = int.Parse(titleNameRowValue.Substring(0, xIndex));
                    }
                    else
                    {
                        var titleNodes = currentNode.Descendants("td");
                        string period = titleNodes
                            .First()
                            .InnerText
                            .Trim();
                        string? entity = null;

                        if (titleNodes.Count() > 1)
                        {
                            var entityValue = titleNodes
                                .First(node => node.HasClass("no-border-links"))
                                .InnerText
                                .Trim();
                            int remainderIndex = entityValue.IndexOf("\n");

                            if (remainderIndex > 0)
                            {
                                entityValue = entityValue.Substring(0, remainderIndex);
                            }

                            remainderIndex = entityValue.IndexOf(" - ");

                            if (remainderIndex > 0)
                            {
                                entityValue = entityValue.Substring(0, remainderIndex);
                            }

                            entity = entityValue.ReplaceCountry().ReplaceEntity();
                        }

                        titles.Add(new Title(
                            period,
                            entity
                        ));
                    }
                }

                achievements.Add(new Achievement(
                    name,
                    numberOfTitles,
                    titles
                ));

                return achievements.Where(achievement =>
                {
                    return !achievement.Name.ToLower().Contains("participant") &&
                           !achievement.Name.ToLower().Contains("relegated")   &&
                           !achievement.Name.ToLower().Contains("personality");
                });
            }
            else
            {
                HtmlNode? mostValuablePlayersHeader = htmlDoc
                    .DocumentNode
                    .Descendants("h2")
                    .FirstOrDefault(node => node.InnerText.ToLower().Trim() == "most valuable players");
                return mostValuablePlayersHeader is null ? Array.Empty<Achievement>() : null;
            }
        }
    }
}

