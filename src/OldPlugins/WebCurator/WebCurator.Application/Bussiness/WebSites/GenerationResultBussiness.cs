using System;

using Bau.Libraries.WebCurator.Model.WebSites;
using Bau.Libraries.WebCurator.Repository.WebSites;

namespace Bau.Libraries.WebCurator.Application.Bussiness.WebSites
{
	/// <summary>
	///		Clase de negocio de <see cref=""/>
	/// </summary>
	public class GenerationResultBussiness
	{
		/// <summary>
		///		Carga el resultado de la generación de un proyecto
		/// </summary>
		public GenerationResultModel Load(ProjectModel project)
		{
			return new GenerationResultRepository().Load(project);
		}

		/// <summary>
		///		Graba el resultado de un archivo
		/// </summary>
		public void Save(ProjectModel project, GenerationResultModel result)
		{
			new GenerationResultRepository().Save(project, result);
		}

		internal void Save(ProjectModel Project, string projectTarget)
		{
			throw new NotImplementedException();
		}
	}
}
