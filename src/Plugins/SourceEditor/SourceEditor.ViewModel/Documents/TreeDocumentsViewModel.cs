using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.MVVM.ViewModels.TreeItems;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.ViewModel.Documents
{
	/// <summary>
	///		ViewModel para un árbol de documentos
	/// </summary>
	public class TreeDocumentsViewModel : TreeViewModel
	{
		// Variables privadas
		private TreeViewItemsViewModelCollection _nodes;
		private bool _isTreeUpdated;

		public TreeDocumentsViewModel(FileModel file, FileModelCollection files)
		{
			File = file;
			FilesSelected = files;
		}

		/// <summary>
		///		Carga los nodos del árbol
		/// </summary>
		protected override TreeViewItemsViewModelCollection LoadNodes()
		{
			TreeViewItemsViewModelCollection nodes = new TreeViewItemsViewModelCollection();

				// Carga los nodos
				LoadNodes(nodes, null, File.Files);
				// Devuelve la colección de nodos
				return nodes;
		}

		/// <summary>
		///		Carga los nodos hijos de un árbol
		/// </summary>
		private void LoadNodes(TreeViewItemsViewModelCollection nodes, TreeViewItemViewModel nodeParent, FileModelCollection files)
		{
			foreach (FileModel file in files)
			{
				TreeViewItemViewModel node;

					// Crea un nuevo nodo
					node = new TreeViewItemViewModel(file.FullFileName, file.Name, nodeParent, false, file);
					node.IsExpanded = true;
					// Selecciona el nodo
					if (files != null)
						node.IsChecked = FilesSelected.ExistsByFileName(file.FullFileName);
					// Lo añade al árbol
					if (nodeParent == null)
						nodes.Add(node);
					else
						nodeParent.Children.Add(node);
					// Añade el manejador de eventos
					if (node != null)
						node.PropertyChanged += (sender, evntArgs) =>
															{
																if (evntArgs.PropertyName.EqualsIgnoreCase("IsChecked") ||
																		evntArgs.PropertyName.EqualsIgnoreCase("IsSelected"))
																	IsTreeeUpdated = true;
															};
					// Añade los nodos hijo
					if (file.IsFolder)
						LoadNodes(nodes, node, file.Files);
			}
		}

		/// <summary>
		///		Obtiene los archivos seleccionados
		/// </summary>
		public FileModelCollection GetisCheckedFiles()
		{
			FileModelCollection files = new FileModelCollection();

				// Añade los nodos seleccionados
				files.AddRange(GetFilesIsChecked(Nodes));
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Obtiene los archivos seleccionados
		/// </summary>
		private FileModelCollection GetFilesIsChecked(TreeViewItemsViewModelCollection nodes)
		{
			FileModelCollection files = new FileModelCollection();

				// Busca los nodos seleccionados
				if (nodes != null)
					foreach (TreeViewItemViewModel node in nodes)
					{
						// Añade el archivo si está seleccionado
						if (node.IsChecked && (node.Tag is FileModel))
							files.Add(node.Tag as FileModel);
						// Añade los archivos hijo
						files.AddRange(GetFilesIsChecked(node.Children));
					}
				// Devuelve los archivos
				return files;
		}

		/// <summary>
		///		Ejecuta una acción: no hace nada simplemente implementa la interface
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			return false;
		}

		/// <summary>
		///		Archivo donde se encuentra el documento
		/// </summary>
		public FileModel File { get; }

		/// <summary>
		///		Archivos seleccionados en el árbol
		/// </summary>
		public FileModelCollection FilesSelected { get; }

		/// <summary>
		///		Nodos del árbol
		/// </summary>
		public override TreeViewItemsViewModelCollection Nodes
		{
			get
			{
				// Carga los nodos si no están en memoria
				if (_nodes == null)
					_nodes = LoadNodes();
				// Devuelve la colección de nodos
				return _nodes;
			}
			set { CheckObject(ref _nodes, value); }
		}

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
