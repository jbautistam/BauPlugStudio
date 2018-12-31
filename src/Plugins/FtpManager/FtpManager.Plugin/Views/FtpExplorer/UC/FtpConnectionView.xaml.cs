using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.FtpManager.ViewModel.FtpExplorer.FtpConnectionItems;

namespace Bau.Plugins.FtpManager.Views.FtpExplorer.UC
{
	/// <summary>
	///		Control que muestra un árbol y una lista de archivos de una conexión FTP
	/// </summary>
	public partial class FtpConnectionView : UserControl
	{ 
		// Variables privadas
		private FtpTreeExplorerViewModel _viewModel;

		public FtpConnectionView()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Actualiza el árbol
		/// </summary>
		public void Refresh()
		{
			ViewModelData?.LoadNodes();
		}

		/// <summary>
		///		ViewModel
		/// </summary>
		public FtpTreeExplorerViewModel ViewModelData
		{
			get { return _viewModel; }
			set
			{
				if (!ReferenceEquals(_viewModel, value))
				{
					_viewModel = value;
					grdData.DataContext = value;
					trvExplorer.DataContext = value;
					lswFiles.DataContext = value;
				}
			}
		}

		private void trvExplorer_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (trvExplorer.DataContext is FtpTreeExplorerViewModel && (sender as TreeView).SelectedItem is FtpFileNodeViewModel node)
				(trvExplorer.DataContext as FtpTreeExplorerViewModel).SelectedNode = node;
		}

		private void trvExplorer_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModelData.SelectedNode = null;
		}
	}
}
