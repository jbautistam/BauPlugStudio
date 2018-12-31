using System;
using System.Collections.Generic;
using System.IO;

using Bau.Libraries.Aggregator.Providers.Base;
using Bau.Libraries.DatabaseStudio.Models;
using Bau.Libraries.DatabaseStudio.Models.Connections;
using Bau.Libraries.DatabaseStudio.Models.Deployment;
using Bau.Libraries.LibDataStructures.Base;
using Bau.Libraries.LibReports;
using Bau.Libraries.LibCommonHelper.Files;

namespace Bau.Libraries.DatabaseStudio.Application.Processor
{
	/// <summary>
	///		Procesador para un <see cref="DeploymentModel"/>
	/// </summary>
    public class DeploymentProcessor
    {
		/// <summary>
		///		Evento de progreso
		/// </summary>
		public event EventHandler<EventArguments.ProgressEventArgs> Progress;

		public DeploymentProcessor(string projectPath, ProjectModel project, DeploymentModel deployment)
		{
			ProjectPath = projectPath;
			Project = project;
			Deployment = deployment;
		}

		/// <summary>
		///		Procesa un modelo de distribución (y en su caso, un archivo en concreto)
		/// </summary>
		public void Process(string fileName = null)
		{
			List<ScriptModel> files = GetScripts(fileName);

				// Log
				RaiseMessage("Comienzo del proceso de archivos");
				// Procesa los archivos
				if (files.Count == 0)
					RaiseMessage("No hay ningún script para procesar", true);
				else
					Process(files);
				// Log
				RaiseMessage("Fin del proceso de archivos");
		}

		/// <summary>
		///		Procesa una serie de archivos
		/// </summary>
		private void Process(List<ScriptModel> scripts)
		{
			foreach (ScriptModel script in scripts)
			{
				string fileName = GetFullFileName(script);

					// Log
					RaiseEvent(script.RelativeFileName, "Comenzando el proceso", scripts.IndexOf(script), scripts.Count);
					// Procesa el archivo
					if (!File.Exists(fileName))
						RaiseMessage($"No se encuentra el archivo del script {script.RelativeFileName}", true);
					else if (Path.GetExtension(fileName).Equals(ProjectModel.ReportExtension, StringComparison.CurrentCultureIgnoreCase))
						ProcessReport(script, fileName);
					else if (Path.GetExtension(fileName).Equals(ProjectModel.ImportScriptExtension, StringComparison.CurrentCultureIgnoreCase))
						ProcessImportScript(script, fileName);
					else
						RaiseEvent(script.RelativeFileName, "No se encuentra ningún procesador para el archivo", scripts.IndexOf(script), scripts.Count, true);
			}
		}

		/// <summary>
		///		Procesa el informe
		/// </summary>
		private void ProcessReport(ScriptModel script, string fileName)
		{
			ReportManager manager = new ReportManager(GetDataProviders(Deployment));
			string fileParsed = Path.Combine(Deployment.PathFilesTarget, Path.GetFileNameWithoutExtension(fileName) + ".generated");

				// Log
				RaiseMessage($"Procesando el informe {Path.GetFileName(fileName)}");
				// Procesa el informe
				manager.GenerateByFile(fileName, fileParsed, GetParameters(Deployment.Parameters, script.Parameters));
				// Añade los errores
				if (manager.Errors.Count > 0)
					foreach (string error in manager.Errors)
						RaiseEvent(fileName, error, 0, 0, true);
				else
					RenderFile(fileParsed, fileName);
				// Elimina el archivo intermedio
				HelperFiles.KillFile(fileParsed);
		}

