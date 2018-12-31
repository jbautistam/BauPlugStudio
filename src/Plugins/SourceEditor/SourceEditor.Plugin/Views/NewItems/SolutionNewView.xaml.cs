using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Bau.Libraries.SourceEditor.Model.Solutions;
using Bau.Libraries.SourceEditor.ViewModel.Solutions.NewItems;

namespace Bau.Libraries.SourceEditor.Plugin.Views.NewItems
{
	/// <summary>
	///		Ventana para creación de un proyecto en SourceEditor
	/// </summary>
	public partial class SolutionNewView : Window
	{
		public SolutionNewView(SolutionModelCollection objColSolutions)
		{ // Inicializa los componentes
				InitializeComponent();
			// Inicializa el ViewModel
				ViewModel = new SolutionNewViewModel(objColSolutions);
				DataContext = ViewModel;
				ViewModel.RequestClose += (objSender, objResult) => 
																					{ DialogResult = objResult.DialogResult;
																						Close();
																					};
		}

		/// <summary>
		///		ViewModel de la ventana
		/// </summary>
		public SolutionNewViewModel ViewModel { get; private set; }
	}
}
