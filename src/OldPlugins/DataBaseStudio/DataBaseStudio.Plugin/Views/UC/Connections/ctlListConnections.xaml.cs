using System;
using System.Windows.Controls;

namespace Bau.Plugins.DataBaseStudio.Views.UC.Connections
{
	/// <summary>
	///		Control para mostrar y seleccionar una lista de conexiones
	/// </summary>
	public partial class ctlListConnections : UserControl
	{
		public ctlListConnections()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Inicializa el contexto del control
		/// </summary>
		public void InitControl(Libraries.LibDataBaseStudio.ViewModel.Connections.ConnectionListViewModel dataContext)
		{
			DataContext = dataContext;
		}
	}
}
