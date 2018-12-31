//-----------------------------------------------------------------------
// <copyright file="YouTube.cs" company="MyToolkit">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/MyToolkit/MyToolkit/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

// Developed by Rico Suter (http://rsuter.com), http://mytoolkit.codeplex.com
namespace Bau.Libraries.DevConference.Application.Tools
{
	/// <summary>Provides methods to access YouTube streams and data. </summary>
	public class YouTube
	{
		private const string BotUserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";

		/// <summary>Gets the default minimum quality. </summary>
		public const YouTubeUri.YouTubeQuality DefaultMinQuality = YouTubeUri.YouTubeQuality.Quality144P;

		/// <summary>Returns the best matching YouTube stream URI which has an audio and video channel and is not 3D. </summary>
		public async Task<YouTubeUri> GetVideoUriAsync(string youTubeId, YouTubeUri.YouTubeQuality minQuality, YouTubeUri.YouTubeQuality maxQuality, CancellationToken token)
		{
			var uris = await GetUrisAsync(youTubeId, token);
			var uri = TryFindBestVideoUri(uris, minQuality, maxQuality);
			if (uri == null)
				throw new YouTubeUriException("No matching YouTube video or audio stream URI could be found. " +
													  "The video may not be available in your country, " +
													  "is private or uses RTMPE (protected streaming).");
			return uri;
		}

		/// <summary>Returns the best matching YouTube stream URI which has an audio and video channel and is not 3D. </summary>
		public Task<YouTubeUri> GetVideoUriAsync(string youTubeId, YouTubeUri.YouTubeQuality maxQuality)
		{
			return GetVideoUriAsync(youTubeId, DefaultMinQuality, maxQuality, CancellationToken.None);
		}

		/// <summary>Returns the best matching YouTube stream URI which has an audio and video channel and is not 3D. </summary>
		public Task<YouTubeUri> GetVideoUriAsync(string youTubeId, YouTubeUri.YouTubeQuality maxQuality, CancellationToken token)
		{
			return GetVideoUriAsync(youTubeId, DefaultMinQuality, maxQuality, token);
		}

		/// <summary>Returns the best matching YouTube stream URI which has an audio and video channel and is not 3D. </summary>
		public Task<YouTubeUri> GetVideoUriAsync(string youTubeId, YouTubeUri.YouTubeQuality minQuality, YouTubeUri.YouTubeQuality maxQuality)
		{
			return GetVideoUriAsync(youTubeId, minQuality, maxQuality, CancellationToken.None);
		}

		/// <summary>Returns all available URIs (audio-only and video) for the given YouTube ID. </summary>
		public Task<YouTubeUri[]> GetUrisAsync(string youTubeId)
		{
			return GetUrisAsync(youTubeId, CancellationToken.None);
		}

