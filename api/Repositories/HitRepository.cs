using System;
using fut_muse_api.Extensions;
using fut_muse_api.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace fut_muse_api.Repositories
{
    public class HitRepository : IHitRepository
    {
        public async Task<IEnumerable<Hit>> Get(string query)
        {
            // retrieve html page
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("user-agent", "*");
            string response = await client.GetStringAsync($"https://www.transfermarkt.com/schnellsuche/ergebnis/schnellsuche?query={query}");
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(response);

            // get the header node that indicates search hits are found
            HtmlNode? searchResultsHeader = htmlDoc
                .DocumentNode
                .Descendants("h2")
                .FirstOrDefault(node => node.InnerText.ToLower().Trim().Contains("search results for players"));

            if (searchResultsHeader is not null)
            {
                // get the search results
                var searchResultNodes = htmlDoc
                    .DocumentNode
                    .Descendants("tbody")
                    .First()
                    .SelectNodes("tr")
                    .Take(5);
                List<Hit> hits = new();

                for (int i = 0; i < searchResultNodes.Count(); i++)
                {
                    // initialize hit properties
                    int tmId = 0;
                    string name = "";
                    string imageUrl = "";
                    string? club = null;
                    string? clubImageUrl = null;
                    string? mainNationality = null;
                    string? mainNationalityImageUrl = null;
                    string? status = null;

                    // gather necessary tabla cell nodes
                    HtmlNode currentNode = searchResultNodes.ElementAt(i);
                    var tdNodes = currentNode
                        .SelectNodes("td")
                        .Where((node, index) => index % 2 == 0)
                        .Take(3);
                    var nameTds = tdNodes.ElementAt(0).SelectNodes("table/tr/td");
                    HtmlNode clubTd = tdNodes.ElementAt(1);
                    HtmlNode nationalitiesTd = tdNodes.ElementAt(2);

                    HtmlNode nameRefNode = nameTds
                        .ElementAt(1)
                        .Descendants("a")
                        .First();
                    tmId = int.Parse(nameRefNode
                        .Attributes["href"]
                        .Value
                        .Split("/")
                        .Last()
                    );
                    name = nameRefNode.InnerText.Trim();
                    imageUrl = nameTds
                        .Descendants("img")
                        .First()
                        .Attributes["src"]
                        .Value
                        .Replace("/small/", "/medium/");
                    string clubValue = nameTds
                        .Last()
                        .InnerText
                        .Trim();

                    // set club and club image only for active players
                    if (clubValue.ToLower() != "retired" && clubValue != "---")
                    {
                        if (clubValue.ToLower() != "career break" &&
                            clubValue.ToLower() != "without club")
                        {
                            club = clubValue;
                            string clubImageUrlValue = clubTd
                                .Descendants("img")
                                .First()
                                .Attributes["src"]
                                .Value
                                .Replace("/tiny/", "/big/");
                            int urlEndIndex = clubImageUrlValue.IndexOf(".png") + 4;
                            clubImageUrl = clubImageUrlValue.Substring(0, urlEndIndex);
                        }
                        status = "Active";
                    }
                    else
                    {
                        status = clubValue == "---" ? "Deceased" : "Retired";
                    }

                    var nationalitiesImgs = nationalitiesTd.Descendants("img");

                    if (nationalitiesImgs.Any())
                    {
                        mainNationality = nationalitiesImgs
                            .First()
                            .Attributes["title"]
                            .Value
                            .ReplaceCountry();
                        string mainNationalityImageUrlValue = nationalitiesImgs
                            .First()
                            .Attributes["src"]
                            .Value
                            .Replace("/verysmall/", "/head/");
                        int urlEndIndex = mainNationalityImageUrlValue.IndexOf(".png") + 4;
                        mainNationalityImageUrl = mainNationalityImageUrlValue.Substring(0, urlEndIndex);
                    }

                    hits.Add(new Hit(
                        tmId,
                        name,
                        imageUrl,
                        club,
                        clubImageUrl,
                        mainNationality,
                        mainNationalityImageUrl,
                        status
                    ));
                }

                return hits;
            }

            return Array.Empty<Hit>();
        }
    }
}

