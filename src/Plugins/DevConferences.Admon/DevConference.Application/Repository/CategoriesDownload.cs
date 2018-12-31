using System;
using System.Threading.Tasks;

using Bau.Libraries.DevConference.Application.Models;

namespace Bau.Libraries.DevConference.Application.Repository
{
	/// <summary>
	///		Proceso para descarga de las categorías
	/// </summary>
	internal class CategoriesDownload
	{
		/// <summary>
		///		Descarga las entradas de un canal
		/// </summary>
		internal async Task<CategoryModelCollection> DownloadChannelAsync(TrackModel track)
		{
			CategoryModelCollection categories = new CategoryModelCollection();
			string content = await DownloadAsync(track.Url);

				// Interpreta la cadena descargada
				if (!string.IsNullOrWhiteSpace(content))
				{
					// Interpreta las categorías
					categories = new CategoriesRepository().LoadCategories(track, content);
					// Modifica las direcciones de imagen y los vídeos
					UpdateUrls(categories);
				}
				// Devuelve la colección de entradas
				return categories;
		}

		/// <summary>
		///		Descarga el contenido de una URL
		/// </summary>
		private async Task<string> DownloadAsync(string url)
		{
			return await new LibCommonHelper.Communications.HttpWebClient().HttpGetAsync(url);
		}

		/// <summary>
		///		Modifica las Urls de vídeo e imagen de las categorías
		/// </summary>
		private void UpdateUrls(CategoryModelCollection categories)
		{
			FeedsManager manager = new FeedsManager();

				// Modifica las Url de las entradas de las categorías
				foreach (CategoryModel category in categories)
					foreach (EntryModel entry in category.Entries)
					{
						string videoCode = entry.GetUrlVideoCode();

							if (!string.IsNullOrWhiteSpace(videoCode))
							{
								// Modifica la Url de la imagen
								if (string.IsNullOrWhiteSpace(entry.UrlImage))
									entry.UrlImage = manager.GetUriThumbnailYouTube(videoCode)?.ToString();
							}
					}
		}
	}
}
