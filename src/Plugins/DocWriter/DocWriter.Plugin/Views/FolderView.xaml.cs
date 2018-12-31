using System;
using System.Windows;

using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.ViewModel.Solutions;

namespace Bau.Plugins.DocWriter.Views
{
	/// <summary>
	///		Ventana para creación de una carpeta de proyecto en DocWriter
	/// </summary>
	public partial class FolderView : Window
	{
		public FolderView(ProjectModel project, FileModel folderParent, FileModel folder)
		{
			FolderViewModel viewModel = new FolderViewModel(project, folderParent, folder);

				// Inicializa los componentes
				InitializeComponent();
				// Inicializa el ViewModel
				DataContext = viewModel;
				viewModel.Close += (sender, result) =>
												{
													DialogResult = result.IsAccepted;
													Close();
												};
		}
	}
}
