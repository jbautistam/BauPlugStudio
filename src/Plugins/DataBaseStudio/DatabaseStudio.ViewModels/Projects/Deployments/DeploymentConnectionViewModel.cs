using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.DatabaseStudio.Models;
using Bau.Libraries.DatabaseStudio.Models.Connections;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Projects.Deployments
{
	/// <summary>
	///		ViewModel de una conexión asociada a un elemento de distribución
	/// </summary>
	public class DeploymentConnectionViewModel : ControlItemViewModel
	{
		// Variables privadas
		private string _key;
		private Connections.ConnectionComboViewModel _comboConnections;

		public DeploymentConnectionViewModel(ProjectModel project, string key, DatabaseConnectionModel connection) : base(key, null)
		{
			Key = key;
			ComboConnections = new Connections.ConnectionComboViewModel(project);
			ComboConnections.LoadConnections<DatabaseConnectionModel>();
			ComboConnections.SelectConnection(connection);
		}

		/// <summary>
		///		Clave de la conexión para el <see cref="DeploymentModel"/>
		/// </summary>
		public string Key
		{
			get { return _key; }
			set { CheckProperty(ref _key, value); }
		}

		/// <summary>
		///		Combo de conexiones
		/// </summary>
		public Connections.ConnectionComboViewModel ComboConnections
		{
			get { return _comboConnections; }
			set { CheckObject(ref _comboConnections, value); }
		}
	}
}
