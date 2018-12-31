using System;

using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.Plugins.ViewModels.Controllers.Processes;

namespace Bau.Libraries.LibDocWriter.ViewModel.Controllers
{
	/// <summary>
	///		Controlador de la compilación de DocWriter
	/// </summary>
	public class DocWriterCompilerProcessor : AbstractTask
	{
		public DocWriterCompilerProcessor(string source, SolutionModel solution, ProjectModel project, string pathGeneration, bool minimize) : base(source)
		{
			Solution = solution;
			Project = project;
			PathGeneration = pathGeneration;
			Minimize = minimize;
		}

		/// <summary>
		///		Procesa la compilación
		/// </summary>
		public override void Process()
		{	
			// Crea el objeto de generación
			Generator = new Processor.Generator(Solution.FullFileName, Project.File.FullFileName, PathGeneration, false, Minimize);
			// Asigna el manejador de eventos
			Generator.EndProcess += (sender, evntArgs) => RaiseEventEndProcess($"Fin de la compilación del proyecto {Project.Name}", 
																			   GetErrors(Generator.Errors));
			Generator.Progress += (sender, evntArgs) => RaiseEventProgress(evntArgs.Actual, evntArgs.Total, $"Compilando {Project.Name}");
			// Comienza la generación
			Generator.Generate();
		}

		/// <summary>
		///		Obtiene la lista de errores
		/// </summary>
		private System.Collections.Generic.List<string> GetErrors(Processor.Errors.ErrorsMessageCollection compileErrors)
		{
			System.Collections.Generic.List<string> errors = new System.Collections.Generic.List<string>();

				// Crea las cadenas de errores
				foreach (Processor.Errors.ErrorMessage error in compileErrors)
					errors.Add($"{error.Message}. Archivo: {error.FileName}. Token: {error.Token}. Fila: {error.Row}");
				// Devuelve la colección de errores
				return errors;
		}

		/// <summary>
		///		Compilador utilizado
		/// </summary>
		internal Processor.Generator Generator { get; private set; }

		/// <summary>
		///		Solución para la que se está generando
		/// </summary>
		internal SolutionModel Solution { get; }

		/// <summary>
		///		Proyecto que se está generando
		/// </summary>
		internal ProjectModel Project { get; }

		/// <summary>
		///		Directorio de generación
		/// </summary>
		internal string PathGeneration { get; }

		/// <summary>
		///		Indica si se minimiza el resultado de la compilación
		/// </summary>
		internal bool Minimize { get; }
	}
}
