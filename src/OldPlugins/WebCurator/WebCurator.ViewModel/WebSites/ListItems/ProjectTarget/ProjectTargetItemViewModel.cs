using System;

using Bau.Libraries.WebCurator.Model.WebSites;

namespace Bau.Libraries.WebCurator.ViewModel.WebSites.ListItems.ProjectTarget
{
	/// <summary>
	///		ViewModel para un proyecto destino
	/// </summary>
	public class ProjectTargetItemViewModel : BauMvvm.ViewModels.Forms.ControlItems.ControlItemViewModel
	{
		public ProjectTargetItemViewModel(ProjectTargetModel target) : base(target.ProjectFileName, target)
		{
			Target = target;
		}

		/// <summary>
		///		Proyecto destino
		/// </summary>
		public ProjectTargetModel Target { get; }
	}
}
