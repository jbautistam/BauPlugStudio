using System;
using System.Windows.Controls;

using Bau.Libraries.LibDataBaseStudio.ViewModel.Reports;

namespace Bau.Plugins.DataBaseStudio.Views.Reports.UC
{
	/// <summary>
	///		Control para mostrar y seleccionar una lista de parámetros de ejecución
	/// </summary>
	public partial class ctlListFilesParameters : UserControl
	{
		public ctlListFilesParameters()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Inicializa el contexto del control
		/// </summary>
		public void InitControl(ReportExecutionFileListViewModel dataContext)
		{
			DataContext = dataContext;
		}
	}
}
