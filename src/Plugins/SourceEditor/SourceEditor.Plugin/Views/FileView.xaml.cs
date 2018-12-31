using System;
using System.Windows.Controls;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.SourceEditor.Model.Solutions;
using Bau.Libraries.SourceEditor.ViewModel.Solutions;

namespace Bau.Libraries.SourceEditor.Plugin.Views
{
	/// <summary>
	///		Formulario para mostrar el contenido de un archivo de texto
	/// </summary>
	public partial class FileView : UserControl, IFormView
	{
		public FileView(FileModel file, Model.Definitions.FileDefinitionModel fileDefinition)
		{ 
			// Inicializa el componente
			InitializeComponent();
			// Inicializa la vista de datos
			grdData.DataContext = ViewModel = new FileViewModel(file);
			udtEditor.ViewModel = ViewModel;
			udtEditor.Text = ViewModel.Content;
			FormView = new BaseFormView(ViewModel);
			// Asigna el nombre de archivo
			udtEditor.FileName = file.FullFileName;
			// Cambia el modo de resalte del archivo
			if (fileDefinition != null && !string.IsNullOrEmpty(fileDefinition.ExtensionHighlight))
				udtEditor.ChangeHighLightByExtension(fileDefinition.ExtensionHighlight);
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
