using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.BauMvvm.ViewModels.Media;
using Bau.Libraries.PlugStudioProjects.Models;

namespace Bau.Libraries.PlugStudioProjects.ViewModels
{
	/// <summary>
	///		ViewModel para el explorador de proyectos
	/// </summary>
	public abstract class ExplorerProjectViewModel : BauMvvm.ViewModels.Forms.BasePaneViewModel
	{
		// Variables privadas
		private ProjectItemDefinitionModel _projectDefinition;
		private string _projectPath;
		private bool _isProjectLoaded;
		private ObservableCollection<ExplorerProjectNodeViewModel> _children;
		private ExplorerProjectNodeViewModel _selectedNode;

		public ExplorerProjectViewModel(Controllers.IPlugStudioController plugStudioController)
		{
			// Inicializa los datos
			PlugStudioController = plugStudioController;
			Children = new ObservableCollection<ExplorerProjectNodeViewModel>();
			// Inicializa los comandos
			NewFolderCommand = new BaseCommand(parameter => NewFolder(),
											   parameter => CanExecuteAction(nameof(NewFolderCommand), parameter))
										.AddListener(this, nameof(SelectedNode));
			NewFileCommand = new BaseCommand(parameter => NewFile(),
											 parameter => CanExecuteAction(nameof(NewFileCommand), parameter))
										.AddListener(this, nameof(SelectedNode));
			CopyCommand = new BaseCommand(parameter => CopySelectedItem(),
										  parameter => CanExecuteAction(nameof(CopyCommand), parameter))
										.AddListener(this, nameof(SelectedNode));
			PasteCommand = new BaseCommand(parameter => PasteItem(),
										   parameter => CanExecuteAction(nameof(PasteCommand), parameter))
										.AddListener(this, nameof(SelectedNode));
			MenuCommand = new BaseCommand(parameter => ExecuteMenuOption(parameter as MenuModel),
										  parameter => CanExecuteMenuOption(parameter as MenuModel));
			DeleteCommand.AddListener(this, nameof(SelectedNode));
			PropertiesCommand.AddListener(this, nameof(SelectedNode));
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(NewCommand):
						NewProject();
					break;
				case nameof(SaveCommand):
						Save();
					break;
				case nameof(DeleteCommand):
						DeleteNode();
					break;
				case nameof(PropertiesCommand):
						OpenPropertiesNode();
					break;
				case nameof(RefreshCommand):
						LoadNodes();
					break;
			}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(NewCommand):
					return true;
				case nameof(NewFolderCommand):
				case nameof(NewFileCommand):
					return SelectedNodeDefinition?.Type == ProjectItemDefinitionModel.ItemType.Folder || SelectedNodeDefinition?.Type == ProjectItemDefinitionModel.ItemType.Project;
				case nameof(DeleteCommand):
					return CanDeleteNode();
				case nameof(PropertiesCommand):
					return CanOpenNode();
				case nameof(SaveCommand):
				case nameof(RefreshCommand):
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///		Crea un nuevo proyecto
		/// </summary>
		protected abstract void NewProject();

		/// <summary>
		///		Crea una nueva carpeta
		/// </summary>
		private void NewFolder()
		{
			string path = GetNodeFileName();

				if (!string.IsNullOrEmpty(path) && System.IO.Directory.Exists(path))
				{
					string newPath = string.Empty;

						if (PlugStudioController.ControllerWindow.ShowInputString("Nombre de la nueva carpeta", ref newPath) == BauMvvm.ViewModels.Controllers.SystemControllerEnums.ResultType.Yes)
						{
							// Crea el directorio
							LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.Combine(path, newPath));
							// Actualiza el árbol
							Refresh();
						}
				}
		}

