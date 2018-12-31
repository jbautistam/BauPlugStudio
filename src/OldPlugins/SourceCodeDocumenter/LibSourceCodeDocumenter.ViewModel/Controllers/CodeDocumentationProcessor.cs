using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Projects.Models;
using Bau.Libraries.Plugins.ViewModels.Controllers.Processes;

namespace Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Controllers
{
	/// <summary>
	///		Controlador de la compilación de documentación de código fuente
	/// </summary>
	public class CodeDocumentationProcessor : AbstractTask
	{
		public CodeDocumentationProcessor(string source, string projectFileName, string compilerFileName) : base(source)
		{
			ProjectFileName = projectFileName;
			CompilerFileName = compilerFileName;
		}

		/// <summary>
		///		Procesa la compilación
		/// </summary>
		public override void Process()
		{
			ProjectDocumentationModel project = CreateProject(new Application.Bussiness.DocumenterProjectBussiness().Load(ProjectFileName));
			string error = "";

				// Comprueba los errores antes de continuar
				if (project.Providers.Count == 0)
					error = "No se ha encontrado ningún proveedor en el proyecto";
				else
				{
					string fileName = System.IO.Path.GetTempFileName();

					// Graba el archivo
					new LibNSharpDoc.Projects.ProjectDocumentationManager().Save(project, fileName);
					// Ejecuta la compilación
					Compile(fileName, out error);
				}
				// Lanza el evento de fin de proceso
				RaiseEventEndProcess("Fin de la compilación del proyecto " + System.IO.Path.GetFileNameWithoutExtension(ProjectFileName),
									 new System.Collections.Generic.List<string> { error });
		}

		/// <summary>
		///		Crea un proyecto de documentación
		/// </summary>
		private ProjectDocumentationModel CreateProject(Application.Model.DocumenterProjectModel source)
		{
			ProjectDocumentationModel project = new ProjectDocumentationModel();

				// Recupera el tipo y los directorios
				project.IDType = ProjectDocumentationModel.DocumentationType.Html;
				project.OutputPath = source.PathGenerate;
				project.TemplatePath = source.PathTemplates;
				// Recupera los parámetros de generación
				project.GenerationParameters.ShowInternal = source.ShowInternal;
				project.GenerationParameters.ShowPrivate = source.ShowPrivate;
				project.GenerationParameters.ShowProtected = source.ShowProtected;
				project.GenerationParameters.ShowPublic = source.ShowPublic;
				// Añade los proveedores
				project.Providers.AddRange(GetProviders(project, System.IO.Path.GetDirectoryName(ProjectFileName)));
				// Añade las páginas adicionales
				project.Providers.Add(GetProviderAdditionalPages(source.PathPages));
				// Devuelve el proyecto
				return project;
		}

		/// <summary>
		///		Obtiene los proveedores de un directorio
		/// </summary>
		private ProviderModelCollection GetProviders(ProjectDocumentationModel project, string path)
		{
			ProviderModelCollection providers = new ProviderModelCollection();

				// Siempre y cuando no estemos en el directorio de generación ...
				if (!path.EqualsIgnoreCase(project.OutputPath) && !path.EqualsIgnoreCase(project.TemplatePath))
				{ 
					// Obtiene los proveedores de los archivos
					foreach (string file in System.IO.Directory.GetFiles(path))
					{
						string extension = System.IO.Path.GetExtension(file);

							if (extension.EqualsIgnoreCase("." + SourceEditorPluginManager.ExtensionOleDb))
								providers.Add(GetOleDbProvider(file));
							else if (extension.EqualsIgnoreCase("." + SourceEditorPluginManager.ExtensionSource))
								providers.Add(GetSourceProvider(file));
							else if (extension.EqualsIgnoreCase("." + SourceEditorPluginManager.ExtensionSqlServer))
								providers.Add(GetSqlServerProvider(file));
							else if (extension.EqualsIgnoreCase("." + SourceEditorPluginManager.ExtensionStructXml))
								providers.Add(GetStructProvider(file));
					}
					// Busca entre los directorios más proveedores
					foreach (string strChild in System.IO.Directory.GetDirectories(path))
						providers.AddRange(GetProviders(project, strChild));
				}
				// Devuelve la colección de proveedores
				return providers;
		}

		/// <summary>
		///		Obtiene el proveedor para un archivo de estructuras
		/// </summary>
		private ProviderModel GetStructProvider(string file)
		{
			ProviderModel provider = new ProviderModel();

				// Asigna los parámetros
				provider.Type = "XmlStructs";
				provider.Parameters.Add("FileName", file);
				// Devuelve el proveedor
				return provider;
		}

		/// <summary>
		///		Obtiene el proveedor para una base de datos SQLServer
		/// </summary>
		private ProviderModel GetSqlServerProvider(string file)
		{
			Application.Model.SqlServerModel sqlServer = new Application.Bussiness.SqlServerBussiness().Load(file);
			ProviderModel provider = new ProviderModel();

				// Asigna los parámetros
				provider.Type = "SqlServer";
				provider.Parameters.Add("Server", sqlServer.Server);
				provider.Parameters.Add("DataBase", sqlServer.DataBase);
				provider.Parameters.Add("User", sqlServer.User);
				provider.Parameters.Add("Password", sqlServer.Password);
				// Devuelve el proveedor
				return provider;
		}

		/// <summary>
		///		Obtiene un proveedor de código fuente
		/// </summary>
		private ProviderModel GetSourceProvider(string file)
		{
			Application.Model.SourceCodeModel source = new Application.Bussiness.SourceCodeBussiness().Load(file);
			ProviderModel provider = new ProviderModel();

				// Asigna los parámetros
				if (!source.SourceFileName.IsEmpty() &&
						(source.SourceFileName.EndsWith(".csprj", StringComparison.CurrentCultureIgnoreCase) ||
						 source.SourceFileName.EndsWith(".sln", StringComparison.CurrentCultureIgnoreCase)))
					provider.Type = "C#";
				else
					provider.Type = "VisualBasic";
				// Asigna el nombre de archivo
				provider.Parameters.Add("FileName", source.SourceFileName);
				// Devuelve el proveedor
				return provider;
		}

		/// <summary>
		///		Obtiene un proveedor para OleDb
		/// </summary>
		private ProviderModel GetOleDbProvider(string file)
		{
			Application.Model.OleDbModel oleDb = new Application.Bussiness.OleDbBussiness().Load(file);
			ProviderModel provider = new ProviderModel();

				// Asigna los parámetros
				provider.Type = "OleDB";
				provider.Parameters.Add("ConnectionString", oleDb.ConnectionString);
				// Devuelve el proveedor
				return provider;
		}

		/// <summary>
		///		Obtiene el proveedor de páginas adicionales
		/// </summary>
		private ProviderModel GetProviderAdditionalPages(string pathPages)
		{
			ProviderModel provider = new ProviderModel();

				// Asigna los parámetros
				provider.Type = "HelpPages";
				provider.Parameters.Add("Path", pathPages);
				// Devuelve el proveedor
				return provider;
		}

		/// <summary>
		///		Compila el proyecto
		/// </summary>
		private void Compile(string fileName, out string error)
		{
			// Inicializa los argumentos de salida
			error = "";
			// Genera la documentación
			LibSystem.Files.WindowsFiles.ExecuteApplication(CompilerFileName, fileName);
		}

		/// <summary>
		///		Nombre del proyecto que se va a generar
		/// </summary>
		internal string ProjectFileName { get; }

		/// <summary>
		///		Nombre del ejecutable que realiza la documentación
		/// </summary>
		internal string CompilerFileName { get; }
	}
}
