using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.SourceEditor.Model.Messages;
using Bau.Libraries.SourceEditor.Model.Plugins;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.FtpManager.ViewModel.Controllers
{
	/// <summary>
	///		Manager para comunicar este plugin con SourceEditor
	/// </summary>
	internal class SourceEditorPluginManager : IPluginSourceEditor
	{ 
		// Constantes privadas
		private const string ProjectType = "FtpManagerStudio";
		private const string ActionConnect = "Connect";
		// Constantes con las extensiones
		private const string ExtensionProject = "ftpprj";
		internal const string ExtensionConnection = "ftpcnt";
		// Enumerados con los tipos de elemento del árbol
		internal enum TreeType
		{
			Unknown,
			FtpConnection
		}

		/// <summary>
		///		Inicializa el plugin sobre SourceEditor
		/// </summary>
		internal void InitPlugin(MessageRequestPlugin message)
		{ 
			// Inicializa las definiciones de proyecto
			InitProjectDefinitions();
			// Añade el plugin al mensaje
			message?.CreatePlugin(ExtensionProject, this);
		}

		/// <summary>
		///		Inicializa las definiciones del proyecto
		/// </summary>
		private void InitProjectDefinitions()
		{
			ProjectDefinitionModel definition = new ProjectDefinitionModel("Conexiones Ftp",
																		   FtpManagerViewModel.Instance.ViewsController.IconFileProject,
																		   FtpManagerViewModel.Instance.ModuleName,
																		   ProjectType, ExtensionProject);
			FileDefinitionModel file;

				// Añade el archivo de conexión y sus opciones de menú
				file = definition.FilesDefinition.Add(definition, "Servidor Ftp",
													  FtpManagerViewModel.Instance.ViewsController.IconFileConnectionFtp,
													  ExtensionConnection, false);
				file.Menus.Add(MenuModel.MenuType.Command, ActionConnect, "_Conectar",
							   FtpManagerViewModel.Instance.ViewsController.IconMenuOpenFtpConnection);
				// Asigna la definición de proyecto
				Definition = definition;
		}

		/// <summary>
		///		Abre un archivo
		/// </summary>
		public bool OpenFile(FileModel file, bool isNew)
		{
			bool isOpen = false;

				// Abre el archivo
				if (file.Extension.EqualsIgnoreCase(ExtensionConnection))
				{ 
					// Abre el archivo
					FtpManagerViewModel.Instance.ViewsController.OpenFormUpdateConnection
								(new Connections.FtpConnectionViewModel(file.FullFileName, file.Title));
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
			if (menu.Key.EqualsIgnoreCase(ActionConnect))
				OpenConnection(file);
			// Indica que se ha ejecutado correctamente
			return true;
		}

		/// <summary>
		///		Abre la conexión a un servidor Ftp
		/// </summary>
		private void OpenConnection(FileModel file)
		{
			if (file == null)
				FtpManagerViewModel.Instance.ControllerWindow.ShowMessage("Seleccione la conexión que desea abrir");
			else if (file.Extension.EqualsIgnoreCase(ExtensionConnection))
			{
				Model.Connections.FtpConnectionModel ftpConnection = new Application.Bussiness.FtpConnectionBussiness().Load(file.FullFileName);

					if (ftpConnection.Name.IsEmpty())
						FtpManagerViewModel.Instance.ControllerWindow.ShowMessage("No se pueden cargar los datos de la conexión");
					else
						FtpManagerViewModel.Instance.ViewsController.OpenFormFtp(ftpConnection);
			}
		}

		/// <summary>
		///		Cambia el nombre de un archivo --> En este caso no hace nada, simplemente implementa la interface
		/// </summary>
		public bool Rename(FileModel file, string newFileName, string title)
		{
			return true;
		}

		/// <summary>
		///		Carga los hijos de un nodo --> En este caso no hace nada, simplemente implementa la interface
		/// </summary>
		public OwnerChildModelCollection LoadOwnerChilds(FileModel file, OwnerChildModel parent)
		{
			return new OwnerChildModelCollection();
		}

		/// <summary>
		///		Definición de proyectos, archivos y menús
		/// </summary>
		public ProjectDefinitionModel Definition { get; private set; }
	}
}