		/// <summary>
		///		Abre la ventana de creación de un nuevo archivo
		/// </summary>
		private void NewFile()
		{
			Definitions.SelectNewFileViewModel viewModel = new Definitions.SelectNewFileViewModel(PlugStudioController.ControllerWindow, Definition);

				if (PlugStudioController.SelectNewFile(viewModel) == BauMvvm.ViewModels.Controllers.SystemControllerEnums.ResultType.Yes)
				{
					string fileName = viewModel.FileName;

						// Cambia la extensión
						if (!viewModel.SelectedDefinition.IsEqualExtension(System.IO.Path.GetExtension(fileName)))
							fileName += viewModel.SelectedDefinition.Extension;
						// Y lo añade a la carpeta
						fileName = System.IO.Path.Combine(GetNodeFileName(), fileName);
						// Crea el archivo
						if (CreateFile(fileName, viewModel.SelectedDefinition.Template))
						{
							// Abre el archivo
							if (viewModel.SelectedDefinition.EditorType == ProjectItemDefinitionModel.WindowEditorType.Unknown)
								OpenProperties(fileName);
							else
								PlugStudioController.OpenEditor(fileName, viewModel.SelectedDefinition.Template, viewModel.SelectedDefinition.HelpFile);
							// Actualiza el árbol
							Refresh();
						}
				}
		}

		/// <summary>
		///		Crea un archivo
		/// </summary>
		private bool CreateFile(string fileName, string template)
		{
			bool created = false;

				// Crea el archivo
				if (System.IO.File.Exists(fileName))
					PlugStudioController.ControllerWindow.ShowMessage("Ya existe un archivo con el mismo nombre");
				else
					try
					{
						string content = string.Empty;

							// Carga la plantilla
							if (!string.IsNullOrEmpty(template) && System.IO.File.Exists(template))
								content = LibCommonHelper.Files.HelperFiles.LoadTextFile(template);
							// Crea el archivo
							LibCommonHelper.Files.HelperFiles.SaveTextFile(fileName, content);
							// Indica que se ha creado
							created = true;
					}
					catch (Exception exception)
					{
						PlugStudioController.ControllerWindow.ShowMessage($"Error al crear el archivo: {exception.Message}");
					}
				// Devuelve el valor que indica si se ha creado correctamente
				return created;
		}

		/// <summary>
		///		Copia el elemento seleccionado
		/// </summary>
		private void CopySelectedItem()
		{
			PlugStudioController.ControllerWindow.ShowMessage("Copiar");
		}

		/// <summary>
		///		Pega el elemento seleccionado
		/// </summary>
		private void PasteItem()
		{
			PlugStudioController.ControllerWindow.ShowMessage("Pegar");
		}

		/// <summary>
		///		Graba el proyecto
		/// </summary>
		protected abstract void Save();

		/// <summary>
		///		Abre las propiedades del nodo
		/// </summary>
		private void OpenPropertiesNode()
		{
			switch (SelectedNodeDefinition?.EditorType ?? ProjectItemDefinitionModel.WindowEditorType.Unknown)
			{
				case ProjectItemDefinitionModel.WindowEditorType.Unknown:
						OpenProperties();
					break;
				case ProjectItemDefinitionModel.WindowEditorType.Browser:
						// PlugStudioController.OpenWebBrowser();
					break;
				case ProjectItemDefinitionModel.WindowEditorType.Image:
						PlugStudioController.OpenImage(GetNodeFileName());
					break;
				case ProjectItemDefinitionModel.WindowEditorType.Script:
						PlugStudioController.OpenEditor(GetNodeFileName(), SelectedNodeDefinition?.Template, SelectedNodeDefinition?.HelpFile);
					break;
			}
		}

		/// <summary>
		///		Abre la ventana de propiedades del nodo seleccionado
		/// </summary>
		protected abstract void OpenProperties();

		/// <summary>
		///		Abre la ventana de propiedades de un archivo
		/// </summary>
		protected abstract void OpenProperties(string fileName);

