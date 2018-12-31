using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.DatabaseStudio.Models;
using Bau.Libraries.DatabaseStudio.Models.Deployment;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Projects.Deployments
{
	/// <summary>
	///		ViewModel de <see cref="DeploymentModel"/>
	/// </summary>
	public class DeploymentViewModel : BaseFormViewModel
	{
		// Variables privadas
		private string _name, _description, _pathScriptsTarget, _pathFilesTarget;
		private DeploymentConnectionListViewModel _connectionsList;
		private Parameters.ParameterListViewModel _parametersListViewModel;
		private Scripts.TreeScriptsViewModel _scriptsTreeViewModel;
		private BauMvvm.ViewModels.Forms.ControlItems.ControlListViewModel _reportOutputListViewModel;

		public DeploymentViewModel(ProjectExplorerViewModel projectExplorerViewModel, DeploymentModel deployment)
		{
			// Inicializa los datos principales
			ExplorerViewModel = projectExplorerViewModel;
			Project = projectExplorerViewModel.Project;
			IsNew = deployment == null;
			if (deployment == null)
				Deployment = new DeploymentModel();
			else
				Deployment = deployment;
			ConnectionsListViewModel = new DeploymentConnectionListViewModel(Project);
			ParametersListViewModel = new Parameters.ParameterListViewModel();
			ScriptsTreeViewModel = new Scripts.TreeScriptsViewModel(ExplorerViewModel.ProjectPath, Deployment);
			ReportOutputListViewModel = new BauMvvm.ViewModels.Forms.ControlItems.ControlListViewModel();
			// Inicializa el viewModel
			InitViewModel();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitViewModel()
		{
			// Inicializa las propiedades básicas
			Name = Deployment.Name;
			Description = Deployment.Description;
			PathScriptsTarget = Deployment.PathScriptsTarget;
			PathFilesTarget = Deployment.PathFilesTarget;
			// Carga las conexiones
			ConnectionsListViewModel.LoadItems(Deployment);
			ParametersListViewModel.LoadItems(Deployment.Parameters);
			ScriptsTreeViewModel.LoadFiles();
			LoadReportOutputList();
		}

		/// <summary>
		///		Carga la lista de salida de los informes
		/// </summary>
		private void LoadReportOutputList()
		{
			// Limpia la lista
			ReportOutputListViewModel.Items.Clear();
			// Añade los elementos
			ReportOutputListViewModel.Add(GetReportFormatItem("Xml", DeploymentModel.ReportFormat.Xml, Deployment.ReportFormatTypes), false);
			ReportOutputListViewModel.Add(GetReportFormatItem("Html", DeploymentModel.ReportFormat.Html, Deployment.ReportFormatTypes), false);
			ReportOutputListViewModel.Add(GetReportFormatItem("Html con postproceso", DeploymentModel.ReportFormat.HtmlConverted, Deployment.ReportFormatTypes), false);
			ReportOutputListViewModel.Add(GetReportFormatItem("Pdf", DeploymentModel.ReportFormat.Pdf, Deployment.ReportFormatTypes), false);
		}

		/// <summary>
		///		Obtiene un elemento para la lista de formatos
		/// </summary>
		private ControlItemViewModel GetReportFormatItem(string text, DeploymentModel.ReportFormat format, List<DeploymentModel.ReportFormat> reportFormatSelected)
		{
			ControlItemViewModel item = new ControlItemViewModel(text, format);

				// Selecciona el elemento
				foreach (DeploymentModel.ReportFormat formatSelected in reportFormatSelected)	
					if (formatSelected == format)
						item.IsChecked = true;
				// Devuelve el elemento
				return item;
		}

		/// <summary>
		///		Ejecuta una acción
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
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(SaveCommand):
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validated = false;

				// Comprueba los datos introducidos
				if (!Project.GlobalId.Equals(ExplorerViewModel.Project.GlobalId, StringComparison.CurrentCultureIgnoreCase))
					MainViewModel.Instance.ControllerWindow.ShowMessage("Se ha cargado otro proyecto en el explorador. No se puede grabar la distribución en este archivo");
				else if (string.IsNullOrEmpty(Name))
					MainViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de la conexión");
				else if (!ConnectionsListViewModel.ValidateData())
					MainViewModel.Instance.ControllerWindow.ShowMessage("Seleccione la clave y conexión de los elementos de distribución");
				else if (string.IsNullOrEmpty(PathScriptsTarget))
					MainViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el directorio donde se guardan los scripts");
				else if (string.IsNullOrEmpty(PathFilesTarget))
					MainViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el directorio donde se guardan los archivos generados");
				else if (!ParametersListViewModel.ValidateData(out string error))
					MainViewModel.Instance.ControllerWindow.ShowMessage(error);
				else if (!ScriptsTreeViewModel.ValidateData(out error))
					MainViewModel.Instance.ControllerWindow.ShowMessage(error);
				else
					validated = true;
				// Devuelve el valor que indica si los datos son correctos
				return validated;
		}

		/// <summary>
		///		Guarda los datos del formulario en el modelo
		/// </summary>
		private void Save()
		{
			if (ValidateData())
			{
				// Asigna las propiedades
				Deployment.Name = Name;
				Deployment.Description = Description;
				Deployment.PathScriptsTarget = PathScriptsTarget;
				Deployment.PathFilesTarget = PathFilesTarget;
				// Añade los elementos
				ConnectionsListViewModel.GetConnections(Deployment.Connections);
				Deployment.Parameters.Clear();
				Deployment.Parameters.AddRange(ParametersListViewModel.GetParameters());
				Deployment.Scripts.Clear();
				Deployment.Scripts.AddRange(ScriptsTreeViewModel.GetScripts());
				// Añade los formatos de salida
				Deployment.ReportFormatTypes.Clear();
				foreach (ControlItemViewModel item in ReportOutputListViewModel.Items)
					if (item.Tag != null && item.IsChecked)
						Deployment.ReportFormatTypes.Add(item.Tag.ToString().GetEnum(DeploymentModel.ReportFormat.Xml));
				// Añade el modo de distribución al proyecto si es necesario
				if (IsNew)
				{
					Project.Deployments.Add(Deployment);
					IsNew = false;
				}
				// Graba los datos
				ExplorerViewModel.SaveProject();
				IsUpdated = false;
			}
		}

		/// <summary>
		///		ViewModel del explorador
		/// </summary>
		internal ProjectExplorerViewModel ExplorerViewModel { get; }

		/// <summary>
		///		Proyecto asociado al explorador cuando se abrió este documento
		/// </summary>
		private ProjectModel Project { get; }

		/// <summary>
		///		Conexión que se está modificando
		/// </summary>
		public DeploymentModel Deployment { get; }

		/// <summary>
		///		Indica si es un dato nuevo
		/// </summary>
		internal bool IsNew { get; private set; }

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
		///		Directorio destino de los scripts
		/// </summary>
		public string PathScriptsTarget
		{
			get { return _pathScriptsTarget; }
			set { CheckProperty(ref _pathScriptsTarget, value); }
		}

		/// <summary>
		///		Directorio destino de los archivos
		/// </summary>
		public string PathFilesTarget
		{
			get { return _pathFilesTarget; }
			set { CheckProperty(ref _pathFilesTarget, value); }
		}

		/// <summary>
		///		ViewModel con la lista de conexiones
		/// </summary>
		public DeploymentConnectionListViewModel ConnectionsListViewModel
		{
			get { return _connectionsList; }
			set { CheckObject(ref _connectionsList, value); }
		}

		/// <summary>
		///		ViewModel con la lista de parámetros
		/// </summary>
		public Parameters.ParameterListViewModel ParametersListViewModel
		{
			get { return _parametersListViewModel; }
			set { CheckObject(ref _parametersListViewModel, value); }
		}

		/// <summary>
		///		ViewModel con el árbol de scripts
		/// </summary>
		public Scripts.TreeScriptsViewModel ScriptsTreeViewModel
		{
			get { return _scriptsTreeViewModel; }
			set { CheckObject(ref _scriptsTreeViewModel, value); }
		}

		/// <summary>
		///		ViewModel con la lista de tipos de salida para los informes
		/// </summary>
		public BauMvvm.ViewModels.Forms.ControlItems.ControlListViewModel ReportOutputListViewModel
		{
			get { return _reportOutputListViewModel; }
			set { CheckObject(ref _reportOutputListViewModel, value); }
		}
	}
}
