using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.LibBlogReader.ViewModel.Blogs.TreeFolders;
using Bau.Libraries.LibBlogReader.Model;
using Bau.Libraries.LibBlogReader.ViewModel.Blogs.TreeBlogs;

namespace Bau.Plugins.BlogReader.Views.UC
{
	/// <summary>
	///		Control para el árbol de carpetas
	/// </summary>
	public partial class ctlSearchFolder : UserControl
	{ 
		// Propiedades de dependencias
		public static readonly DependencyProperty FolderProperty = DependencyProperty.Register(nameof(Folder), typeof(FolderModel), typeof(ctlSearchFolder),
																							   new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
																															 new PropertyChangedCallback(Folder_PropertyChanged)));

		public ctlSearchFolder()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Abre el árbol de documentos
		/// </summary>
		private void OpenTreeDocuments()
		{ 
			TreeFoldersViewModel tree = new TreeFoldersViewModel(null);

				// Carga los nodos
				tree.LoadNodes();
				// Asigna el dataContext
				wndPopUp.DataContext = tree;
				// Abre la ventana
				wndPopUp.IsOpen = true;
		}

		/// <summary>
		///		Selecciona la carpeta
		/// </summary>
		private void SelectFolder(FolderNodeViewModel node)
		{ 
			// Asigna la carpeta
			Folder = node?.Folder;
			// Cierra la ventana
			wndPopUp.IsOpen = false;
		}

		/// <summary>
		///		Carpeta seleccionada
		/// </summary>
		public FolderModel Folder
		{
			get { return (FolderModel) GetValue(FolderProperty); }
			set { SetValue(FolderProperty, value); }
		}

		private static void Folder_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (obj is ctlSearchFolder searchFolder)
			{
				FolderModel folder = null;

					// Obtiene el valor del evento (evitando los nulos)
					if (e.NewValue != null)
						folder = e.NewValue as FolderModel;
					// Asigna el texto
					if (folder != null)
						searchFolder.txtPage.Text = folder.FullName;
					else
						searchFolder.txtPage.Text = "";
			}
		}

		private void cmdSearchPage_Click(object sender, RoutedEventArgs e)
		{
			OpenTreeDocuments();
		}

		private void cmdRemovePage_Click(object sender, RoutedEventArgs e)
		{
			Folder = null;
		}

		private void trvPages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			SelectFolder(trvPages.SelectedItem as FolderNodeViewModel);
		}
	}
}