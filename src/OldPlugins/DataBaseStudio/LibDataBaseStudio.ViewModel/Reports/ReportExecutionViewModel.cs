using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDataBaseStudio.Model.Reports;
using Bau.Libraries.LibDataStructures.Collections;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Reports
{
	/// <summary>
	///		ViewModel para la edición de los parámetros de ejecución de un informe
	/// </summary>
	public class ReportExecutionViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{   
		// Variables privadas
		private string _name, _description, _parameters, _fixedParameters;
		private Connections.ConnectionListViewModel _listViewConnections;
		private ReportExecutionFileListViewModel _listViewFiles;

		public ReportExecutionViewModel(ReportModel report, ReportExecutionModel execution, string projectPath)
		{
			LoadReport(report, execution, projectPath);
		}

		/// <summary>
		///		Carga los datos de un informe
		/// </summary>
		private void LoadReport(ReportModel report, ReportExecutionModel execution, string projectPath)
		{ 
			// Asigna los parámetros
			Report = report;
			Execution = execution;
			ProjectPath = projectPath;
			// Asigna las propiedades
			if (Execution != null)
			{
				Name = Execution.Name;
				Description = Execution.Description;
				Parameters = ConvertParameters(Execution.Parameters);
				FixedParameters = ConvertParameters(Execution.FixedParameters);
			}
			// Carga la lista de conexiones
			Connections = new Connections.ConnectionListViewModel(this, projectPath);
			Connections.LoadConnections(Execution);
			// Carga la lista de archivos
			Files = new ReportExecutionFileListViewModel(this, projectPath);
			Files.LoadFiles(Execution);
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
				if (Name.IsEmpty())
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre");
				else if (Connections.GetGuidConnectionsSelected().Count == 0)
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Seleccione al menos una conexión");
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
				if (Execution == null)
				{
					Execution = new ReportExecutionModel();
					Report.ExecutionParameters.Add(Execution);
				}
				Execution.Name = Name;
				Execution.Description = Description;
				// Añade los parámetros
				Execution.Parameters.Clear();
				Execution.Parameters.AddRange(SplitParameters(Parameters));
				Execution.FixedParameters.Clear();
				Execution.FixedParameters.AddRange(SplitParameters(FixedParameters));
				Execution.Files.Clear();
				Execution.Files.AddRange(Files.GetFiles());
				// Asigna las conexiones
				Execution.ConnectionsGuid.Clear();
				Execution.ConnectionsGuid.AddRange(Connections.GetGuidConnectionsSelected());
				// Indica que no hay modificaciones pendientes y cierra el formulario
				IsUpdated = false;
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Convierte una colección de parámetros en una cadena
		/// </summary>
		private string ConvertParameters(ParameterModelCollection parameters)
		{
			string result = "";

				// Añade los parámetros
				foreach (ParameterModel parameter in parameters)
					result = result.AddWithSeparator($"{parameter.ID} = {parameter.Value}", Environment.NewLine, false);
				// Devuelve la cadena
				return result;
		}

		/// <summary>
		///		Separa los parámetros
		/// </summary>
		private ParameterModelCollection SplitParameters(string queryParameters)
		{
			ParameterModelCollection parameters = new ParameterModelCollection();

				// Separa los parámetros
				if (!queryParameters.IsEmpty())
				{
					System.Collections.Generic.List<string> parametersLines = queryParameters.SplitByString(Environment.NewLine);

						if (parametersLines != null && parametersLines.Count > 0)
							foreach (string line in parametersLines)
							{
								string[] parts = line.Split('=');

									if (parts != null && parts.Length == 2)
										parameters.Add(parts[0].TrimIgnoreNull(), parts[1].TrimIgnoreNull());
							}
				}
				// Devuelve la colección
				return parameters;
		}

		/// <summary>
		///		Objeto de informe
		/// </summary>
		private ReportModel Report { get; set; }

		/// <summary>
		///		Parámetro de ejecución
		/// </summary>
		private ReportExecutionModel Execution { get; set; }

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
		///		Parámetros del informe
		/// </summary>
		public string Parameters
		{
			get { return _parameters; }
			set { CheckProperty(ref _parameters, value); }
		}

		/// <summary>
		///		Parámetros fijos
		/// </summary>
		public string FixedParameters
		{
			get { return _fixedParameters; }
			set { CheckProperty(ref _fixedParameters, value); }
		}

		/// <summary>
		///		Archivos asociados a la ejecución
		/// </summary>
		public ReportExecutionFileListViewModel Files
		{
			get { return _listViewFiles; }
			set { CheckObject(ref _listViewFiles, value); }
		}

		/// <summary>
		///		Conexiones
		/// </summary>
		public Connections.ConnectionListViewModel Connections
		{
			get { return _listViewConnections; }
			set { CheckObject(ref _listViewConnections, value); }
		}
	}
}
