using System;
using System.Windows;

using Bau.Libraries.BookLibrary.ViewModel.Books.Content.eBook;

namespace Bau.Libraries.BookLibrary.Plugin.Views.Books
{
	/// <summary>
	///		Formulario para compilación de un libro
	/// </summary>
	public partial class BookCompileView : Window
	{
		public BookCompileView(BookCompileViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el viewModel
			DataContext = viewModel;
			viewModel.Close += (sender, result) =>
											{
												DialogResult = result.IsAccepted;
												Close();
											};
		}

		private void txtEditor_TextChanged(object sender, EventArgs e)
		{
			if (DataContext != null && DataContext is BookCompileViewModel)
				(DataContext as BookCompileViewModel).Content = txtEditor.Text;
		}
	}
}
