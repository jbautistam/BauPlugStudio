using System;
using System.Windows;

using Bau.Libraries.LibDocWriter.ViewModel.Solutions.EditorInstruction;

namespace Bau.Plugins.DocWriter.Views.EditorInstructions
{
	/// <summary>
	///		Ventana para el mantenimiento de una lista de instrucciones de código
	/// </summary>
	public partial class ListInstructionsView : Window
	{
		public ListInstructionsView()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = new ListInstructionsViewModel();
			DataContext = ViewModel;
			ViewModel.Close += (sender, result) =>
											{
												DialogResult = result.IsAccepted;
												Close();
											};
		}

		/// <summary>
		///		ViewModel de la ventana
		/// </summary>
		public ListInstructionsViewModel ViewModel { get; }
	}
}
