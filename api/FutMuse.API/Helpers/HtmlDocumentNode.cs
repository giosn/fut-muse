using System;
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
		public static async Task<HtmlNode> Get(string requestUri)
		{
			HttpClient client = new();
			client.DefaultRequestHeaders.Add("user-agent", "*");
			string response = await client.GetStringAsync(requestUri);
			HtmlDocument htmlDoc = new();
			htmlDoc.LoadHtml(response);
			return htmlDoc.DocumentNode;
		}
	}
}

