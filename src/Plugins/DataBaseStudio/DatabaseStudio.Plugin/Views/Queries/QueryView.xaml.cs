using System;
using System.Windows.Controls;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Queries;

namespace Bau.Libraries.FullDatabaseStudio.Plugin.Views.Queries
{
	/// <summary>
	///		Vista del archivo de definición de una consulta
	/// </summary>
	public partial class QueryView : UserControl, IFormView
	{
		public QueryView(QueryViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa la vista de datos
			grdData.DataContext = ViewModel = viewModel;
			FormView = new BaseFormView(ViewModel);
			// Inicializa el editor
			udtEditor.ChangeHighLightByExtension("SQL");
			udtEditor.Text = ViewModel.Sql;
		}

		/// <summary>
		///		ViewModel
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel
		/// </summary>
		public QueryViewModel ViewModel { get; }

		private void udtEditor_Changed(object sender, EventArgs evntArgs)
		{
			ViewModel.Sql = udtEditor.Text;
		}
	}
}
