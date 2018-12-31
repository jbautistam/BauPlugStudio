using System;
using System.Windows;

using Bau.Libraries.DevConferences.Admon.ViewModel.Projects;

namespace Bau.Plugins.DevConferences.Admon.Views.Projects
{
	/// <summary>
	///		Vista para <see cref="TrackManagerViewModel"/>
	/// </summary>
	public partial class ctlTrackManagerView : Window
	{
		public ctlTrackManagerView(TrackManagerViewModel viewModel)
		{
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el contexto de datos
			DataContext = ViewModel = viewModel;
			viewModel.Close += (sender, result) =>
											{
												DialogResult = result.IsAccepted;
												Close();
											};
		}

		/// <summary>
		///		ViewModel
		/// </summary>
		public TrackManagerViewModel ViewModel { get; }
	}
}
