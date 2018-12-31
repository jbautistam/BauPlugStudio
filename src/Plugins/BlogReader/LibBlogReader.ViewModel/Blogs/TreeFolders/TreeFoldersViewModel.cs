using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.Trees;
using Bau.Libraries.LibBlogReader.Model;
using Bau.Libraries.LibBlogReader.ViewModel.Blogs.TreeBlogs;

namespace Bau.Libraries.LibBlogReader.ViewModel.Blogs.TreeFolders
{
	/// <summary>
	///		ViewModel para un árbol de carpetas
	/// </summary>
	public class TreeFoldersViewModel : TreeViewModel<FolderNodeViewModel>
	{ 
		// Variables privadas
		private bool _isTreeUpdated;

		public TreeFoldersViewModel(BauMvvm.ViewModels.BaseObservableObject viewModelParent)
		{
			ViewModelParent = viewModelParent;
		}

		/// <summary>
		///		Carga los nodos del árbol
		/// </summary>
		protected override void LoadNodesData()
		{
			// Carga los nodos de carpetas
			LoadNodes(null, BlogReaderViewModel.Instance.BlogManager.File.Folders);
			// Indica que no ha habido modificaciones
			if (ViewModelParent != null)
				ViewModelParent.IsUpdated = false;
		}

		/// <summary>
		///		Carga recursivamente los nodos hijos de un árbol de carpetas
		/// </summary>
		private void LoadNodes(FolderNodeViewModel parent, FoldersModelCollection folders)
		{
			foreach (FolderModel folder in folders)
			{
				FolderNodeViewModel node = new FolderNodeViewModel(parent, folder, false);

					// Asigna las propiedades al nodo
					node.IsExpanded = true;
					// Lo añade al árbol
					if (parent == null)
						Children.Add(node);
					else
						parent.Children.Add(node);
					// Añade el manejador de eventos
					if (node != null)
						node.PropertyChanged += (sender, evntArgs) =>
														{
															if (evntArgs.PropertyName.EqualsIgnoreCase(nameof(FolderNodeViewModel.IsSelected)) ||
																	evntArgs.PropertyName.EqualsIgnoreCase(nameof(FolderNodeViewModel.IsChecked)))
																IsTreeeUpdated = true;
														};
					// Carga los nodos hijo
					if (folder.Folders != null && folder.Folders.Count > 0)
						LoadNodes(node, folder.Folders);
			}
		}

		/// <summary>
		///		ViewModel padre
		/// </summary>
		public BauMvvm.ViewModels.BaseObservableObject ViewModelParent { get; }

		/// <summary>
		///		Indica si se ha modificado el árbol
		/// </summary>
		public bool IsTreeeUpdated
		{
			get { return _isTreeUpdated; }
			set { CheckProperty(ref _isTreeUpdated, value); }
		}
	}
}
