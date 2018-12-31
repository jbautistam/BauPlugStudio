using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.Common.Controllers;
using Bau.Applications.BauPlugStudio.ViewModels.Tools.TextFiles;
using Bau.Controls.BauMVVMControls.HelpPages.Model;
using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Applications.BauPlugStudio.Views.Tools.TextFiles
{
	/// <summary>
	///		Formulario para edición de un archivo con un árbol de ayuda
	/// </summary>
	public partial class TextFileHelpView : System.Windows.Controls.UserControl, IFormView
	{
		public TextFileHelpView(string fileName, string template = null, SystemControllerEnums.Editor editor = SystemControllerEnums.Editor.Unknown,
								string fileNameHelp = "")
		{ 
			// Inicializa el componente
			InitializeComponent();
			// Inicializa la vista de datos
			grdData.DataContext = ViewModel = new TextFileViewModel(fileName, template);
			udtEditor.Text = ViewModel.Content;
			FormView = new BaseFormView(ViewModel);
			// Cambia el modo de resalte del archivo
			udtEditor.FileName = fileName;
			switch (editor)
			{
				case SystemControllerEnums.Editor.Xml:
						udtEditor.ChangeHighLightByExtension(".xml");
					break;
				case SystemControllerEnums.Editor.Html:
						udtEditor.ChangeHighLightByExtension(".htm");
					break;
			}
			// Asigna el manejador de eventos de la apertura de ayudas
			trvHelp.HelpFileName = fileNameHelp;
			trvHelp.OpenHelp += (sender, evntArgs) => OpenHelp(evntArgs.HelpItem);
			// Indica que no se ha modificado el contenido
			ViewModel.IsUpdated = false;
		}

		/// <summary>
		///		Añade el código de ayuda al archivo que se está editando
		/// </summary>
		private void OpenHelp(HelpItemModel objHelp)
		{
			string code = objHelp.GetCode(0);

				if (!code.IsEmpty())
					udtEditor.InsertText(code);
		}

		/// <summary>
		///		ViewModel de los datos
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel de los datos
		/// </summary>
		public TextFileViewModel ViewModel { get; }

		private void udtEditor_Changed(object sender, EventArgs evntArgs)
		{
			ViewModel.Content = udtEditor.Text;
		}

		/// <summary>
		///		Directorio del cómic que se está editando
		/// </summary>
		private string PathComic { get; }
	}
}
