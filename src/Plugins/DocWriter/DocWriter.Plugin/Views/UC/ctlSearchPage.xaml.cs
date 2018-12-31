using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.ViewModel.Solutions.TreeExplorer;
using Bau.Libraries.BauMvvm.Views.Forms.Trees;

namespace Bau.Plugins.DocWriter.Views.UC
{
	/// <summary>
	///		Control para la búsqueda de una página
	/// </summary>
	public partial class ctlSearchPage : UserControl
	{ 
		// Propiedades de dependencias
		public static readonly DependencyProperty PageProperty = DependencyProperty.Register(nameof(Page), typeof(string), typeof(ctlSearchPage),
																							 new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
																														   new PropertyChangedCallback(Page_PropertyChanged)));
		// Controlador de eventos de Drag & drop desde el explorador de archivos
		private DragDropTreeExplorerController _dragDropController = new DragDropTreeExplorerController();

		public ctlSearchPage()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Abre el árbol de documentos
		/// </summary>
		private void OpenTreeDocuments()
		{ 
			// Asigna el dataContext
			wndPopUp.DataContext = new Libraries.LibDocWriter.ViewModel.Documents.TreeDocumentsViewModel(null, FileType, Project.File,
																										 new FilesModelCollection(Project));
			// Abre la ventana
			wndPopUp.IsOpen = true;
		}

		/// <summary>
		///		Selecciona la página
		/// </summary>
		private void SelectPage(Libraries.MVVM.ViewModels.TreeItems.TreeViewItemViewModel trnView)
		{ 
			// Asigna la página
			if (trnView != null && trnView.Tag != null && trnView.Tag is FileModel)
			{
				FileModel file = trnView.Tag as FileModel;

					if (file != null)
						if (file.FileType == FileModel.DocumentType.Reference && file.IDFileName.Length > 4)
							Page = file.IDFileName.Substring(0, file.IDFileName.Length - 4); // ... le quita la extensión al nombre de archivo
						else
							Page = file.IDFileName;
			}
			// Cierra la ventana
			wndPopUp.IsOpen = false;
		}

		/// <summary>
		///		Página
		/// </summary>
		public string Page
		{
			get { return (string) GetValue(PageProperty); }
			set { SetValue(PageProperty, value); }
		}

		/// <summary>
		///		Proyecto al que se asocia el árbol de páginas
		/// </summary>
		public ProjectModel Project { get; set; }

		/// <summary>
		///		Tipo de archivo a mostrar en el árbol
		/// </summary>
		public FileModel.DocumentType FileType { get; set; }

		private static void Page_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (obj is ctlSearchPage)
			{
				string value = "";

					// Obtiene el valor del evento (evitando los nulos)
					if (e.NewValue != null)
						value = e.NewValue.ToString();
					// Asigna el texto
					(obj as ctlSearchPage).txtPage.Text = value;
			}
		}

		private void cmdSearchPage_Click(object sender, RoutedEventArgs e)
		{
			OpenTreeDocuments();
		}

		private void cmdRemovePage_Click(object sender, RoutedEventArgs e)
		{
			Page = null;
		}

		private void txtPage_TextChanged(object sender, TextChangedEventArgs e)
		{
			Page = txtPage.Text;
		}

		private void trvPages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			SelectPage(trvPages.SelectedItem as Libraries.MVVM.ViewModels.TreeItems.TreeViewItemViewModel);
		}

		private void txtPage_DragEnter(object sender, DragEventArgs e)
		{
			_dragDropController.TreatDragEnter(e);
		}

		private void txtPage_Drop(object sender, DragEventArgs e)
		{
			FileNodeViewModel fileNode = _dragDropController.GetDragDropFileNode(e.Data) as FileNodeViewModel;

				// Asigna el vínculo
				if (fileNode != null)
					txtPage.Text = fileNode.File.IDFileName;
		}
	}
}