using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.WebCurator.Model.WebSites;

namespace Bau.Libraries.WebCurator.Repository.WebSites
{
	/// <summary>
	///		Repository de <see cref="GenerationResultProjectModel"/>
	/// </summary>
	public class GenerationResultProjectRepository
	{   
		// Constantes privadas
		private const string TagRoot = "Result";
		private const string TagImageSource = "ImageSource";

		/// <summary>
		///		Carga los resultados de un proyecto
		/// </summary>
		public GenerationResultProjectModel Load(ProjectModel project, ProjectTargetModel target)
		{
			GenerationResultProjectModel result = new GenerationResultProjectModel();
			string fileName = GetFileName(project, target);
			MLFile fileML = new XMLParser().Load(fileName);

				// Carga los nodos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
							foreach (MLNode childML in nodeML.Nodes)
								switch (childML.Name)
								{
									case TagImageSource:
											result.ImagesSource.Add(childML.Value);
										break;
								}
				// Devuelve el objeto
				return result;
		}

		/// <summary>
		///		Graba los resultados de un proyecto
		/// </summary>
		public void Save(ProjectModel project, ProjectTargetModel target, GenerationResultProjectModel result)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Añade los nodos
				foreach (string image in result.ImagesSource)
					nodeML.Nodes.Add(TagImageSource, image);
				// Graba el archivo
				new XMLWriter().Save(GetFileName(project, target), fileML);
		}

		/// <summary>
		///		Obtiene el nombre de un archivo de resultado de una generación de un proyecto
		/// </summary>
		private string GetFileName(ProjectModel project, ProjectTargetModel target)
		{
			return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(project.FileName),
										  LibCommonHelper.Files.HelperFiles.Normalize($"Generate {project.Name} {target.ProjectName}.rsml"));
		}
	}
}
