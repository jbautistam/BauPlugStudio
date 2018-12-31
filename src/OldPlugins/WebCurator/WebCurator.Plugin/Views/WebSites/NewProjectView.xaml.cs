using System;
using System.Windows;

using Bau.Libraries.WebCurator.ViewModel.WebSites;

namespace Bau.Libraries.WebCurator.Plugin.Views.WebSites
{
	/// <summary>
	///		Formulario para mantenimiento de un nuevo <see cref="ProjectModel"/>
	/// </summary>
	public partial class NewProjectView : Window
	{
		public NewProjectView(NewProjectViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = viewModel;
			DataContext = ViewModel;
			ViewModel.Close += (sender, result) =>
												{
													DialogResult = result.IsAccepted;
													Close();
												};
		}

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public NewProjectViewModel ViewModel { get; }
	}
}