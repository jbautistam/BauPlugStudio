using System;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibBlogReader.Model
{
	/// <summary>
	///		Colección de <see cref="FolderModel"/>
	/// </summary>
	public class FoldersModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<FolderModel>
	{
		/// <summary>
		///		Añade una carpeta
		/// </summary>
		public FolderModel Add(string name)
		{
			FolderModel folder = new FolderModel { Name = name };

				// Añade la carpeta a la colección
				Add(folder);
				// Devuelve la carpeta
				return folder;
		}

		/// <summary>
		///		Borra una entrada de la carpeta
		/// </summary>
		internal bool Delete(EntryModel entry)
		{
			bool deleted = false;

				// Recorre las carpetas borrando la entrada
				foreach (FolderModel folder in this)
					if (!deleted)
						deleted = folder.Delete(entry);
				// Devuelve el valor que indica si se ha borrado
				return deleted;
		}

		/// <summary>
		///		Obtiene el número de elementos no leídos
		/// </summary>
		internal int GetNumberNotRead()
		{
			int notRead = 0;

				// Acumula el número de elementos no leídos
				foreach (FolderModel folder in this)
					notRead += folder.GetNumberNotRead();
				// Devuelve el número de elementos no leídos
				return notRead;
		}
	}
}
