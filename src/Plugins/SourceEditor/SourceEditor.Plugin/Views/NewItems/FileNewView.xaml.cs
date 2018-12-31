using System;
using System.Windows;

using Bau.Libraries.SourceEditor.ViewModel.Solutions.NewItems;

namespace Bau.Libraries.SourceEditor.Plugin.Views.NewItems
{
	/// <summary>
	///		Ventana para creación de un documento en SourceEditor
	/// </summary>
	public partial class FileNewView : Window
	{
		public FileNewView(FileNewViewModel viewModel)
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
		///		ViewModel de la ventana
		/// </summary>
		public FileNewViewModel ViewModel { get; }
	}
}
