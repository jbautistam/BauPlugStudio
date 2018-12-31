using System;

using Bau.Libraries.MVVM.ViewModels.ListItems;
using Bau.Libraries.LibDataBaseStudio.Model.Reports;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Reports
{
	/// <summary>
	///		ViewModel para la selección de una lista de parámetros de ejecución
	/// </summary>
	public class ReportExecutionListViewModel : ListViewUpdatableViewModel<ReportExecutionListItemViewModel>
	{
		public ReportExecutionListViewModel(BauMvvm.ViewModels.BaseObservableObject form, ReportModel report, string projectPath)
		{
			FormParent = form;
			LoadData(report, projectPath);
		}

		/// <summary>
		///		Carga la lista de conexiones
		/// </summary>
		public void LoadData(ReportModel report, string projectPath)
		{   
			// Inicializa los parámetros
			Report = report;
			ProjectPath = projectPath;
			// Inicializa los informes
			ListItems.Clear();
			// Carga los elementos en la lista
			foreach (ReportExecutionModel execution in report.ExecutionParameters)
				Add(new ReportExecutionListItemViewModel(FormParent, execution));
		}

		/// <summary>
		///		Crea un nuevo elemento
		/// </summary>
		protected override bool NewItem()
		{
			return UpdateItem(null);
		}

		/// <summary>
		///		Modifica un elemento
		/// </summary>
		protected override bool UpdateItem(ReportExecutionListItemViewModel selectedItem)
		{
			ReportExecutionModel parameter = null;
			bool updated = false;

				// Asigna el parámetro de ejecución
				parameter = TransformItem(selectedItem);
				// Abre el formulario de modificación
				if (DataBaseStudioViewModel.Instance.ViewsController.OpenFormUpdateReportExecutionParameter(new ReportExecutionViewModel(Report, parameter, ProjectPath)))
				{
					updated = true;
					Refresh();
				}
				// Devuelve el valor que indica si se ha modificado
				return updated;
		}

		/// <summary>
		///		Borra un elemento
		/// </summary>
		protected override bool DeleteItem(ReportExecutionListItemViewModel selectedItem)
		{
			bool deleted = false;
			ReportExecutionModel parameter = TransformItem(selectedItem);

				// Borra el elemento
				if (parameter != null &&
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowQuestion("¿Realmente desea borrar este parámetro?"))
				{ 
					// Elimina el elemento
					Report.ExecutionParameters.RemoveByID(parameter.GlobalId);
					// Actualiza la lista e indica que se ha borrado
					Refresh();
					deleted = true;
				}
				// Devuelve el valor que indica si se ha borrado
				return deleted;
		}

		/// <summary>
		///		Obtiene el objeto ReportExecutionModel a partir del elemento seleccionado (ReportExecutionListItemViewModel)
		/// </summary>
		private ReportExecutionModel TransformItem(ReportExecutionListItemViewModel item)
		{
			if (item != null && item.Tag is ReportExecutionModel)
				return item.Tag as ReportExecutionModel;
			else
				return null;
		}

		/// <summary>
		///		Actualiza la lista
		/// </summary>
		protected override void Refresh()
		{ 
			// Actualiza los elementos
			LoadData(Report, ProjectPath);
			// Llama al método base
			base.Refresh();
		}

		/// <summary>
		///		Formulario padre
		/// </summary>
		public BauMvvm.ViewModels.BaseObservableObject FormParent { get; }

		/// <summary>
		///		Informe
		/// </summary>
		public ReportModel Report { get; private set; }

		/// <summary>
		///		Directorio del proyecto
		/// </summary>
		public string ProjectPath { get; private set; }
	}
}