		/// <summary>
		///		Procesa el informe interpretado
		/// </summary>
		private void RenderFile(string fileParsed, string fileName)
		{
			foreach (DeploymentModel.ReportFormat format in Deployment.ReportFormatTypes)
				try
				{
					switch (format)
					{
						case DeploymentModel.ReportFormat.Xml:
								HelperFiles.CopyFile(fileParsed, ChangeExtension(fileParsed, ".xml"));
							break;
						case DeploymentModel.ReportFormat.Html:
								if (!ExistsFormat(DeploymentModel.ReportFormat.HtmlConverted, Deployment.ReportFormatTypes))
									HelperFiles.CopyFile(fileParsed, ChangeExtension(fileParsed, ".html"));
							break;
						case DeploymentModel.ReportFormat.HtmlConverted:
								new LibReports.Renderer.Html.RendererHTML().Render(ChangeExtension(fileParsed, ".html"), fileParsed);
							break;
						case DeploymentModel.ReportFormat.Pdf:
								new LibReports.Renderer.PDF.RendererPdf().Render(ChangeExtension(fileParsed, ".pdf"), fileParsed);
							break;
					}
				}
				catch (Exception exception)
				{
					RaiseEvent(fileName, $"Error when render file {fileName} to format {format.ToString()}. {exception.Message}", 0, 0, true);
				}
		}

		/// <summary>
		///		Cambia la extensión a un archivo grabado
		/// </summary>
		private string ChangeExtension(string fileParsed, string extension)
		{
			return Path.Combine(Path.GetDirectoryName(fileParsed), Path.GetFileNameWithoutExtension(fileParsed) + extension);
		}

		/// <summary>
		///		Comprueba si existe un formato seleccionado
		/// </summary>
		private bool ExistsFormat(DeploymentModel.ReportFormat searched, List<DeploymentModel.ReportFormat> formats)
		{
			// Recorre la colección
			foreach (DeploymentModel.ReportFormat format in formats)
				if (searched == format)
					return true;
			// Si llega hasta aquí es porque no ha encontrdo nada
			return false;
		}

		/// <summary>
		///		Procesa el script de importación
		/// </summary>
		private void ProcessImportScript(ScriptModel script, string fileName)
		{
			LibDbScripts.Generator.DbScriptManager manager = new LibDbScripts.Generator.DbScriptManager(GetDataProviders(Deployment),
																										GetParameters(Deployment.Parameters, script.Parameters),
																										ProjectPath);

				// Log
				RaiseMessage($"Procesando el script {Path.GetFileName(fileName)}");
				// Procesa el archivo
				manager.Log += (sender, args) => RaiseMessage(args.Message, args.Type == LibDbScripts.Generator.EventArguments.MessageEventArgs.MessageType.Error);
				manager.ProcessByFile(fileName);
		}

		/// <summary>
		///		Obtiene los parámetros
		/// </summary>
		private Dictionary<string, object> GetParameters(BaseModelCollection<ParameterModel> deploymentParameters, BaseModelCollection<ParameterModel> scriptParameters)
		{
			Dictionary<string, object> converted = new Dictionary<string, object>();

				// Asigna los parámetros del script (antes que los de la distribución)
				foreach (ParameterModel parameter in scriptParameters)
					if (!converted.ContainsKey(parameter.GlobalId.ToUpper()))
						converted.Add(parameter.GlobalId, parameter.Value);
				// Asigna los parámetros de la distribución
				foreach (ParameterModel parameter in deploymentParameters)
					if (!converted.ContainsKey(parameter.GlobalId.ToUpper()))
						converted.Add(parameter.GlobalId, parameter.Value);
				// Devuelve los parámetros convertidos
				return converted;
		}

		/// <summary>
		///		Obtiene los proveedores de datos
		/// </summary>
		private DataProviderCollection GetDataProviders(DeploymentModel deployment)
		{
			DataProviderCollection providers = new DataProviderCollection();

				// Obtiene los proveedores
				foreach (KeyValuePair<string, DatabaseConnectionModel> connection in deployment.Connections)
				{
					switch (connection.Value.Type)
					{
						case DatabaseConnectionModel.DataBaseType.SqLite:
								providers.Add(GetSqLiteProvider(connection.Key, connection.Value));
							break;
						case DatabaseConnectionModel.DataBaseType.SqlServer:
								providers.Add(GetSqlServerProvider(connection.Key, connection.Value));
							break;
						default:
								System.Diagnostics.Debug.WriteLine("Esto hay que controlarlo");
							break;
					}
				}
				// Devuelve los proveedores
				return providers;
		}

