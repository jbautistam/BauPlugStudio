using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDataBaseStudio.Model.Reports;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Reports
{
	/// <summary>
	///		ViewModel para la edición de un archivo asociado a un informe
	/// </summary>
	public class ReportExecutionFileViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{   
		// Variables privadas
		private string _key, _fileName;

		public ReportExecutionFileViewModel(ReportExecutionModel execution, ReportExecutionFileModel file, string projectPath)
		{
			LoadReport(execution, file, projectPath);
		}

		/// <summary>
		///		Carga los datos de un informe
		/// </summary>
		private void LoadReport(ReportExecutionModel execution, ReportExecutionFileModel file, string projectPath)
		{ 
			// Combo con el modo de visualización de páginas hija
			ComboTypes = new MVVM.ViewModels.ComboItems.ComboViewModel(this, "ComboTypes");
			ComboTypes.AddItem((int) ReportExecutionFileModel.FileType.Image, "Imagen");
			ComboTypes.AddItem((int) ReportExecutionFileModel.FileType.Style, "Estilo");
			ComboTypes.AddItem((int) ReportExecutionFileModel.FileType.Font, "Fuente");
			ComboTypes.SelectedID = (int) ReportExecutionFileModel.FileType.Image;
			// Asigna los parámetros
			Execution = execution;
			File = file;
			ProjectPath = projectPath;
			// Asigna las propiedades
			if (File != null)
			{
				Key = File.GlobalId;
				FileName = File.FileName;
				ComboTypes.SelectedID = (int) File.IDType;
			}
			// Indica que aún no se ha hecho ninguna modificación
			base.IsUpdated = false;
		}

		/// <summary>
		///		Comprueba los datos introducidos en el formulario
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos
				if (Key.IsEmpty())
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Introduzca la clave del archivo");
				else if (ComboTypes.SelectedID == null)
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el tipo de archivo");
				else if (FileName.IsEmpty() || !System.IO.File.Exists(FileName))
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Seleccione un archivo");
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
				// Pasa los valores del formulario al objeto
				if (File == null)
				{
					File = new ReportExecutionFileModel();
					Execution.Files.Add(File);
				}
				File.GlobalId = Key;
				File.IDType = (ReportExecutionFileModel.FileType) (ComboTypes.SelectedID ?? 0);
				File.FileName = FileName;
				// Indica que no hay modificaciones pendientes y cierra el formulario
				IsUpdated = false;
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Parámetro de ejecución
		/// </summary>
		private ReportExecutionModel Execution { get; set; }

		/// <summary>
		///		Archivo
		/// </summary>
		private ReportExecutionFileModel File { get; set; }

		/// <summary>
		///		Directorio del proyecto
		/// </summary>
		public string ProjectPath { get; set; }

		/// <summary>
		///		Nombre
		/// </summary>
		public string Key
		{
			get { return _key; }
			set { CheckProperty(ref _key, value); }
		}

		/// <summary>
		///		Archivo
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
			set { CheckProperty(ref _fileName, value); }
		}

		/// <summary>
		///		Combo de tipos
		/// </summary>
		public MVVM.ViewModels.ComboItems.ComboViewModel ComboTypes { get; private set; }
	}
}
