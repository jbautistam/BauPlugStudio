using System;

using Bau.Libraries.SourceEditor.Model.Definitions;

namespace Bau.Libraries.SourceEditor.ViewModel.Solutions.ListItems
{
	/// <summary>
	///		ViewModel para definiciones de proyectos
	/// </summary>
	public class ProjectDefinitionListItemViewModel : MVVM.ViewModels.ListItems.BaseListItemViewModel
	{
		public ProjectDefinitionListItemViewModel(ProjectDefinitionModel project)
		{
			Project = project;
			base.Text = project.Name;
			Module = project.Module;
			Type = project.Type;
			base.Tag = project;
		}

		/// <summary>
		///		Módulo
		/// </summary>
		public string Module { get; set; }

		/// <summary>
		///		Tipo de proyecto
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		///		Proyecto destino
		/// </summary>
		public ProjectDefinitionModel Project { get; }
	}
}
