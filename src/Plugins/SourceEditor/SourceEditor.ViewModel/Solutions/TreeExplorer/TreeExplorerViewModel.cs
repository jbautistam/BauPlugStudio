using System;

using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.Menus;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.MVVM.ViewModels.TreeItems;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.ViewModel.Solutions.TreeExplorer
{
	/// <summary>
	///		ViewModel para el árbol de soluciones
	/// </summary>
	public class TreeExplorerViewModel : TreeViewModel
	{
		// Constantes privadas
		private const string ActionNewSolution = "NewSolution";
		private const string ActionOpenSolution = "OpenSolution";
		private const string ActionNewProject = "NewProject";
		private const string ActionAddProject = "AddProject";
		private const string ActionNewFolder = "NewFolder";
		private const string ActionNewFile = "NewFile";
		private const string ActionAddExistingFile = "AddExistingFile";
		private const string ActionSeeAtExplorer = "SeeAtExplorer";
		private const string ActionOpenWithWindows = "OpenWithWindows";
		private const string ActionCopy = "Copy";
		private const string ActionCut = "Cut";
		private const string ActionPaste = "Paste";
		private const string ActionPasteImage = "PasteImage";
		private const string ActionRename = "Rename";
		private const string ActionOptionsFile = "OptionsFile";
		private const string SolutionExtension = "sesln";
		// Enumerados privados
		private enum NodeType
		{
			Unknown,
			FolderSolution,
			Project,
			Folder,
			Package,
			File
		}
		// Variables privadas
		private TreeViewItemsViewModelCollection _nodes;
		private ITreeViewItemViewModel _nodeToCopy;
		private SolutionModel _solution;
		private bool _cut;

		public TreeExplorerViewModel()
		{
			InitCommands();
			InitMenus();
			InitMessagesControl();
		}

		/// <summary>
		///		Inicializa los comandos
		/// </summary>
		private void InitCommands()
		{
			NewSolutionCommand = new BaseCommand("Nueva solución", parameter => ExecuteAction(ActionNewSolution, parameter),
																   parameter => CanExecuteAction(ActionNewSolution, parameter));
			OpenSolutionCommand = new BaseCommand("Abrir solución", parameter => ExecuteAction(ActionOpenSolution, parameter),
																	parameter => CanExecuteAction(ActionOpenSolution, parameter));
			NewProjectCommand = new BaseCommand("Nuevo proyecto",
												parameter => ExecuteAction(ActionNewProject, parameter),
												parameter => CanExecuteAction(ActionNewProject, parameter))
										.AddListener(this, "SelectedItem");
			AddProjectCommand = new BaseCommand("Añadir proyecto",
												parameter => ExecuteAction(ActionAddProject, parameter),
												parameter => CanExecuteAction(ActionAddProject, parameter))
										.AddListener(this, "SelectedItem");
			NewFolderCommand = new BaseCommand("Nueva carpeta",
											   parameter => ExecuteAction(ActionNewFolder, parameter),
											   parameter => CanExecuteAction(ActionNewFolder, parameter))
										.AddListener(this, "SelectedItem");
			NewFileCommand = new BaseCommand("Nuevo archivo",
											 parameter => ExecuteAction(ActionNewFile, parameter),
											 parameter => CanExecuteAction(ActionNewFile, parameter))
										.AddListener(this, "SelectedItem");
			AddExistingFileCommand = new BaseCommand("Añadir archivo existente",
													 parameter => ExecuteAction(ActionAddExistingFile, parameter),
													 parameter => CanExecuteAction(ActionAddExistingFile, parameter))
										.AddListener(this, "SelectedItem");
			PropertiesCommand.AddListener(this, "SelectedItem");
			SeeAtExplorerCommand = new BaseCommand("Ver explorador",
												   parameter => ExecuteAction(ActionSeeAtExplorer, parameter),
												   parameter => CanExecuteAction(ActionSeeAtExplorer, parameter))
										.AddListener(this, "SelectedItem");
			OpenWithWindowsCommand = new BaseCommand("Abrir con ...", parameter => ExecuteAction(ActionOpenWithWindows, parameter),
																						 parameter => CanExecuteAction(ActionOpenWithWindows, parameter))
																.AddListener(this, "SelectedItem");
			CopyCommand = new BaseCommand("Copiar", parameter => ExecuteAction(ActionCopy, parameter),
																					parameter => CanExecuteAction(ActionCopy, parameter))
																.AddListener(this, "SelectedItem");
			CutCommand = new BaseCommand("Cortar", parameter => ExecuteAction(ActionCut, parameter),
																					parameter => CanExecuteAction(ActionCut, parameter))
																.AddListener(this, "SelectedItem");
			PasteCommand = new BaseCommand("Pegar", parameter => ExecuteAction(ActionPaste, parameter),
																					parameter => CanExecuteAction(ActionPaste, parameter))
																.AddListener(this, "SelectedItem");
			PasteImageCommand = new BaseCommand("Pegar imagen", parameter => ExecuteAction(ActionPasteImage, parameter),
																					parameter => CanExecuteAction(ActionPasteImage, parameter))
																.AddListener(this, "SelectedItem");
			DeleteCommand.AddListener(this, "SelectedItem");
			RenameCommand = new BaseCommand("Cambiar nombre", parameter => ExecuteAction(ActionRename, parameter),
																			parameter => CanExecuteAction(ActionRename, parameter))
																.AddListener(this, "SelectedItem");
			FileOptionsCommand = new BaseCommand("Opciones de comando", parameter => ExecuteAction(ActionOptionsFile, parameter),
																					 parameter => CanExecuteAction(ActionOptionsFile, parameter));
		}

		/// <summary>
		///		Inicializa los menús
		/// </summary>
		private void InitMenus()
		{
			MenuGroupViewModel menuGroup;

				// Añade los menús de la ventana principal. Archivo Abrir
				menuGroup = base.MenuCompositionData.Menus.Add("Abrir", MenuGroupViewModel.TargetMenuType.MainMenu,
															   MenuGroupViewModel.TargetMainMenuItemType.FileOpenItems);
				menuGroup.MenuItems.Add("Abrir solución", SourceEditorViewModel.Instance.GetIconRoute(SourceEditorViewModel.IconIndex.OpenSolution),
										OpenSolutionCommand);
				// Añade los menús de la ventana principal. Archivo Nuevo
				menuGroup = base.MenuCompositionData.Menus.Add("Nuevo", MenuGroupViewModel.TargetMenuType.MainMenu,
															   MenuGroupViewModel.TargetMainMenuItemType.FileNewItems);
				menuGroup.MenuItems.Add("Nueva solución", SourceEditorViewModel.Instance.GetIconRoute(SourceEditorViewModel.IconIndex.NewProject),
										NewSolutionCommand);
				menuGroup.MenuItems.AddSeparator();
				menuGroup.MenuItems.Add("Nuevo proyecto", SourceEditorViewModel.Instance.GetIconRoute(SourceEditorViewModel.IconIndex.NewProject),
										NewProjectCommand);
				menuGroup.MenuItems.Add("Añadir proyecto existente", SourceEditorViewModel.Instance.GetIconRoute(SourceEditorViewModel.IconIndex.NewProject),
										AddProjectCommand);
				menuGroup.MenuItems.AddSeparator();
				menuGroup.MenuItems.Add("Nueva carpeta", SourceEditorViewModel.Instance.GetIconRoute(SourceEditorViewModel.IconIndex.NewFolder),
										NewFolderCommand);
				menuGroup.MenuItems.Add("Nuevo archivo", SourceEditorViewModel.Instance.GetIconRoute(SourceEditorViewModel.IconIndex.NewDocument),
										NewFileCommand);
		}

		/// <summary>
		///		Inicializa el controlador de mensajes
		/// </summary>
		private void InitMessagesControl()
		{
			SourceEditorViewModel.Instance.HostController.Messenger.Sent += (sender, evntArgs) =>
					 {
						 if (evntArgs.MessageSent is Plugins.ViewModels.Controllers.Messengers.Common.MessageRecentFileUsed message && 
								message.Source.EqualsIgnoreCase(SourceEditorViewModel.Instance.ModuleName))
							 OpenSolution(message.FileName);
					 };
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			NodeType intNodeType = GetSelectedNodeType();

				switch (action)
				{
					case ActionNewSolution:
							OpenFormNewSolution();
						break;
					case ActionOpenSolution:
							OpenFormOpenSolution();
						break;
					case ActionNewFile:
							OpenFormUpdateFile(GetSelectedProject(), GetSelectedFolder(), null);
						break;
					case ActionAddExistingFile:
							OpenFormAddExistingFile(GetSelectedProject(), GetSelectedFolder());
						break;
					case ActionNewFolder:
							if (intNodeType == NodeType.Unknown || intNodeType == NodeType.FolderSolution)
								OpenFormUpdateSolutionFolder(GetSelectedSolutionFolder(), null);
							else
								OpenFormUpdateFolder(GetSelectedProject(), GetSelectedFolder(), null);
						break;
					case ActionNewProject:
							OpenFormUpdateProject(GetSelectedProject());
						break;
					case ActionAddProject:
							OpenFormAddExistingProject();
						break;
					case nameof(PropertiesCommand):
							switch (intNodeType)
							{
								case NodeType.Project:
										OpenFormUpdateProject(GetSelectedProject());
									break;
								case NodeType.FolderSolution:
										OpenFormUpdateSolutionFolder(null, GetSelectedSolutionFolder());
									break;
								case NodeType.Folder:
										OpenFormUpdateFolder(GetSelectedProject(), null, GetSelectedFolder());
									break;
								case NodeType.File:
								case NodeType.Package:
										OpenFormUpdateFile(GetSelectedProject(), null, GetSelectedFile());
									break;
							}
						break;
					case ActionSeeAtExplorer:
							OpenFileExplorer();
						break;
					case ActionOpenWithWindows:
							OpenWithWindows();
						break;
					case ActionCopy:
					case ActionCut:
							AddFileToCopyBuffer(action == ActionCut);
						break;
					case ActionPaste:
							PasteFile();
						break;
					case ActionPasteImage:
							PasteImage(GetSelectedFolder());
						break;
					case nameof(DeleteCommand):
							switch (GetSelectedNodeType())
							{
								case NodeType.Project:
										DeleteProject((SelectedItem as ProjectNodeViewModel).Project);
									break;
								case NodeType.FolderSolution:
										DeleteSolutionFolder(GetSelectedSolutionFolder());
									break;
								case NodeType.Folder:
								case NodeType.File:
								case NodeType.Package:
										DeleteFile((SelectedItem as FileNodeViewModel).File);
									break;
							}
						break;
					case nameof(RefreshCommand):
							Refresh();
						break;
					case ActionRename:
							Rename(GetSelectedFile());
						break;
					case ActionOptionsFile:
							ActionFileExecute(GetSelectedProject(), GetSelectedFile(), parameter as Model.Definitions.MenuModel);
						break;
				}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			NodeType intNodeType = GetSelectedNodeType();

			switch (action)
			{
				case ActionAddProject:
				case ActionNewProject:
					return intNodeType == NodeType.Unknown || intNodeType == NodeType.FolderSolution;
				case ActionNewFile:
				case ActionAddExistingFile:
					return intNodeType == NodeType.Project || intNodeType == NodeType.Folder || intNodeType == NodeType.Package;
				case ActionNewFolder:
					return intNodeType == NodeType.Unknown || intNodeType == NodeType.Project ||
							intNodeType == NodeType.Folder || intNodeType == NodeType.Package || intNodeType == NodeType.FolderSolution;
				case ActionOptionsFile:
				case nameof(PropertiesCommand):
				case nameof(DeleteCommand):
					return intNodeType != NodeType.Unknown;
				case ActionSeeAtExplorer:
				case ActionCopy:
				case ActionCut:
					return intNodeType == NodeType.Folder || intNodeType == NodeType.File || intNodeType == NodeType.Package || intNodeType == NodeType.Project;
				case ActionPaste:
					return _nodeToCopy != null &&
									(((intNodeType == NodeType.Project || intNodeType == NodeType.Folder ||
										 intNodeType == NodeType.File || intNodeType == NodeType.Package) && _nodeToCopy is FileNodeViewModel) ||
									 (intNodeType == NodeType.FolderSolution && _nodeToCopy is ProjectNodeViewModel));
				case ActionPasteImage:
					return (intNodeType == NodeType.Project || intNodeType == NodeType.Folder || intNodeType == NodeType.Package) &&
								 System.Windows.Clipboard.ContainsImage();
				case ActionNewSolution:
				case ActionOpenSolution:
				case nameof(RefreshCommand):
					return true;
				case ActionRename:
					return intNodeType == NodeType.FolderSolution ||
								 intNodeType == NodeType.Project || intNodeType == NodeType.Folder ||
								 intNodeType == NodeType.Package || intNodeType == NodeType.File;
				case ActionOpenWithWindows:
					return intNodeType == NodeType.File;
				default:
					return false;
			}
		}

		/// <summary>
		///		Obtiene los menús del elemento seleccionado
		/// </summary>
		public Model.Definitions.MenuModelCollection GetMenus()
		{
			FileModel file = GetSelectedFile();

				// Obtiene los menús
				if (file != null)
					return file.SearchProject().Definition.GetMenus(file);
				else
					return new Model.Definitions.MenuModelCollection();
		}

		/// <summary>
		///		Añade un archivo al buffer de copia
		/// </summary>
		private void AddFileToCopyBuffer(bool cut)
		{
			_nodeToCopy = SelectedItem;
			_cut = cut;
		}

		/// <summary>
		///		Pega un archivo
		/// </summary>
		private void PasteFile()
		{
			if (_nodeToCopy != null)
			{
				// Copia el elemento que teníamos en memoria sobre el nodo seleccionado
				Copy(_nodeToCopy, SelectedItem, !_cut);
				// Indica que ya no hay ningún archivo que copiar
				_nodeToCopy = null;
			}
		}

		/// <summary>
		///		Copia un nodo sobre otro
		/// </summary>
		public void Copy(ITreeViewItemViewModel nodeSource, ITreeViewItemViewModel nodeTarget, bool copy)
		{
			if (CanCopy(nodeSource, nodeTarget))
			{
				// Dependiendo de cuál sea el destino, llama a una rutina de copia
				if (nodeTarget is SolutionFolderNodeViewModel)
				{
					if (nodeSource is ProjectNodeViewModel)
						PasteProject(nodeSource as ProjectNodeViewModel, Solution,
												 (nodeTarget as SolutionFolderNodeViewModel).Folder, copy);
					else if (nodeSource is SolutionFolderNodeViewModel)
						PasteSolution((nodeSource as SolutionFolderNodeViewModel).Folder,
													(nodeTarget as SolutionFolderNodeViewModel).Folder, copy);
				}
				else
					PasteFile(GetCopyFile(nodeSource), GetCopyFile(nodeTarget), copy);
				// Actualiza
				Refresh();
			}
		}

		/// <summary>
		///		Copia una serie de archivos
		/// </summary>
		public void Copy(BaseNodeViewModel nodeTarget, string[] fileNames)
		{
			if (!(nodeTarget is SolutionFolderNodeViewModel))
			{
				string pathTarget = GetPath(nodeTarget);

				// Copia los archivos
				if (!pathTarget.IsEmpty() && System.IO.Directory.Exists(pathTarget))
				{ // Copia los archivos
					foreach (string fileName in fileNames)
						CopyFile(fileName, pathTarget);
					// Actualiza el árbol
					Refresh();
				}
			}
		}

		/// <summary>
		///		Comprueba si puede copiar un archivo en otro
		/// </summary>
		private bool CanCopy(ITreeViewItemViewModel nodeSource, ITreeViewItemViewModel nodeTarget)
		{
			bool canCopy = false; // ... supone que no se puede copiar

				// Comprueba si se puede copiar
				if (!nodeSource.NodeID.EqualsIgnoreCase(nodeTarget.NodeID))
				{
					if (nodeTarget is SolutionFolderNodeViewModel &&
								  (nodeSource is SolutionFolderNodeViewModel || nodeSource is ProjectNodeViewModel))
						canCopy = true;
					else if (nodeTarget is ProjectNodeViewModel && nodeSource is FileNodeViewModel)
					{
						ProjectModel projectTarget = (nodeTarget as ProjectNodeViewModel).Project;
						FileModel fileSource = (nodeSource as FileNodeViewModel).File;

							if (projectTarget.Definition.GlobalId.EqualsIgnoreCase(fileSource.SearchProject().Definition.GlobalId))
								canCopy = true;
					}
					else if (nodeTarget is FileNodeViewModel && nodeSource is FileNodeViewModel)
					{
						FileModel source = (nodeSource as FileNodeViewModel).File;
						FileModel target = (nodeTarget as FileNodeViewModel).File;

							if (source.SearchProject().Definition.GlobalId.EqualsIgnoreCase(target.SearchProject().Definition.GlobalId))
							{
								if (target.IsFolder)
									canCopy = true;
							}
					}
				}
				// Devuelve el valor que indica si se puede copiar
				return canCopy;
		}

		/// <summary>
		///		Pega un proyecto en una carpeta de solución
		/// </summary>
		private void PasteProject(ProjectNodeViewModel projectSource, SolutionModel solutionTarget, SolutionFolderModel folder, bool copy)
		{
			if (projectSource != null)
			{
				ProjectModel project = projectSource.Project;

					// Añade el proyecto a la carpeta seleccionada o al raíz
					if (folder != null)
						folder.Projects.Add(project);
					else
						solutionTarget.Projects.Add(project);
					// Si era una acción de cortar borra el proyecto de dónde estaba
					if (!copy)
					{
						if (projectSource.Parent == null)
							projectSource.Project.Solution.Projects.Delete(project);
						else if (projectSource.Parent is SolutionFolderNodeViewModel)
							(projectSource.Parent as SolutionFolderNodeViewModel).Folder.Projects.Delete(project);
					}
					// Graba la solución
					SaveSolution();
			}
		}

		/// <summary>
		///		Pega una carpeta de solución sobre otra
		/// </summary>
		private void PasteSolution(SolutionFolderModel folderSource, SolutionFolderModel folderTarget, bool copy)
		{
			if (folderSource != null && folderTarget != null)
			{
				// Clona la carpeta origen sobre la destino 
				folderTarget.Folders.Add(folderSource.Clone());
				// Si se está cortando, elimina la carpeta origen
				if (!copy)
					folderSource.Solution.Delete(folderSource);
				// Graba la solución
				SaveSolution();
			}
		}

		/// <summary>
		///		Obtiene el archivo a copiar a partir de un nodo
		/// </summary>
		private FileModel GetCopyFile(ITreeViewItemViewModel nodeToCopy)
		{
			if (nodeToCopy is ProjectNodeViewModel)
				return (nodeToCopy as ProjectNodeViewModel).Project;
			else if (nodeToCopy is FileNodeViewModel)
				return (nodeToCopy as FileNodeViewModel).File;
			else
				return null;
		}

		/// <summary>
		///		Pega un archivo
		/// </summary>
		private void PasteFile(FileModel fileToCopy, FileModel fileTarget, bool copy)
		{
			if (fileToCopy != null)
			{
				string pathTarget = fileTarget.FullFileName;
				bool isCopied = false;

					// Obtiene el directorio destino
					if (!System.IO.Directory.Exists(pathTarget))
						pathTarget = System.IO.Path.GetDirectoryName(pathTarget);
					// Copia / mueve el archivo / carpeta
					if (fileToCopy.IsFolder || CheckIsPackage(fileToCopy))
					{
						LibCommonHelper.Files.HelperFiles.CopyPath(fileToCopy.FullFileName,
															 LibCommonHelper.Files.HelperFiles.GetConsecutivePath(pathTarget,
																											System.IO.Path.GetFileName(fileToCopy.FullFileName)));
						isCopied = true; // ... supone que se ha podido copiar
					}
					else
						isCopied = CopyFile(fileToCopy.FullFileName, pathTarget);
					// Si la acción es para cortar, elimina el archivo inicial
					if (isCopied && !copy)
					{
						if (fileToCopy.IsFolder || CheckIsPackage(fileToCopy))
							LibCommonHelper.Files.HelperFiles.KillPath(fileToCopy.FullFileName);
						else
							LibCommonHelper.Files.HelperFiles.KillFile(fileToCopy.FullFileName);
					}
			}
		}

		/// <summary>
		///		Copia un archivo en un directorio
		/// </summary>
		private bool CopyFile(string fileSource, string pathTarget)
		{
			return LibCommonHelper.Files.HelperFiles.CopyFile(fileSource,
														LibCommonHelper.Files.HelperFiles.GetConsecutiveFileName(pathTarget,
																										   System.IO.Path.GetFileName(fileSource)));
		}

		/// <summary>
		///		Comprueba si el archivo es de tipo paquete
		/// </summary>
		private bool CheckIsPackage(FileModel file)
		{
			return file.FileDefinition is Model.Definitions.PackageDefinitionModel;
		}

		/// <summary>
		///		Abre el formulario de modificación de un proyecto
		/// </summary>
		private void OpenFormUpdateProject(ProjectModel project)
		{
			if (project == null)
			{
				if (SourceEditorViewModel.Instance.ViewsController.OpenFormNewProject
								  (new NewItems.ProjectNewViewModel(Solution,
																	GetSelectedSolutionFolder())) == SystemControllerEnums.ResultType.Yes)
					SaveSolution();
			}
			else
				OpenFormUpdateFile(project, null, project);
		}

		/// <summary>
		///		Abre el formulario de añadir proyecto existente
		/// </summary>
		private void OpenFormAddExistingProject()
		{
			string filter = null;

				// Carga los proyectos en el filtro
				foreach (Model.Definitions.ProjectDefinitionModel project in SourceEditorViewModel.Instance.PluginsController.ProjectDefinitions)
					filter = filter.AddWithSeparator($"{project.Name} (*.{project.Extension})|*.{project.Extension}",
																								 "|", false);
				// Añade un filtro con todos los archivos y abre la carga de proyecto
				if (filter.IsEmpty())
					SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("No se ha definido ningún tipo de proyecto");
				else
				{
					string fileName;

						// Añade el filtro para todos los archivos
						filter = filter.AddWithSeparator("Todos los archivos (*.*)|*.*", "|", false);
						// Carga el archivo
						fileName = SourceEditorViewModel.Instance.DialogsController.OpenDialogLoad(null, filter);
						// Si tenemos un nombre de archivo ...
						if (!fileName.IsEmpty() && System.IO.File.Exists(fileName))
						{
							Model.Definitions.ProjectDefinitionModel definition;

								// Busca la definición del proyecto
								definition = SourceEditorViewModel.Instance.PluginsController.ProjectDefinitions.SearchByExtension(System.IO.Path.GetExtension(fileName));
								// Si se ha encontrado alguna definición de proyecto válida, se añade a la solución
								if (definition != null)
								{
									ProjectModel project = new ProjectModel(Solution, definition, fileName); // System.IO.Path.GetDirectoryName(fileName));
									ProjectsModelCollection projects = Solution.Projects;

										// Si es una carpeta, escoge los proyectos de la carpeta
										if (GetSelectedSolutionFolder() != null)
											projects = GetSelectedSolutionFolder().Projects;
										// Si no existe ningún proyecto igual, lo añade
										if (projects.ExistsByFileName(project.FullFileName))
											SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("Ya existe un proyecto con ese nombre en la solución");
										else
										{
											// Añade el proyecto a la colección
											projects.Add(project);
											// Graba la solución
											SaveSolution();
										}
								}
							else
								SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("No se ha encontrado nigún proyecto que coincida con esta extensión");
						}
				}
		}

		/// <summary>
		///		Abre el formulario de modificación / creación de una carpeta
		/// </summary>
		private void OpenFormUpdateFolder(ProjectModel project, FileModel folderParent, FileModel folder)
		{
			if (project == null || folderParent == null)
				SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el proyecto al que desea añadir la carpeta");
			else
			{
				string newFolder = folder?.Name;

					if (SourceEditorViewModel.Instance.ControllerWindow.ShowInputString("Introduzca el nombre de la carpeta", ref newFolder) == SystemControllerEnums.ResultType.Yes)
					{
						// Crea el directorio
						LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.Combine(GetPath(folderParent.FullFileName), newFolder));
						// Actualiza el árbol
						Refresh();
					}
			}
		}

		/// <summary>
		///		Obtiene el directorio de un nodo
		/// </summary>
		private string GetPath(BaseNodeViewModel node)
		{
			string pathTarget = null;

				// Obtiene el directorio
				if (node is ProjectNodeViewModel)
					pathTarget = (node as ProjectNodeViewModel).Project.FullFileName;
				else if (node is FileNodeViewModel)
					pathTarget = (node as FileNodeViewModel).File.FullFileName;
				// Normaliza el directorio
				if (!pathTarget.IsEmpty())
					pathTarget = GetPath(pathTarget);
				// Devuelve el directorio
				return pathTarget;
		}

		/// <summary>
		///		Obtiene un directorio a partir del nombre de archivo
		/// </summary>
		private string GetPath(string fileName)
		{
			if (System.IO.Directory.Exists(fileName))
				return fileName;
			else
				return System.IO.Path.GetDirectoryName(fileName);
		}

		/// <summary>
		///		Abre el formulario de modificación / creación de una carpeta de solución
		/// </summary>
		private void OpenFormUpdateSolutionFolder(SolutionFolderModel solutionFolderParent, SolutionFolderModel solutionFolder)
		{
			if (SourceEditorViewModel.Instance.ViewsController.OpenUpdateFolderSolutionView
								  (new SolutionFolderViewModel(_solution, solutionFolderParent, solutionFolder)) == SystemControllerEnums.ResultType.Yes)
				SaveSolution();
		}

		/// <summary>
		///		Abre el formulario de modificación / creación de un archivo
		/// </summary>
		private void OpenFormUpdateFile(ProjectModel project, FileModel folderParent, FileModel file)
		{
			if (project == null)
				SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("No se ha seleccionado ningún proyecto");
			else
			{
				bool isNew = file == null;

					// Abre la ventana de nuevo archivo
					if (file == null)
					{
						NewItems.FileNewViewModel newViewModel = new NewItems.FileNewViewModel(project, folderParent);

							if (SourceEditorViewModel.Instance.ViewsController.OpenFormNewFile(newViewModel) == SystemControllerEnums.ResultType.Yes)
								file = newViewModel.File;
					}
					// Si se ha creado un nuevo archivo (o es una modificación, abre el formulario de modificación)
					if (file != null)
					{
						// Abre el formulario de modificación
						SourceEditorViewModel.Instance.ViewsController.OpenFormUpdateFile(project.Definition, file, isNew);
						// Actualiza el árbol
						if (isNew)
							Refresh();
					}
			}
		}

		/// <summary>
		///		Añade un archivo existente al proyecto / carpeta
		/// </summary>
		private void OpenFormAddExistingFile(ProjectModel project, FileModel folder)
		{
			string targetPath = GetPath(project, folder);

				if (!System.IO.Directory.Exists(targetPath))
					SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("No se encuentra el directorio");
				else
				{
					string[] files = SourceEditorViewModel.Instance.DialogsController.OpenDialogLoadFilesMultiple(null, "Todos los archivos|*.*");

						// Si se ha seleccionado algún archivo
						if (files != null && files.Length > 0)
						{
							// Copia los archivos
							foreach (string file in files)
								if (System.IO.File.Exists(file))
								{
									string fileTarget = LibCommonHelper.Files.HelperFiles.GetConsecutiveFileName(targetPath, System.IO.Path.GetFileName(file));

									// Copia el archivo
									LibCommonHelper.Files.HelperFiles.CopyFile(file, fileTarget);
								}
							// Actualiza el árbol
							Refresh();
						}
				}
		}

		/// <summary>
		///		Obtiene el directorio a partir del proyecto o de la carpeta seleccionada
		/// </summary>
		private string GetPath(ProjectModel project, FileModel folder)
		{
			string path = null;

				// Obtiene el directorio del proyecto
				if (project != null)
					path = project.PathBase;
				// Si se ha pasado una carpeta, recoge el directorio
				if (folder != null)
					path = folder.FullFileName;
				// Devuelve el directorio
				return path;
		}

		/// <summary>
		///		Abre el formulario para crear una nueva solución
		/// </summary>
		private void OpenFormNewSolutionFolder()
		{
			SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("Abrir el formulario de nueva carpeta de solución");
			//if (SourceEditorViewModel.Instance.ViewsController.OpenFormNewSolution(WindowOwner, Solution) == BauMvvm.Common.Controllers.SystemControllerEnums.ResultType.Yes)
			//	SaveSolution();
		}

		/// <summary>
		///		Abre el archivo en el explorador
		/// </summary>
		private void OpenFileExplorer()
		{
			FileModel file = GetSelectedFile();

				if (file != null)
				{
					string path = file.FullFileName;

						// Obtiene el nombre del directorio si se le ha pasado un archivo
						if (!System.IO.Directory.Exists(path))
							path = System.IO.Path.GetDirectoryName(path);
						// Abre el explorador de archivos
						if (System.IO.Directory.Exists(path))
							LibSystem.Files.WindowsFiles.OpenDocumentShell("explorer.exe", path);
				}
		}

		/// <summary>
		///		Abre el archivo con Windows
		/// </summary>
		private void OpenWithWindows()
		{
			FileModel file = GetSelectedFile();

				if (file != null)
				{
					if (file.FullFileName.IsEmpty() || !System.IO.File.Exists(file.FullFileName))
						SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("No se encuentra el archivo");
					else
						LibSystem.Files.WindowsFiles.OpenDocumentShell(file.FullFileName);
				}
		}

		/// <summary>
		///		Pega la imagen
		/// </summary>
		private void PasteImage(FileModel file)
		{
			SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("¿De qué tamaño tienes que ser la imagen?");
			//if (!new ViewModel.Helper.ClipboardHelper().PasteImage(file.PathBase, file.Project.MaxWidthImage, file.Project.ThumbsWidth, out error))
			//	{ if (!error.IsEmpty())
			//			SourceEditorViewModel.Instance.HostController.ControllerWindow.ShowMessage(error);
			//	}
			//else
			//	RefreshChilds();
		}

		/// <summary>
		///		Quita un proyecto de la solución
		/// </summary>
		private void DeleteProject(ProjectModel project)
		{
			if (SourceEditorViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea borrar el proyecto {project.Name}?"))
			{
				// Borra el proyecto de la solución
				project.Solution.Delete(project);
				// Graba la solución
				SaveSolution();
			}
		}

		/// <summary>
		///		Quita una carpeta de solución
		/// </summary>
		private void DeleteSolutionFolder(SolutionFolderModel folder)
		{
			if (folder != null &&
				SourceEditorViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea borrar la carpeta {folder.Name}?"))
			{
				// Borra la carpeta de la solución
				folder.Solution.Delete(folder);
				// Graba la solución
				SaveSolution();
			}
		}

		/// <summary>
		///		Graba la solución
		/// </summary>
		private void SaveSolution()
		{
			// Graba la solución
			new Application.Bussiness.Solutions.SolutionBussiness().Save(Solution);
			// Actualiza el árbol
			Refresh();
		}

		/// <summary>
		///		Borra un archivo / carpeta
		/// </summary>
		private void DeleteFile(FileModel file)
		{
			if (SourceEditorViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea borrar {file.Name}?"))
			{
				// Borra la carpeta / archivo
				if (file.IsFolder)
					LibCommonHelper.Files.HelperFiles.KillPath(file.FullFileName);
				else
					LibCommonHelper.Files.HelperFiles.KillFile(file.FullFileName);
				// Actualiza el árbol
				Refresh();
			}
		}

		/// <summary>
		///		Cambia el nombre de un archivo
		/// </summary>
		private void Rename(FileModel file)
		{
			if (SourceEditorViewModel.Instance.ViewsController.OpenFormChangeFileName(file) == SystemControllerEnums.ResultType.Yes)
				Refresh();
		}

		/// <summary>
		///		Ejecuta las acciones del archivo
		/// </summary>
		private void ActionFileExecute(ProjectModel project, FileModel file, Model.Definitions.MenuModel menu)
		{
			if (menu != null)
				SourceEditorViewModel.Instance.MessagesController.ExecuteAction(project.Definition, file, menu);
		}

		/// <summary>
		///		Abre el formulario de creación de una nueva solución
		/// </summary>
		private void OpenFormNewSolution()
		{
			string fileName = SourceEditorViewModel.Instance.DialogsController.OpenDialogSave
																	  (SourceEditorViewModel.Instance.PathData, GetFilterSolution(),
																	   "Solution." + SolutionExtension, SolutionExtension);

				if (!fileName.IsEmpty())
				{
					// Crea el directorio y el archivo inicial
					LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileName));
					new Application.Bussiness.Solutions.SolutionBussiness().Save(new SolutionModel(fileName));
					// Cambia la solución activa
					OpenSolution(fileName);
					// Envía el mensaje de solución abierta
					SendMessageOpenSolution(fileName);
				}
		}

		/// <summary>
		///		Abre el formulario de apertura de una solución
		/// </summary>
		private void OpenFormOpenSolution()
		{
			string fileName = SourceEditorViewModel.Instance.DialogsController.OpenDialogLoad
															  (SourceEditorViewModel.Instance.PathData, GetFilterSolution(),
															   "Solution." + SolutionExtension, SolutionExtension);

				if (!fileName.IsEmpty())
				{
					if (!System.IO.File.Exists(fileName))
						SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("Seleccione un nombre de archivo");
					else
					{
						// Abre el archivo
						OpenSolution(fileName);
						// Envía el mensaje para añadir el archivo a la lista de últimos archivos abiertos
						SendMessageOpenSolution(fileName);
					}
				}
		}

		/// <summary>
		///		Obtiene el filtro para los cuadros de apertura y grabación de soluciones
		/// </summary>
		private string GetFilterSolution()
		{
			return string.Format("Archivos de solución|*.{0}|Todos los archivos|*.*", SolutionExtension);
		}

		/// <summary>
		///		Envía el mensaje de solución abierta
		/// </summary>
		private void SendMessageOpenSolution(string fileName)
		{
			SourceEditorViewModel.Instance.HostController.Messenger.SendRecentFileOpened(SourceEditorViewModel.Instance.ModuleName,
																						 fileName, fileName);
		}

		/// <summary>
		///		Abre una solución
		/// </summary>
		private void OpenSolution(string fileName)
		{
			// Carga la solución
			Solution = LoadSolution(fileName);
			// Cambia la última solución activa
			SourceEditorViewModel.Instance.LastFileSolution = fileName;
			SourceEditorViewModel.Instance.HostController.Configuration.Save();
			// Actualiza el árbol
			Refresh();
		}

		/// <summary>
		///		Carga una solución
		/// </summary>
		private SolutionModel LoadSolution(string fileName = null)
		{
			SolutionModel solution;

				// Obtiene el nombre de la solución
				if (fileName.IsEmpty())
					fileName = SourceEditorViewModel.Instance.LastFileSolution;
				if (fileName.IsEmpty())
					fileName = System.IO.Path.Combine(SourceEditorViewModel.Instance.PathData, "Solution." + SolutionExtension);
				if (!System.IO.File.Exists(fileName)) // ... por las antiguas
					fileName = System.IO.Path.Combine(SourceEditorViewModel.Instance.PathData, "Solutions.xml");
				// Crea la solución (una vez se ha normalizado el nombre de archivo)
				solution = new SolutionModel(fileName);
				// Carga la solución
				try
				{
					solution = new Application.Bussiness.Solutions.SolutionBussiness().Load(ProjectDefinitions, fileName);
				}
				catch (Exception exception)
				{
					SourceEditorViewModel.Instance.ControllerWindow.ShowMessage($"Error al cargar la solución: {fileName}.{Environment.NewLine}{exception.Message}");
					solution = new SolutionModel(fileName);
				}
				// Devuelve la solución
				return solution;
		}

		/// <summary>
		///		Carga los nodos
		/// </summary>
		protected override TreeViewItemsViewModelCollection LoadNodes()
		{
			TreeViewItemsViewModelCollection nodes = new TreeViewItemsViewModelCollection();

				// Ordena las carpetas de solución
				Solution.Folders.SortByName();
				// Añade las carpetas de solución
				foreach (SolutionFolderModel folder in Solution.Folders)
					nodes.Add(new SolutionFolderNodeViewModel(null, folder));
				// Ordena los archivos de proyecto
				Solution.Projects.SortByName();
				// Muestra los archivos hijo
				foreach (ProjectModel project in Solution.Projects)
					nodes.Add(new ProjectNodeViewModel(null, project));
				// Devuelve la colección de nodos
				return nodes;
		}

		/// <summary>
		///		Obtiene el tipo de nodo seleccionado
		/// </summary>
		private NodeType GetSelectedNodeType()
		{
			NodeType type = NodeType.Unknown;

				// Obtiene el tipo de nodo a partir del nodo seleccionado
				if (SelectedItem != null)
				{
					if (SelectedItem is ProjectNodeViewModel)
						type = NodeType.Project;
					else if (SelectedItem is SolutionFolderNodeViewModel)
						type = NodeType.FolderSolution;
					else if (SelectedItem is FileNodeViewModel)
					{
						FileNodeViewModel fileNode = SelectedItem as FileNodeViewModel;

							if (CheckIsPackage(fileNode.File)) // ... debe estar el primero
								type = NodeType.Package;
							else if (fileNode.File.IsFolder)
								type = NodeType.Folder;
							else
								type = NodeType.File;
					}
				}
				// Devuelve el tipo de nodo seleccionado
				return type;
		}

		/// <summary>
		///		Obtiene el proyecto seleccionado
		/// </summary>
		public ProjectModel GetSelectedProject()
		{
			NodeType type = GetSelectedNodeType();

				if (type == NodeType.Project)
					return (SelectedItem as ProjectNodeViewModel).Project;
				else if (type == NodeType.Folder || type == NodeType.File || type == NodeType.Package)
					return (SelectedItem as FileNodeViewModel).File.SearchProject();
				else
					return null;
		}

		/// <summary>
		///		Obtiene la carpeta de solución seleccionada
		/// </summary>
		public SolutionFolderModel GetSelectedSolutionFolder()
		{
			SolutionFolderModel folder = null;

				// Obtiene la carpeta de solución
				if (GetSelectedNodeType() == NodeType.FolderSolution)
				{
					SolutionFolderNodeViewModel node = SelectedItem as SolutionFolderNodeViewModel;

						if (node != null)
							folder = node.Folder;
				}
				// Devuelve la carpeta de solución seleccionada
				return folder;
		}

		/// <summary>
		///		Obtiene la carpeta seleccionada
		/// </summary>
		public FileModel GetSelectedFolder()
		{
			return GetSelectedFile();
		}

		/// <summary>
		///		Obtiene el archivo seleccionado
		/// </summary>
		public FileModel GetSelectedFile()
		{
			NodeType type = GetSelectedNodeType();

				if (type == NodeType.Folder || type == NodeType.File || type == NodeType.Package)
					return (SelectedItem as FileNodeViewModel).File;
				else if (type == NodeType.Project)
					return (SelectedItem as ProjectNodeViewModel).Project;
				else
					return null;
		}

		/// <summary>
		///		Solución base
		/// </summary>
		public SolutionModel Solution
		{
			get
			{
				// Carga la solución
				if (_solution == null)
					_solution = new Application.Bussiness.Solutions.SolutionBussiness().Load(ProjectDefinitions,
																							   SourceEditorViewModel.Instance.LastFileSolution);
				// Devuelve la solución
				return _solution;
			}
			private set { _solution = value; }
		}

		/// <summary>
		///		Definiciones de proyecto
		/// </summary>
		public Model.Definitions.ProjectDefinitionModelCollection ProjectDefinitions
		{
			get { return SourceEditorViewModel.Instance.PluginsController.ProjectDefinitions; }
		}

		/// <summary>
		///		Carpeta inicial a mostrar en el árbol
		/// </summary>
		public override TreeViewItemsViewModelCollection Nodes
		{
			get
			{
				// Carga los nodos
				if (_nodes == null)
					_nodes = LoadNodes();
				// Devuelve los nodos
				return _nodes;
			}
			set { CheckObject(ref _nodes, value); }
		}

		/// <summary>
		///		Indica si está seleccionada una carpeta
		/// </summary>
		private bool IsSelectedFolder
		{
			get { return SelectedItem != null && SelectedItem is FileNodeViewModel; }
		}

		/// <summary>
		///		Comando para abrir una solución
		/// </summary>
		public BaseCommand OpenSolutionCommand { get; private set; }

		/// <summary>
		///		Comando para crear una nueva solución
		/// </summary>
		public BaseCommand NewSolutionCommand { get; private set; }

		/// <summary>
		///		Comando de nuevo proyecto
		/// </summary>
		public BaseCommand NewProjectCommand { get; private set; }

		/// <summary>
		///		Comando de añadir proyecto existente
		/// </summary>
		public BaseCommand AddProjectCommand { get; private set; }

		/// <summary>
		///		Comando de nueva carpeta
		/// </summary>
		public BaseCommand NewFolderCommand { get; private set; }

		/// <summary>
		///		Comando de nuevo archivo
		/// </summary>
		public BaseCommand NewFileCommand { get; private set; }

		/// <summary>
		///		Comando de "Añadir archivo existente"
		/// </summary>
		public BaseCommand AddExistingFileCommand { get; private set; }

		/// <summary>
		///		Comando para ver el archivo en el explorador
		/// </summary>
		public BaseCommand SeeAtExplorerCommand { get; private set; }

		/// <summary>
		///		Comando para abrir el archivo en Windows
		/// </summary>
		public BaseCommand OpenWithWindowsCommand { get; private set; }

		/// <summary>
		///		Comando para copiar archivos / proyectos
		/// </summary>
		public BaseCommand CopyCommand { get; private set; }

		/// <summary>
		///		Comando para cortar archivos / proyectos
		/// </summary>
		public BaseCommand CutCommand { get; private set; }

		/// <summary>
		///		Comando para pegar archivos / proyectos
		/// </summary>
		public BaseCommand PasteCommand { get; private set; }

		/// <summary>
		///		Comando de pegar imagen
		/// </summary>
		public BaseCommand PasteImageCommand { get; private set; }

		/// <summary>
		///		Comando para cambiar nombre de archivo
		/// </summary>
		public BaseCommand RenameCommand { get; private set; }

		/// <summary>
		///		Comando de opciones de archivo
		/// </summary>
		public BaseCommand FileOptionsCommand { get; private set; }
	}
}
