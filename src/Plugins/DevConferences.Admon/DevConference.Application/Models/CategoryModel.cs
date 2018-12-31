using System;

namespace Bau.Libraries.DevConference.Application.Models
{
	/// <summary>
	///		Clase con los datos de una categoría
	/// </summary>
	public class CategoryModel : BaseModel
	{   
		public CategoryModel(TrackModel track)
		{
			Track = track;
		}

		/// <summary>
		///		Clona los datos
		/// </summary>
		public CategoryModel Clone(TrackModel parent)
		{
			CategoryModel target = new CategoryModel(parent);

				// Clona las propiedades
				target.Id = Id;
				target.Title = Title;
				target.IsLoaded = IsLoaded;
				// Clona las entradas
				target.Entries.AddRange(Entries.Clone());
				// Devuelve el dato clonado
				return target;
		}

		/// <summary>
		///		Cuenta el número de elementos no leídos
		/// </summary>
		public int CountUnread()
		{
			int number = 0;

				// Cuenta el número de elementos
				if (!IsDeleted)
					foreach (EntryModel entry in Entries)
						if (entry.State == EntryModel.Status.Unread)
							number++;
				// Devuelve el número de elementos
				return number;
		}

		/// <summary>
		///		Canal al que se asocia la categoría
		/// </summary>
		public TrackModel Track { get; }

		/// <summary>
		///		Indica si la categoría se ha cargado
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		///		Indica si la categoría se ha borrado
		/// </summary>
		public bool IsDeleted { get; set; }

		/// <summary>
		///		Nombre del archivo local
		/// </summary>
		public string LocalFileName
		{
			get
			{
				string localFileName = $"{Title}_{Id}";

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
						localFileName += ".xml";
					}
					// Devuelve el nombre del archivo local
					return localFileName;
			}
		}

		/// <summary>
		///		Entradas
		/// </summary>
		public EntryModelCollection Entries { get; } = new EntryModelCollection();
	}
}
