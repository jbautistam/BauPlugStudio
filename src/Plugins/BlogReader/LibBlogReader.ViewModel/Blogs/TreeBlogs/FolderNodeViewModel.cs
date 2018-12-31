using System;

using Bau.Libraries.LibBlogReader.Model;

namespace Bau.Libraries.LibBlogReader.ViewModel.Blogs.TreeBlogs
{
	/// <summary>
	///		Nodo del árbol <see cref="FolderModel"/>
	/// </summary>
	public class FolderNodeViewModel : BaseNodeViewModel
	{
		public FolderNodeViewModel(FolderNodeViewModel parent, FolderModel folder, bool lazyLoad = true) : base(parent, folder.FullName, folder.Name, lazyLoad)
		{
			Folder = folder;
			Foreground = BauMvvm.ViewModels.Media.MvvmColor.Navy;
		}

		/// <summary>
		///		Carga los hijos del nodo
		/// </summary>
		public override void LoadChildrenData()
		{ 
			// Carga las carpetas hija
			Folder.Folders.SortByName();
			foreach (FolderModel folder in Folder.Folders)
				Children.Add(new FolderNodeViewModel(this, folder));
			// Carga los blog hijos
			Folder.Blogs.SortByName();
			foreach (BlogModel blog in Folder.Blogs)
				Children.Add(new BlogNodeViewModel(this, blog));
		}

		/// <summary>
		///		Obtiene el número de elementos no leídos
		/// </summary>
		public override int GetNumberNotRead()
		{
			return Folder.NumberNotRead;
		}

		/// <summary>
		///		Obtiene los blogs asociados
		/// </summary>
		public override BlogsModelCollection GetBlogs()
		{
			BlogsModelCollection blogs = new BlogsModelCollection();

				// Obtiene los blogs de la carpeta
				if (Folder != null)
					blogs = Folder.GetBlogsRecursive();
				// Devuelve la colección de blogs
				return blogs;
		}

		/// <summary>
		///		Carpeta
		/// </summary>
		public FolderModel Folder { get; }
	}
}
