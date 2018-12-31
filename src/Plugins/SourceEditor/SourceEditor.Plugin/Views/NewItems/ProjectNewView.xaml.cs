using System;
using System.Windows;

using Bau.Libraries.SourceEditor.ViewModel.Solutions.NewItems;

namespace Bau.Libraries.SourceEditor.Plugin.Views.NewItems
{
	/// <summary>
	///		Ventana para creación de un proyecto en SourceEditor
	/// </summary>
	public partial class ProjectNewView : Window
	{
		public ProjectNewView(ProjectNewViewModel projectNewViewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = projectNewViewModel;
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
