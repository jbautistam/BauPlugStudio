using System;
using System.Windows;

using Bau.Libraries.DevConferences.Admon.ViewModel.Projects;

namespace Bau.Plugins.DevConferences.Admon.Views.Projects
{
	/// <summary>
	///		Vista para <see cref="EntryViewModel"/>
	/// </summary>
	public partial class ctlEntryView : Window
	{
		public ctlEntryView(EntryViewModel viewModel)
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
		public EntryViewModel ViewModel { get; }
	}
}
