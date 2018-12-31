using System;

using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDataBaseStudio.Model.Queries;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Queries
{
	/// <summary>
	///		ViewModel para las tablas pivot
	/// </summary>
	public class QueryViewModel : BaseFormViewModel
	{   
		// Variables privadas
		private string _name, _description, _title, _sql;
		private System.Data.DataTable _results = null;

		public QueryViewModel(string title, string fileName, string projectPath)
		{
			Title = title;
			InitViewModel();
			LoadPivotTable(fileName, projectPath);
		}

		/// <summary>
		///		Inicializa el ViewModel
		/// </summary>
		private void InitViewModel()
		{ 
			ProcessCommand = new BaseCommand(parameter => ExecuteAction(nameof(ProcessCommand), parameter),
											 parameter => CanExecuteAction(nameof(ProcessCommand), parameter))
										.AddListener(this, nameof(ComboConnections));
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
		private void LoadComboConnections(Model.Connections.SchemaConnectionModelCollection connections)
		{ 
			// Limpia el combo
			ComboConnections = new MVVM.ViewModels.ComboItems.ComboViewModel(this, "ComboConnections");
			ComboConnections.Items.Clear();
			// Añade los elementos
			ComboConnections.AddItem(null, "<Seleccione una conexión>");
			foreach (Model.Connections.SchemaConnectionModel connection in connections)
				ComboConnections.AddItem(ComboConnections.Items.Count + 1, connection.Name, connection);
			// Selecciona el primer elemento
			ComboConnections.SelectedID = null;
		}

		/// <summary>
		///		Carga los datos de una tabla pivot
		/// </summary>
		private void LoadPivotTable(string fileName, string projectPath)
		{
			Model.Connections.SchemaConnectionModelCollection connections = new Application.Bussiness.SchemaConnectionBussiness().LoadByPath(projectPath);

				// Carga los datos de la tabla pivot
				Query = new Application.Bussiness.QueryBussiness().Load(fileName);
				// Carga el combo de conexiones
				LoadComboConnections(connections);
				// Asigna las propiedades
				FileName = fileName;
				ProjectPath = projectPath;
				Name = Query.Name;
				if (Name.IsEmpty())
					Name = System.IO.Path.GetFileNameWithoutExtension(FileName);
				Description = Query.Description;
				Sql = Query.SQL;
				ConnectionSelected = connections.Search(Query.LastConnectionGuid);
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
				case nameof(ProcessCommand):
						ProcessSql();
					break;
				case nameof(CopyCommand):
						CopyData();
					break;
				case nameof(ExportCommand):
						DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Exportar");
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
				case nameof(ProcessCommand):
					return ConnectionSelected != null;
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
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre");
				else if (Sql.IsEmpty())
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el contenido de la consulta");
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
				Query.Name = Name;
				Query.Description = Description;
				Query.SQL = Sql;
				Query.LastConnectionGuid = ConnectionSelected.GlobalId;
				// ... y graba el objeto
				new Application.Bussiness.QueryBussiness().Save(Query, FileName);
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
				DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Introduzca la sentencia SQL");
			else if (ConnectionSelected == null)
				DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Seleccione la conexión");
			else
				try
				{
					DataResults = new Application.Bussiness.SchemaConnectionBussiness().ProcessSelect(ConnectionSelected, Sql);
				}
				catch (Exception exception)
				{
					DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage($"Error al cargar los datos. {exception.Message}");
				}
		}

		/// <summary>
		///		Copia los datos en el portapapeles
		/// </summary>
		private void CopyData()
		{
		}

		/// <summary>
		///		Objeto de la consulta
		/// </summary>
		private QueryModel Query { get; set; }

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		///		Directorio de proyecto
		/// </summary>
		public string ProjectPath { get; private set; }

		/// <summary>
		///		Título
		/// </summary>
		public string Title
		{
			get { return _title; }
			set { CheckProperty(ref _title, value); }
		}

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
		///		Conexión seleccionada
		/// </summary>
		public Model.Connections.SchemaConnectionModel ConnectionSelected
		{
			get
			{
				if (ComboConnections.SelectedID != null &&
						  ComboConnections.SelectedItem.Tag is Model.Connections.SchemaConnectionModel)
					return (ComboConnections.SelectedItem.Tag as Model.Connections.SchemaConnectionModel);
				else
					return null;
			}
			set
			{
				if (value == null)
					ComboConnections.SelectedID = null;
				else
					foreach (MVVM.ViewModels.ComboItems.ComboItem itemCombo in ComboConnections.Items)
						if (itemCombo != null && itemCombo.Tag is Model.Connections.SchemaConnectionModel &&
								((itemCombo.Tag as Model.Connections.SchemaConnectionModel).GlobalId).EqualsIgnoreCase(value.GlobalId))
							ComboConnections.SelectedItem = itemCombo;
			}
		}

		/// <summary>
		///		Combo con las conexiones
		/// </summary>
		public MVVM.ViewModels.ComboItems.ComboViewModel ComboConnections { get; private set; }

		/// <summary>
		///		Comando para procesar la sentencia SQL
		/// </summary>
		public BaseCommand ProcessCommand { get; private set; }

		/// <summary>
		///		Comando para copiar los resultados
		/// </summary>
		public BaseCommand CopyCommand { get; private set; }

		/// <summary>
		///		Comando para exportar los resultados
		/// </summary>
		public BaseCommand ExportCommand { get; private set; }
	}
}
