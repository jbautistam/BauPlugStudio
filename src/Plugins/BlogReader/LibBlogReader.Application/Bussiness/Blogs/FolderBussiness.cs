using System;

using Bau.Libraries.LibBlogReader.Model;
using Bau.Libraries.LibBlogReader.Repository;

namespace Bau.Libraries.LibBlogReader.Application.Bussiness.Blogs
{
	/// <summary>
	///		Clase de negocio de <see cref="FolderModel"/>
	/// </summary>
	public class FolderBussiness
	{
		/// <summary>
		///		Carga las carpetas y blogs
		/// </summary>
		public FolderModel Load(string path)
		{
			return new BlogsStructureRepository().Load(path);
		}

		/// <summary>
		///		Graba las carpetas y blogs
		/// </summary>
		public void Save(FolderModel folder, string path)
		{
			new BlogsStructureRepository().Save(folder, path);
		}

		/// <summary>
		///		Modifica el número de elementos no leídos de la carpeta
		/// </summary>
		public void UpdateNumberNotRead(FolderModel folder)
		{
			folder.NumberNotRead = folder.GetNumberNotRead();
		}
	}
}
