using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.FtpManager.Application.Services;
using Bau.Libraries.Plugins.ViewModels.Controllers.Settings;
using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers.Common;

namespace Bau.Libraries.FtpManager.ViewModel.Controllers.Services
{
	/// <summary>
	///		Servicio para el proceso de envío de archivos de un directorio
	/// </summary>
	internal class UploadProcessService
	{
		/// <summary>
		///		Procesa la subida de archivos con una lista de parámetros
		/// </summary>
		/// <remarks>
		///		Parámetros: FtpConnection=WebsInteresantes PathLocal="C:\Users\jbautistam\Proyectos\WebSites\Generate\Ant2e6" PathRemote="/test" Mode="CheckSize"
		/// </remarks>
		internal void Process(ParametersModelCollection parameters)
		{
			string module = FtpManagerViewModel.Instance.ModuleName;

				// Procesa la subida
				Process(parameters.Search(module, "FtpConnection")?.Value,
						parameters.Search(module, "PathLocal")?.Value,
						parameters.Search(module, "PathRemote")?.Value,
						(parameters.Search(module, "ProcessNumber")?.Value ?? "").GetInt(1),
						GetUploadMode(parameters.Search(module, "Mode")?.Value));
		}

		/// <summary>
		///		Obtiene el modo de subida a partir de un nombre
		/// </summary>
		private FtpUploadService.UploadMode GetUploadMode(string value)
		{
			if (value.EqualsIgnoreCase(FtpUploadService.UploadMode.CheckSize.ToString()))
				return FtpUploadService.UploadMode.CheckSize;
			else
				return FtpUploadService.UploadMode.All;
		}

		/// <summary>
		///		Procesa la subida de archivos
		/// </summary>
		internal void Process(string ftpConnection, string localPath, string remotePath, int process, FtpUploadService.UploadMode mode)
		{
			Model.Connections.FtpConnectionModel connection = SearchConnection(ftpConnection);

				if (connection == null)
					ShowLog("Error en la subida de archivos", $"No se encuentra la conexión '{ftpConnection}'", true);
				else if (localPath.IsEmpty())
					ShowLog("Error en la subida de archivos", "No se ha recibido ningún valor para el directorio local", true);
				else if (!System.IO.Directory.Exists(localPath) && !System.IO.File.Exists(localPath))
					ShowLog("Error en la subida de archivos", $"No se encuentra el directorio local {localPath}", true);
				else if (remotePath.IsEmpty())
					ShowLog("Error en la subida de archivos", "No se ha recibido ningún valor para el directorio remoto", true);
				else
				{
					FtpUploadService ftpUpload = new FtpUploadService();

						// Asigna los manejadores de eventos
						ftpUpload.ProgressMessage += (sender, evntArgs) =>
									{
										FtpManagerViewModel.Instance.HostController.Messenger.SendProgress($"Upload_{evntArgs.TaskId}",
																										   FtpManagerViewModel.Instance.ModuleName,
																										   $"Subir archivos - Tarea {evntArgs.TaskId}",
																										   evntArgs.Message, evntArgs.Progress,
																										   evntArgs.Maximum, null);
									};
						// Comienza el proceso
						ftpUpload.Upload(connection, localPath, remotePath, process, mode);
						// Log
						ShowLog("Proceso de subida de archivos", "Fin del proceso", false);
				}
		}

		/// <summary>
		///		Obtiene una conexión
		/// </summary>
		private Model.Connections.FtpConnectionModel SearchConnection(string name)
		{
			SourceEditor.Model.Messages.MessageGetProjects messageProjects;

				// Crea el mensaje de obtención de proyectos		
				messageProjects = new SourceEditor.Model.Messages.MessageGetProjects(FtpManagerViewModel.Instance.SourceEditorPluginManager.Definition);
				// Lanza el mensaje para obtener los nombres de los archivos de proyecto abiertos
				FtpManagerViewModel.Instance.HostController.Messenger.Send(FtpManagerViewModel.Instance.ModuleName, "GetProjects", "GetProjects",
																		   messageProjects);
				// Obtiene los archivos de proyecto
				foreach (string project in messageProjects.ProjectFiles)
				{
					System.Collections.Generic.List<string> files = LibCommonHelper.Files.HelperFiles.ListRecursive(System.IO.Path.GetDirectoryName(project));

						foreach (string fileName in files)
							if (fileName.EndsWith($".{SourceEditorPluginManager.ExtensionConnection}", StringComparison.CurrentCultureIgnoreCase))
							{
								Model.Connections.FtpConnectionModel ftpConnection = new Application.Bussiness.FtpConnectionBussiness().Load(fileName);

								if (ftpConnection.Name.EqualsIgnoreCase(name))
									return ftpConnection;
							}
				}
				// Si ha llegado hasta aquí es porque no ha encontrado la conexión
				return null;
		}

		/// <summary>
		///		Muestra un mensaje de log
		/// </summary>
		private void ShowLog(string message, string description, bool error)
		{
			FtpManagerViewModel.Instance.HostController.Messenger.SendLog(FtpManagerViewModel.Instance.ModuleName,
																		  error ? MessageLog.LogType.Error :
																				  MessageLog.LogType.Information,
																		  message, description, null);
		}
	}
}
