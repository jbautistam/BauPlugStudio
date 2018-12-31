using System;

using Bau.Libraries.LibBlogReader.Model;

namespace Bau.Libraries.LibBlogReader.Application.Bussiness.Blogs
{
	/// <summary>
	///		Clase de negocio de <see cref="BlogModel"/>
	/// </summary>
	public class BlogBussiness
	{
		/// <summary>
		///		Modifica el número de elementos no leídos
		/// </summary>
		public void UpdateNumberNotRead(BlogsModelCollection blogs)
		{
			foreach (BlogModel blog in blogs)
				UpdateNumberNotRead(blog);
		}

		/// <summary>
		///		Modifica el número de elementos no leídos
		/// </summary>
		public void UpdateNumberNotRead(BlogModel blog)
		{
			int numberNotRead = blog.GetNumberNotRead();

				// Modifica el número de elementos no leídos del blog
				if (numberNotRead != blog.NumberNotRead)
				{ 
					// Modifica el número de elementos no leídos
					blog.NumberNotRead = numberNotRead;
					// Modifica el número de elementos no leídos de la carpeta
					if (blog.Folder != null)
						new FolderBussiness().UpdateNumberNotRead(blog.Folder);
				}
		}
	}
}
