using System;
using System.Windows;

namespace Bau.Applications.BauPlugStudio.Views.Tools.Configuration
{
	/// <summary>
	///		Ventana para mantenimiento de la lista de directorios de plugins
	/// </summary>
	public partial class PluginPathsView : Window
	{
		public PluginPathsView()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = new ViewModels.Tools.Configuration.PluginPathsViewModel();
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
		public ViewModels.Tools.Configuration.PluginPathsViewModel ViewModel { get; }
	}
}
