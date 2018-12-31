using System;

using Bau.Libraries.Plugins.ViewModels.Controllers.Processes;
using Bau.Libraries.WebCurator.Model.WebSites;

namespace Bau.Libraries.WebCurator.ViewModel.Controllers
{
	/// <summary>
	///		Procesador completo de una Web de WebCurator: compila las frases, compila el resultado y lo envía a la cola de FTP
	/// </summary>
	internal class FullProcessor : AbstractTask
	{
		internal FullProcessor(string source, ProjectModel project, bool sendQueue) : base(source)
		{
			Project = project;
			SendQueue = sendQueue;
		}

		/// <summary>
		///		Procesa un proyecto
		/// </summary>
		public override void Process()
		{
			Application.Services.Generator.ProjectCompiler compiler = new Application.Services.Generator.ProjectCompiler(Project);

				// Carga el proyecto
				Project = new Application.Bussiness.WebSites.ProjectBussiness().Load(Project.FileName);
				// Crea las páginas
				compiler.Compile();
				// Compila y envía por FTP
				if (compiler.Errors.Count == 0 && SendQueue)
				{ // Compila con DocWriter
					CompileDocWriter(Project);
				}
				// Lanza el evento de fin de proceso
				RaiseEventEndProcess("Fin de compilación del proyecto " + Project.Name, compiler.Errors);
		}

		/// <summary>
		///		Compila un proyecto
		/// </summary>
		private void CompileDocWriter(ProjectModel project)
		{
			LibDocWriter.Processor.Generator generator;

				foreach (ProjectTargetModel target in project.ProjectsTarget)
				{ 
					// Crea el objeto de generación
					generator = new LibDocWriter.Processor.Generator(target.ProjectFileName, target.ProjectFileName, 
																	 WebCuratorViewModel.Instance.PathGeneration,
																	 false, true);
					// Compila el proyecto
					generator.Generate();
				}
		}


		/// <summary>
		///		Obtiene el directorio de compilación de un proyecto
		/// </summary>
		public string GetTargetCompile(ProjectTargetModel target)
		{
			return System.IO.Path.Combine(WebCuratorViewModel.Instance.PathGeneration,
										  System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(target.ProjectFileName)));
		}

		/// <summary>
		///		Proyecto a compilar
		/// </summary>
		public ProjectModel Project { get; private set; }

		/// <summary>
		///		Indica si se debe enviar el proyecto a la cola
		/// </summary>
		public bool SendQueue { get; }
	}
}