		/// <summary>
		///		Comprueba si se puede abrir la ventana de edición del nodo seleccionado
		/// </summary>
		private bool CanOpenNode()
		{
			if ((SelectedNodeDefinition?.EditorType ?? ProjectItemDefinitionModel.WindowEditorType.Unknown) == ProjectItemDefinitionModel.WindowEditorType.Unknown)
				return CanOpen();
			else
				return true;
		}

		/// <summary>
		///		Indica si se puede abrir el nodo seleccionado
		/// </summary>
		protected abstract bool CanOpen();

		/// <summary>
		///		Comprueba si se puede borrar un nodo
		/// </summary>
		private bool CanDeleteNode()
		{
			if (IsNodeFile())
				return true;
			else
				return CanDelete();
		}

		/// <summary>
		///		Borra el nodo
		/// </summary>
		private void DeleteNode()
		{
			if (!IsNodeFile())
				DeleteFixedNode();
			else 
			{
				string fileName = GetSelectedNodeFileName();

					// Borra el archivo o directorio
					if (System.IO.Directory.Exists(fileName) && 
							PlugStudioController.ControllerWindow.ShowQuestion($"¿Realmente desea borrar el directorio {fileName}?"))
					{
						LibCommonHelper.Files.HelperFiles.KillPath(fileName);
						Refresh();
					}
					else if (System.IO.File.Exists(fileName) && 
							 PlugStudioController.ControllerWindow.ShowQuestion($"¿Realmente desea borrar el archivo {fileName}?"))
					{
						LibCommonHelper.Files.HelperFiles.KillFile(fileName);
						Refresh();
					}
			}
		}

		/// <summary>
		///		Comprueba si es un nodo de carpeta o archivo
		/// </summary>
		private bool IsNodeFile()
		{
			return SelectedNodeDefinition != null && (SelectedNodeDefinition.Type == ProjectItemDefinitionModel.ItemType.File || 
													  SelectedNodeDefinition.Type == ProjectItemDefinitionModel.ItemType.Folder);
		}

		/// <summary>
		///		Borra el nodo seleccionado
		/// </summary>
		protected abstract void DeleteFixedNode();

		/// <summary>
		///		Indica si se puede borrar el nodo seleccionado
		/// </summary>
		protected abstract bool CanDelete();

		/// <summary>
		///		Ejecuta una opción de menú
		/// </summary>
		protected abstract void ExecuteMenuOption(MenuModel menuItem);

		/// <summary>
		///		Comprueba si puede ejecutar una opción de menú
		/// </summary>
		protected abstract bool CanExecuteMenuOption(MenuModel menuItem);

		/// <summary>
		///		Crea la definición de proyecto
		/// </summary>
		protected abstract ProjectItemDefinitionModel CreateProjectDefinition();

		/// <summary>
		///		Carga los nodos
		/// </summary>
		protected abstract void LoadNodes();

		/// <summary>
		///		Carga los nodos hijo (necesario porque <see cref="LoadChildrenNodes(ExplorerProjectNodeViewModel)"/> 
		///	es protected y no se puede llamar desde <see cref="ExplorerProjectNodeViewModel"/>)
		/// </summary>
		internal void LoadNodes(ExplorerProjectNodeViewModel node)
		{
			LoadChildrenNodes(node);
		}

		/// <summary>
		///		Carga los nodos hijo de un nodo
		/// </summary>
		protected abstract void LoadChildrenNodes(ExplorerProjectNodeViewModel node);

		/// <summary>
		///		Actualiza el árbol
		/// </summary>
		public virtual void Refresh()
		{
			List<Tuple<string, string>> nodesExpanded = GetNodesExpanded(Children);

				// Limpia los nodos
				Children.Clear();
				// Carga los nodos
				LoadNodes();
				//// Vuelve a expandir los elementos
				//ExpandNodes(Children, nodesExpanded);
		}

