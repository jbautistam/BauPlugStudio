using System;
using System.Windows;

using Bau.Libraries.DevConferences.Admon.ViewModel.Projects;

namespace Bau.Plugins.DevConferences.Admon.Views.Projects
{
	/// <summary>
	///		Vista para <see cref="TrackViewModel"/>
	/// </summary>
	public partial class ctlTrackView : Window
	{
		public ctlTrackView(TrackViewModel viewModel)
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
		public TrackViewModel ViewModel { get; }
	}
}
