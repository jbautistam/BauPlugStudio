using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.WebCurator.Model.WebSites;

namespace Bau.Libraries.WebCurator.Repository.WebSites
{
	/// <summary>
	///		Repository de <see cref="GenerationResultModel"/>
	/// </summary>
	public class GenerationResultRepository
	{   
		// Constantes privadas
		private const string TagRoot = "Result";
		private const string TagDateLast = "DateLast";
		private const string TagImageSource = "ImageSource";

		/// <summary>
		///		Carga el resultado de la generación de un proyecto
		/// </summary>
		public GenerationResultModel Load(ProjectModel project)
		{
			GenerationResultModel result = new GenerationResultModel();
			string fileName = GetFileName(project);
			MLFile fileML = new XMLParser().Load(fileName);

				// Carga el archivo
				foreach (MLNode nodeML in fileML.Nodes)
					if (nodeML.Name == TagRoot)
						foreach (MLNode childML in nodeML.Nodes)
							switch (childML.Name)
							{
								case TagDateLast:
										result.DateLast = childML.Value.GetDateTime() ?? DateTime.Now.AddDays(-1);
									break;
							}
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Graba el resultado de un archivo
		/// </summary>
		public void Save(ProjectModel project, GenerationResultModel result)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Añade los nodos
				nodeML.Nodes.Add(TagDateLast, result.DateLast);
				// Graba el archivo
				new XMLWriter().Save(GetFileName(project), fileML);
		}

		/// <summary>
		///		Obtiene el nombre de un archivo de resultado de una generación de un proyecto
		/// </summary>
		private string GetFileName(ProjectModel project)
		{
			return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(project.FileName),
										  $"{System.IO.Path.GetFileNameWithoutExtension(project.FileName)}_Generate.rsml");
		}
	}
}
