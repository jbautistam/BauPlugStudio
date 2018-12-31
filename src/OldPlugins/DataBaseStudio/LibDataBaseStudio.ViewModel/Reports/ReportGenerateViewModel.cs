using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDataBaseStudio.Model.Reports;
using Bau.Libraries.LibDataBaseStudio.Application.Services;
using Bau.Libraries.LibDataBaseStudio.Model.Connections;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Reports
{
	/// <summary>
	///		ViewModel para la edición de los parámetros de ejecución de un informe
	/// </summary>
	public class ReportGenerateViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{   
		// Variables privadas
		private string _name, _description, _fileName, _fileMask;

		public ReportGenerateViewModel(ReportModel report, string reportFileName, string projectPath)
		{
			ReportFileName = reportFileName;
			LoadReport(report, projectPath);
		}

		/// <summary>
		///		Carga los datos de un informe
		/// </summary>
		private void LoadReport(ReportModel report, string projectPath)
		{ 
			// Asigna los parámetros
			Report = report;
			ProjectPath = projectPath;
			// Asigna las propiedades
			Name = Report.Name;
			Description = Report.Description;
			// Inicializa el combo con los parámetros de ejecución
			ComboExecutionParameters = new MVVM.ViewModels.ComboItems.ComboViewModel(this, "ComboExecutionParameters");
			ComboExecutionParameters.AddItem(0, "<Seleccione un elemento>");
			foreach (ReportExecutionModel execution in report.ExecutionParameters)
				ComboExecutionParameters.AddItem(execution.ID, execution.Name, execution);
			ComboExecutionParameters.SelectedID = 0;
			// Incializa el combo con los tipos de informes
			ComboOutputMode = new MVVM.ViewModels.ComboItems.ComboViewModel(this, "ComboOutputMode");
			ComboOutputMode.AddItem((int) ReportCompiler.ReportType.Unknown, "<Seleccione un elemento>");
			ComboOutputMode.AddItem((int) ReportCompiler.ReportType.Pdf, "PDF");
			ComboOutputMode.AddItem((int) ReportCompiler.ReportType.Html, "HTML");
			ComboOutputMode.SelectedID = (int) ReportCompiler.ReportType.Unknown;
			ComboOutputMode.PropertyChanged += (sender, evntArgs) =>
													{
														if (evntArgs.PropertyName.EqualsIgnoreCase("SelectedItem"))
															TreatChangeReportType();
													};
			// Asigna la máscara de archivos inicial
			TreatChangeReportType();
			// Asigna los últimos parámetros
			AssignLastParameters(report);
			// Indica que aún no se ha hecho ninguna modificación
			base.IsUpdated = false;
		}

		/// <summary>
		///		Asigna los últimos parámetros
		/// </summary>
		private void AssignLastParameters(ReportModel report)
		{
			if (!report.LastExecutionParameterName.IsEmpty())
				ComboExecutionParameters.SelectedText = report.LastExecutionParameterName;
			if (report.LastExecutionOuputMode > 0)
				ComboOutputMode.SelectedID = report.LastExecutionOuputMode;
			if (!report.LastExecutionFileName.IsEmpty())
				FileName = report.LastExecutionFileName;
		}

		/// <summary>
		///		Asigna la máscara de archivos y cambia la extensión del archivo
		/// </summary>
		private void TreatChangeReportType()
		{
			ReportCompiler.ReportType reportType = GetOutputMode();
			string extension = "";

				// Asigna el tipo de archivos
				switch (reportType)
				{
					case ReportCompiler.ReportType.Pdf:
							MaskFiles = "Archivos PDF (*.pdf)|*.pdf";
							extension = ".pdf";
						break;
					case ReportCompiler.ReportType.Html:
							MaskFiles = "Archivos HTML (*.htm)|*.htm";
							extension = ".htm";
						break;
				}
				// Añade todos los archivos a la máscara
				MaskFiles = MaskFiles.AddWithSeparator("Todos los archivos (*.*)|*.*", "|", false);
				// Cambia la extensión del archivo
				if (!FileName.IsEmpty())
					FileName = LibCommonHelper.Files.HelperFiles.ChangeFileNameExtension(FileName, extension);
		}

		/// <summary>
		///		Obtiene el modo de salida
		/// </summary>
		private ReportCompiler.ReportType GetOutputMode()
		{
			return (ReportCompiler.ReportType) (ComboOutputMode.SelectedID ?? 0);
		}

		/// <summary>
		///		Comprueba los datos introducidos en el formulario
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos
				if (ComboExecutionParameters.SelectedID == 0)
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Seleccione los parámetros de ejecución del informe");
				else if (GetOutputMode() == ReportCompiler.ReportType.Unknown)
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el tipo de archivo de salida del informe");
				else if (FileName.IsEmpty())
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de archivo que se va a generar");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos del archivo
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{
				ReportExecutionModel reportExecution = ComboExecutionParameters.SelectedTag as ReportExecutionModel;
				bool generated = false;

					// Genera el archivo
					generated = new ReportCompiler().Generate(System.IO.Path.GetDirectoryName(ReportFileName), Report,
															  GetSelectedConnections(reportExecution),
															  reportExecution,
															  (ReportCompiler.ReportType) (ComboOutputMode.SelectedID ?? 0),
															  FileName, out string error);
					// Muestra el resultado
					if (!generated)
						DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage($"Error en la generación del archivo.{Environment.NewLine}{error}");
					else if (DataBaseStudioViewModel.Instance.ControllerWindow.ShowQuestion
											($"Archivo generado.{Environment.NewLine}¿Desea abrir el archivo?"))
						LibSystem.Files.WindowsFiles.OpenDocumentShell(FileName);
					// Graba los últimos parámetros
					SaveLastExecutionParameters();
					// Indica que no hay modificaciones pendientes y cierra el formulario
					IsUpdated = false;
					RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Graba los últimos parámetros de ejecución
		/// </summary>
		private void SaveLastExecutionParameters()
		{
			ReportModel report = new Application.Bussiness.ReportBussiness().Load(ReportFileName);

				if (!report.ReportDefinition.IsEmpty())
				{ 
					// Asigna los últimos parámetros de ejecución
					report.LastExecutionParameterName = ComboExecutionParameters.SelectedText;
					report.LastExecutionOuputMode = ComboOutputMode.SelectedID ?? 0;
					report.LastExecutionFileName = FileName;
					// Graba el informe
					new Application.Bussiness.ReportBussiness().Save(report, ReportFileName);
				}
		}

		/// <summary>
		///		Obtiene las conexiones seleccionadas
		/// </summary>
		private SchemaConnectionModelCollection GetSelectedConnections(ReportExecutionModel reportExecution)
		{
			return new Application.Bussiness.SchemaConnectionBussiness().LoadByConnectionItem(ProjectPath, reportExecution);
		}

		/// <summary>
		///		Nombre del archivo donde se encuentra el informe
		/// </summary>
		private string ReportFileName { get; set; }

		/// <summary>
		///		Objeto de informe
		/// </summary>
		private ReportModel Report { get; set; }

		/// <summary>
		///		Directorio del proyecto
		/// </summary>
		private string ProjectPath { get; set; }

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
		///		Nombre de archivo que se va a generar
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
			set { CheckProperty(ref _fileName, value); }
		}

		/// <summary>
		///		Máscara de selección de archivos para grabación
		/// </summary>
		public string MaskFiles
		{
			get { return _fileMask; }
			set { CheckProperty(ref _fileMask, value); }
		}

		/// <summary>
		///		Combo con los diferentes parámetros de ejecución
		/// </summary>
		public MVVM.ViewModels.ComboItems.ComboViewModel ComboExecutionParameters { get; private set; }

		/// <summary>
		///		Combo con los modos de salida
		/// </summary>
		public MVVM.ViewModels.ComboItems.ComboViewModel ComboOutputMode { get; private set; }
	}
}
