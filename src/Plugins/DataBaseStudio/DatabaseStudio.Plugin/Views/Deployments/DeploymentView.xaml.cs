using System;
using System.Windows.Controls;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Deployments;

namespace Bau.Libraries.FullDatabaseStudio.Plugin.Views.Deployments
{
	/// <summary>
	///		Vista del archivo de definición de una consulta
	/// </summary>
	public partial class DeploymentView : UserControl, IFormView
	{
		public DeploymentView(DeploymentViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa la vista de datos
			grdData.DataContext = ViewModel = viewModel;
			FormView = new BaseFormView(ViewModel);
		}

		/// <summary>
		///		ViewModel
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel
		/// </summary>
		public DeploymentViewModel ViewModel { get; }
	}
}
