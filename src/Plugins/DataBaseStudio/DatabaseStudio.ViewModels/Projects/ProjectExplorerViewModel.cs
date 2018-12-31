using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Media;
using Bau.Libraries.DatabaseStudio.Application.Bussiness;
using Bau.Libraries.DatabaseStudio.Models;
using Bau.Libraries.DatabaseStudio.Models.Connections;
using Bau.Libraries.DatabaseStudio.Models.Deployment;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Connections;
using Bau.Libraries.PlugStudioProjects.Models;
using Bau.Libraries.PlugStudioProjects.ViewModels;
using Bau.Libraries.LibDbProviders.Base.Schema;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Projects
{
	/// <summary>
	///		Arbol de explorador de un proyecto
	/// </summary>
	public class ProjectExplorerViewModel : ExplorerProjectViewModel
	{
		/// <summary>
		///		Tipo de nodo
		/// </summary>
		public enum NodeType
		{
			/// <summary>Desconocido. No se debería utilizar</summary>
			Unknown,
			/// <summary>Proyecto</summary>
			Project,
			/// <summary>Raíz de conexiones</summary>
			ConnectionsRoot,
			/// <summary>Conexión</summary>
			Connection,
			/// <summary>Raíz de distribuciones</summary>
			DeploymentsRoot,
			/// <summary>Distribuciones</summary>
			Deployment,
			/// <summary>Raíz para las tablas</summary>
			TablesRoot,
			/// <summary>Tabla</summary>
			Table,
			/// <summary>Campo</summary>
			Field,
			/// <summary>Raíz para las vistas</summary>
			ViewsRoot,
			/// <summary>Vista</summary>
			View,
			/// <summary>Raíz para los procedimientos</summary>
			ProceduresRoot,
			/// <summary>Procedimiento</summary>
			Procedure,
			/// <summary>Carpeta</summary>
			Folder,
			/// <summary>Archivo</summary>
			File,
			/// <summary>Archivo de estilo</summary>
			Style,
			/// <summary>Script de importación</summary>
			ImportScript,
			/// <summary>Consulta</summary>
			Query,
			/// <summary>Informe</summary>
			Report,
			/// <summary>Imagen</summary>
			Image
		}
		/// <summary>
		///		Elementos del menú
		/// </summary>
		public enum MenuType
		{
			/// <summary>Desconocido. No se debería utilizar</summary>
			Unknown,
			/// <summary>Nuevo proyecto</summary>
			NewProject,
			/// <summary>Nueva conexión</summary>
			NewConnection,
			/// <summary>Nuevo elemento de distribución</summary>
			NewDeployment,
			/// <summary>Enviar los scripts a producción</summary>
			Send,
			/// <summary>Procesar un script</summary>
			Process
		}
		// Variables privadas
		private string _script;
		private string _parameters;

		public ProjectExplorerViewModel(PlugStudioProjects.Controllers.IPlugStudioController plugStudioController) : base(plugStudioController)
		{
			// Inicializa los datos
			Project = new ProjectModel();
			// Carga el proyecto
			LoadProject(MainViewModel.Instance.LastProject);
			// Inicializa los comandos
			SendCommand = new BaseCommand(parameter => ProcessFile(),
										  parameter => CanExecuteCommand(nameof(SendCommand)))
										.AddListener(this, nameof(SelectedNode));
			ProcessCommand = new BaseCommand(parameter => ProcessFile(),
											 parameter => CanExecuteCommand(nameof(ProcessCommand)))
										.AddListener(this, nameof(SelectedNode));
		}

		/// <summary>
		///		Crea la definición de proyecto
		/// </summary>
		protected override ProjectItemDefinitionModel CreateProjectDefinition()
		{
			return new PlugStudioProjects.Builders.ProjectItemDefinitionBuilder(NodeType.Project.ToString(), "Database",
																				ProjectItemDefinitionModel.ItemType.Project)
							.WithIcon(MainViewModel.Instance.ViewsController.GetIcon(NodeType.Project))
							.WithForeground(MvvmColor.Red)
							.WithMenu(MenuModel.MenuType.NewFile, MenuType.NewProject.ToString(), "Proyecto",
									  MainViewModel.Instance.ViewsController.GetIcon(MenuType.NewProject))
							.WithMenu(MenuModel.MenuType.NewFile, MenuType.NewConnection.ToString(), "Conexión",
									  MainViewModel.Instance.ViewsController.GetIcon(MenuType.NewConnection))
							.WithMenu(MenuModel.MenuType.NewFile, MenuType.NewDeployment.ToString(), "Distribución",
									  MainViewModel.Instance.ViewsController.GetIcon(MenuType.NewDeployment))
							.WithBold()
							.WithItem(NodeType.ConnectionsRoot.ToString(), "Conexiones", ProjectItemDefinitionModel.ItemType.Fixed)
								.WithIcon(MainViewModel.Instance.ViewsController.GetIcon(NodeType.Folder))
								.WithBold()
								.WithForeground(MvvmColor.Navy)
								.WithMenu(MenuModel.MenuType.NewFile, MenuType.NewConnection.ToString(), "Conexión",
										  MainViewModel.Instance.ViewsController.GetIcon(MenuType.NewConnection))
								.WithItem(NodeType.Connection.ToString(), "Conexión", ProjectItemDefinitionModel.ItemType.Fixed)
									.WithIcon(MainViewModel.Instance.ViewsController.GetIcon(NodeType.Connection))
									.Back()
								.Back()
							.WithItem(NodeType.DeploymentsRoot.ToString(), "Distribuciones", ProjectItemDefinitionModel.ItemType.Fixed)
								.WithIcon(MainViewModel.Instance.ViewsController.GetIcon(NodeType.Folder))
								.WithBold()
								.WithForeground(MvvmColor.Navy)
								.WithMenu(MenuModel.MenuType.NewFile, MenuType.NewDeployment.ToString(), "Distribución",
										  MainViewModel.Instance.ViewsController.GetIcon(MenuType.NewDeployment))
								.WithItem(NodeType.Deployment.ToString(), "Distribución", ProjectItemDefinitionModel.ItemType.Fixed)
									.WithIcon(MainViewModel.Instance.ViewsController.GetIcon(NodeType.Deployment))
									.WithMenu(MenuModel.MenuType.Command, MenuType.Send.ToString(), "Enviar",
											  MainViewModel.Instance.ViewsController.GetIcon(MenuType.Send))
									.WithMenu(MenuModel.MenuType.Command, MenuType.Process.ToString(), "Procesar",
											  MainViewModel.Instance.ViewsController.GetIcon(MenuType.Process))
									.Back()
								.Back()
							.WithItem(NodeType.Folder.ToString(), "Carpeta", ProjectItemDefinitionModel.ItemType.Folder)
								.WithIcon(MainViewModel.Instance.ViewsController.GetIcon(NodeType.Folder))
								.WithForeground(MvvmColor.Navy)
								.Back()
							.WithItem(NodeType.Report.ToString(), "Informe", ProjectItemDefinitionModel.ItemType.File)
								.WithIcon(MainViewModel.Instance.ViewsController.GetIcon(NodeType.Report))
								.WithExtension(ProjectModel.ReportExtension)
								.WithEditor(ProjectItemDefinitionModel.WindowEditorType.Script)
								.WithHelpFile(MainViewModel.Instance.ViewsController.GetHelpFileName(NodeType.Report))
								.WithTemplate(MainViewModel.Instance.ViewsController.GetHelpFileName(NodeType.Report))
								.WithMenu(MenuModel.MenuType.Command, MenuType.Process.ToString(), "Procesar",
										  MainViewModel.Instance.ViewsController.GetIcon(MenuType.Process))
								.Back()
							.WithItem(NodeType.Query.ToString(), "Query", ProjectItemDefinitionModel.ItemType.File)
								.WithIcon(MainViewModel.Instance.ViewsController.GetIcon(NodeType.Query))
								.WithExtension(ProjectModel.QueryExtension)
								.Back()
							.WithItem(NodeType.ImportScript.ToString(), "Script de importación", ProjectItemDefinitionModel.ItemType.File)
								.WithIcon(MainViewModel.Instance.ViewsController.GetIcon(NodeType.ImportScript))
								.WithExtension(ProjectModel.ImportScriptExtension)
								.WithEditor(ProjectItemDefinitionModel.WindowEditorType.Script)
								.WithTemplate(MainViewModel.Instance.ViewsController.GetTemplate(NodeType.ImportScript))
								.WithMenu(MenuModel.MenuType.Command, MenuType.Process.ToString(), "Procesar",
										  MainViewModel.Instance.ViewsController.GetIcon(MenuType.Process))
								.Back()
							.WithItem(NodeType.Style.ToString(), "Estilo", ProjectItemDefinitionModel.ItemType.File)
								.WithIcon(MainViewModel.Instance.ViewsController.GetIcon(NodeType.Style))
								.WithExtension(ProjectModel.StyleExtension)
								.WithEditor(ProjectItemDefinitionModel.WindowEditorType.Script)
								.Back()
							.WithItem(NodeType.Image.ToString(), "Imagen", ProjectItemDefinitionModel.ItemType.File)
								.WithIcon(MainViewModel.Instance.ViewsController.GetIcon(NodeType.Image))
								.WithEditor(ProjectItemDefinitionModel.WindowEditorType.Image)
								.WithExtension(".png;.bmp;.gif;.tiff;.tif;.jpg")
								.Back()
						.Build();
		}

		/// <summary>
		///		Carga el proyecto
		/// </summary>
		private void LoadProject(string projectPath)
		{
			// Crea un nuevo proyecto
			Project = new ProjectModel();
			IsProjectLoaded = false;
			// Carga los datos del proyecto
			if (!string.IsNullOrEmpty(projectPath) && System.IO.Directory.Exists(projectPath))
			{
				// Carga el proyecto
				Project = new ProjectBussiness().Load(projectPath);
				// Guarda el último proyecto
				MainViewModel.Instance.LastProject = projectPath;
				MainViewModel.Instance.HostController.Configuration.Save();
				// Indica que se ha cargado el proyecto
				IsProjectLoaded = true;
				ProjectPath = projectPath;
				// Carga los nodos
				LoadNodes(ProjectPath);
			}
		}

		/// <summary>
		///		Carga los nodos (refresh)
		/// </summary>
		protected override void LoadNodes()
		{
			LoadNodes(ProjectPath);
		}

		/// <summary>
		///		Carga los nodos
		/// </summary>
		private void LoadNodes(string projectPath)
		{
			ExplorerProjectNodeViewModel root;

				// Limpia los nodos hijo
				Children.Clear();
				// Añade el nodo de proyecto
				root = AddNode(null, Project.Name, NodeType.Project.ToString(), Project);
				root.IsExpanded = true;
				// Añade las conexiones y distribuciones
				LoadConnectionNodes(root, Project);
				LoadDeploymentNodes(root, Project);
				// Añade los archivos
				LoadProjectFiles(root, projectPath, NodeType.Folder.ToString());
		}

		/// <summary>
		///		Carga los nodos de conexión
		/// </summary>
		private void LoadConnectionNodes(ExplorerProjectNodeViewModel parent, ProjectModel project)
		{
			ExplorerProjectNodeViewModel root = AddNode(parent, "Conexiones", NodeType.ConnectionsRoot.ToString(), null);

				// Añade las conexiones
				foreach (AbstractConnectionModel connection in project.Connections)
					AddNode(root, connection.Name, NodeType.Connection.ToString(), connection, true);
		}

		/// <summary>
		///		Carga los nodos de distribución
		/// </summary>
		private void LoadDeploymentNodes(ExplorerProjectNodeViewModel parent, ProjectModel project)
		{
			ExplorerProjectNodeViewModel root = AddNode(parent, "Distribuciones", NodeType.DeploymentsRoot.ToString(), null);

				// Añade los nodos de distribución
				foreach (DeploymentModel deployment in project.Deployments)
				{
					ExplorerProjectNodeViewModel node = AddNode(root, deployment.Name, NodeType.Deployment.ToString(), deployment);

						// Añade las conexiones
						foreach (KeyValuePair<string, DatabaseConnectionModel> connectionKey in deployment.Connections)
							AddNode(node, connectionKey.Value.Name, NodeType.Connection.ToString(), connectionKey.Value, false);
				}
		}

		/// <summary>
		///		Carga los nodos hijo
		/// </summary>
		protected override void LoadChildrenNodes(ExplorerProjectNodeViewModel node)
		{
			switch (node.Tag)
			{
				case DatabaseConnectionModel connection:
						LoadSchemaNodes(node, connection);
					break;
			}
		}

		/// <summary>
		///		Carga los nodos de esquema
		/// </summary>
		private void LoadSchemaNodes(ExplorerProjectNodeViewModel node, DatabaseConnectionModel connection)
		{
			try
			{
				SchemaDbModel schema = new Application.Providers.DataProvider().LoadSchema(connection);

					LoadSchemaNodes(node, "Tablas", schema.Tables, NodeType.TablesRoot, NodeType.Table);
					LoadSchemaNodes(node, "Vistas", schema.Views, NodeType.ViewsRoot, NodeType.Connection);
					AddNode(node, "Procedimientos", null, true, MvvmColor.Red, MainViewModel.Instance.ViewsController.GetIcon(NodeType.ProceduresRoot));
			}
			catch (Exception exception)
			{
				MainViewModel.Instance.ControllerWindow.ShowMessage($"Error al cargar el esquema {exception.Message}");
			}
		}

		/// <summary>
		///		Carga los nodos de esquema de un tipo
		/// </summary>
		private void LoadSchemaNodes(ExplorerProjectNodeViewModel node, string rootText, List<TableDbModel> tables, NodeType iconRoot, NodeType iconItem)
		{
			ExplorerProjectNodeViewModel root = AddNode(node, rootText, null, true, MvvmColor.Red,
														MainViewModel.Instance.ViewsController.GetIcon(iconRoot));

				// Ordena las tablas
				tables.Sort((first, second) => first.Name.CompareTo(second.Name));
				// Añade la información de las tablas
				foreach (TableDbModel table in tables)
				{
					ExplorerProjectNodeViewModel nodeTable = AddNode(root, table.Name, null, true, MvvmColor.Navy,
																	 MainViewModel.Instance.ViewsController.GetIcon(iconItem));

						// Ordena los campos
						table.Fields.Sort((first, second) => first.Name.CompareTo(second.Name));
						// Añade los campos
						foreach (FieldDbModel field in table.Fields)
							AddNode(nodeTable, field.Name, null, false, MvvmColor.Black,
									MainViewModel.Instance.ViewsController.GetIcon(NodeType.Field));
				}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar un comando
		/// </summary>
		private bool CanExecuteCommand(string command)
		{
			switch (command)
			{
				case nameof(SendCommand):
				case nameof(ProcessCommand):
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///		Ejecuta una opción de menú
		/// </summary>
		protected override void ExecuteMenuOption(MenuModel menuItem)
		{
			if (menuItem != null)
				switch (menuItem.Key.GetEnum(MenuType.Unknown))
				{
					case MenuType.NewProject:
							NewProject();
						break;
					case MenuType.NewConnection:
							OpenFormConnection(null);
						break;
					case MenuType.NewDeployment:
							OpenFormDeployment(null);
						break;
					case MenuType.Send:
							SendDeployment();
						break;
					case MenuType.Process:
							ProcessFile();
						break;
				}
		}

		/// <summary>
		///		Comprueba si puede ejecutar una opción de menú
		/// </summary>
		protected override bool CanExecuteMenuOption(MenuModel menuItem)
		{
			return true;
		}

		/// <summary>
		///		Crea un proyecto
		/// </summary>
		protected override void NewProject()
		{
			string project = "NewProject"; 

				if (MainViewModel.Instance.ControllerWindow.ShowInputString("Introduzca el nombre del proyecto", ref project) == BauMvvm.ViewModels.Controllers.SystemControllerEnums.ResultType.Yes &&
					!string.IsNullOrEmpty(project))
				{
					string path = System.IO.Path.Combine(MainViewModel.Instance.ProjectsPathBase, project);

						if (System.IO.Directory.Exists(path))
							MainViewModel.Instance.ControllerWindow.ShowMessage("Ya existe un directorio con ese nombre de proyecto");
						else
						{
							// Crea el directorio
							LibCommonHelper.Files.HelperFiles.MakePath(path);
							// y carga el proyecto
							LoadProject(path);
						}
				}
		}

		/// <summary>
		///		Abre el formulario de mantenimiento de una conexión
		/// </summary>
		private void OpenFormConnection(DatabaseConnectionModel connection)
		{
			DataBaseConnectionViewModel viewModel = new DataBaseConnectionViewModel(connection);

				if (MainViewModel.Instance.ViewsController.OpenConnectionView(viewModel))
				{
					// Añade la conexión si no existía
					if (connection == null)
						Project.Connections.Add(viewModel.Connection);
					// Graba el proyecto y actualiza
					Save();
				}
		}

		/// <summary>
		///		Abre el formulario de mantenimiento de una distribución
		/// </summary>
		private void OpenFormDeployment(DeploymentModel deployment)
		{
			MainViewModel.Instance.ViewsController.OpenDeploymentView(new Deployments.DeploymentViewModel(this, deployment));
		}

		/// <summary>
		///		Indica si se puede abrir el nodo seleccionado
		/// </summary>
		protected override bool CanOpen()
		{
			switch (GetSelectedNodeType())
			{
				case NodeType.Project:
				case NodeType.Connection:
				case NodeType.Deployment:
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///		Abre el formulario de propiedades del elemento actual
		/// </summary>
		protected override void OpenProperties()
		{
			switch (SelectedNode?.Tag)
			{
				case DatabaseConnectionModel connection:
						OpenFormConnection(connection);
					break;
				case DeploymentModel deployment:
						OpenFormDeployment(deployment);
					break;
				default:
						switch (SelectedNodeDefinition?.Type)
						{
							case ProjectItemDefinitionModel.ItemType.File:
									OpenProperties(GetNodeFileName());
								break;
						}
					break;
			}
		}

		/// <summary>
		///		Abre el formulario de propiedades de un archivo con el editor del proyecto
		/// </summary>
		protected override void OpenProperties(string fileName)
		{
			if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(System.IO.Path.GetExtension(fileName)))
				switch (System.IO.Path.GetExtension(fileName).ToLower())
				{
					case ProjectModel.ImportScriptExtension:
					case ProjectModel.StyleExtension:
							PlugStudioController.OpenEditor(fileName, string.Empty, string.Empty);
						break;
					case ProjectModel.QueryExtension:
							MainViewModel.Instance.ViewsController.OpenQueryView(new Queries.QueryViewModel(Project, fileName));
						break;
					case ProjectModel.ReportExtension:
							MainViewModel.Instance.ViewsController.OpenReportView(new Reports.ReportViewModel(fileName));
						break;
				}
		}

		/// <summary>
		///		Indica si se puede borrar el nodo seleccionado
		/// </summary>
		protected override bool CanDelete()
		{
			NodeType type = GetSelectedNodeType();

				return type == NodeType.Project || type == NodeType.Connection || type == NodeType.Deployment;
		}

		/// <summary>
		///		Borra el elemento seleccionado
		/// </summary>
		protected override void DeleteFixedNode()
		{
			if (CanDelete() && MainViewModel.Instance.ControllerWindow.ShowQuestion("¿Realmente desea eliminar este elemento?"))
			{
				// Borra el elemento
				switch (SelectedNode.Tag)
				{
					case AbstractConnectionModel item:
							Project.Connections.Remove(item);
						break;
					case DeploymentModel item:
							Project.Deployments.Remove(item);
						break;
					case ProjectModel item:
							MainViewModel.Instance.ControllerWindow.ShowMessage("Borrar proyecto");
						break;
				}
				// Graba
				Save();
			}
		}

		/// <summary>
		///		Distribuye el elemento
		/// </summary>
		private void SendDeployment()
		{
			MainViewModel.Instance.ControllerWindow.ShowMessage("Send");
		}

		/// <summary>
		///		Procesa el archivo
		/// </summary>
		private void ProcessFile()
		{
			if (SelectedNode.Tag is DeploymentModel deployment)
				Process(deployment, string.Empty);
			else
			{
				string fileName = GetSelectedNodeFileName();

					if (string.IsNullOrEmpty(fileName) || !System.IO.File.Exists(fileName))
						MainViewModel.Instance.ControllerWindow.ShowMessage("Seleccione un archivo");
					else
					{
						Deployments.DeploymentSelectViewModel viewModel = new Deployments.DeploymentSelectViewModel(Project);

							if (MainViewModel.Instance.ViewsController.OpenSelectDeploymentView(viewModel) == BauMvvm.ViewModels.Controllers.SystemControllerEnums.ResultType.Yes &&
									viewModel.SelectedDeployment != null)
								Process(viewModel.SelectedDeployment, fileName);
					}
			}
		}

		/// <summary>
		///		Procesa una distribución
		/// </summary>
		private void Process(DeploymentModel deployment, string fileName)
		{
			Application.Processor.DeploymentProcessor processor = new Application.Processor.DeploymentProcessor(ProjectPath, Project, deployment);

				// Asigna los manejadores de eventos
				processor.Progress += (sender, args) =>
											{
												MainViewModel.Instance.HostController.Messenger.SendBarProgress(MainViewModel.Instance.ModuleName,
																												args.Message,
																												args.Actual, args.Total, null);
												MainViewModel.Instance.HostController.Messenger.SendLog
													(MainViewModel.Instance.ModuleName,
													 args.IsError ? Plugins.ViewModels.Controllers.Messengers.Common.MessageLog.LogType.Error :
																	Plugins.ViewModels.Controllers.Messengers.Common.MessageLog.LogType.Information,
													 args.Message, args.FileName, null);
											};
				// Procesa la distribución
				processor.Process(fileName);
		}

		/// <summary>
		///		Graba el proyecto (para llamarlo desde otros ViewModel)
		/// </summary>
		internal void SaveProject()
		{
			Save();
		}

		/// <summary>
		///		Graba el proyecto
		/// </summary>
		protected override void Save()
		{
			// Graba el proyecto
			new ProjectBussiness().Save(Project, ProjectPath);
			// ... y actualiza el árbol
			LoadProject(ProjectPath);
		}

		/// <summary>
		///		Obtiene el tipo seleccionado del nodo
		/// </summary>
		private NodeType GetSelectedNodeType()
		{
			return SelectedNodeDefinition?.Id.GetEnum(NodeType.Unknown) ?? NodeType.Unknown;
		}

		/// <summary>
		///		Proyecto actual
		/// </summary>
		public ProjectModel Project { get; private set; }

		/// <summary>
		///		Script
		/// </summary>
		public string Script
		{
			get { return _script; }
			set { CheckProperty(ref _script, value); }
		}

		/// <summary>
		///		Parámetros de ejecución del script
		/// </summary>
		public string Parameters
		{
			get { return _parameters; }
			set { CheckProperty(ref _parameters, value); }
		}

		/// <summary>
		///		Comando para hacer un deploy del proyecto
		/// </summary>
		public BaseCommand SendCommand { get; }

		/// <summary>
		///		Comando para procesar el script
		/// </summary>
		public BaseCommand ProcessCommand { get; }
	}
}
