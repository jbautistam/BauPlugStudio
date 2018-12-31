using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.Trees;
using Bau.Libraries.LibBlogReader.Model;

namespace Bau.Libraries.LibBlogReader.ViewModel.Blogs.TreeBlogs
{
	/// <summary>
	///		ViewModel para el árbol de blogs
	/// </summary>
	public class TreeBlogsViewModel : TreeViewModel<BaseNodeViewModel>
	{
		public TreeBlogsViewModel(FolderModel folder)
		{
			Folder = folder;
		}

		/// <summary>
		///		Carga los nodos
		/// </summary>
		protected override void LoadNodesData()
		{
			// Ordena las carpetas
			Folder.Folders.SortByName();
			// Añade las carpetas
			foreach (FolderModel folder in Folder.Folders)
				Children.Add(new FolderNodeViewModel(null, folder));
			// Ordena los blogs
			Folder.Blogs.SortByName();
			// Añade los blogs
			foreach (BlogModel blog in Folder.Blogs)
				Children.Add(new BlogNodeViewModel(null, blog));
		}

		/// <summary>
		///		Chequea un nodo
		/// </summary>
		public void CheckNode(int? id)
		{
			CheckNode(ConvertNodes(), id);
		}

		/// <summary>
		///		Chequea un nodo de los existentes
		/// </summary>
		private void CheckNode(List<BaseNodeViewModel> nodes, int? id)
		{
			foreach (BaseNodeViewModel node in nodes)
				if (node is BlogNodeViewModel blogNode)
				{
					if (blogNode.Blog.ID == id)
						blogNode.IsChecked = true;
				}
				else
				{
					node.IsExpanded = true;
					CheckNode(ConvertNodes(node.Children), id);
				}
		}

		/// <summary>
		///		Obtiene los nodos chequeados
		/// </summary>
		public List<int> GetCheckedNodes()
		{
			return GetCheckedNodes(ConvertNodes(Children));
		}

		/// <summary>
		///		Obtiene los nodos chequeados
		/// </summary>
		private List<int> GetCheckedNodes(List<BaseNodeViewModel> nodes)
		{
			List<int> ids = new List<int>();

				// Obtiene los nodos
				foreach (IHierarchicalViewModel node in nodes)
					if (node is BlogNodeViewModel && node.IsChecked)
						ids.Add((node as BlogNodeViewModel).Blog.ID ?? 0);
					else
						ids.AddRange(GetCheckedNodes(ConvertNodes(node.Children)));
				// Devuelve la colección de IDs
				return ids;
		}

		/// <summary>
		///		Indica si está seleccionada una carpeta
		/// </summary>
		public bool IsSelectedFolder
		{
			get { return SelectedNode != null && SelectedNode is FolderNodeViewModel; }
		}

		/// <summary>
		///		Carpeta inicial del árbol
		/// </summary>
		private FolderModel Folder { get; }
	}
}
