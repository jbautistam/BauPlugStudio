using System;

namespace Bau.Libraries.LibBlogReader.Model
{
	/// <summary>
	///		Colección de <see cref="BlogModel"/>
	/// </summary>
	public class BlogsModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<BlogModel>
	{
		/// <summary>
		///		Añade un blog a la colección
		/// </summary>
		public BlogModel Add(string name, string description, string url)
		{
			BlogModel blog = new BlogModel { Name = name, Description = description, URL = url };

				// Añade el blog a la colección
				Add(blog);
				// Devuelve el blog
				return blog;
		}

		/// <summary>
		///		Obtiene el número de elementos no leídos
		/// </summary>
		public int GetNumberNotRead()
		{
			int number = 0;

				// Obtiene el número de elementos no leídos
				foreach (BlogModel blog in this)
					number += blog.GetNumberNotRead();
				// Devuelve el número de elementos no leídos
				return number;
		}

		/// <summary>
		///		Borra una entrada
		/// </summary>
		internal bool Delete(EntryModel entry)
		{
			bool deleted = false;

				// Borra la entrada de los blogs
				foreach (BlogModel blog in this)
					if (!deleted)
						deleted = blog.Delete(entry);
				// Devuelve el valor que indica si se ha borrado
				return deleted;
		}
	}
}
