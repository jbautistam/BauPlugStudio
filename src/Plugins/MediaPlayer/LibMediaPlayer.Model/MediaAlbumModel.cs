using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibMediaPlayer.Model
{
	/// <summary>
	///		Clase con los datos de un album
	/// </summary>
	public class MediaAlbumModel : LibDataStructures.Base.BaseExtendedModel
	{ 
		// Variables privadas
		private string path;

		/// <summary>
		///		Obtiene el número de elementos no leídos
		/// </summary>
		public int GetNumberNotRead()
		{ 
			return Files.GetNumberNotPlayed();
		}

		/// <summary>
		///		Borra una entrada
		/// </summary>
		internal bool Delete(MediaFileModel entry)
		{
			return Files.Delete(entry);
		}

		/// <summary>
		///		Directorio para grabación de las entradas
		/// </summary>
		public string Path
		{
			get
			{
				// Obtiene el nombre de archivo
				if (path.IsEmpty())
					path = LibCommonHelper.Files.HelperFiles.Normalize(Name, false);
				// Devuelve el nombre de archivo
				return path;
			}
			set { path = value; }
		}

		/// <summary>
		///		Carpeta a la que pertenece el album
		/// </summary>
		public MediaFolderModel Folder { get; set; }

		/// <summary>
		///		Archivos del album
		/// </summary>
		public MediaFileModelCollection Files { get; } = new MediaFileModelCollection();
	}
}