		/// <summary>Returns all available URIs (audio-only and video) for the given YouTube ID. </summary>
		public async Task<YouTubeUri[]> GetUrisAsync(string youTubeId, CancellationToken token)
		{
			YouTubeDecipherer decipherer = new YouTubeDecipherer();
			var urls = new List<YouTubeUri>();
			string javaScriptCode = null;

			var response = await HttpGetAsync($"https://www.youtube.com/watch?v={youTubeId}&nomobile=1", token);
			var match = Regex.Match(response, "url_encoded_fmt_stream_map\": ?\"(.*?)\"");
			var data = Uri.UnescapeDataString(match.Groups [1].Value);
			match = Regex.Match(response, "adaptive_fmts\": ?\"(.*?)\"");
			var data2 = Uri.UnescapeDataString(match.Groups [1].Value);

			var arr = Regex.Split($"{data},{data2}", ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"); // split by comma but outside quotes


			foreach (var d in arr)
			{
				string url = "";
				string signature = "";
				YouTubeUri tuple = new YouTubeUri();
				foreach (var p in d.Replace("\\u0026", "\t").Split('\t'))
				{
					int index = p.IndexOf('=');
					if (index != -1 && index < p.Length)
					{
						try
						{
							string key = p.Substring(0, index);
							string value = Uri.UnescapeDataString(p.Substring(index + 1));
							if (key == "url")
								url = value;
							else if (key == "itag")
								tuple.Itag = int.Parse(value);
							else if (key == "type" && (value.Contains("video/mp4") || value.Contains("audio/mp4")))
								tuple.Type = value;
							else if (key == "sig")
								signature = value;
							else if (key == "s")
							{
								if (javaScriptCode == null)
								{
									string javaScriptUri;
									Match urlMatch = Regex.Match(response, "\"\\\\/\\\\/s.ytimg.com\\\\/yts\\\\/jsbin\\\\/html5player-(.+?)\\.js\"");
									if (urlMatch.Success)
										javaScriptUri = $"http://s.ytimg.com/yts/jsbin/html5player-{urlMatch.Groups[1]}.js";
									else
									{
										// new format
										javaScriptUri = "https://s.ytimg.com/yts/jsbin/player-" +
											Regex.Match(response, "\"\\\\/\\\\/s.ytimg.com\\\\/yts\\\\/jsbin\\\\/player-(.+?)\\.js\"").Groups[1] + ".js";
									}
									javaScriptCode = await HttpGetAsync(javaScriptUri, token);
								}

								signature = decipherer.Decipher(value, javaScriptCode);
							}
						}
						catch (Exception exception)
						{
							throw new YouTubeUriException($"YouTube parse exception: {exception.Message}");
						}
					}
				}

				if (!string.IsNullOrEmpty(url))
				{
					if (url.Contains("&signature=") || url.Contains("?signature="))
						tuple.Uri = new Uri(url, UriKind.Absolute);
					else
						tuple.Uri = new Uri(url + "&signature=" + signature, UriKind.Absolute);

					if (tuple.IsValid)
						urls.Add(tuple);
				}
			}

			return urls.ToArray();
		}

		private YouTubeUri TryFindBestVideoUri(IEnumerable<YouTubeUri> uris, YouTubeUri.YouTubeQuality minQuality, YouTubeUri.YouTubeQuality maxQuality)
		{
			return uris
				.Where(u => u.HasVideo && u.HasAudio && !u.Is3DVideo && u.VideoQuality >= minQuality && u.VideoQuality <= maxQuality)
				.OrderByDescending(u => u.VideoQuality)
				.FirstOrDefault();
		}

		private async Task<string> HttpGetAsync(string uri, CancellationToken token)
		{
			HttpClientHandler handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip };

				using (var client = new HttpClient(handler))
				{
					client.DefaultRequestHeaders.Add("User-Agent", BotUserAgent);
					var response = await client.GetAsync(new Uri(uri, UriKind.Absolute));
					return await response.Content.ReadAsStringAsync();
				}
		}

		/// <summary>Returns the title of the YouTube video. </summary>
		public Task<string> GetVideoTitleAsync(string youTubeId)
		{
			return GetVideoTitleAsync(youTubeId, CancellationToken.None);
		}

		/// <summary>Returns the title of the YouTube video. </summary>
		public async Task<string> GetVideoTitleAsync(string youTubeId, CancellationToken token)
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("User-Agent", BotUserAgent);
				HttpResponseMessage response = await client.GetAsync($"http://www.youtube.com/watch?v={youTubeId}&nomobile=1", token);
				string html = await response.Content.ReadAsStringAsync();
				int startIndex = html.IndexOf(" title=\"");

					if (startIndex != -1)
					{
						startIndex = html.IndexOf(" title=\"", startIndex + 1);
						if (startIndex != -1)
						{
							startIndex += 8;
							int endIndex = html.IndexOf("\">", startIndex);
							if (endIndex != -1)
								return html.Substring(startIndex, endIndex - startIndex);
						}
					}
					return null;
			}
		}

		/// <summary>Returns a thumbnail for the given YouTube ID. </summary>
		public Uri GetThumbnailUri(string youTubeId, YouTubeUri.YouTubeThumbnailSize size = YouTubeUri.YouTubeThumbnailSize.Medium)
		{
			switch (size)
			{
				case YouTubeUri.YouTubeThumbnailSize.Small:
					return new Uri($"http://img.youtube.com/vi/{youTubeId}/default.jpg", UriKind.Absolute);
				case YouTubeUri.YouTubeThumbnailSize.Medium:
					return new Uri($"http://img.youtube.com/vi/{youTubeId}/hqdefault.jpg", UriKind.Absolute);
				case YouTubeUri.YouTubeThumbnailSize.Large:
					return new Uri($"http://img.youtube.com/vi/{youTubeId}/maxresdefault.jpg", UriKind.Absolute);
			}
			throw new ArgumentException("The value of 'size' is not defined.");
		}
	}
}