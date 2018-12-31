using System;

using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.DatabaseStudio.Models.Queries;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Connections;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Projects.Queries
{
	/// <summary>
	///		ViewModel para las tablas pivot
	/// </summary>
	public class QueryViewModel : BaseFormViewModel
	{   
		// Variables privadas
		private string _name, _description, _sql;
		private ConnectionComboViewModel _comboConnections;
		private System.Data.DataTable _results;

		public QueryViewModel(Models.ProjectModel project, string fileName)
		{
			// Asigna las propiedades
			Project = project;
			FileName = fileName;
			LoadComboConnections();
			LoadQuery(fileName);
			// Asigna los comandos
			ProcessCommand = new BaseCommand(parameter => ExecuteAction(nameof(ProcessCommand), parameter));
			CopyCommand = new BaseCommand(parameter => ExecuteAction(nameof(CopyCommand), parameter),
										  parameter => CanExecuteAction(nameof(CopyCommand), parameter))
										.AddListener(this, nameof(DataResults));
			ExportCommand = new BaseCommand(parameter => ExecuteAction(nameof(ExportCommand), parameter),
										    parameter => CanExecuteAction(nameof(ExportCommand), parameter))
										.AddListener(this, nameof(DataResults));
		}

		/// <summary>
		///		Carga el combo de conexiones
		/// </summary>
		private void LoadComboConnections()
		{
			ComboConnections = new ConnectionComboViewModel(Project);
			ComboConnections.LoadConnections<Models.Connections.DatabaseConnectionModel>();
		}

		/// <summary>
		///		Carga los datos de la query
		/// </summary>
		private void LoadQuery(string fileName)
		{
			QueryModel query = new Application.Bussiness.QueryBussiness().Load(fileName);

				// Asigna las propiedades
				if (query != null)
				{
					Name = query.Name;
					Description = query.Description;
					Sql = query.SQL;
				}
				// Asigna el nombre del archivo si el nombre de la consulta está vacío
				if (string.IsNullOrEmpty(Name))
					Name = System.IO.Path.GetFileNameWithoutExtension(fileName);
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
				case nameof(ProcessCommand):
						ProcessSql();
					break;
				case nameof(CopyCommand):
						CopyData();
					break;
				case nameof(ExportCommand):
						MainViewModel.Instance.ControllerWindow.ShowMessage("Exportar");
					break;
			}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar un comando
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(CopyCommand):
				case nameof(ExportCommand):
					return _results != null && _results.Rows.Count > 0;
				default:
					return true;
			}
		}

		/// <summary>
		///		Comprueba los datos introducidos en el formulario
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos
				if (Name.IsEmpty())
					MainViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de la consulta");
				else if (Sql.IsEmpty())
					MainViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el contenido de la consulta");
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
				QueryModel query = new QueryModel();

					// Pasa los valores del formulario al objeto
					query.Name = Name;
					query.Description = Description;
					query.SQL = Sql;
					// ... y graba el objeto
					new Application.Bussiness.QueryBussiness().Save(query, FileName);
					// Indica que no hay modificaciones pendientes
					base.IsUpdated = false;
			}
		}

		/// <summary>
		///		Ejecuta la cadena SQL
		/// </summary>
		private void ProcessSql()
		{
			if (Sql.IsEmpty())
				MainViewModel.Instance.ControllerWindow.ShowMessage("Introduzca la sentencia SQL");
			else if (ComboConnections.SelectedConnection == null)
				MainViewModel.Instance.ControllerWindow.ShowMessage("Seleccione la conexión");
			else
				try
				{
					DataResults = new Application.Providers.DataProvider().LoadData(ComboConnections.SelectedConnection, Sql, null, 0, 200, out long records);
				}
				catch (Exception exception)
				{
					MainViewModel.Instance.ControllerWindow.ShowMessage($"Error al cargar los datos. {exception.Message}");
				}
		}

		/// <summary>
		///		Copia los datos en el portapapeles
		/// </summary>
		private void CopyData()
		{
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		private Models.ProjectModel Project { get; }

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string FileName { get; }

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
		///		SQL de la consulta
		/// </summary>
		public string Sql
		{
			get { return _sql; }
			set { CheckProperty(ref _sql, value); }
		}

		/// <summary>
		///		Tabla con los resultados
		/// </summary>
		public System.Data.DataTable DataResults
		{
			get { return _results; }
			set { CheckObject(ref _results, value); }
		}

		/// <summary>
		///		Combo de conexiones
		/// </summary>
		public ConnectionComboViewModel ComboConnections 
		{ 
			get { return _comboConnections; }
			set { CheckObject(ref _comboConnections, value); }
		}

		/// <summary>
		///		Comando para procesar la sentencia SQL
		/// </summary>
		public BaseCommand ProcessCommand { get; }

		/// <summary>
		///		Comando para copiar los resultados
		/// </summary>
		public BaseCommand CopyCommand { get; }

		/// <summary>
		///		Comando para exportar los resultados
		/// </summary>
		public BaseCommand ExportCommand { get; }
	}
}
