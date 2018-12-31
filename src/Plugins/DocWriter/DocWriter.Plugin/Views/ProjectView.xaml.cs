using System;
using System.Windows.Controls;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.ViewModel.Solutions;

namespace Bau.Plugins.DocWriter.Views
{
	/// <summary>
	///		Ventana para mostrar los datos de un proyecto de DocWriter
	/// </summary>
	public partial class ProjectView : UserControl, IFormView
	{
		public ProjectView(ProjectModel project)
		{ 
			// Inicializa el componente
			InitializeComponent();
			// Inicializa la vista de datos
			grdData.DataContext = ViewModel = new ProjectViewModel(project);
			FormView = new BaseFormView(ViewModel);
			// Inicializa el control de plantillas
			udtTemplates.Project = project;
		}

		/// <summary>
		///		ViewModel asociado al formulario
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel asociado al formulario
		/// </summary>
		public ProjectViewModel ViewModel { get; }
	}
}