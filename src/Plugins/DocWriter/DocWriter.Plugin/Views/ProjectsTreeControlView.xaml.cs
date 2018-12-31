using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.LibDocWriter.ViewModel.Solutions.TreeExplorer;
using Bau.Libraries.BauMvvm.Views.Forms.Trees;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;

namespace Bau.Plugins.DocWriter.Views
{
	/// <summary>
	///		Ventana que muestra el árbol de proyectos de DocWriter
	/// </summary>
	public partial class ProjectsTreeControlView : UserControl, IFormView
	{ 
		// Variables privadas
		private Point _startDrag;
		private DragDropTreeExplorerController _dragDropController = new DragDropTreeExplorerController();

		public ProjectsTreeControlView()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el formulario
			trvExplorer.DataContext = ViewModel = new TreeExplorerViewModel();
			trvExplorer.ItemsSource = ViewModel.Nodes;
			FormView = new BaseFormView(ViewModel);
		}

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public TreeExplorerViewModel ViewModel { get; }

		private void trvExplorer_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (trvExplorer.DataContext is TreeExplorerViewModel && (sender as TreeView).SelectedItem is Libraries.MVVM.ViewModels.TreeItems.ITreeViewItemViewModel)
				(trvExplorer.DataContext as TreeExplorerViewModel).SelectedItem = (Libraries.MVVM.ViewModels.TreeItems.ITreeViewItemViewModel) (sender as TreeView).SelectedItem;
		}

		private void trvExplorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ViewModel.PropertiesCommand.Execute(null);
		}

		private void trvExplorer_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModel.SelectedItem = null;
		}

		private void trvExplorer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_startDrag = e.GetPosition(null);
		}

		private void trvExplorer_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				Point pntMouse = e.GetPosition(null);
				Vector vctDifference = _startDrag - pntMouse;

					if (pntMouse.X < trvExplorer.ActualWidth - 50 &&
							(Math.Abs(vctDifference.X) > SystemParameters.MinimumHorizontalDragDistance ||
							 Math.Abs(vctDifference.Y) > SystemParameters.MinimumVerticalDragDistance))
						_dragDropController.InitDragOperation(trvExplorer, trvExplorer.SelectedItem as IHierarchicalViewModel);
			}
		}

		private void trvExplorer_DragEnter(object sender, DragEventArgs e)
		{
			_dragDropController.TreatDragEnter(e);
		}

		private void trvExplorer_Drop(object sender, DragEventArgs e)
		{
			BaseNodeViewModel nodeSource = _dragDropController.GetDragDropFileNode(e.Data) as BaseNodeViewModel;

			if (nodeSource != null)
			{
				TreeViewItem trvNode = new Libraries.BauMvvm.Views.Tools.ToolsWpf().FindAncestor<TreeViewItem>((DependencyObject) e.OriginalSource);

					if (trvNode != null)
					{
						BaseNodeViewModel nodeTarget = trvNode.Header as BaseNodeViewModel;

							if (nodeSource != null && nodeTarget != null)
								ViewModel.Copy(nodeSource, nodeTarget,
														(e.KeyStates & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey);
					}
			}
		}
	}
}