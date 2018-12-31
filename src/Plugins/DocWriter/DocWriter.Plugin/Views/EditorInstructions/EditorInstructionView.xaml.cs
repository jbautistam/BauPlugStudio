using System;
using System.Windows;

using Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction.Model;
using Bau.Libraries.LibDocWriter.ViewModel.Solutions.EditorInstruction;

namespace Bau.Plugins.DocWriter.Views.EditorInstructions
{
	/// <summary>
	///		Ventana para el mantenimiento de una instrucción de código
	/// </summary>
	public partial class EditorInstructionView : Window
	{
		public EditorInstructionView(EditorInstructionModel instruction)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = new EditorInstructionViewModel(instruction);
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
		public EditorInstructionViewModel ViewModel { get; }
	}
}
