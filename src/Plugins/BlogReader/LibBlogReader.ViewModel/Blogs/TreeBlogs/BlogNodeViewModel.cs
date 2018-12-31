using System;

using Bau.Libraries.LibBlogReader.Model;

namespace Bau.Libraries.LibBlogReader.ViewModel.Blogs.TreeBlogs
{
	/// <summary>
	///		Nodo del árbol <see cref="BlogModel"/>
	/// </summary>
	public class BlogNodeViewModel : BaseNodeViewModel
	{
		public BlogNodeViewModel(FolderNodeViewModel parent, BlogModel blog) : base(parent, blog.ID.ToString(), blog.Name, false)
		{
			Blog = blog;
			Foreground = BauMvvm.ViewModels.Media.MvvmColor.Black;
		}

		/// <summary>
		///		Modifica el elemento seleccionado
		/// </summary>
		internal void UpdateItem()
		{
			BlogReaderViewModel.Instance.ViewsController.OpenUpdateBlogView(new BlogViewModel(null, Blog));
		}

		/// <summary>
		///		Obtiene el número de elementos no leídos
		/// </summary>
		public override int GetNumberNotRead()
		{
			return Blog.NumberNotRead;
		}

		/// <summary>
		///		Obtiene los blogs asociados
		/// </summary>
		public override BlogsModelCollection GetBlogs()
		{
			BlogsModelCollection blogs = new BlogsModelCollection();

				// Añade el blog a la colección
				if (Blog != null)
					blogs.Add(Blog);
				// Devuelve la colección de blogs
				return blogs;
		}

		/// <summary>
		///		Blog
		/// </summary>
		public BlogModel Blog { get; }
	}
}
