using System;

using Bau.Libraries.LibBlogReader.Model;

namespace Bau.Libraries.LibBlogReader.Application.Bussiness.Blogs
{
	/// <summary>
	///		Clase de negocio de <see cref="EntryModel"/>
	/// </summary>
	public class EntryBussiness
	{
		/// <summary>
		///		Graba las entradas de un blog
		/// </summary>
		public void Save(BlogModel blog, string path)
		{
			new Repository.EntryXmlRepository().Save(blog, path);
		}
	}
}
