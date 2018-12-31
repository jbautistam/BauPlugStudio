using System;
using System.Windows;

using Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Documenter;

namespace Bau.Plugins.SourceCodeDocumenter.Views.Documenter
{
	/// <summary>
	///		Formulario para el mantenimiento de los datos de un proyecto
	/// </summary>
	public partial class ProjectFileView : Window
	{
		public ProjectFileView(DocumenterProjectViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Asigna el contexto de la ventana
			DataContext = viewModel;
			viewModel.Close += (sender, result) =>
											{
												DialogResult = result.IsAccepted;
												Close();
											};
			// Inicializa el directorio de plantillas predeterminado
			if (string.IsNullOrEmpty(viewModel.PathTemplates))
				viewModel.PathTemplates = System.IO.Path.Combine(viewModel.ProjectPath, "Templates");
			// Inicializa el directorio de páginas predeterminado
			if (string.IsNullOrEmpty(viewModel.PathPages))
				viewModel.PathPages = System.IO.Path.Combine(viewModel.ProjectPath, "Pages");
			// Inicializa el directorio de generación predeterminado
			if (string.IsNullOrEmpty(viewModel.PathGenerate))
				viewModel.PathGenerate = System.IO.Path.Combine(viewModel.ProjectPath, "Build");
		}
	}
}
