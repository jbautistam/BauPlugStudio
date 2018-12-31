using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.SourceEditor.Model.Solutions;
using Bau.Libraries.SourceEditor.ViewModel.Solutions.TreeExplorer;
using Bau.Libraries.BauMvvm.Views.Forms.Trees;

namespace Bau.Libraries.SourceEditor.Plugin.Views.UC
{
	/// <summary>
	///		Control para la búsqueda de un archivo
	/// </summary>
	public partial class ctlSearchPage : UserControl
	{ 
		// Propiedades de dependencias
		public static readonly DependencyProperty PageProperty = DependencyProperty.Register("FileSelected", typeof(FileModel), typeof(ctlSearchPage),
																							 new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
																							 new PropertyChangedCallback(Page_PropertyChanged)));
		// Controlador de eventos de Drag & drop desde el explorador de archivos
		private DragDropTreeExplorerController dragDropController = new DragDropTreeExplorerController();

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
			wndPopUp.DataContext = new ViewModel.Documents.TreeDocumentsViewModel(File, new FileModelCollection());
			// Abre la ventana
			wndPopUp.IsOpen = true;
		}

		/// <summary>
		///		Selecciona la página
		/// </summary>
		private void SelectPage(MVVM.ViewModels.TreeItems.TreeViewItemViewModel trnView)
		{ 
			// Asigna la página
			if (trnView != null && trnView.Tag != null && trnView.Tag is FileModel)
			{
				FileModel file = trnView.Tag as FileModel;

					if (file != null)
						FileSelected = file;
			}
			// Cierra la ventana
			wndPopUp.IsOpen = false;
		}

		/// <summary>
		///		Archivo seleccionado
		/// </summary>
		public FileModel FileSelected
		{
			get { return (FileModel) GetValue(PageProperty); }
			set { SetValue(PageProperty, value); }
		}

		/// <summary>
		///		Archivo al que se asocia el árbol de páginas
		/// </summary>
		public FileModel File { get; set; }

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
			FileSelected = null;
		}

		private void txtPage_TextChanged(object sender, TextChangedEventArgs e)
		{
			SourceEditorPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowMessage("¿Para qué es esto?");
			// FileSelected = File.Name;
		}

		private void trvPages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			SelectPage(trvPages.SelectedItem as Libraries.MVVM.ViewModels.TreeItems.TreeViewItemViewModel);
		}

		private void txtPage_DragEnter(object sender, DragEventArgs e)
		{
			dragDropController.TreatDragEnter(e);
		}

		private void txtPage_Drop(object sender, DragEventArgs e)
		{
			FileNodeViewModel fileNode = dragDropController.GetDragDropFileNode(e.Data) as FileNodeViewModel;

				// Asigna el vínculo
				if (fileNode != null)
					txtPage.Text = fileNode.File.GetRelativeFileNameToProject(); // , fileNode.File.Title);
		}
	}
}