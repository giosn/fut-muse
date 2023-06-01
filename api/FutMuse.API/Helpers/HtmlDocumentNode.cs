using HtmlAgilityPack;

namespace FutMuse.API.Helpers
{
	public static class HtmlDocumentNode
	{
		/// <summary>
		/// Creates the html document for the specified request URI
		/// </summary>
		/// <param name="requestUri"></param>
		/// <returns>
		/// The html document node
		/// </returns>
		public static async Task<HtmlNode> Get(string requestUri, string apiKey)
		{
            string proxyUrl = $"https://proxy.scrapeops.io/v1/?api_key={apiKey}&url={Uri.EscapeDataString(requestUri)}";

			HttpClient client = new()
            {
                Timeout = TimeSpan.FromSeconds(120)
            };

            string response = await client.GetStringAsync(proxyUrl);

			HtmlDocument htmlDoc = new();
			htmlDoc.LoadHtml(response);

			return htmlDoc.DocumentNode;
		}
	}
}

