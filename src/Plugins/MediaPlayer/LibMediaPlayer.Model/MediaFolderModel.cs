using System;

namespace Bau.Libraries.LibMediaPlayer.Model
{
	/// <summary>
	///		Clase con los datos de una carpeta
	/// </summary>
	public class MediaFolderModel : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		Obtiene la carpeta principal
		/// </summary>
		public MediaFolderModel GetRoot()
		{
			if (Parent == null)
				return this;
			else
				return Parent.GetRoot();
		}

		/// <summary>
		///		Obtiene recursivamente los albums
		/// </summary>
		public MediaAlbumModelCollection GetAlbumsRecursive()
		{
			var albums = new MediaAlbumModelCollection();

				// Añade los albums de la carpeta
				foreach (MediaAlbumModel album in albums)
					albums.Add(album);
				// Añade recursivamente los albums de las carpetas
				foreach (MediaFolderModel folder in Folders)
					albums.AddRange(folder.GetAlbumsRecursive());
				// Devuelve la colección de albums
				return albums;
		}

		/// <summary>
		///		Obtiene el álbum que se corresponde con un nombre
		/// </summary>
		public MediaAlbumModel SearchAlbumByName(string name)
		{
			return GetAlbumsRecursive().SearchByName(name);
		}

		/// <summary>
		///		Obtiene el número de elementos no escuchados
		/// </summary>
		public int GetNumberNotPlayed()
		{ 
			return Albums.GetNumberNotPlayed() + Folders.GetNumberNotPlayed();
		}

		/// <summary>
		///		Borra una carpeta
		/// </summary>
		public void Delete(MediaFolderModel folder)
		{
			if (Folders.Search(folder.GlobalId) != null)
				Folders.RemoveByID(folder.GlobalId);
			else
				foreach (MediaFolderModel childFolder in Folders)
					childFolder.Delete(folder);
		}

		/// <summary>
		///		Borra un album
		/// </summary>
		public void Delete(MediaAlbumModel album)
		{
			if (Albums.Search(album.GlobalId) != null)
				Albums.RemoveByID(album.GlobalId);
			else
				foreach (MediaFolderModel childFolder in Folders)
					childFolder.Delete(album);
		}

		/// <summary>
		///		Borra un archivo
		/// </summary>
		public bool Delete(MediaFileModel file)
		{
			bool deleted = Albums.Delete(file);

				// Borra la entrada de las carpetas
				if (!deleted)
					deleted = Folders.Delete(file);
				// Devuelve el valor que indica si se ha borrado
				return deleted;
		}

		/// <summary>
		///		Carpeta padre
		/// </summary>
		public MediaFolderModel Parent { get; set; }

		/// <summary>
		///		Carpetas contenidas en esta carpeta
		/// </summary>
		public MediaFolderModelCollection Folders { get; } = new MediaFolderModelCollection();

		/// <summary>
		///		albums de esta carpeta
		/// </summary>
		public MediaAlbumModelCollection Albums { get; } = new MediaAlbumModelCollection();

		/// <summary>
		///		Nombre completo de la carpeta
		/// </summary>
		public string FullName
		{
			get
			{
				string fullName = Name;

					// Recoge el nombre de la carpeta padre
					if (Parent != null && !string.IsNullOrWhiteSpace(Parent.Name))
						fullName = Parent.FullName + "\\" + fullName;
					// Devuelve el nombre completo
					return fullName;
			}
		}
	}
}
