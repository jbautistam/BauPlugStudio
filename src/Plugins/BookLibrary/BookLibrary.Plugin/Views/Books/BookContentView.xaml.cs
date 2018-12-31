using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.BookLibrary.ViewModel.Books.Content;

namespace Bau.Libraries.BookLibrary.Plugin.Views.Books
{
	/// <summary>
	///		Control para mostrar el contenido de un libro
	/// </summary>
	public partial class BookContentView : UserControl, IFormView
	{
		public BookContentView(BookContentViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Asigna la clase del documento
			DataContext = ViewModel = viewModel;
			FormView = new BaseFormView(ViewModel);
			// Asigna los manejadores de eventos
			ViewModel.PageShow += (sender, evntArgs) => ShowPage(evntArgs.FileName);
			// Interpreta el libro
			ViewModel.Parse();
			trvPages.DataContext = ViewModel;
			trvPages.ItemsSource = viewModel.TreePages.Children;
		}

		/// <summary>
		///		Muestra la página
		/// </summary>
		private void ShowPage(string fileName)
		{
			wbExplorer.ShowUrl(fileName);
		}

		/// <summary>
		///		ViewModel asociado al control
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel asociado al control
		/// </summary>
		public BookContentViewModel ViewModel { get; }

		private void trvPages_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (trvPages.DataContext is BookContentViewModel content && 
					(sender as TreeView).SelectedItem is Bau.Libraries.BookLibrary.ViewModel.Books.Content.TreePages.PageNodeViewModel node)
				content.TreePages.SelectedNode = node;
		}

		private void trvPages_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModel.TreePages.SelectedNode = null;
		}
	}
}
