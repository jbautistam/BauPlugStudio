using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.LibBlogReader.ViewModel.Blogs.TreeBlogs;

namespace Bau.Plugins.BlogReader.Views
{
	/// <summary>
	///		Ventana que muestra el árbol de blogs
	/// </summary>
	public partial class BlogTreeControlView : UserControl, IFormView
	{
		public BlogTreeControlView()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el formulario
			trvBlogs.DataContext = ViewModel = new PaneTreeBlogsViewModel();
			trvBlogs.ItemsSource = ViewModel.Tree.Children;
			FormView = new BaseFormView(ViewModel);
			BlogReaderPlugin.MainInstance.HostPluginsController.HostViewModelController.Messenger.Sent += (sender, evntArgs) =>
					{
						if (evntArgs != null &&
								evntArgs.MessageSent is Libraries.LibBlogReader.ViewModel.Controllers.Messengers.MessageChangeStatusNews)
							Dispatcher.Invoke(new Action(() => ViewModel.Refresh()), null);
					};
			// Carga los blogs
			ViewModel.LoadNodes();
		}

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public PaneTreeBlogsViewModel ViewModel { get; }

		private void trvBlogs_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (trvBlogs.DataContext is PaneTreeBlogsViewModel && 
					(sender as TreeView).SelectedItem is Bau.Libraries.LibBlogReader.ViewModel.Blogs.TreeBlogs.BaseNodeViewModel node)
				(trvBlogs.DataContext as PaneTreeBlogsViewModel).Tree.SelectedNode = node;
		}

		private void trvBlogs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ViewModel.SeeNewsCommand.Execute(null);
		}

		private void trvBlogs_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModel.Tree.SelectedNode = null;
		}
	}
}