		/// <summary>
		///		Obtiene recursivamente los elementos seleccionados
		/// </summary>
		private List<Tuple<string, string>> GetNodesExpanded(ObservableCollection<ExplorerProjectNodeViewModel> nodes)
		{
			List<Tuple<string, string>> expanded = new List<Tuple<string, string>>();

				// Recorre los nodos obteniendo los seleccionados
				foreach (ExplorerProjectNodeViewModel node in nodes)
				{ 
					// Añade el nodo si se ha expandido
					if (node.IsExpanded)
						expanded.Add(new Tuple<string, string>(node.GetType().ToString(), node.ID));
					// Añade los nodos hijo
					//if (node.Children != null && node.Children.Count > 0)
					//	expanded.AddRange(GetNodesExpanded(node.Children));
				}
				// Devuelve la colección de nodos
				return expanded;
		}

		/// <summary>
		///		Expande un nodo
		/// </summary>
		private void ExpandNodes(ObservableCollection<IHierarchicalViewModel> nodes, List<Tuple<string, string>> nodesExpanded)
		{
			if (nodes != null)
				foreach (ExplorerProjectNodeViewModel node in nodes)
					if (CheckExpanded(node, nodesExpanded))
					{ 
						// Expande el nodo
						node.IsExpanded = true;
						// Expande los hijos
						ExpandNodes(node.Children, nodesExpanded);
					}
		}

		/// <summary>
		///		Comprueba si un nodo estaba en la lista de nodos abiertos
		/// </summary>
		private bool CheckExpanded(ExplorerProjectNodeViewModel node, List<Tuple<string, string>> nodesExpanded)
		{ 
			// Recorre la colección
			foreach (Tuple<string, string> nodeExpanded in nodesExpanded)
				if (nodeExpanded.Item1.Equals(node.GetType().ToString(), StringComparison.CurrentCultureIgnoreCase) &&
						nodeExpanded.Item2.Equals(node.ID, StringComparison.CurrentCultureIgnoreCase))
					return true;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return false;
		}

		/// <summary>
		///		Añade un nodo
		/// </summary>
		protected ExplorerProjectNodeViewModel AddNode(IHierarchicalViewModel parent, string text, string itemDefinitionId, object tag, bool lazyLoad = false)
		{
			return AddNode(parent, text, Definition.Search(itemDefinitionId), tag, lazyLoad);
		}

		/// <summary>
		///		Añade un nodo
		/// </summary>
		protected ExplorerProjectNodeViewModel AddNode(IHierarchicalViewModel parent, string text, ProjectItemDefinitionModel definition, object tag, bool lazyLoad = false)
		{
			ExplorerProjectNodeViewModel node = new ExplorerProjectNodeViewModel(this, parent, text, definition, tag, lazyLoad);

				// Cambia la negrita
				node.IsBold = definition?.IsBold ?? false;
				node.Icon = definition?.Icon;
				node.Foreground = definition?.Foreground ?? MvvmColor.Black;
				// Añade el nodo a la lista
				if (parent == null)
					Children.Add(node);
				else
					parent.Children.Add(node);
				// Devuelve el nodo
				return node;
		}

		/// <summary>
		///		Añade un nodo
		/// </summary>
		protected ExplorerProjectNodeViewModel AddNode(IHierarchicalViewModel parent, string text, object tag, bool isBold, MvvmColor foreground, 
													   string icon, bool lazyLoad = false)
		{
			ExplorerProjectNodeViewModel node = new ExplorerProjectNodeViewModel(this, parent, text, null, tag, lazyLoad, foreground);

				// Cambia la negrita
				node.IsBold = isBold;
				node.Icon = icon;
				node.Foreground = foreground;
				// Añade el nodo a la lista
				if (parent == null)
					Children.Add(node);
				else
					parent.Children.Add(node);
				// Devuelve el nodo
				return node;
		}

