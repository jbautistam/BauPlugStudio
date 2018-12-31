using System;

namespace Bau.Libraries.DevConference.Application.Models
{
	/// <summary>
	///		Clase con los datos de un canal
	/// </summary>
	public class TrackModel : BaseModel
	{
		/// <summary>
		///		Clona los datos
		/// </summary>
		public TrackModel Clone()
		{
			TrackModel target = new TrackModel();

				// Clona los datos
				target.Id = Id;
				target.Title = Title;
				target.Description = Description;
				target.Url = Url;
				target.Enabled = Enabled;
				// Clona las categorías
				target.Categories.AddRange(Categories.Clone(target));
				// Devuelve el objeto clonado
				return target;
		}

		/// <summary>
		///		Cuenta el número de elementos no leídos
		/// </summary>
		public int CountUnread()
		{
			int number = 0;

				// Cuenta los elementos no leídos
				foreach (CategoryModel category in Categories)
					number += category.CountUnread();
				// Devuelve el número de elementos no leídos
				return number;
		}

		/// <summary>
		///		Descripción del canal
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		///		Url del canal
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		///		Indica si el canal está activo
		/// </summary>
		public bool Enabled { get; set; } = true;

		/// <summary>
		///		Indica si se han cargado las entradas
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		///		Fecha de última descarga
		/// </summary>
		public DateTime DateLastDownload { get; set; } = DateTime.Now.AddDays(-2);

		/// <summary>
		///		Fecha de último error
		/// </summary>
		public DateTime? DateLastError { get; set; } = null;

		/// <summary>
		///		Nombre del archivo local
		/// </summary>
		public string LocalFileName
		{
			get
			{
				string localFileName = Url;

					// Asigna el nombre de archivo local si no existía
					if (!string.IsNullOrEmpty(localFileName))
					{ 
						// Quita los caracteres raros de la URL
						localFileName = localFileName.Replace(':', '_');
						localFileName = localFileName.Replace('\\', '_');
						localFileName = localFileName.Replace('/', '_');
						localFileName = localFileName.Replace('%', '_');
						localFileName = localFileName.Replace('?', '_');
						// Devuelve el nombre del archivo local
						if (!Url.EndsWith(".xml", StringComparison.CurrentCultureIgnoreCase))
							localFileName += ".xml";
					}
					// Devuelve el nombre del archivo local
					return localFileName;
			}
		}

		/// <summary>
		///		Categorías
		/// </summary>
		public CategoryModelCollection Categories { get; } = new CategoryModelCollection();
	}
}
