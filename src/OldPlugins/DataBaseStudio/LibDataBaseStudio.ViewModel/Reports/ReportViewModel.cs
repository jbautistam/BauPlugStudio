using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDataBaseStudio.Model.Reports;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Reports
{
	/// <summary>
	///		ViewModel para los informes
	/// </summary>
	public class ReportViewModel : BaseFormViewModel
	{   
		// Variables privadas
		private string _name, _description, _title, _content;
		private ReportExecutionListViewModel _listViewExecutions;

		public ReportViewModel(string title, string fileName, string projectPath)
		{
			Title = title;
			LoadReport(fileName, projectPath);
		}

		/// <summary>
		///		Carga los datos de un informe
		/// </summary>
		private void LoadReport(string fileName, string projectPath)
		{
			Report = new Application.Bussiness.ReportBussiness().Load(fileName);

				// Asigna las propiedades
				FileName = fileName;
				Name = Report.Name;
				if (Name.IsEmpty())
					Name = System.IO.Path.GetFileNameWithoutExtension(FileName);
				Description = Report.Description;
				Content = Report.ReportDefinition;
				// Carga la lista de parámetros de ejecución
				Executions = new ReportExecutionListViewModel(this, Report, projectPath);
				Executions.PropertyChanged += (sender, evntArgs) =>
														{
															if (evntArgs.PropertyName.EqualsIgnoreCase(nameof(Executions.ItemsUpdated)))
																IsUpdated = true;
														};
				// Indica que aún no se ha hecho ninguna modificación
				IsUpdated = false;
		}

		/// <summary>
		///		Ejecuta un comando
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(SaveCommand):
						Save();
					break;
			}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar un comando
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			return true;
		}

		/// <summary>
		///		Comprueba los datos introducidos en el formulario
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos
				if (Name.IsEmpty())
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre");
				else if (Content.IsEmpty())
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el contenido del informe");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos del archivo
		/// </summary>
		private void Save()
		{
			if (ValidateData())
			{ 
				// Pasa los valores del formulario al objeto
				Report.Name = Name;
				Report.Description = Description;
				Report.ReportDefinition = Content;
				// ... y graba el objeto
				new Application.Bussiness.ReportBussiness().Save(Report, FileName);
				// Indica que no hay modificaciones pendientes
				IsUpdated = false;
			}
		}

		/// <summary>
		///		Objeto de informe
		/// </summary>
		private ReportModel Report { get; set; }

		/// <summary>
		///		Título
		/// </summary>
		public string Title
		{
			get { return _title; }
			set { CheckProperty(ref _title, value); }
		}

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		///		Nombre
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Descripción
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { CheckProperty(ref _description, value); }
		}

		/// <summary>
		///		Definición del informe
		/// </summary>
		public string Content
		{
			get { return _content; }
			set { CheckProperty(ref _content, value); }
		}

		/// <summary>
		///		Conexiones
		/// </summary>
		public ReportExecutionListViewModel Executions
		{
			get { return _listViewExecutions; }
			set { CheckObject(ref _listViewExecutions, value); }
		}
	}
}
