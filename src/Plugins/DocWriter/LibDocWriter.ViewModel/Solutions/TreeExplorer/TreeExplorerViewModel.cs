using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.MVVM.ViewModels.TreeItems;
using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.Menus;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers.Common;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions.TreeExplorer
{
	/// <summary>
	///		ViewModel para el árbol de soluciones
	/// </summary>
	public class TreeExplorerViewModel : TreeViewModel
	{   
		// Constantes privadas
		private const string SolutionExtension = "dwsln";
		// Enumerados privados
		private enum NodeType
		{
			Unknown,
			FolderSolution,
			Project,
			Folder,
			FolderDocument,
			File
		}
		// Variables privadas
		private TreeViewItemsViewModelCollection _nodes;
		private ITreeViewItemViewModel _nodeToCopy;
		private bool _cutNode;

		public TreeExplorerViewModel()
		{
			Solution = LoadSolution(null);
			InitCommands();
			InitMenus();
			InitMessagesControl();
		}

		/// <summary>
		///		Inicializa los comandos
		/// </summary>
		private void InitCommands()
		{
			NewSolutionCommand = new BaseCommand("Nueva solución",
												 parameter => ExecuteAction(nameof(NewSolutionCommand) , parameter));
			OpenSolutionCommand = new BaseCommand("Abrir solución",
												  parameter => ExecuteAction(nameof(OpenSolutionCommand), parameter));
			NewProjectCommand = new BaseCommand("Nuevo proyecto",
												parameter => ExecuteAction(nameof(NewProjectCommand), parameter),
												parameter => CanExecuteAction(nameof(NewProjectCommand), parameter))
										.AddListener(this, nameof(SelectedItem));
			AddProjectCommand = new BaseCommand("Añadir proyecto", parameter => ExecuteAction(nameof(AddProjectCommand), parameter),
												parameter => CanExecuteAction(nameof(AddProjectCommand), parameter))
										.AddListener(this, nameof(SelectedItem));
			NewFolderCommand = new BaseCommand("Nueva carpeta", parameter => ExecuteAction(nameof(NewFolderCommand), parameter),
											   parameter => CanExecuteAction(nameof(NewFolderCommand), parameter))
										.AddListener(this, nameof(SelectedItem));
			NewFileCommand = new BaseCommand("Nuevo archivo", parameter => ExecuteAction(nameof(NewFileCommand), parameter),
											 parameter => CanExecuteAction(nameof(NewFileCommand), parameter))
										.AddListener(this, nameof(SelectedItem));
			NewReferenceCommand = new BaseCommand("Nueva referencia", parameter => ExecuteAction(nameof(NewReferenceCommand), parameter),
												  parameter => CanExecuteAction(nameof(NewReferenceCommand), parameter))
										.AddListener(this, nameof(SelectedItem));
			AddExistingFileCommand = new BaseCommand("Añadir archivo existente", parameter => ExecuteAction(nameof(AddExistingFileCommand), parameter),
													 parameter => CanExecuteAction(nameof(AddExistingFileCommand), parameter))
										.AddListener(this, nameof(SelectedItem));
			PropertiesCommand.AddListener(this, nameof(SelectedItem));
			CopyCommand = new BaseCommand("Copiar", parameter => ExecuteAction(nameof(CopyCommand), parameter),
										  parameter => CanExecuteAction(nameof(CopyCommand), parameter))
								.AddListener(this, nameof(SelectedItem));
			CutCommand = new BaseCommand("Cortar", parameter => ExecuteAction(nameof(CutCommand), parameter),
										 parameter => CanExecuteAction(nameof(CutCommand), parameter))
								.AddListener(this, nameof(SelectedItem));
			PasteCommand = new BaseCommand("Pegar", parameter => ExecuteAction(nameof(PasteCommand), parameter),
										   parameter => CanExecuteAction(nameof(PasteCommand), parameter))
								.AddListener(this, nameof(SelectedItem));
			PasteImageCommand = new BaseCommand("Pegar imagen", parameter => ExecuteAction(nameof(PasteImageCommand), parameter),
												parameter => CanExecuteAction(nameof(PasteImageCommand), parameter))
								.AddListener(this, nameof(SelectedItem));
			DeleteCommand.AddListener(this, nameof(SelectedItem));
			CompileCommand = new BaseCommand("Compilar", parameter => ExecuteAction(nameof(CompileCommand), parameter),
											 parameter => CanExecuteAction(nameof(CompileCommand), parameter))
									.AddListener(this, nameof(SelectedItem));
			CompileAndExecuteCommand = new BaseCommand("Compilar y ejecutar", parameter => ExecuteAction(nameof(CompileAndExecuteCommand), parameter),
													   parameter => CanExecuteAction(nameof(CompileAndExecuteCommand), parameter))
												.AddListener(this, nameof(SelectedItem));
			SeeAtExplorerCommand = new BaseCommand("Ver explorador", parameter => ExecuteAction(nameof(SeeAtExplorerCommand), parameter),
												   parameter => CanExecuteAction(nameof(SeeAtExplorerCommand), parameter))
											.AddListener(this, nameof(SelectedItem));
			RenameCommand = new BaseCommand("Cambiar nombre", parameter => ExecuteAction(nameof(RenameCommand), parameter),
											parameter => CanExecuteAction(nameof(RenameCommand), parameter))
									.AddListener(this, nameof(SelectedItem));
			ReferenceTransformCommand = new BaseCommand("Transformar referencia", parameter => ExecuteAction(nameof(ReferenceTransformCommand), parameter),
														parameter => CanExecuteAction(nameof(ReferenceTransformCommand), parameter))
												.AddListener(this, nameof(SelectedItem));
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
				menuGroup.MenuItems.Add("Abrir solución", DocWriterViewModel.Instance.GetIconRoute(DocWriterViewModel.IconIndex.OpenSolution), OpenSolutionCommand);
				// Añade los menús de la ventana principal. Archivo Nuevo
				menuGroup = base.MenuCompositionData.Menus.Add("Nuevo", MenuGroupViewModel.TargetMenuType.MainMenu,
															   MenuGroupViewModel.TargetMainMenuItemType.FileNewItems);
				menuGroup.MenuItems.Add("Nueva solución", DocWriterViewModel.Instance.GetIconRoute(DocWriterViewModel.IconIndex.NewProject),
										NewSolutionCommand);
				menuGroup.MenuItems.AddSeparator();
				menuGroup.MenuItems.Add("Nuevo proyecto", DocWriterViewModel.Instance.GetIconRoute(DocWriterViewModel.IconIndex.NewProject),
										NewProjectCommand);
				menuGroup.MenuItems.Add("Añadir proyecto existente", DocWriterViewModel.Instance.GetIconRoute(DocWriterViewModel.IconIndex.NewProject),
										AddProjectCommand);
				menuGroup.MenuItems.AddSeparator();
				menuGroup.MenuItems.Add("Nueva carpeta", DocWriterViewModel.Instance.GetIconRoute(DocWriterViewModel.IconIndex.NewFolder),
										NewFolderCommand);
				menuGroup.MenuItems.Add("Nuevo archivo", DocWriterViewModel.Instance.GetIconRoute(DocWriterViewModel.IconIndex.NewDocument),
										NewFileCommand);
				menuGroup.MenuItems.Add("Añadir referencia", DocWriterViewModel.Instance.GetIconRoute(DocWriterViewModel.IconIndex.AddReference),
										NewReferenceCommand);
				menuGroup.MenuItems.Add("Añadir archivo existente", DocWriterViewModel.Instance.GetIconRoute(DocWriterViewModel.IconIndex.NewDocument),
										AddExistingFileCommand);
				// Añade los menús de la ventana principal. Archivo
				menuGroup = MenuCompositionData.Menus.Add("Files", MenuGroupViewModel.TargetMenuType.MainMenu,
														  MenuGroupViewModel.TargetMainMenuItemType.FileAdditionalItems);
				menuGroup.MenuItems.Add("Compilar", DocWriterViewModel.Instance.GetIconRoute(DocWriterViewModel.IconIndex.Compile),
										CompileCommand);
				menuGroup.MenuItems.Add("Compilar y ejecutar comandos", DocWriterViewModel.Instance.GetIconRoute(DocWriterViewModel.IconIndex.Compile),
										CompileAndExecuteCommand);
		}

		/// <summary>
		///		Inicializa el controlador de mensajes
		/// </summary>
		private void InitMessagesControl()
		{
			DocWriterViewModel.Instance.HostController.Messenger.Sent += (sender, evntArgs) =>
					  {
						  var message = evntArgs.MessageSent as MessageRecentFileUsed;

							  if (message != null && message.Source.EqualsIgnoreCase(DocWriterViewModel.Instance.ModuleName))
								  OpenSolution(message.FileName);
					  };
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(NewSolutionCommand):
						OpenFormNewSolution();
					break;
				case nameof(OpenSolutionCommand):
						OpenFormOpenSolution();
					break;
				case nameof(NewFileCommand):
						OpenFormUpdateFile(GetSelectedProject(), GetSelectedFolder(), null);
					break;
				case nameof(AddExistingFileCommand):
						OpenFormAddExistingFile(GetSelectedProject(), GetSelectedFolder());
					break;
				case nameof(NewReferenceCommand):
						OpenFormUpdateReference(GetSelectedProject(), GetSelectedFolder());
					break;
				case nameof(NewFolderCommand):
						if (GetSelectedSolutionFolder() != null || (GetSelectedFolder() == null && GetSelectedProject() == null))
							OpenFormUpdateSolutionFolder(GetSelectedSolutionFolder(), null);
						else
							OpenFormUpdateFolder(GetSelectedProject(), GetSelectedFolder(), null);
					break;
				case nameof(NewProjectCommand):
						OpenFormUpdateProject(null);
					break;
				case nameof(AddProjectCommand):
						OpenFormAddExistingProject();
					break;
				case nameof(PropertiesCommand):
						switch (GetSelectedNodeType())
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
							case NodeType.FolderDocument:
									OpenFormUpdateFile(GetSelectedProject(), null, GetSelectedFile());
								break;
						}
					break;
				case nameof(CopyCommand):
				case nameof(CutCommand):
						AddFileToCopyBuffer(action == nameof(CutCommand));
					break;
				case nameof(PasteCommand):
						PasteFile();
					break;
				case nameof(PasteImageCommand):
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
						case NodeType.FolderDocument:
								DeleteFile((SelectedItem as FileNodeViewModel).File);
							break;
					}
					break;
				case nameof(RefreshCommand):
						Refresh();
					break;
				case nameof(CompileCommand):
				case nameof(CompileAndExecuteCommand):
						Compile(GetSelectedProject(), action == nameof(CompileAndExecuteCommand));
					break;
				case nameof(SeeAtExplorerCommand):
						OpenFileExplorer();
					break;
				case nameof(RenameCommand):
						Rename(GetSelectedFile());
					break;
				case nameof(ReferenceTransformCommand):
						ReferenceTransform(GetSelectedFile());
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
					case nameof(AddProjectCommand):
					case nameof(NewProjectCommand):
						return intNodeType == NodeType.Unknown || intNodeType == NodeType.FolderSolution;
					case nameof(NewFileCommand):
					case nameof(AddExistingFileCommand):
						return intNodeType == NodeType.Project || intNodeType == NodeType.Folder || intNodeType == NodeType.FolderDocument;
					case nameof(NewReferenceCommand):
						return intNodeType == NodeType.Project || intNodeType == NodeType.Folder;
					case nameof(NewFolderCommand):
						return intNodeType == NodeType.Unknown || intNodeType == NodeType.Project || intNodeType == NodeType.Folder ||
									 intNodeType == NodeType.FolderSolution || intNodeType == NodeType.FolderDocument;
					case nameof(PropertiesCommand):
					case nameof(DeleteCommand):
						return intNodeType != NodeType.Unknown;
					case nameof(CopyCommand):
					case nameof(CutCommand):
						return intNodeType == NodeType.Folder || intNodeType == NodeType.File || intNodeType == NodeType.FolderDocument || intNodeType == NodeType.Project;
					case nameof(PasteCommand):
						return _nodeToCopy != null &&
										(((intNodeType == NodeType.Project || intNodeType == NodeType.Folder ||
											 intNodeType == NodeType.File || intNodeType == NodeType.FolderDocument) && _nodeToCopy is FileNodeViewModel) ||
										 (intNodeType == NodeType.FolderSolution && _nodeToCopy is ProjectNodeViewModel));
					case nameof(PasteImageCommand):
						return (intNodeType == NodeType.Project || intNodeType == NodeType.Folder || intNodeType == NodeType.FolderDocument) &&
									 System.Windows.Clipboard.ContainsImage();
					case nameof(RefreshCommand):
						return true;
					case nameof(CompileCommand):
					case nameof(CompileAndExecuteCommand):
						return intNodeType == NodeType.Project || intNodeType == NodeType.Folder || intNodeType == NodeType.FolderDocument;
					case nameof(SeeAtExplorerCommand):
					case nameof(RenameCommand):
						return intNodeType == NodeType.Project || intNodeType == NodeType.Folder ||
									 intNodeType == NodeType.FolderDocument || intNodeType == NodeType.File;
					case nameof(ReferenceTransformCommand):
						return intNodeType == NodeType.File;
					default:
						return false;
				}
		}

		/// <summary>
		///		Añade un archivo al buffer de copia
		/// </summary>
		private void AddFileToCopyBuffer(bool blnCut)
		{
			_nodeToCopy = SelectedItem;
			_cutNode = blnCut;
		}

		/// <summary>
		///		Pega un archivo
		/// </summary>
		private void PasteFile()
		{
			if (_nodeToCopy != null)
			{ 
				// Copia el elemento que teníamos en memoria sobre el nodo seleccionado
				Copy(_nodeToCopy, SelectedItem, !_cutNode);
				// Indica que ya no hay ningún archivo que copiar
				_nodeToCopy = null;
			}
		}

		/// <summary>
		///		Copia un nodo sobre otro
		/// </summary>
		public void Copy(ITreeViewItemViewModel nodeSource, ITreeViewItemViewModel nodeTarget, bool blnCopy)
		{
			if (CanCopy(nodeSource, nodeTarget))
			{ 
				// Dependiendo de cuál sea el destino, llama a una rutina de copia
				if (nodeTarget is SolutionFolderNodeViewModel)
				{
					if (nodeSource is ProjectNodeViewModel)
						PasteProject(nodeSource as ProjectNodeViewModel,
									 (nodeTarget as SolutionFolderNodeViewModel).Folder, blnCopy);
					else if (nodeSource is SolutionFolderNodeViewModel)
						PasteSolution((nodeSource as SolutionFolderNodeViewModel).Folder,
									  (nodeTarget as SolutionFolderNodeViewModel).Folder, blnCopy);
				}
				else
					PasteFile(GetCopyFile(nodeSource), GetCopyFile(nodeTarget), blnCopy);
				// Actualiza
				Refresh();
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
						canCopy = true;
					else if (nodeTarget is FileNodeViewModel && nodeSource is FileNodeViewModel)
					{
						FileModel.DocumentType idTargetType = (nodeTarget as FileNodeViewModel).FileType;
						FileModel.DocumentType idSourceType = (nodeSource as FileNodeViewModel).File.FileType;

							if (idTargetType == FileModel.DocumentType.Folder)
								canCopy = true;
							else if ((idTargetType == FileModel.DocumentType.Document || idTargetType == FileModel.DocumentType.Tag) &&
									 (idSourceType == FileModel.DocumentType.Document || idSourceType == FileModel.DocumentType.Image))
								canCopy = true;
					}
				}
				// Devuelve el valor que indica si se puede copiar
				return canCopy;
		}

		/// <summary>
		///		Pega un proyecto en una carpeta de solución
		/// </summary>
		private void PasteProject(ProjectNodeViewModel projectSource, SolutionFolderModel folder, bool blnCopy)
		{
			if (projectSource != null)
			{
				ProjectModel project = projectSource.Project;

					// Añade el proyecto a la carpeta seleccionada o al raíz
					if (folder != null)
						folder.Projects.Add(project);
					else
						Solution.Projects.Add(project);
					// Si era una acción de cortar borra el proyecto de dónde estaba
					if (!blnCopy)
					{
						if (projectSource.Parent == null)
							Solution.Projects.Delete(project);
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
		private void PasteSolution(SolutionFolderModel folderSource, SolutionFolderModel folderTarget, bool blnCopy)
		{
			if (folderSource != null && folderTarget != null)
			{ 
				// Clona la carpeta origen sobre la destino 
				folderTarget.Folders.Add(folderSource.Clone());
				// Si se está cortando, elimina la carpeta origen
				if (!blnCopy)
					Solution.Delete(folderSource);
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
				return (nodeToCopy as ProjectNodeViewModel).Project.File;
			else if (nodeToCopy is FileNodeViewModel)
				return (nodeToCopy as FileNodeViewModel).File;
			else
				return null;
		}

		/// <summary>
		///		Pega un archivo
		/// </summary>
		private void PasteFile(FileModel fileToCopy, FileModel fileTarget, bool blnCopy)
		{
			if (fileToCopy != null)
			{
				string pathTarget = fileTarget.FullFileName;
				bool isCopied = false;

					// Obtiene el directorio destino
					if (!System.IO.Directory.Exists(pathTarget))
						pathTarget = System.IO.Path.GetDirectoryName(pathTarget);
					// Copia / mueve el archivo / carpeta
					if (fileToCopy.IsFolder || fileToCopy.IsDocumentFolder)
					{
						LibCommonHelper.Files.HelperFiles.CopyPath(fileToCopy.FullFileName,
															 LibCommonHelper.Files.HelperFiles.GetConsecutivePath(pathTarget,
																											System.IO.Path.GetFileName(fileToCopy.FullFileName)));
						isCopied = true; // ... supone que se ha podido copiar
					}
					else
						isCopied = LibCommonHelper.Files.HelperFiles.CopyFile(fileToCopy.FullFileName,
																		LibCommonHelper.Files.HelperFiles.GetConsecutiveFileName(pathTarget,
																														   System.IO.Path.GetFileName(fileToCopy.FullFileName)));
					// Si la acción es para cortar, elimina el archivo inicial
					if (isCopied && !blnCopy)
					{
						if (fileToCopy.IsFolder || fileToCopy.IsDocumentFolder)
							LibCommonHelper.Files.HelperFiles.KillPath(fileToCopy.FullFileName);
						else
							LibCommonHelper.Files.HelperFiles.KillFile(fileToCopy.FullFileName);
					}
			}
		}

		/// <summary>
		///		Abre el formulario de modificación de un proyecto
		/// </summary>
		private void OpenFormUpdateProject(ProjectModel project)
		{
			if (project == null)
			{
				if (DocWriterViewModel.Instance.ViewsController.OpenFormNewProject(Solution, GetSelectedSolutionFolder()) == SystemControllerEnums.ResultType.Yes)
					Refresh();
			}
			else
				DocWriterViewModel.Instance.ViewsController.OpenFormUpdateProject(project);
		}

		/// <summary>
		///		Abre el formulario de añadir proyecto existente
		/// </summary>
		private void OpenFormAddExistingProject()
		{
			string fileName = DocWriterViewModel.Instance.ViewsController.SearchFile(ProjectModel.FileName, "Archivos de proyecto (*.wdx)|*.wdx");

				if (fileName != null && System.IO.Path.GetFileName(fileName).EndsWith(ProjectModel.FileName, StringComparison.CurrentCultureIgnoreCase) &&
					System.IO.File.Exists(fileName))
				{ 
					// Añade el proyecto a la solución
					new Application.Bussiness.Solutions.ProjectFactory().Create(Solution, GetSelectedSolutionFolder(), fileName);
					// Actualiza el árbol
					Refresh();
				}
		}

		/// <summary>
		///		Abre el formulario de modificación / creación de una carpeta
		/// </summary>
		private void OpenFormUpdateFolder(ProjectModel project, FileModel folderParent, FileModel folder)
		{
			if (project == null)
				DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el proyecto al que desea añadir la carpeta");
			else if (DocWriterViewModel.Instance.ViewsController.OpenUpdateFolderView(project, folderParent, folder) == SystemControllerEnums.ResultType.Yes)
				Refresh();
		}

		/// <summary>
		///		Abre el formulario de modificación / creación de una carpeta de solución
		/// </summary>
		private void OpenFormUpdateSolutionFolder(SolutionFolderModel solutionFolderParent, SolutionFolderModel solutionFolder)
		{
			if (DocWriterViewModel.Instance.ViewsController.OpenUpdateFolderSolutionView(Solution, solutionFolderParent, solutionFolder) == SystemControllerEnums.ResultType.Yes)
				SaveSolution();
		}

		/// <summary>
		///		Abre el formulario de modificación / creación de un archivo
		/// </summary>
		private void OpenFormUpdateFile(ProjectModel project, FileModel folderParent, FileModel file)
		{
			if (project == null)
				DocWriterViewModel.Instance.ControllerWindow.ShowMessage("No se ha seleccionado ningún proyecto");
			else if (DocWriterViewModel.Instance.ViewsController.OpenFormUpdateFile(Solution,
																					project, folderParent, file) == SystemControllerEnums.ResultType.Yes)
			{
				if (file == null)
					Refresh();
			}
		}

		/// <summary>
		///		Abre el formulario para añadir un archivo existente
		/// </summary>
		private void OpenFormAddExistingFile(ProjectModel project, FileModel file)
		{
			string [] files = DocWriterViewModel.Instance.DialogsController.OpenDialogLoadFilesMultiple(project.File.Path, "Todos los archivos|*.*");

			if (files != null && files.Length > 0)
			{
				string pathTarget;

					// Obtiene el directorio destino
					if (file == null)
						pathTarget = project.File.Path;
					else
						pathTarget = file.Path;
					// Copia los archivos	
					foreach (string fileName in files)
						LibCommonHelper.Files.HelperFiles.CopyFile(fileName, LibCommonHelper.Files.HelperFiles.GetConsecutiveFileName(pathTarget, fileName));
					// Actualiza el árbol
					Refresh();
			}
		}

		/// <summary>
		///		Abre el formulario de modificación de referencias
		/// </summary>
		private void OpenFormUpdateReference(ProjectModel project, FileModel folderParent)
		{
			if (project == null)
				DocWriterViewModel.Instance.ControllerWindow.ShowMessage("No se ha seleccionado ningún proyecto");
			else if (DocWriterViewModel.Instance.ViewsController.OpenFormNewReference(project, folderParent) == SystemControllerEnums.ResultType.Yes)
				Refresh();
		}

		/// <summary>
		///		Pega la imagen
		/// </summary>
		private void PasteImage(FileModel file)
		{
			if (!new Helper.ClipboardHelper().PasteImage(file.Path, file.Project.MaxWidthImage, file.Project.ThumbsWidth, out string error))
			{
				if (!error.IsEmpty())
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage(error);
			}
			else
				Refresh();
		}

		/// <summary>
		///		Quita un proyecto de la solución
		/// </summary>
		private void DeleteProject(ProjectModel project)
		{
			if (DocWriterViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea borrar el proyecto {project.Name}?"))
			{ 
				// Borra el proyecto de la solución
				Solution.Delete(project);
				// Graba la solución
				SaveSolution();
			}
		}

		/// <summary>
		///		Quita una carpeta de solución
		/// </summary>
		private void DeleteSolutionFolder(SolutionFolderModel folder)
		{
			if (folder != null && DocWriterViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea borrar la carpeta {folder.Name}?"))
			{ 
				// Borra la carpeta de la solución
				Solution.Delete(folder);
				// Graba la solución
				SaveSolution();
			}
		}

		/// <summary>
		///		Abre el formulario de apertura de una solución
		/// </summary>
		private void OpenFormOpenSolution()
		{
			string fileName = DocWriterViewModel.Instance.DialogsController.OpenDialogLoad
															  (DocWriterViewModel.Instance.PathData, GetFilterSolution(),
															   "Solution." + SolutionExtension, SolutionExtension);

			if (!fileName.IsEmpty())
			{
				if (!System.IO.File.Exists(fileName))
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Seleccione un nombre de archivo");
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
		///		Abre una solución
		/// </summary>
		private void OpenSolution(string fileName)
		{   
			// Carga la solución
			Solution = LoadSolution(fileName);
			// Cambia la última solución activa
			DocWriterViewModel.Instance.LastFileSolution = fileName;
			DocWriterViewModel.Instance.HostController.Configuration.Save();
			// Actualiza el árbol
			Refresh();
		}

		/// <summary>
		///		Carga una solución
		/// </summary>
		private SolutionModel LoadSolution(string fileName = null)
		{
			SolutionModel solution = new SolutionModel();

				// Obtiene el nombre de la solución
				if (fileName.IsEmpty())
					fileName = DocWriterViewModel.Instance.LastFileSolution;
				if (fileName.IsEmpty())
					fileName = System.IO.Path.Combine(DocWriterViewModel.Instance.PathData, "Solution." + SolutionExtension);
				if (!System.IO.File.Exists(fileName)) // ... por las antiguas
					fileName = System.IO.Path.Combine(DocWriterViewModel.Instance.PathData, "Solutions.xml");
				// Carga la solución
				try
				{
					solution = new Application.Bussiness.Solutions.SolutionBussiness().Load(fileName);
				}
				catch (Exception exception)
				{
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage($"Error al cargar la solución: {fileName}.{Environment.NewLine}{exception.Message}");
					solution = new SolutionModel();
					solution.FullFileName = fileName;
				}
				// Devuelve la solución
				return solution;
		}

		/// <summary>
		///		Abre el formulario de creación de una solución
		/// </summary>
		private void OpenFormNewSolution()
		{
			string fileName = DocWriterViewModel.Instance.DialogsController.OpenDialogSave
																  (DocWriterViewModel.Instance.PathData, GetFilterSolution(),
																   "Solution", SolutionExtension);

			if (!fileName.IsEmpty())
			{ 
				// Crea el directorio y el archivo inicial
				LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileName));
				new Application.Bussiness.Solutions.SolutionBussiness().Save(new SolutionModel { FullFileName = fileName });
				// Cambia la solución activa
				OpenSolution(fileName);
				// Envía el mensaje de solución abierta
				SendMessageOpenSolution(fileName);
			}
		}

		/// <summary>
		///		Abre el mensaje de solución abierta
		/// </summary>
		private void SendMessageOpenSolution(string fileName)
		{
			DocWriterViewModel.Instance.HostController.Messenger.SendRecentFileOpened(DocWriterViewModel.Instance.ModuleName,
																					  fileName, fileName);
		}

		/// <summary>
		///		Obtiene el filtro para los cuadros de apertura y grabación de soluciones
		/// </summary>
		private string GetFilterSolution()
		{
			return $"Archivos de solución|*.{SolutionExtension}|Todos los archivos|*.*";
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
			if (DocWriterViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea borrar {file.Name}?"))
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
		///		Compila un proyecto
		/// </summary>
		private void Compile(ProjectModel project, bool executePostProcess)
		{
			if (project == null)
				DocWriterViewModel.Instance.ControllerWindow.ShowMessage("No se ha seleccionado ningún proyecto");
			else if (DocWriterViewModel.Instance.HostController.TasksProcessor.ExistsByType(typeof(Controllers.DocWriterCompilerProcessor)))
				DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Ya se está compilando un proyecto");
			else
			{
				Controllers.DocWriterCompilerProcessor processor;

					// Si se debe grabar antes de compilar, lo graba todo
					if (DocWriterViewModel.Instance.SaveBeforeCompile)
						DocWriterViewModel.Instance.ViewsController.SaveAllDocuments();
					// Actualiza el proyecto
					project = new Application.Bussiness.Solutions.ProjectBussiness().Load(project.Solution, project.File.FullFileName);
					// Crea el procesador
					processor = new Controllers.DocWriterCompilerProcessor(DocWriterViewModel.Instance.ModuleName,
																		   project.Solution, project,
																		   DocWriterViewModel.Instance.PathGeneration,
																		   DocWriterViewModel.Instance.Minimize);
					// Asigna el manejador de eventos
					processor.EndProcess += (sender, evntArgs) => TreatEndCompile(processor, project, executePostProcess);
					// Añade el procesador a la cola
					DocWriterViewModel.Instance.HostController.TasksProcessor.Process(processor);
			}
		}

		/// <summary>
		///		Trata el evento de fin de compilación
		/// </summary>
		private void TreatEndCompile(Controllers.DocWriterCompilerProcessor processor, ProjectModel project, bool executePostProcess)
		{
			if (processor.Generator.Errors.Count > 0)
			{ 
				// Lanza los mensajes de error
				foreach (Processor.Errors.ErrorMessage error in processor.Generator.Errors)
					DocWriterViewModel.Instance.HostController.Messenger.SendError
															(project.Name, error.FileName, $"{error.Message} (Token: {error.Token})",
															 error.Row, error.Column, null);
			}
			else
			{
				string pageMain;

					// Normaliza el nombre de la página principal
					if (project.PageMain.IsEmpty())
						project.PageMain = "index.htm";
					// Crea el nombre de la página principal
					pageMain = System.IO.Path.Combine(processor.Generator.PathTarget, project.PageMain);
					// Llama al navegador para que abra la Web
					if (DocWriterViewModel.Instance.OpenExternalWebBrowser)
						LibSystem.Files.WindowsFiles.OpenBrowser(pageMain);
					else
						DocWriterViewModel.Instance.ViewsController.ShowWebBrowser(pageMain);
					// Procesa los comandos
					if (executePostProcess)
						ProcessCommands(project);
			}
		}

		/// <summary>
		///		Procesa los comandos
		/// </summary>
		private void ProcessCommands(ProjectModel project)
		{
			if (!project.PostCompileCommands.IsEmpty())
			{
				string [] commands = project.PostCompileCommands.Split('\n');
				bool hasError = false;

					// Ejecuta los comandos
					foreach (string command in commands)
						if (!hasError)
							hasError = ExecuteCommand(command.TrimIgnoreNull());
			}
		}

		/// <summary>
		///		Ejecuta un comando
		/// </summary>
		private bool ExecuteCommand(string command)
		{
			bool hasError = false;

				// Quita los espacios
				command = command.TrimIgnoreNull();
				// Si no es un comentario...
				if (!command.IsEmpty() && !command.StartsWith("//") && !command.StartsWith("_"))
				{
					if (command.StartsWith("PLUGIN", StringComparison.CurrentCultureIgnoreCase))
						DocWriterViewModel.Instance.HostController.Messenger.SendCommand(DocWriterViewModel.Instance.ModuleName,
																						 command.Right(command.Length - "PLUGIN".Length), null);
					else
						try
						{
							// Log
							DocWriterViewModel.Instance.HostController.Messenger.SendLog(DocWriterViewModel.Instance.ModuleName,
																						 MessageLog.LogType.Information,
																						 "Ejecución del comando", command, null);
							// Ejecuta el comando
							LibSystem.Files.WindowsFiles.ExecuteApplication(command.TrimIgnoreNull());
							// Log
							DocWriterViewModel.Instance.HostController.Messenger.SendLog(DocWriterViewModel.Instance.ModuleName,
																						 MessageLog.LogType.Information,
																						 "Fin de la ejecución", command, null);
						}
						catch (Exception exception)
						{
							DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Error al ejecutar el comando de posptroceso: " + command +
																					 Environment.NewLine + exception.Message);
							hasError = true;
						}
				}
				// Devuelve el valor que indica si ha habido algún error
				return hasError;
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
							LibSystem.Files.WindowsFiles.ExecuteApplication("explorer.exe", path);
				}
		}

		/// <summary>
		///		Cambia el nombre de un archivo
		/// </summary>
		private void Rename(FileModel file)
		{
			if (DocWriterViewModel.Instance.ViewsController.OpenFormChangeFileName(file) == SystemControllerEnums.ResultType.Yes)
				Refresh();
		}

		/// <summary>
		///		Transforma una referencia en un archivo
		/// </summary>
		private void ReferenceTransform(FileModel file)
		{
			if (file.FileType != FileModel.DocumentType.Reference)
				DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Seleccione un archivo de referencia");
			else
			{
				Model.Documents.ReferenceModel reference = new Application.Bussiness.Documents.ReferenceBussiness().Load(file);

					// Busca el archivo al que se hace referencia
					if (!reference.FileNameReference.IsEmpty())
					{
						string fileName = new Application.Bussiness.Documents.ReferenceBussiness().GetFileName(Solution, reference);

							// Modifica el archivo de referencia
							if (fileName.IsEmpty() || !System.IO.File.Exists(fileName))
								DocWriterViewModel.Instance.ControllerWindow.ShowMessage("No se encuentra el archivo al que se hacer referencia");
							else if (!LibCommonHelper.Files.HelperFiles.CopyFile(fileName,
																		   System.IO.Path.Combine(System.IO.Path.GetDirectoryName(file.FullFileName),
																								  System.IO.Path.GetFileNameWithoutExtension(file.FullFileName))))
								DocWriterViewModel.Instance.ControllerWindow.ShowMessage($"No se ha podido transformar: {reference.FileNameReference}");
							else
							{ 
								// Borra la referencia inicial
								LibCommonHelper.Files.HelperFiles.KillFile(file.FullFileName);
								// Actualiza los nodos
								Refresh();
							}
					}
			}
		}

		/// <summary>
		///		Carga los nodos
		/// </summary>
		protected override TreeViewItemsViewModelCollection LoadNodes()
		{
			TreeViewItemsViewModelCollection nodes = new TreeViewItemsViewModelCollection();

				// Ordena los nodos
				Solution.Folders.SortByName();
				Solution.Projects.SortByName();
				// Obtiene los nodos de las carpetas de solución
				foreach (SolutionFolderModel folder in Solution.Folders)
					nodes.Add(new SolutionFolderNodeViewModel(folder));
				// Obtiene los nodos de los proyectos de solución
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

							if (fileNode.File.IsDocumentFolder) // ... debe estar el primero
								type = NodeType.FolderDocument;
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
				else if (type == NodeType.Folder || type == NodeType.File || type == NodeType.FolderDocument)
					return (SelectedItem as FileNodeViewModel).File.Project;
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

				if (type == NodeType.Folder || type == NodeType.File || type == NodeType.FolderDocument)
					return (SelectedItem as FileNodeViewModel).File;
				else if (type == NodeType.Project)
					return (SelectedItem as ProjectNodeViewModel).Project.File;
				else
					return null;
		}

		/// <summary>
		///		Solución base
		/// </summary>
		public SolutionModel Solution { get; private set; }

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
		///		Comando de abrir solución
		/// </summary>
		public BaseCommand OpenSolutionCommand { get; private set; }

		/// <summary>
		///		Comando de nueva solución
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
		///		Comando de nueva referencia
		/// </summary>
		public BaseCommand NewReferenceCommand { get; private set; }

		/// <summary>
		///		Comando para añadir archivo existente
		/// </summary>
		public BaseCommand AddExistingFileCommand { get; private set; }

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
		///		Comando de compilación
		/// </summary>
		public BaseCommand CompileCommand { get; private set; }

		/// <summary>
		///		Comando de compilación y ejecución de procesos posteriores
		/// </summary>
		public BaseCommand CompileAndExecuteCommand { get; private set; }

		/// <summary>
		///		Comando para ver el archivo en el explorador
		/// </summary>
		public BaseCommand SeeAtExplorerCommand { get; private set; }

		/// <summary>
		///		Comando para cambiar nombre de archivo
		/// </summary>
		public BaseCommand RenameCommand { get; private set; }

		/// <summary>
		///		Comando para transformar una referencia en un archivo normal
		/// </summary>
		public BaseCommand ReferenceTransformCommand { get; private set; }
	}
}
