using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.SourceEditor.Model.Messages;
using Bau.Libraries.SourceEditor.Model.Plugins;
using Bau.Libraries.SourceEditor.Model.Solutions;
using Bau.Libraries.LibDataBaseStudio.Application.Bussiness;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Controllers
{
	/// <summary>
	///		Manager para comunicar este plugin con SourceEditor
	/// </summary>
	internal class SourceEditorPluginManager : IPluginSourceEditor
	{
		// Constantes privadas
		private const string ProjectType = "DataBaseStudio";
		private const string ActionCompile = "Compile";
		// Constantes con las extensiones
		private const string ExtensionProject = "sdprj";
		private const string ExtensionReport = "schrpt";
		private const string ExtensionQuery = "schsql";
		// Enumerados con los tipos de elemento del árbol
		internal enum TreeType
		{
			Unknown,
			TablesRoot,
			Table,
			ViewsRoot,
			View,
			Column,
			RoutinesRoot,
			StoredProceduresRoot,
			StoredProcedure,
			FunctionsRoot,
			Function
		}

		/// <summary>
		///		Inicializa el plugin sobre SourceEditor
		/// </summary>
		internal void InitPlugin(MessageRequestPlugin message)
		{
			// Inicializa las definicioens de proyecto
			InitProjectDefinitions();
			// Añade el plugin al mensaje
			message?.CreatePlugin(ExtensionProject, this);
		}

		/// <summary>
		///		Inicializa las definiciones del proyecto
		/// </summary>
		private void InitProjectDefinitions()
		{
			ProjectDefinitionModel definition = new ProjectDefinitionModel("IDE de base de datos e informes",
																		   DataBaseStudioViewModel.Instance.ViewsController.IconFileProject,
																		   DataBaseStudioViewModel.Instance.ModuleName,
																		   ProjectType, ExtensionProject);
			FileDefinitionModel file;

				// Añade el archivo de conexión y sus opciones de menú
				file = definition.FilesDefinition.Add(definition, "Conexión a base de datos",
													  DataBaseStudioViewModel.Instance.ViewsController.IconFileConnection,
													  SchemaConnectionBussiness.ExtensionConnection, false);
				// Añade los nodos propietarios al archivo de conexión
				AddOwnerNodes(file);
				// Añade el archivo de informes y sus opciones de menú
				file = definition.FilesDefinition.Add(definition, "Informes",
													  DataBaseStudioViewModel.Instance.ViewsController.IconFileReport,
													  ExtensionReport, false);
				file.Menus.Add(MenuModel.MenuType.Command, ActionCompile, "_Generar",
							   DataBaseStudioViewModel.Instance.ViewsController.IconMenuGenerate);
				file = definition.FilesDefinition.Add(definition, "Consultas",
													  DataBaseStudioViewModel.Instance.ViewsController.IconFileQuery,
													  ExtensionQuery, false);
				// Asigna la definición de proyecto
				Definition = definition;
		}

		/// <summary>
		///		Añade los nodos propietarios de una conexión
		/// </summary>
		private void AddOwnerNodes(FileDefinitionModel file)
		{
			OwnerObjectDefinitionModel ownerObject, childObject;

				// Añade los objetos de tablas
				ownerObject = file.OwnerChilds.Add(TreeType.TablesRoot.ToString(), "Tablas",
												   DataBaseStudioViewModel.Instance.ViewsController.IconFileTablesRoot, true);
				ownerObject = ownerObject.OwnerChilds.Add(TreeType.Table.ToString(), "Tabla",
														  DataBaseStudioViewModel.Instance.ViewsController.IconFileTable, false);
				ownerObject = ownerObject.OwnerChilds.Add(TreeType.Column.ToString(), "Columna",
														  DataBaseStudioViewModel.Instance.ViewsController.IconFileColumn, false);
				// Añade los objetos de vistas
				ownerObject = file.OwnerChilds.Add(TreeType.ViewsRoot.ToString(), "Vistas",
												   DataBaseStudioViewModel.Instance.ViewsController.IconFileViewsRoot, true);
				ownerObject = ownerObject.OwnerChilds.Add(TreeType.View.ToString(), "Vista",
														  DataBaseStudioViewModel.Instance.ViewsController.IconFileView, false);
				ownerObject = ownerObject.OwnerChilds.Add(TreeType.Column.ToString(), "Columna",
														  DataBaseStudioViewModel.Instance.ViewsController.IconFileColumn, false);
				// Añade los objetos de rutinas
				ownerObject = file.OwnerChilds.Add(TreeType.RoutinesRoot.ToString(), "Rutinas",
												   DataBaseStudioViewModel.Instance.ViewsController.IconFileRoutines, true);
				// Añade los procedimientos almacenados a las rutinas
				childObject = ownerObject.OwnerChilds.Add(TreeType.StoredProceduresRoot.ToString(), "Procedimientos almacenados",
														  DataBaseStudioViewModel.Instance.ViewsController.IconFileStoredProceduresRoot, true);
				childObject.OwnerChilds.Add(TreeType.StoredProcedure.ToString(), "Procedimiento almacenado",
											DataBaseStudioViewModel.Instance.ViewsController.IconFileStoredProcedure, false);
				// Añade las funciones a las rutinas
				childObject = ownerObject.OwnerChilds.Add(TreeType.FunctionsRoot.ToString(), "Funciones",
														  DataBaseStudioViewModel.Instance.ViewsController.IconFileFunctionsRoot, false);
				childObject.OwnerChilds.Add(TreeType.Function.ToString(), "Función",
											DataBaseStudioViewModel.Instance.ViewsController.IconFileFunction, false);
		}

		/// <summary>
		///		Abre un archivo
		/// </summary>
		public bool OpenFile(FileModel file, bool isNew)
		{
			bool isOpen = false;

				// Abre el archivo
				if (file.Extension.EqualsIgnoreCase(SchemaConnectionBussiness.ExtensionConnection))
				{
					// Abre el archivo
					DataBaseStudioViewModel.Instance.ViewsController.OpenFormUpdateConnection
								(new Connections.ConnectionViewModel(file.FullFileName, file.Title));
					// Indica que se ha abierto correctamente
					isOpen = true;
				}
				else if (file.Extension.EqualsIgnoreCase(ExtensionReport))
				{
					// Abre el archivo
					DataBaseStudioViewModel.Instance.ViewsController.OpenFormUpdateReport(new Reports.ReportViewModel(file.Title, file.FullFileName,
																													  file.SearchProject().PathBase));
					// Indica que se ha abierto correctamente
					isOpen = true;
				}
				else if (file.Extension.EqualsIgnoreCase(ExtensionQuery))
				{
					// Abre el archivo
					DataBaseStudioViewModel.Instance.ViewsController.OpenFormUpdateQuery
									(new Queries.QueryViewModel(file.Title, file.FullFileName, file.SearchProject().PathBase));
					// Indica que se ha abierto correctamente
					isOpen = true;
				}
				// Devuelve el valor que indica si se ha abierto
				return isOpen;
		}

		/// <summary>
		///		Ejecuta una acción sobre un archivo --> En este caso simplemente implementa la interface
		/// </summary>
		public bool ExecuteAction(FileModel file, MenuModel menu)
		{
			// Ejecuta los comandos
			if (menu.Key.EqualsIgnoreCase(ActionCompile))
				Compile(file);
			// Indica que se ha ejecutado correctamente
			return true;
		}

		/// <summary>
		///		Compila un archivo
		/// </summary>
		private void Compile(FileModel file)
		{
			if (file == null)
				DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el informe que desea mostrar");
			else if (file.Extension.EqualsIgnoreCase(ExtensionReport))
			{
				Model.Reports.ReportModel report = new ReportBussiness().Load(file.FullFileName);

					if (report.Name.IsEmpty())
						DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("No se puede cargar la definición del informe");
					else
						DataBaseStudioViewModel.Instance.ViewsController.OpenFormReportGenerate
												(new Reports.ReportGenerateViewModel(report, file.FullFileName, file.SearchProject().PathBase));
			}
		}

		/// <summary>
		///		Carga los elementos hijo de un archivo
		/// </summary>
		public OwnerChildModelCollection LoadOwnerChilds(FileModel file, OwnerChildModel parent)
		{
			TreeType type = GetTreeType(parent.Definition.GlobalId);
			OwnerChildModelCollection childs = new OwnerChildModelCollection();

				// Dependiendo del tipo de nodo
				try
				{
					switch (type)
					{
						case TreeType.TablesRoot:
								childs = new TreeNodesSchemaManager().GetNodesTables(file);
							break;
						case TreeType.ViewsRoot:
								childs = new TreeNodesSchemaManager().GetNodesViews(file);
							break;
						case TreeType.RoutinesRoot:
								childs = new TreeNodesSchemaManager().GetNodesStoredProcedures(file);
							break;
					}
				}
				catch (Exception exception)
				{
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Error al obtener los nodos: " + Environment.NewLine + exception.Message);
				}
				// Devuelve la colección de nodos hijo
				return childs;
		}

		/// <summary>
		///		Obtiene el tipo de nodo a partir de la clave
		/// </summary>
		private TreeType GetTreeType(string id)
		{
			TreeType type = TreeType.Unknown;

				// Obtiene el tipo 
				if (id.EqualsIgnoreCase(TreeType.TablesRoot.ToString()))
					type = TreeType.TablesRoot;
				else if (id.EqualsIgnoreCase(TreeType.Table.ToString()))
					type = TreeType.Table;
				else if (id.EqualsIgnoreCase(TreeType.ViewsRoot.ToString()))
					type = TreeType.ViewsRoot;
				else if (id.EqualsIgnoreCase(TreeType.View.ToString()))
					type = TreeType.View;
				else if (id.EqualsIgnoreCase(TreeType.RoutinesRoot.ToString()))
					type = TreeType.RoutinesRoot;
				else if (id.EqualsIgnoreCase(TreeType.StoredProcedure.ToString()))
					type = TreeType.StoredProcedure;
				// Devuelve el tipo
				return type;
		}

		/// <summary>
		///		Cambia el nombre de un archivo --> En este caso no hace nada, simplemente implementa la interface
		/// </summary>
		public bool Rename(FileModel file, string newFileName, string title)
		{
			return true;
		}

		/// <summary>
		///		Definición de proyectos, archivos y menús
		/// </summary>
		public ProjectDefinitionModel Definition { get; private set; }
	}
}

