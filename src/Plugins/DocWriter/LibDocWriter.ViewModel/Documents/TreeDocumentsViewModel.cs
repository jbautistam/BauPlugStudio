using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.MVVM.ViewModels.TreeItems;
using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Documents
{
	/// <summary>
	///		ViewModel para un árbol de documentos
	/// </summary>
	public class TreeDocumentsViewModel : TreeViewModel
	{   
		// Variables privadas
		private TreeViewItemsViewModelCollection _nodes;
		private FileModel.DocumentType _documentType;
		private bool _treeUpdated;

		public TreeDocumentsViewModel(BauMvvm.ViewModels.BaseObservableObject viewModelParent, FileModel.DocumentType documentType,
									  FileModel file, FilesModelCollection files)
		{
			ViewModelParent = viewModelParent;
			DocumentType = documentType;
			File = file;
			Files = files;
		}

		/// <summary>
		///		Carga los nodos del árbol
		/// </summary>
		protected override TreeViewItemsViewModelCollection LoadNodes()
		{
			FilesModelCollection files = new Application.Bussiness.Solutions.FileBussiness().Load(File.Project);
			TreeViewItemsViewModelCollection nodes = new TreeViewItemsViewModelCollection();

				// Carga los nodos
				LoadNodes(nodes, null, files);
				// Indica que no ha habido modificaciones
				if (ViewModelParent != null)
					ViewModelParent.IsUpdated = false;
				// Devuelve la colección de nodos
				return nodes;
		}

		/// <summary>
		///		Carga los nodos hijos de un árbol
		/// </summary>
		private void LoadNodes(TreeViewItemsViewModelCollection nodes, TreeViewItemViewModel nodeParent, FilesModelCollection files)
		{
			foreach (FileModel file in files)
			{
				TreeViewItemViewModel node;

					// Si es un archivo del tipo seleccionado, se añade al árbol, si no, se considera el nodo igual que el padre
					// y se siguen añadiendo hijos
					if (DocumentType == FileModel.DocumentType.Unknown || DocumentType == FileModel.DocumentType.File || DocumentType == file.FileType ||
						IsReference(file, DocumentType))
					{ 
						// Crea un nuevo nodo
						node = new TreeViewItemViewModel(file.FullFileName, file.Title, nodeParent, false, file);
						node.IsExpanded = true;
						// Selecciona el nodo
						if (Files != null)
							node.IsChecked = Files.ExistsByIDFileName(file.IDFileName);
						// Lo añade al árbol
						if (nodeParent == null)
							nodes.Add(node);
						else
							nodeParent.Children.Add(node);
					}
					else
						node = nodeParent;
					// Añade el manejador de eventos
					if (node != null)
						node.PropertyChanged += (sender, evntArgs) =>
														{
															if (evntArgs.PropertyName.EqualsIgnoreCase(nameof(TreeViewItemViewModel.IsChecked)) || 
																	evntArgs.PropertyName.EqualsIgnoreCase(nameof(TreeViewItemViewModel.IsSelected)))
																IsTreeeUpdated = true;
														};
					// Añade los nodos hijo
					if (file.IsFolder)
						LoadNodes(nodes, node, new Application.Bussiness.Solutions.FileBussiness().Load(file));
			}
		}

		/// <summary>
		///		Comprueba si un archivo hace referencia al tipo de documento buscado
		/// </summary>
		private bool IsReference(FileModel file, FileModel.DocumentType documentType)
		{
			bool isReference = false;

				// Comprueba si es una referencia
				if (file.FileType == FileModel.DocumentType.Reference)
				{
					ReferenceModel reference = new Application.Bussiness.Documents.ReferenceBussiness().Load(file);
					FileModel fileReferenced = new Application.Bussiness.Solutions.FileFactory().GetInstance(file.Project, reference.FileNameReference);

						// Indica si el archivo referenciado es del tipo buscado
						isReference = fileReferenced.FileType == documentType;
				}
				// Devuelve el valor que indica si el archivo es una referencia al tipo buscado
				return isReference;
		}

		/// <summary>
		///		Obtiene los archivos seleccionados
		/// </summary>
		public FilesModelCollection GetIsCheckedFiles()
		{
			FilesModelCollection files = new FilesModelCollection(File.Project);

				// Añade los nodos seleccionados
				files.AddRange(GetFilesIsChecked(Nodes));
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Obtiene los archivos seleccionados
		/// </summary>
		private FilesModelCollection GetFilesIsChecked(TreeViewItemsViewModelCollection nodes)
		{
			FilesModelCollection files = new FilesModelCollection(File.Project);

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
		///		Tipo de documento
		/// </summary>
		public FileModel.DocumentType DocumentType
		{
			get { return _documentType; }
			set { CheckProperty(ref _documentType, value); }
		}

		/// <summary>
		///		ViewModel padre
		/// </summary>
		public BauMvvm.ViewModels.BaseObservableObject ViewModelParent { get; }

		/// <summary>
		///		Archivo donde se encuentra el documento
		/// </summary>
		public FileModel File { get; }

		/// <summary>
		///		Lista de archivos que se asocia el árbol
		/// </summary>
		public FilesModelCollection Files { get; }

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
			get { return _treeUpdated; }
			set { CheckProperty(ref _treeUpdated, value); }
		}
	}
}