		/// <summary>
		///		Carga los archivos de proyecto
		/// </summary>
		protected void LoadProjectFiles(ExplorerProjectNodeViewModel root, string path, string folderDefinitionId)
		{
			if (!string.IsNullOrEmpty(path) && System.IO.Directory.Exists(path))
			{
				// Carga las carpetas
				foreach (string child in System.IO.Directory.GetDirectories(path))
				{
					ExplorerProjectNodeViewModel folder = AddNode(root, System.IO.Path.GetFileName(child), folderDefinitionId, child);

						LoadProjectFiles(folder, child, folderDefinitionId);
				}
				// Carga los archivos
				foreach (string file in System.IO.Directory.GetFiles(path))
				{
					ProjectItemDefinitionModel definition = Definition.SearchByExtension(System.IO.Path.GetExtension(file));

						if (definition != null)
							AddNode(root, System.IO.Path.GetFileName(file), definition, file);
				}
			}
		}

		/// <summary>
		///		Nombre de archivo seleccionado
		/// </summary>
		public string GetSelectedNodeFileName()
		{ 
			if (SelectedNode != null && SelectedNode.Tag is string fileName)
				return fileName;
			else
				return string.Empty;
		}

		/// <summary>
		///		Obtiene los menús asociados al elemento seleccionado
		/// </summary>
		public MenuModelCollection GetMenus()
		{
			if (SelectedNodeDefinition != null)
				return SelectedNodeDefinition.Menus;
			else
				return new MenuModelCollection();
		}

		/// <summary>
		///		Obtiene el nombre de archivo del nodo seleccionado
		/// </summary>
		protected string GetNodeFileName()
		{
			if (SelectedNode != null)
				return SelectedNode.GetNodeFileName(ProjectPath);
			else
				return string.Empty;
		}

		/// <summary>
		///		Definición del proyecto
		/// </summary>
		public ProjectItemDefinitionModel Definition 
		{ 
			get
			{
				// Crea la definición de proyecto si no existía
				if (_projectDefinition == null)
					_projectDefinition = CreateProjectDefinition();
				// Devuelve la definición de proyecto
				return _projectDefinition;
			}
		}

		/// <summary>
		///		Controlador de ventanas
		/// </summary>
		public Controllers.IPlugStudioController PlugStudioController { get; }

		/// <summary>
		///		Nodos
		/// </summary>
		public ObservableCollection<ExplorerProjectNodeViewModel> Children 
		{ 
			get { return _children; }
			set { CheckObject(ref _children, value); }
		}

		/// <summary>
		///		Nodo seleccionado
		/// </summary>
		public ExplorerProjectNodeViewModel SelectedNode
		{	
			get { return _selectedNode; }
			set { CheckObject(ref _selectedNode, value); }
		}

		/// <summary>
		///		Definición de proyecto seleccionado
		/// </summary>
		public ProjectItemDefinitionModel SelectedNodeDefinition
		{
			get { return SelectedNode?.ItemDefinition; }
		}

		/// <summary>
		///		Indica si se ha cargado el proyecto
		/// </summary>
		public bool IsProjectLoaded
		{
			get { return _isProjectLoaded; }
			set { CheckProperty(ref _isProjectLoaded, value); }
		}

		/// <summary>
		///		Directorio de proyecto
		/// </summary>
		public string ProjectPath
		{
			get { return _projectPath; }
			set { CheckProperty(ref _projectPath, value); }
		}

		/// <summary>
		///		Comando para crear una nueva carpeta
		/// </summary>
		public BaseCommand NewFolderCommand { get; }

		/// <summary>
		///		Comando para crear un nuevo archivo
		/// </summary>
		public BaseCommand NewFileCommand { get; }

		/// <summary>
		///		Comando para copiar
		/// </summary>
		public BaseCommand CopyCommand { get; }

		/// <summary>
		///		Comando para pegar
		/// </summary>
		public BaseCommand PasteCommand { get; }

		/// <summary>
		///		Comando de menú
		/// </summary>
		public BaseCommand MenuCommand { get; }
	}
}
