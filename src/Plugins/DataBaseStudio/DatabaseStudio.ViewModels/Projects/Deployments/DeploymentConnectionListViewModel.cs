using System;
using System.Collections.Generic;

using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.DatabaseStudio.Models;
using Bau.Libraries.DatabaseStudio.Models.Connections;
using Bau.Libraries.DatabaseStudio.Models.Deployment;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Projects.Deployments
{
	/// <summary>
	///		Lista de <see cref="DeploymentConnectionViewModel"/>
	/// </summary>
	public class DeploymentConnectionListViewModel : ControlListViewModel
	{
		public DeploymentConnectionListViewModel(ProjectModel project)
		{
			// Inicializa las propiedades
			Project = project;
			// Inicializa los comandos
			NewItemCommand = new BaseCommand(parameter => NewItem());
			DeleteItemCommand = new BaseCommand(parameter => DeleteItem(), parameter => SelectedItem != null)
											.AddListener(this, nameof(SelectedItem));
		}

		/// <summary>
		///		Carga los elementos
		/// </summary>
		internal void LoadItems(DeploymentModel deployment)
		{
			foreach (KeyValuePair<string, DatabaseConnectionModel> item in deployment.Connections)
				Add(new DeploymentConnectionViewModel(Project, item.Key, item.Value), false);
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		internal bool ValidateData()
		{
			bool validated = true;

				// Comprueba los elementos
				foreach (ControlItemViewModel viewModel in Items)
					if (viewModel is DeploymentConnectionViewModel item)
					{
						if (string.IsNullOrWhiteSpace(item.Key) ||
								item.ComboConnections.SelectedConnection == null)
							validated = false;
					}
				// Devuelve el valor que indica si los datos son correctos
				return validated;
		}

		/// <summary>
		///		Añade las conexiones
		/// </summary>
		internal void GetConnections(Dictionary<string, DatabaseConnectionModel> connections)
		{
			// Limpia las conexiones
			connections.Clear();
			// Añade las seleccionadas
			foreach (ControlItemViewModel viewModel in Items)
				if (viewModel is DeploymentConnectionViewModel item)
					connections.Add(item.Key, item.ComboConnections.SelectedConnection as DatabaseConnectionModel);
		}

		/// <summary>
		///		Crea un nuevo elemento
		/// </summary>
		private void NewItem()
		{
			Add(new DeploymentConnectionViewModel(Project, string.Empty, null), true);
		}

		/// <summary>
		///		Borra el elemento seleccionado
		/// </summary>
		private void DeleteItem()
		{
			if (SelectedItem != null &&
					MainViewModel.Instance.ControllerWindow.ShowQuestion("¿Desea eliminar el elemento?"))
				Items.Remove(SelectedItem);
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		public ProjectModel Project { get; }

		/// <summary>
		///		Comando de nuevo elemento
		/// </summary>
		public BaseCommand NewItemCommand { get; }

		/// <summary>
		///		Comando para borrar elemento
		/// </summary>
		public BaseCommand DeleteItemCommand { get; }
	}
}
