using System;

using Bau.Libraries.Plugins.ViewModels.Controllers.Processes;
using Bau.Libraries.WebCurator.Model.WebSites;

namespace Bau.Libraries.WebCurator.ViewModel.Controllers
{
	/// <summary>
	///		Procesador automático de WebCurator
	/// </summary>
	internal class AutomaticProcessor : AbstractPlannedProcess
	{
		public AutomaticProcessor(bool autoStart, int minutesBetweenProcess) : base(WebCuratorViewModel.Instance.ModuleName,
																					"WebCurator", "Procesador automático de WebCurator",
																					minutesBetweenProcess, autoStart)
		{
		}

		/// <summary>
		///		Procesa la generación de sitios automáticos
		/// </summary>
		protected override void Execute()
		{
			ProjectModelCollection projects = new Application.Bussiness.WebSites.ProjectBussiness().LoadAllFull(WebCuratorViewModel.Instance.PathLibrary);

				// Recorre las cuentas enviando los datos necesarios
				foreach (ProjectModel project in projects)
				{
					GenerationResultModel result = new Application.Bussiness.WebSites.GenerationResultBussiness().Load(project);

						if (result.DateLast.AddHours(project.HoursBetweenGenerate) <= DateTime.Now)
							new FullProcessor(WebCuratorViewModel.Instance.ModuleName, project, true).Process();
				}
		}
	}
}
