using System;
using System.Windows;

using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.ViewModel.Solutions;

namespace Bau.Plugins.DocWriter.Views
{
	/// <summary>
	///		Ventana para creación de un documento en DocWriter
	/// </summary>
	public partial class FileNewView : Window
	{
		public FileNewView(ProjectModel project, FileModel folderParent)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = new FileNewViewModel(project, folderParent);
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
		public FileNewViewModel ViewModel { get; private set; }
	}
}
