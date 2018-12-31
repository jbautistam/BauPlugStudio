using System;

using Bau.Libraries.LibMediaPlayer.Model;

namespace Bau.Libraries.LibMediaPlayer.Application
{
	/// <summary>
	///		Manager de MediaPlayer
	/// </summary>
	public class MediaPlayerManager
	{   
		public MediaPlayerManager()
		{
			File = new MediaFolderModel();
		}

		/// <summary>
		///		Carga los archivos
		/// </summary>
		public void Load()
		{
			if (!string.IsNullOrEmpty(PathFiles) && System.IO.Directory.Exists(PathFiles))
				File = new Bussiness.FolderBussiness().Load(GetFileNameAlbums());
		}

		/// <summary>
		///		Graba los archivos
		/// </summary>
		public void Save()
		{
			new Bussiness.FolderBussiness().Save(File, GetFileNameAlbums());
		}

		/// <summary>
		///		Carga los archivos de la lista de reproducción
		/// </summary>
		public MediaFolderModel LoadReproductionList()
		{
			if (!string.IsNullOrEmpty(PathFiles) && System.IO.Directory.Exists(PathFiles))
				return new Bussiness.FolderBussiness().Load(GetFileNameReproductionList());
			else
				return new MediaFolderModel();
		}

		/// <summary>
		///		Graba los archivos
		/// </summary>
		public void SaveReproductionList(MediaFolderModel folder)
		{
			new Bussiness.FolderBussiness().Save(folder, GetFileNameReproductionList());
		}

		/// <summary>
		///		Obtiene el nombre de archivo para los albums
		/// </summary>
		private string GetFileNameAlbums()
		{
			return System.IO.Path.Combine(PathFiles, "Albums.xml");
		}

		/// <summary>
		///		Obtiene el nombre de archivo para la lista de reproducción
		/// </summary>
		private string GetFileNameReproductionList()
		{
			return System.IO.Path.Combine(PathFiles, "AlbumsList.xml");
		}

		/// <summary>
		///		Archivo con los elementos multimedia
		/// </summary>
		public MediaFolderModel File { get; private set; }

		/// <summary>
		///		Directorio donde se dejan los datos de los blogs
		/// </summary>
		public string PathFiles { get; set; }
	}
}
