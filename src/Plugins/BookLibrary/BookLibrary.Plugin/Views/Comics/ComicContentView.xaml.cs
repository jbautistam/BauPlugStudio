using System;
using System.Windows.Controls;
using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.BookLibrary.ViewModel.Books.Content.Comic;

namespace Bau.Libraries.BookLibrary.Plugin.Views.Comics
{
	/// <summary>
	///		Formulario para mostrar el contenido de un cómic
	/// </summary>
	public partial class ComicContentView : UserControl, IFormView
	{
		public ComicContentView(ComicContentViewModel viewModel)
		{	
			// Inicializa los componentes
			InitializeComponent();
			// Asigna la clase del documento
			DataContext = ViewModel = viewModel;
			FormView = new BaseFormView(ViewModel);
			// Interpreta el libro
			ViewModel.Parse();
		}

		/// <summary>
		///		ViewModel asociado al control
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel asociado al control
		/// </summary>
		public ComicContentViewModel ViewModel { get; }

		private void lstPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if ((sender as ListBox)?.SelectedItem is PageListItemViewModel page && page != null)
				ViewModel.ActualPage = page.PageNumber;
		}
	}
}