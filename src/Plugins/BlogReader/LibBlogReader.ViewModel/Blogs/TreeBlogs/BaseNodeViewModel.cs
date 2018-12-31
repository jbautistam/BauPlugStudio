using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.LibBlogReader.Model;

namespace Bau.Libraries.LibBlogReader.ViewModel.Blogs.TreeBlogs
{
	/// <summary>
	///		Clase base para los nodos del árbol <see cref="PaneTreeBlogsViewModel"/>
	/// </summary>
	public abstract class BaseNodeViewModel : ControlHierarchicalViewModel
	{
		public BaseNodeViewModel(ControlHierarchicalViewModel parent, string nodeID, string text, bool lazyLoadChildren = true)
								: base(parent, text, parent, lazyLoadChildren)
		{
			NodeId = nodeID;
		}

		/// <summary>
		///		Número de elementos no leídos
		/// </summary>
		public abstract int GetNumberNotRead();

		/// <summary>
		///		Blogs asociados al nodo
		/// </summary>
		public abstract BlogsModelCollection GetBlogs();

		/// <summary>
		///		Clave del nodo
		/// </summary>
		public string NodeId { get; }

		/// <summary>
		///		Número de elementos no leídos
		/// </summary>
		public int NumberNotRead
		{
			get { return GetNumberNotRead(); }
		}

		/// <summary>
		///		Número de elementos no leídos transformado en una cadena
		/// </summary>
		public string NumberNotReadToString
		{
			get
			{
				int numberNotRead = NumberNotRead;

					// Indica si está en negrita
					IsBold = numberNotRead != 0;
					// Formatea el número de elementos no leídos
					if (numberNotRead == 0)
						return "";
					else
						return $" ({numberNotRead})";
			}
		}
	}
}
