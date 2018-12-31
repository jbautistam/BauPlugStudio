using System;

using Bau.Libraries.LibDataBaseStudio.Model.Reports;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Reports
{
	/// <summary>
	///		ViewModel para los elementos del listView de conexiones de documentación
	/// </summary>
	public class ReportExecutionListItemViewModel : MVVM.ViewModels.ListItems.BaseListItemViewModel
	{
		public ReportExecutionListItemViewModel(BauMvvm.ViewModels.BaseObservableObject form, ReportExecutionModel execution) : base(form)
		{
			Execution = execution;
			Text = execution.Name;
			Tag = execution;
		}

		/// <summary>
		///		Conexión
		/// </summary>
		public ReportExecutionModel Execution { get; }
	}
}
