using System;
using System.Windows;

using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.ViewModel.Documents;

namespace Bau.Plugins.DocWriter.Views
{
	/// <summary>
	///		Formulario para seleccionar archivos de un proyecto
	/// </summary>
	public partial class SelectFilesProjectView : Window
	{
		public SelectFilesProjectView(ProjectModel project, FileModel.DocumentType idDocumentType, FilesModelCollection filesSelected)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = new SelectFilesProjectViewModel(project, idDocumentType, filesSelected);
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
		public SelectFilesProjectViewModel ViewModel { get; }
	}
}
