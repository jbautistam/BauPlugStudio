using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.BauMvvm.Views.Forms.Trees;
using Bau.Libraries.BookLibrary.ViewModel.Books.TreeBooks;

namespace Bau.Libraries.BookLibrary.Plugin.Views.Books
{
	/// <summary>
	///		Ventana que muestra el árbol de libros
	/// </summary>
	public partial class BookTreeExplorerView : UserControl, IFormView
	{ 
		// Variables privadas
		private DragDropTreeExplorerController _dragDropController = new DragDropTreeExplorerController();

		public BookTreeExplorerView()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el viewModel
			trvBooks.DataContext = ViewModel = new PaneTreeBooksViewModel();
			trvBooks.ItemsSource = ViewModel.TreeBooks.Children;
			FormView = new BaseFormView(ViewModel);
		}

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public PaneTreeBooksViewModel ViewModel { get; }

		private void trvBooks_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (trvBooks.DataContext is PaneTreeBooksViewModel pane && (sender as TreeView).SelectedItem is BaseNodeViewModel node)
				pane.TreeBooks.SelectedNode = node;
		}

		private void trvBooks_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModel.TreeBooks.SelectedNode = null;
		}

		private void trvBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModel.OpenBookCommand.Execute(null);
		}

		private void trvBooks_Drop(object sender, DragEventArgs e)
		{
			try
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
				{
					string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

						if (fileNames != null && fileNames.Length > 0)
						{
							TreeViewItem trvNode = new BauMvvm.Views.Tools.ToolsWpf().FindAncestor<TreeViewItem>((DependencyObject) e.OriginalSource);
							BaseNodeViewModel nodeTarget = null;

								// Si se ha encontrado algún nodo, recoge el objeto ViewModel
								if (trvNode != null)
									nodeTarget = trvNode.Header as BaseNodeViewModel;
								// Crea los nodos de eBook
								ViewModel.CreateEBookNodes(nodeTarget, fileNames,
														   BookLibraryPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowQuestion("¿Desea eliminar los archivos originales?"));
						}
				}
			}
			catch (Exception exception)
			{
				BookLibraryPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowMessage("Excepción al recoger los archivos: " + exception.Message);
			}
		}
	}
}