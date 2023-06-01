using System;
using FutMuse.API.Extensions;
using FutMuse.API.Helpers;
using FutMuse.API.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FutMuse.API.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly IConfiguration configuration;

        public SearchRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Search> Get(string query, int page)
        {
            // safely retrieve ScrapeOps API key
            string scrapeOpsApiKey = configuration["SCRAPEOPS_API_KEY"];

            // retrieve html page
            string requestUri = $"https://www.transfermarkt.com/schnellsuche/ergebnis/schnellsuche?query={query}";
            requestUri += page > 1 ? $"&Spieler_page={page}" : "";
            HtmlNode htmlDoc = await HtmlDocumentNode.Get(requestUri, scrapeOpsApiKey);

            // get the header node that indicates search hits are found
            HtmlNode? searchResultsHeader = htmlDoc
                .Descendants("h2")
                .FirstOrDefault(node => node.InnerText.ToLower().Trim().Contains("search results for players"));

            bool extendedSearchAvailable = false;
            int totalHits = 0;
            List<Hit> hits = new();

            if (searchResultsHeader is not null)
            {
                totalHits = int.Parse(searchResultsHeader
                    .InnerText
                    .ToLower()
                    .Trim()
                    .Split(" - ")
                    .Last()
                    .Replace(" hits", "")
                );

                // get the search results
                var searchResultNodes = htmlDoc
                    .Descendants("tbody")
                    .First()
                    .SelectNodes("tr");

                // enable extended search for more than 5 hits
                if (searchResultNodes.Count > 5)
                {
                    extendedSearchAvailable = true;
                }

                for (int i = 0; i < searchResultNodes.Count; i++)
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
            }

            return new Search(
                extendedSearchAvailable,
                totalHits,
                hits
            );
        }
    }
}

