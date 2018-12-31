using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.BauMessenger.ViewModel.Explorer;
using Bau.Libraries.BauMessenger.ViewModel.Explorer.JabberNodes;
using Bau.Libraries.BauMvvm.Views.Forms;

namespace Bau.Plugins.BauMessenger.Views.Explorer
{
	/// <summary>
	///		Arbol de conexiones de Jabber
	/// </summary>
	public partial class TreeConnectionsView : UserControl, IFormView
	{ 
		public TreeConnectionsView()
		{	
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el formulario
			trvConnections.DataContext = ViewModel = new PaneTreeConnectionViewModel();
			trvConnections.ItemsSource = ViewModel.Tree.Children;
			FormView = new BaseFormView(ViewModel);
		}

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel
		/// </summary>
		public PaneTreeConnectionViewModel ViewModel { get; }

		private void trvConnections_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (trvConnections.DataContext is PaneTreeConnectionViewModel && (sender as TreeView).SelectedItem is BaseNodeViewModel node)
				(trvConnections.DataContext as PaneTreeConnectionViewModel).Tree.SelectedNode = node;
		}

		private void trvConnections_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ViewModel.StartChatCommand.Execute(null);
		}

		private void trvConnections_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModel.Tree.SelectedNode = null;
		}
	}
}
