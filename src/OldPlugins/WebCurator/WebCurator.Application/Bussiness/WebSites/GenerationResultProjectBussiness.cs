using System;

namespace Bau.Libraries.WebCurator.Application.Bussiness.WebSites
{
	/// <summary>
	///		Clase de negocio de <see cref="GenerationResultProjectModel"/>
	/// </summary>
	public class GenerationResultProjectBussiness
	{
		/// <summary>
		///		Carga los resultados de un proyecto
		/// </summary>
		public Model.WebSites.GenerationResultProjectModel Load(Model.WebSites.ProjectModel project, Model.WebSites.ProjectTargetModel target)
		{
			return new Repository.WebSites.GenerationResultProjectRepository().Load(project, target);
		}

		/// <summary>
		///		Graba los resultados de un proyecto
		/// </summary>
		public void Save(Model.WebSites.ProjectModel project, Model.WebSites.ProjectTargetModel target, Model.WebSites.GenerationResultProjectModel result)
		{
			new Repository.WebSites.GenerationResultProjectRepository().Save(project, target, result);
		}
	}
}
