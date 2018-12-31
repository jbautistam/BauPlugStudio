using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.DevConference.Application.Models
{
	/// <summary>
	///		Clase con los datos de un artículo
	/// </summary>
	public class EntryModel : BaseModel
	{
		/// <summary>
		///		Estado de la entrada
		/// </summary>
		public enum Status
		{
			/// <summary>No leído</summary>
			Unread,
			/// <summary>Borrado</summary>
			Deleted,
			/// <summary>Leído</summary>
			Read,
			/// <summary>Marcado como favorito</summary>
			Starred
		}

		/// <summary>
		///		Clona la entrada
		/// </summary>
		public EntryModel Clone()
		{
			EntryModel target = new EntryModel();

				// Clona las propiedades
				target.Id = Id;
				target.Title = Title;
				target.Summary = Summary;
				target.Authors = Authors;
				target.UrlImage = UrlImage;
				target.UrlVideo = UrlVideo;
				target.UrlWebSite = UrlWebSite;
				target.UrlSlides = UrlSlides;
				target.PublishedAt = PublishedAt;
				target.CreatedAt = CreatedAt;
				// Devuelve el objeto clonado
				return target;
		}

		/// <summary>
		///		Obtiene el código del vídeo de YouTube
		/// </summary>
		public string GetUrlVideoCode()
		{
			string videoCode = null;

				// Obtiene el código de vídeo
				if (!string.IsNullOrWhiteSpace(UrlVideo) && UrlVideo.IndexOf("youtube.com", StringComparison.CurrentCultureIgnoreCase) >= 0)
				{
					string [] urlParts = UrlVideo.Split('?');

						// Obtiene los parámetros
						if (urlParts.Length > 1 && !string.IsNullOrWhiteSpace(urlParts[1]))
						{
							string [] queryStrings = urlParts[1].Split('&');

								foreach (string queryString in queryStrings)
									if (!string.IsNullOrWhiteSpace(queryString))
									{
										string [] queryParts = queryString.Split('=');

											if (queryParts.Length == 2 && queryParts[0].EqualsIgnoreCase("v"))
												videoCode = queryParts[1];
									}
						}
				}
				// Devuelve el código de vídeo
				return videoCode;
		}

		/// <summary>
		///		Resumen de la entrada
		/// </summary>
		public string Summary { get; set; }

		/// <summary>
		///		Autor/es de la entrada
		/// </summary>
		public string Authors { get; set; }

		/// <summary>
		///		Url de la imagen
		/// </summary>
		public string UrlImage { get; set; }

		/// <summary>
		///		Url del vídeo
		/// </summary>
		public string UrlVideo { get; set; }

		/// <summary>
		///		Url del sitio Web
		/// </summary>
		public string UrlWebSite { get; set; }

		/// <summary>
		///		Url de las slides
		/// </summary>
		public string UrlSlides { get; set; }

		/// <summary>
		///		Fecha de publicación
		/// </summary>
		public DateTime PublishedAt { get; set; } = DateTime.Now;

		/// <summary>
		///		Fecha de creación
		/// </summary>
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		/// <summary>
		///		Estado de la entrada
		/// </summary>
		public Status State { get; set; } = Status.Unread;
	}
}
