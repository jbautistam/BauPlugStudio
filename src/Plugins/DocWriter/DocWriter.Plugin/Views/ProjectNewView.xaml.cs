using System;
using System.Windows;

using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.ViewModel.Solutions;

namespace Bau.Plugins.DocWriter.Views
{
	/// <summary>
	///		Ventana para creación de un proyecto en DocWriter
	/// </summary>
	public partial class ProjectNewView : Window
	{
		public ProjectNewView(SolutionModel solution, SolutionFolderModel folder)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = new ProjectNewViewModel(solution, folder);
			DataContext = ViewModel;
			ViewModel.Close += (sender, result) =>
											{
												DialogResult = result.IsAccepted;
												Close();
											};
		}

		/// <summary>
		///		ViewModel de la ventana
		/// </summary>
		public ProjectNewViewModel ViewModel { get; }
	}
}
