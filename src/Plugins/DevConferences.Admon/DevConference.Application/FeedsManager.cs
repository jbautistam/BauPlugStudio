using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Bau.Libraries.DevConference.Application.Models;

namespace Bau.Libraries.DevConference.Application
{
	/// <summary>
	///		Manager para el tratamiento de Feeds
	/// </summary>
	public class FeedsManager
	{
		/// <summary>
		///		Carga una serie de canales de un texto XML
		/// </summary>
		public TrackModelCollection LoadTracks(string xml)
		{
			//? Carga el XML en un try porque cuando se da de alta un canal a una dirección que no tiene
			//? contenido, el servidor devuelve un HTML que da una excepción de seguridad al intentar
			//? interpretarlo como XML
			try
			{
				return new Repository.TracksRepository().LoadTracks(xml);
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine(exception.Message);
				return new TrackModelCollection();
			}
		}

		/// <summary>
		///		Comprueba si una cadena XML se corresponde a un archivo de canales
		/// </summary>
		public bool ValidateXmlTracks(string xml)
		{
			//? Carga el XML en un try porque cuando se da de alta un canal a una dirección que no tiene
			//? contenido, el servidor devuelve un HTML que da una excepción de seguridad al intentar
			//? interpretarlo como XML
			try
			{
				return new Repository.CategoriesRepository().LoadCategories(new TrackModel(), xml).Count != 0;
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine(exception.Message);
				return false;
			}
		}

		/// <summary>
		///		Obtiene el XML de una colección de canales
		/// </summary>
		public string GetTracksXml(TrackModelCollection tracks)
		{
			return new Repository.TracksRepository().GetXml(tracks);
		}

		/// <summary>
		///		Carga una serie de canales de un texto XML
		/// </summary>
		public TrackModelCollection LoadTracksFromExchangeFile(string xml)
		{
			return new Repository.TrackExchangeRepository().LoadTracks(xml);
		}

		/// <summary>
		///		Obtiene el XML de un archivo intercambio
		/// </summary>
		public string GetXmlExchangeFile(TrackModelCollection tracks)
		{
			return new Repository.TrackExchangeRepository().GetXml(tracks);
		}

		/// <summary>
		///		Carga una serie de entradas de un texto XML
		/// </summary>
		public EntryModelCollection LoadCategoryEntries(string xml)
		{
			//? Carga el XML en un try porque cuando se da de alta un canal a una dirección que no tiene
			//? contenido, el servidor devuelve un HTML que da una excepción de seguridad al intentar
			//? interpretarlo como XML
			try
			{
				return new Repository.EntriesRepository().LoadCategoryEntries(xml);
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine(exception.Message);
				return new EntryModelCollection();
			}
		}

		/// <summary>
		///		Obtiene el XML de las entradas de una categoría
		/// </summary>
		public string GetEntriesXml(CategoryModel category)
		{
			return new Repository.EntriesRepository().GetXml(category);
		}

		/// <summary>
		///		Obtiene el XML de las entradas de un canal
		/// </summary>
		public string GetEntriesXml(TrackModel track)
		{
			return new Repository.CategoriesRepository().GetXml(track);
		}

		/// <summary>
		///		Descarga y mezcla las conferencias de un canal
		/// </summary>
		public async Task<CategoryModelCollection> DownloadConferencesAsync(TrackModel track)
		{
			return await new Repository.CategoriesDownload().DownloadChannelAsync(track);
		}

		/// <summary>
		///		Obtiene la Url de un vídeo de YouTube
		/// </summary>
		public async Task<string> GetUriYouTubeVideoAsync(string videoCode)
		{
			try
			{
				Tools.YouTubeUri uri = (await new Tools.YouTube().GetVideoUriAsync(videoCode, Tools.YouTubeUri.YouTubeQuality.QualityMedium));

					if (uri != null)
						return uri.Uri.ToString();
					else
						return null;
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine(exception.Message);
				return null;
			}
		}

		/// <summary>
		///		Obtiene la Url de una imagen de YouTube
		/// </summary>
		public Uri GetUriThumbnailYouTube(string videoCode)
		{
			try
			{
				return new Tools.YouTube().GetThumbnailUri(videoCode);
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine(exception.Message);
				return null;
			}
		}
	}
}
