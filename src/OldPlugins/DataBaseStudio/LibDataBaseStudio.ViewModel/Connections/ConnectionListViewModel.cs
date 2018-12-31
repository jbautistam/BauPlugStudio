using System;
using System.Collections.Generic;

using Bau.Libraries.MVVM.ViewModels.ListItems;
using Bau.Libraries.LibDataBaseStudio.Application.Bussiness;
using Bau.Libraries.LibDataBaseStudio.Model.Base;
using Bau.Libraries.LibDataBaseStudio.Model.Connections;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Connections
{
	/// <summary>
	///		ViewModel para la selección de una lista de conexiones
	/// </summary>
	public class ConnectionListViewModel : ListViewModel<ConnectionListItemViewModel>
	{
		public ConnectionListViewModel(BauMvvm.ViewModels.BaseObservableObject form, string projectPath)
		{
			FormParent = form;
			ProjectPath = projectPath;
		}

		/// <summary>
		///		Carga la lista de conexiones
		/// </summary>
		public void LoadConnections(ConnectionItemBase connectionItem)
		{
			SchemaConnectionBussiness bussines = new SchemaConnectionBussiness();
			SchemaConnectionModelCollection connectionsProject = bussines.LoadByPath(ProjectPath);
			SchemaConnectionModelCollection connectionsSelected = bussines.LoadByConnectionItem(ProjectPath, connectionItem);

				// Inicializa las conexiones
				ListItems.Clear();
				// Carga las conexiones en la lista
				foreach (SchemaConnectionModel connection in connectionsProject)
				{
					bool isChecked = false;

						// Comprueba si está seleccionado
						isChecked = connectionItem?.ExistsConnection(connection) ?? false;
						// Añade el elemento
						Add(new ConnectionListItemViewModel(FormParent, connection, isChecked));
				}
		}

		/// <summary>
		///		Obtiene una lista de los Guids de las conexiones seleccionadas
		/// </summary>
		public List<string> GetGuidConnectionsSelected()
		{
			List<string> guids = new List<string>();

				// Añade los Guid de las conexiones seleccionadas
				foreach (ConnectionListItemViewModel listItem in ListItems)
					if (listItem.IsChecked)
						guids.Add(listItem.Connection.GlobalId);
				// Devuelve la colección de Guid
				return guids;
		}

		/// <summary>
		///		Formulario padre
		/// </summary>
		public BauMvvm.ViewModels.BaseObservableObject FormParent { get; }

		/// <summary>
		///		Directorio del proyecto
		/// </summary>
		public string ProjectPath { get; }
	}
}
