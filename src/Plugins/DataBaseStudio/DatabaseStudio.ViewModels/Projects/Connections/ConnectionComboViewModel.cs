using System;
using System.Collections.ObjectModel;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.DatabaseStudio.Models;
using Bau.Libraries.DatabaseStudio.Models.Connections;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Projects.Connections
{
	/// <summary>
	///		Combo de conexiones a base de datos
	/// </summary>
	public class ConnectionComboViewModel : BaseObservableObject
	{
		// Variables privadas
		private ObservableCollection<ControlItemViewModel> _items;
		private ControlItemViewModel _selectedItem;

		public ConnectionComboViewModel(ProjectModel project)
		{
			Project = project;
			Items = new ObservableCollection<ControlItemViewModel>();
		}

		/// <summary>
		///		Carga la lista de conexiones
		/// </summary>
		public void LoadConnections<TypeData>() where TypeData : AbstractConnectionModel
		{
			// Limpia las conexiones
			Items.Clear();
			// Añade las conexiones
			Items.Add(new ControlItemViewModel("<Seleccione una conexión>", null));
			foreach (AbstractConnectionModel connection in Project.Connections)
				if (connection is TypeData)
					Items.Add(new ControlItemViewModel(connection.Name, connection));
			// Selecciona la primera conexión (la vacía)
			SelectedItem = Items[0];
		}

		/// <summary>
		///		Selecciona una conexión
		/// </summary>
		public void SelectConnection(AbstractConnectionModel connection)
		{
			SelectConnection(connection?.GlobalId);
		}

		/// <summary>
		///		Selecciona una conexión
		/// </summary>
		public void SelectConnection(string id)
		{
			foreach (ControlItemViewModel connection in Items)
				if (connection.Tag == null && string.IsNullOrEmpty(id))
					SelectedItem = connection;
				else if ((connection.Tag as AbstractConnectionModel)?.GlobalId.EqualsIgnoreCase(id) ?? false)
					SelectedItem = connection;
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		private ProjectModel Project { get; }

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
		///		Conexión seleccionada
		/// </summary>
		public AbstractConnectionModel SelectedConnection
		{
			get { return SelectedItem?.Tag as AbstractConnectionModel; }
		}
	}
}
