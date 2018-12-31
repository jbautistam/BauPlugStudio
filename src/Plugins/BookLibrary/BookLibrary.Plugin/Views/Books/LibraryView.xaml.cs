using System;
using System.Windows;

using Bau.Libraries.BookLibrary.ViewModel.Books;

namespace Bau.Libraries.BookLibrary.Plugin.Views.Books
{
	/// <summary>
	///		Formulario para mantenimiento de un <see cref="LibraryModel"/>
	/// </summary>
	public partial class LibraryView : Window
	{
		public LibraryView(LibraryViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			DataContext = viewModel;
			viewModel.Close += (sender, result) =>
												{
													DialogResult = result.IsAccepted;
													Close();
												};
		}
	}
}