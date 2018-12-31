using System;

namespace Bau.Libraries.LibMediaPlayer.Model
{
	/// <summary>
	///		Colección de <see cref="MediaFolderModel"/>
	/// </summary>
	public class MediaFolderModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<MediaFolderModel>
	{
		/// <summary>
		///		Añade una carpeta
		/// </summary>
		public MediaFolderModel Add(string name)
		{
			var folder = new MediaFolderModel { Name = name };

				// Añade la carpeta a la colección
				Add(folder);
				// Devuelve la carpeta
				return folder;
		}

		/// <summary>
		///		Borra un archivo de la carpeta
		/// </summary>
		internal bool Delete(MediaFileModel file)
		{
			bool deleted = false;

				// Recorre las carpetas borrando la entrada
				foreach (MediaFolderModel folder in this)
					if (!deleted)
						deleted = folder.Delete(file);
				// Devuelve el valor que indica si se ha borrado
				return deleted;
		}

		/// <summary>
		///		Obtiene el número de elementos no escuchados
		/// </summary>
		internal int GetNumberNotPlayed()
		{
			int notRead = 0;

				// Acumula el número de elementos no leídos
				foreach (MediaFolderModel folder in this)
					notRead += folder.GetNumberNotPlayed();
				// Devuelve el número de elementos no leídos
				return notRead;
		}
	}
}
