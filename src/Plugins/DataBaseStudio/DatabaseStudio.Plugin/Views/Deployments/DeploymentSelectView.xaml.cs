using System;
using System.Windows;

using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Deployments;

namespace Bau.Libraries.FullDatabaseStudio.Plugin.Views.Deployments
{
	/// <summary>
	///		Ventana con los datos de conexión a una base de datos
	/// </summary>
	public partial class DeploymentSelectView : Window
	{
		public DeploymentSelectView(DeploymentSelectViewModel viewModel)
		{
			// Inicializa los componentes
			InitializeComponent();
			// Asocia el contexto
			DataContext = ViewModel = viewModel;
			ViewModel.Close += (sender, eventArgs) => 
									{
										DialogResult = eventArgs.IsAccepted; 
										Close();
									};
		}

		/// <summary>
		///		ViewModel
		/// </summary>
		private DeploymentSelectViewModel ViewModel { get; }
	}
}
