using HtmlAgilityPack;

namespace FutMuse.API.Helpers
{
	public class HtmlDocumentNode
	{
		private readonly IConfiguration configuration;
		private readonly IWebHostEnvironment environment;

		public HtmlDocumentNode(IConfiguration configuration, IWebHostEnvironment environment)
		{
			this.configuration = configuration;
			this.environment = environment;
		}

		/// <summary>
		/// Creates the html document for the specified request URI
		/// </summary>
		/// <param name="requestUri"></param>
		/// <returns>
		/// The html document node
		/// </returns>
		public async Task<HtmlNode> Get(string requestUri)
		{
			string scrapeOpsApiKey = configuration["Secrets:SCRAPEOPS_API_KEY"];
			string userAgent = configuration["UserAgent"];
			string response;

			if (environment.IsDevelopment())
			{
				HttpClient client = new();
				client.DefaultRequestHeaders.Add("user-agent", userAgent);
				response = await client.GetStringAsync(requestUri);
			}
			else
			{
				string proxyUrl = $"https://proxy.scrapeops.io/v1/?api_key={scrapeOpsApiKey}&url={Uri.EscapeDataString(requestUri)}&residential=true&country=us";
				HttpClient client = new()
				{
					Timeout = TimeSpan.FromSeconds(120)
				};
				response = await client.GetStringAsync(proxyUrl);
			}

			HtmlDocument htmlDoc = new();
			htmlDoc.LoadHtml(response);

			return htmlDoc.DocumentNode;
		}
	}
}

