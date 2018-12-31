using System;

using Bau.Libraries.LibMediaPlayer.Model;
using Bau.Libraries.LibMediaPlayer.Repository;

namespace Bau.Libraries.LibMediaPlayer.Application.Bussiness
{
	/// <summary>
	///		Clase de negocio de <see cref="MediaFolderModel"/>
	/// </summary>
	public class FolderBussiness
	{
		/// <summary>
		///		Carga los datos
		/// </summary>
		public MediaFolderModel Load(string fileName)
		{
			return new MediaRepository().Load(fileName);
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		public void Save(MediaFolderModel folder, string fileName)
		{
			new MediaRepository().Save(folder, fileName);
		}
	}
}