		/// <summary>
		///		Obtiene un proveedor para SQL Server
		/// </summary>
		private Aggregator.Providers.SQLServer.ScriptsSqlServerProvider GetSqlServerProvider(string key, DatabaseConnectionModel connection)
		{
			return new Aggregator.Providers.SQLServer.ScriptsSqlServerProvider(key, "SqlServer", connection.Server, connection.Port, connection.DataBase, 
																			   connection.User, connection.Password, 
																			   connection.IntegratedSecurity);
		}

		/// <summary>
		///		Obtiene un proveedor para SqLite
		/// </summary>
		private Aggregator.Providers.SqLite.ScriptsSqLiteProvider GetSqLiteProvider(string key, DatabaseConnectionModel connection)
		{
			return new Aggregator.Providers.SqLite.ScriptsSqLiteProvider(key, "SqLite", connection.FileName);
		}

		/// <summary>
		///		Obtiene los scripts a ejecutar
		/// </summary>
		private List<ScriptModel> GetScripts(string fileName = null)
		{
			List<ScriptModel> scripts = new List<ScriptModel>();

				// Añade los archivos de la distribución
				if (!string.IsNullOrEmpty(fileName))
				{
					ScriptModel script = SearchScript(fileName);

						// Crea el script si no se ha encontrado ningún script para ese archivo
						if (script == null && File.Exists(fileName))
							script = new ScriptModel
											{
												RelativeFileName = fileName.Substring(ProjectPath.Length + 1)
											};
						// Añade el script a la colección
						if (script != null)
							scripts.Add(script);
						else
							RaiseMessage($"No se puede encontrar ningún script para el archivo {fileName}", true);
				}
				else
					foreach (ScriptModel script in Deployment.Scripts)
						scripts.Add(script);
				// Devuelve la colección de archivos
				return scripts;
		}

		/// <summary>
		///		Busca un script en la colección
		/// </summary>
		private ScriptModel SearchScript(string fileName)
		{
			// Busca el script
			foreach (ScriptModel script in Deployment.Scripts)
				if (GetFullFileName(script).Equals(fileName, StringComparison.CurrentCultureIgnoreCase))
					return script;
			// Si ha llegado hasta aquí es porque no ha encontrado el script
			return null;
		}

		/// <summary>
		///		Obtiene el nombre de archivo completo de un script
		/// </summary>
		private string GetFullFileName(ScriptModel script)
		{
			if (string.IsNullOrEmpty(script.RelativeFileName))
				return string.Empty;
			else
				return System.IO.Path.Combine(ProjectPath, script.RelativeFileName);
		}

		/// <summary>
		///		Lanza el evento adecuado
		/// </summary>
		internal void RaiseMessage(string message, bool isError = false)
		{
			RaiseEvent(string.Empty, message, 0, 0, isError);
		}

		/// <summary>
		///		Lanza el evento adecuado
		/// </summary>
		internal void RaiseEvent(string fileName, string message, int actual, int total, bool isError = false)
		{
			Progress?.Invoke(this, new EventArguments.ProgressEventArgs(Deployment, fileName, message, actual, total, isError));
		}

		/// <summary>
		///		Directorio de proyecto
		/// </summary>
		public string ProjectPath { get; }

		/// <summary>
		///		Proyecto
		/// </summary>
		public ProjectModel Project { get; }

		/// <summary>
		///		Datos de distribución
		/// </summary>
		public DeploymentModel Deployment { get; }
    }
}
