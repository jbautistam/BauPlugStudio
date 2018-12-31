using System;
using System.Windows;
using System.Windows.Controls;

using Bau.Libraries.DevConferences.Admon.ViewModel.Projects;

namespace Bau.Plugins.DevConferences.Admon.Views.Projects
{
	/// <summary>
	///		Vista para <see cref="CategoryViewModel"/>
	/// </summary>
	public partial class ctlCategoryView : Window
	{
		public ctlCategoryView(CategoryViewModel viewModel)
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
		public CategoryViewModel ViewModel { get; }
	}
}
