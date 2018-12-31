using System;

using Bau.Libraries.LibDataBaseStudio.Model.Connections;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Connections
{
	/// <summary>
	///		ViewModel para los elementos del listView de conexiones de documentación
	/// </summary>
	public class ConnectionListItemViewModel : MVVM.ViewModels.ListItems.BaseListItemViewModel
	{
		public ConnectionListItemViewModel(BauMvvm.ViewModels.BaseObservableObject form, SchemaConnectionModel connection, bool isChecked) : base(form)
		{
			Connection = connection;
			Text = connection.Name;
			Tag = connection;
			IsChecked = isChecked;
		}

		/// <summary>
		///		Conexión
		/// </summary>
		public SchemaConnectionModel Connection { get; }
	}
}
