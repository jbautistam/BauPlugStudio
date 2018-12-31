using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Application.Bussiness.Solutions;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.Processor
{
	/// <summary>
	///		Clase para generación de documentos HTML
	/// </summary>
	public class Generator
	{ 
		// Constantes privadas
		private const int MaxSteps = 14;
		// Eventos públicos
		public event EventHandler<EventArguments.EventArgsProgress> Progress;
		public event EventHandler EndProcess;

		public Generator(string fileSolution, string fileProject, string pathTarget, bool endFirstError, bool minimize = false)
		{
			Solution = new SolutionBussiness().Load(fileSolution);
			Project = new ProjectBussiness().Load(Solution, fileProject);
			PathTarget = System.IO.Path.Combine(pathTarget, LibCommonHelper.Files.HelperFiles.Normalize(Project.Name, false));
			MustEndFirstError = endFirstError;
			MustMinimize = minimize;
		}

		/// <summary>
		///		Genera un sitio Web a partir de los datos de un archivo de proyecto
		/// </summary>
		public void Generate()
		{
			Steps.CompilerData compilerData = new Steps.CompilerData(this);

				// Limpia el directorio de salida
				LibCommonHelper.Files.HelperFiles.KillPath(PathTarget);
				// 1.- Ejecuta el paso de carga de los archivos
				RaiseEventProgress(1, MaxSteps, "Cargando archivos");
				new Steps.FilesSteps.StepLoadFilesProcessor(this, compilerData).Process();
				// 2.- Ejecuta el paso de copia inicial de los archivos
				RaiseEventProgress(2, MaxSteps, "Copiando archivos");
				new Steps.FilesSteps.StepCopyProcessor(this, compilerData).Process();
				// 3.- Ejecuta el paso de compilación de archivos SmallCss
				RaiseEventProgress(3, MaxSteps, "Compilando SmallCss");
				new Steps.AdditionalSteps.StepCompileScssProcessor(this, compilerData).Process();
				// 4.- Ejecuta el paso de compilación corta de los documentos
				RaiseEventProgress(4, MaxSteps, "Compilando contenido corto de las páginas");
				new Steps.PagesSteps.StepPagesPrecompileProcessor(this, compilerData).Process();
				// 5.- Ejecuta el paso de carga secciones y plantilla
				RaiseEventProgress(5, MaxSteps, "Cargando secciones y plantillas");
				new Steps.SectionSteps.StepLoadSectionsProcessor(this, compilerData).Process();
				// 6.- Ejecuta el paso de compilación de las secciones Rss
				RaiseEventProgress(6, MaxSteps, "Compilando secciones de noticias");
				new Steps.SectionSteps.StepSectionsNewsProcessor(this, compilerData).Process();
				// 7.- Ejecuta el paso de compilación de las secciones Web
				RaiseEventProgress(7, MaxSteps, "Compilando secciones del sitio");
				new Steps.SectionSteps.StepSectionsWebProcessor(this, compilerData).Process();
				// 8.- Ejecuta el paso de compilación de las categorías
				RaiseEventProgress(8, MaxSteps, "Compilando páginas de categorías");
				new Steps.CategoriesSteps.StepCategoriesCompileProcessor(this, compilerData).Process();
				// 9.- Ejecuta el paso de compilación de los mapas del sitio
				RaiseEventProgress(9, MaxSteps, "Compilando mapas del sitio");
				new Steps.CategoriesSteps.StepSitemapCompileProcessor(this, compilerData).Process();
				// 10.- Ejecuta el paso de compilación de las etiquetas
				RaiseEventProgress(10, MaxSteps, "Compilando etiquetas");
				new Steps.CategoriesSteps.StepTagsCompileProcessor(this, compilerData).Process();
				// 11.- Ejecuta el paso de compilación de las páginas
				RaiseEventProgress(11, MaxSteps, "Compilando páginas");
				new Steps.PagesSteps.StepPagesCompileProcessor(this, compilerData).Process();
				// 12.- Ejecuta el paso de creación de Rss (antes del postproceso porque
				//			utiliza los archivos compilados en corto que se borran en el siguiente paso)
				RaiseEventProgress(12, MaxSteps, "Creando archivos RSS");
				new Steps.RssSteps.StepRssProcessor(this, compilerData).Process();
				// 13.- Ejecuta el paso de postproceso
				RaiseEventProgress(13, MaxSteps, "Postproceso de páginas");
				new Steps.PagesSteps.StepPagesPostCompileProcessor(this, compilerData).Process();
				// 14.- Ejecuta el paso de creación de SiteMap
				RaiseEventProgress(14, MaxSteps, "Creando sitemap");
				new Steps.SitemapSteps.StepSitemapProcessor(this, compilerData).Process();
				// Lanza el evento de fin de proceso
				EndProcess?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		///		Compila un documento
		/// </summary>
		public string Compile(string content)
		{
			NhamlCompiler.Compiler compiler = new NhamlCompiler.Compiler();
			string parsed = "";

				// Compila el contenido
				if (!content.IsEmpty())
					parsed = compiler.Parse(content);
				// Añade los errores
				Errors.AddRange("sinarchivo", compiler.LocalErrors);
				// Devuelve la cadena compilada
				return parsed;
		}

		/// <summary>
		///		Lanza el evento de progreso
		/// </summary>
		internal void RaiseEventProgress(int actual, int total, string message = null)
		{
			Progress?.Invoke(this, new EventArguments.EventArgsProgress(actual, total, message));
		}

		/// <summary>
		///		Solución
		/// </summary>
		public SolutionModel Solution { get; }

		/// <summary>
		///		Proyecto
		/// </summary>
		public ProjectModel Project { get; }

		/// <summary>
		///		Directorio destino
		/// </summary>
		public string PathTarget { get; }

		/// <summary>
		///		Indica si se debe minimizar el resultado
		/// </summary>
		public bool MustMinimize { get; }

		/// <summary>
		///		Indica si se debe terminar la compilación con el primer error
		/// </summary>
		public bool MustEndFirstError { get; }

		/// <summary>
		///		Errores de compilación
		/// </summary>
		public Errors.ErrorsMessageCollection Errors { get; } = new Processor.Errors.ErrorsMessageCollection();
	}
}
