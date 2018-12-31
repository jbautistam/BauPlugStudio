using System;
using System.Collections.Generic;

using Bau.Libraries.WebCurator.Model.WebSites;

namespace Bau.Libraries.WebCurator.Application.Services.Generator
{
	/// <summary>
	///		Compilador de un sitio de WebCurator
	/// </summary>
	public class ProjectCompiler
	{
		public ProjectCompiler(ProjectModel project)
		{
			Project = project;
		}

		/// <summary>
		///		Compila el libro
		/// </summary>
		public void Compile()
		{
			GenerationResultModel result;

				// Carga el archivo de proyecto y los resultados
				Project = new Bussiness.WebSites.ProjectBussiness().Load(Project.FileName);
				result = new Bussiness.WebSites.GenerationResultBussiness().Load(Project);
				// Crea el generador de sentencias y lee los archivos
				SentencesGenerator = new FilesSentencesGenerator(this);
				// Genera los documentos de los diferentes proyectos
				foreach (ProjectTargetModel target in Project.ProjectsTarget)
					Generate(target);
				// Graba los resultados
				result.DateLast = DateTime.Now;
				new Bussiness.WebSites.GenerationResultBussiness().Save(Project, result);
		}

		/// <summary>
		///		Genera los archivos de un proyecto
		/// </summary>
		private void Generate(ProjectTargetModel target)
		{
			GenerationResultProjectModel result = new Bussiness.WebSites.GenerationResultProjectBussiness().Load(Project, target);
			DocumentGalleryGenerator generator = new DocumentGalleryGenerator(this, result);

				// Carga los archivos del directorio
				FilesImagesSources = LoadImagesSources();
				// Compila los proyectos
				generator.Generate(target, Project.NumberDocuments);
				// Graba el resultado del proyecto
				new Bussiness.WebSites.GenerationResultProjectBussiness().Save(Project, target, result);
		}

		/// <summary>
		///		Carga la lista de imágenes origen
		/// </summary>
		private List<string> LoadImagesSources()
		{
			List<string> images = new List<string>();

				// Obtiene los archivos
				foreach (string pathource in Project.PathImagesSources)
					if (System.IO.Directory.Exists(pathource))
					{
						List<string> files = LibCommonHelper.Files.HelperFiles.ListRecursive(pathource);

							// Añade las imágenes a las colección de archivos
							foreach (string fileName in files)
								if (fileName.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) ||
										fileName.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) ||
										fileName.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase) ||
										fileName.EndsWith(DocumentGalleryGenerator.ExtensionUrl, StringComparison.CurrentCultureIgnoreCase))
									images.Add(fileName);
					}
				// Devuelve la colección de imágenes
				return images;
		}

		/// <summary>
		///		Proyecto que se está compilando
		/// </summary>
		public ProjectModel Project { get; private set; }

		/// <summary>
		///		Generador de sentencias
		/// </summary>
		internal Generator.FilesSentencesGenerator SentencesGenerator { get; private set; }

		/// <summary>
		///		Archivos de imagen origen
		/// </summary>
		internal List<string> FilesImagesSources { get; private set; }

		/// <summary>
		///		Errores del proceso de compilación
		/// </summary>
		public List<string> Errors { get; } = new List<string>();
	}
}
