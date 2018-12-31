using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.DevConferences.Admon.ViewModel.Explorer;

namespace Bau.Plugins.DevConferences.Admon.Views.Explorer
{
	/// <summary>
	///		Arbol de canales
	/// </summary>
	public partial class TreeTracksView : UserControl, IFormView
	{ 
		public TreeTracksView()
		{	
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el formulario
			trvExplorer.DataContext = ViewModel = new TreeTracksViewModel();
			ViewModel.Load();
			trvExplorer.ItemsSource = ViewModel.Children;
			FormView = new BaseFormView(ViewModel);
		}

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public TreeTracksViewModel ViewModel { get; }

		private void trvExplorer_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (trvExplorer.DataContext is TreeTracksViewModel && (sender as TreeView).SelectedItem is TreeExplorerNodeViewModel)
				(trvExplorer.DataContext as TreeTracksViewModel).SelectedNode = (TreeExplorerNodeViewModel) (sender as TreeView).SelectedItem;
		}

		private void trvExplorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ViewModel.OpenCommand.Execute(null);
		}

		private void trvExplorer_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModel.SelectedNode = null;
		}
	}
}
