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
            // retrieve html page
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("user-agent", "*");
            string response = await client.GetStringAsync($"https://www.transfermarkt.com/_/profil/spieler/{id}");
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(response);

            // get the nodes for the name a player is most known for
            HtmlNodeCollection nameNodes = htmlDoc.DocumentNode.SelectNodes("//header/div/h1");

            if (nameNodes is not null)
            {
                nameNodes = nameNodes.First().ChildNodes;
                HtmlNode strongNode = nameNodes.First(node => node.Name == "strong");
                int strongNodeIndex = nameNodes.IndexOf(strongNode);
                var actualNames = nameNodes
                    .Skip(strongNodeIndex - 1)
                    .Take(2)
                    .Select(node => node.InnerText);

                // player property initialization
                string name = string.Join("", actualNames).Trim();
                string fullName = name;
                string imageUrl = htmlDoc
                    .DocumentNode
                    .QuerySelector(".data-header__profile-image")
                    .Attributes["src"]
                    .Value;
                string? dateOfBirth = null;
                string? placeOfBirth = null;
                string? countryOfBirth = null;
                string? dateOfDeath = null;
                int age = 0;
                int? height = null;
                string position = "";
                string? club = null;
                string? clubImageUrl = null;
                string? status = null;

                // get the main node where the key profile info is located
                HtmlNode playerDataNode = htmlDoc
                    .DocumentNode
                    .QuerySelector(".spielerdatenundfakten .info-table.info-table--right-space");

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
                            .Trim();                 // e.g. Puskás (geb.Purczeld) Ferenc => Puskás (b. Purczeld) Ferenc
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

                        // country of birth is taken from the flag img next to the place of birth
                        // text due to players sometimes having more than one citizenship
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

                        // age is taken from the date of death row for deceased players
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

                    var currentClubHeader = dataNodes.FirstOrDefault(node => node.InnerText.ToLower().Contains("current club"));

                    if (currentClubHeader is not null)
                    {
                        int currentClubHeaderIndex = dataNodes.IndexOf(currentClubHeader);
                        string currentClubValue = dataNodes[currentClubHeaderIndex + 2]
                            .InnerText
                            .Trim();

                        // a player's status is determined based on the current club value
                        if (currentClubValue.ToLower() != "retired" && currentClubValue != "---")
                        {
                            if (currentClubValue.ToLower() != "career break" &&
                                currentClubValue.ToLower() != "without club")
                            {
                                club = currentClubValue;
                                string clubImageUrlValue = dataNodes[currentClubHeaderIndex + 2]
                                    .Descendants("img")
                                    .First()
                                    .Attributes["srcset"]
                                    .Value
                                    .Replace("/small/", "/big/");
                                int urlEndIndex = clubImageUrlValue.IndexOf(".png") + 4;
                                clubImageUrl = clubImageUrlValue.Substring(0, urlEndIndex);
                            }
                            status = "Active";
                        }
                        else
                        {
                            status = currentClubValue == "---" ? "Deceased" : "Retired";
                        }
                    }
                }

                // get the list of the summarized profile info
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

                position = headerDetailNodes
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
                    club,
                    clubImageUrl,
                    status
                );

                return player;
            }

            // player not found
            return null;
        }

        public async Task<IEnumerable<Achievement>?> GetAchivements(int id)
        {
            // retrieve html page
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("user-agent", "*");
            string response = await client.GetStringAsync($"https://www.transfermarkt.com/_/erfolge/spieler/{id}");
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(response);

            // get the main node where a player's titles are listed
            HtmlNode? allTitlesHeader = htmlDoc
                .DocumentNode
                .Descendants("h2")
                .FirstOrDefault(node => node.InnerText.ToLower().Trim() == "all titles");

            if (allTitlesHeader is not null)
            {
                // get the titles list
                var tableBodyNodes = allTitlesHeader
                    .ParentNode
                    .Descendants("tbody")
                    .First()
                    .Descendants("tr");

                // achievement property initialization
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
                        // start building a new achievement once a title header is found
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

                        // check for entity (team, organization, tournament, etc.)
                        // individually won titles do not have an entity
                        if (titleNodes.Count() > 1)
                        {
                            var entityValue = titleNodes
                                .First(node => node.HasClass("no-border-links"))
                                .InnerText
                                .Trim();

                            // filter out extra info (e.g. FC Barcelona - 10 goals => FC Barcelona)
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

                // add remaining achievement
                achievements.Add(new Achievement(
                    name,
                    numberOfTitles,
                    titles
                ));

                // filter out unmemorable achivements
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

