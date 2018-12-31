using System;
using System.Windows.Controls;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.ViewModel.Solutions;

namespace Bau.Plugins.DocWriter.Views
{
	/// <summary>
	///		Formulario para mostrar el contenido de un archivo de texto
	/// </summary>
	public partial class FileView : UserControl, IFormView
	{
		public FileView(FileModel file)
		{ 
			// Inicializa el componente
			InitializeComponent();
			// Inicializa la vista de datos
			grdData.DataContext = ViewModel = new FileViewModel(file);
			udtEditor.ViewModel = ViewModel;
			udtEditor.Text = ViewModel.Content;
			FormView = new BaseFormView(ViewModel);
			// Cambia el modo de resalte del archivo
			udtEditor.FileName = file.FullFileName;
			// Indica que no se ha modificado el contenido
			ViewModel.IsUpdated = false;
		}

		/// <summary>
		///		ViewModel de los datos
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel de los datos
		/// </summary>
		public FileViewModel ViewModel { get; }

		private void udtEditor_Changed(object sender, EventArgs evntArgs)
		{
			ViewModel.Content = udtEditor.Text;
		}
	}
}
