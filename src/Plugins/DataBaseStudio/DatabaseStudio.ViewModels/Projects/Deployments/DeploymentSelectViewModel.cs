using System;
using System.Collections.ObjectModel;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.BauMvvm.ViewModels.Forms.Dialogs;
using Bau.Libraries.DatabaseStudio.Models;
using Bau.Libraries.DatabaseStudio.Models.Deployment;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Projects.Deployments
{
	/// <summary>
	///		ViewModel para seleccionar de <see cref="DeploymentModel"/>
	/// </summary>
	public class DeploymentSelectViewModel : BaseDialogViewModel
	{
		// Variables privadas
		private ObservableCollection<ControlItemViewModel> _items;
		private ControlItemViewModel _selectedItem;

		public DeploymentSelectViewModel(ProjectModel project)
		{
			LoadDeployments(project);
		}

		/// <summary>
		///		Carga las distribuciones
		/// </summary>
		public void LoadDeployments(ProjectModel project)
		{
			// Crea los elementos del combo
			Items = new ObservableCollection<ControlItemViewModel>();
			// Añade los datos
			Items.Add(new ControlItemViewModel("<Seleccione una distribución>", null));
			foreach (DeploymentModel deployment in project.Deployments)
				Items.Add(new ControlItemViewModel(deployment.Name, deployment));
			// Selecciona el primer elemento
			SelectedItem = Items[0];
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validated = false;

				// Comprueba los datos introducidos
				if (SelectedDeployment == null)
					MainViewModel.Instance.ControllerWindow.ShowMessage("Seleccione la distribución");
				else
					validated = true;
				// Devuelve el valor que indica si los datos son correctos
				return validated;
		}

		/// <summary>
		///		Guarda los datos del formulario en el modelo
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
				RaiseEventClose(true);
		}

		/// <summary>
		///		Conexiones
		/// </summary>
		public ObservableCollection<ControlItemViewModel> Items
		{
			get { return _items; }
			set { CheckObject(ref _items, value); }
		}

		/// <summary>
		///		Elemento seleccionado
		/// </summary>
		public ControlItemViewModel SelectedItem
		{
			get { return _selectedItem; }
			set { CheckProperty(ref _selectedItem, value); }
		}

		/// <summary>
		///		Distribución seleccionada
		/// </summary>
		public DeploymentModel SelectedDeployment
		{
			get { return SelectedItem?.Tag as DeploymentModel; }
		}
	}
}
