using System;
using System.Windows;

using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.ViewModel.Documents;

namespace Bau.Plugins.DocWriter.Views
{
	/// <summary>
	///		Formulario para crear una nueva referencia
	/// </summary>
	public partial class ReferenceNewView : Window
	{
		public ReferenceNewView(ProjectModel project, FileModel folderParent)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = new ReferenceNewViewModel(project, folderParent);
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
		public ReferenceNewViewModel ViewModel { get; }
	}
}
