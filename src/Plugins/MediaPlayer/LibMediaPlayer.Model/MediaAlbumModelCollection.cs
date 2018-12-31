using System;

namespace Bau.Libraries.LibMediaPlayer.Model
{
	/// <summary>
	///		Colección de <see cref="MediaAlbumModel"/>
	/// </summary>
	public class MediaAlbumModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<MediaAlbumModel>
	{
		/// <summary>
		///		Añade un album a la colección
		/// </summary>
		public MediaAlbumModel Add(string name, string description)
		{
			MediaAlbumModel album = new MediaAlbumModel { Name = name, Description = description };

				// Añade el album a la colección
				Add(album);
				// Devuelve el album
				return album;
		}

		/// <summary>
		///		Obtiene el número de elementos no escuchados
		/// </summary>
		public int GetNumberNotPlayed()
		{
			int number = 0;

				// Obtiene el número de elementos no escuchados
				foreach (MediaAlbumModel album in this)
					number += album.GetNumberNotRead();
				// Devuelve el número de elementos no escuchados
				return number;
		}

		/// <summary>
		///		Borra un archivo
		/// </summary>
		internal bool Delete(MediaFileModel file)
		{
			bool deleted = false;

				// Borra la entrada de los albums
				foreach (MediaAlbumModel album in this)
					if (!deleted)
						deleted = album.Delete(file);
				// Devuelve el valor que indica si se ha borrado
				return deleted;
		}
	}
}
