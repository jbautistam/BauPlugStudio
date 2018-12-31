using System;
using System.Windows.Controls;

using Bau.Controls.BauMVVMControls.HelpPages.Model;
using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Reports;

namespace Bau.Libraries.FullDatabaseStudio.Plugin.Views.Reports
{
	/// <summary>
	///		Vista del archivo de definición de la documentación
	/// </summary>
	public partial class ReportView : UserControl, IFormView
	{
		public ReportView(ReportViewModel viewModel, string helpFileName)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa la vista de datos
			grdData.DataContext = ViewModel = viewModel;
			FormView = new BaseFormView(ViewModel);
			// Inicializa el editor
			udtEditor.ChangeHighLightByExtension("xml");
			udtEditor.Text = ViewModel.Content;
			// Inicializa el árbol de ayudas
			udtHelpTree.HelpFileName = helpFileName;
			udtHelpTree.OpenHelp += (sender, evntArgs) => OpenHelp(evntArgs.HelpItem);
		}

		/// <summary>
		///		Añade la estructura de un elemento de ayuda
		/// </summary>
		private void OpenHelp(HelpItemModel helpItem)
		{
			string code = helpItem.GetCode(0);

				if (!string.IsNullOrEmpty(code))
					udtEditor.InsertText(code);
		}

		/// <summary>
		///		ViewModel
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel
		/// </summary>
		public ReportViewModel ViewModel { get; }

		private void udtEditor_Changed(object sender, EventArgs evntArgs)
		{
			ViewModel.Content = udtEditor.Text;
		}
	}
}
