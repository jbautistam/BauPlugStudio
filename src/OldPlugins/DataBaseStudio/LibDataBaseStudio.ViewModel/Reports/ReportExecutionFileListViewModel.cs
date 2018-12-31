using System;

using Bau.Libraries.MVVM.ViewModels.ListItems;
using Bau.Libraries.LibDataBaseStudio.Model.Reports;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Reports
{
	/// <summary>
	///		ViewModel para la selección de una lista de conexiones
	/// </summary>
	public class ReportExecutionFileListViewModel : ListViewUpdatableViewModel<ReportExecutionFileListItemViewModel>
	{
		public ReportExecutionFileListViewModel(BauMvvm.ViewModels.BaseObservableObject form, string projectPath)
		{
			FormParent = form;
			ProjectPath = projectPath;
		}

		/// <summary>
		///		Carga la lista de archivos asociados a una ejecución de informe
		/// </summary>
		public void LoadFiles(ReportExecutionModel reportExecution)
		{ 
			// Guarda el parámetro
			ReportExecution = reportExecution ?? new ReportExecutionModel();
			// Inicializa la lista
			ListItems.Clear();
			// Carga los archivos en la lista
			foreach (ReportExecutionFileModel file in ReportExecution.Files)
				Add(new ReportExecutionFileListItemViewModel(FormParent, file));
		}

		/// <summary>
		///		Obtiene los archivos de la lista
		/// </summary>
		public ReportExecutionFileModelCollection GetFiles()
		{
			ReportExecutionFileModelCollection files = new ReportExecutionFileModelCollection();

				// Añade los elementos de la lista a la colección
				foreach (ReportExecutionFileListItemViewModel listItem in ListItems)
					if (listItem != null && listItem.Tag != null && listItem.Tag is ReportExecutionFileModel)
						files.Add(listItem.Tag as ReportExecutionFileModel);
				// Devuelve la colección de archivos
				return files;
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
		protected override bool UpdateItem(ReportExecutionFileListItemViewModel selectedItem)
		{
			ReportExecutionFileModel parameter = null;
			bool updated = false;

				// Asigna el parámetro de ejecución
				parameter = TransformItem(selectedItem);
				// Abre el formulario de modificación
				if (DataBaseStudioViewModel.Instance.ViewsController.OpenFormUpdateReportExecutionFile(new ReportExecutionFileViewModel(ReportExecution, parameter, ProjectPath)))
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
		protected override bool DeleteItem(ReportExecutionFileListItemViewModel selectedItem)
		{
			bool deleted = false;
			ReportExecutionFileModel parameter = TransformItem(selectedItem);

				// Borra el elemento
				if (parameter != null &&
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowQuestion("¿Realmente desea borrar este archivo?"))
				{ 
					// Elimina el elemento
					ReportExecution.Files.RemoveByID(parameter.GlobalId);
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
		private ReportExecutionFileModel TransformItem(ReportExecutionFileListItemViewModel item)
		{
			if (item != null && item.Tag is ReportExecutionFileModel)
				return item.Tag as ReportExecutionFileModel;
			else
				return null;
		}

		/// <summary>
		///		Actualiza la lista
		/// </summary>
		protected override void Refresh()
		{ 
			// Actualiza los elementos
			LoadFiles(ReportExecution);
			// Llama al método base
			base.Refresh();
		}

		/// <summary>
		///		Formulario padre
		/// </summary>
		public BauMvvm.ViewModels.BaseObservableObject FormParent { get; }

		/// <summary>
		///		Parámetro de ejecución
		/// </summary>
		public ReportExecutionModel ReportExecution { get; private set; }

		/// <summary>
		///		Directorio del proyecto
		/// </summary>
		public string ProjectPath { get; }
	}
}
