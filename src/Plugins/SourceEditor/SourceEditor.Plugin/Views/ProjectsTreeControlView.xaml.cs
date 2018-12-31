using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.SourceEditor.ViewModel.Solutions.TreeExplorer;
using Bau.Libraries.BauMvvm.Views.Forms.Trees;
using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;

namespace Bau.Libraries.SourceEditor.Plugin.Views
{
	/// <summary>
	///		Ventana que muestra el árbol de proyectos de SourceEditor
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
		///		Carga los menús
		/// </summary>
		private void LoadMenus(ContextMenu ctxMenu)
		{
			Model.Definitions.MenuModelCollection menus = ViewModel.GetMenus();

				// Elimina los menús antiguos
				RemoveMenus(ctxMenu.Items);
				// Añade los menús de opciones
				AddMenus(ctxMenu.Items, menus, Model.Definitions.MenuModel.MenuType.Command);
				// Muestra / oculta el separador de las opciones de herramientas
				foreach (Control optMenu in ctxMenu.Items)
					if (optMenu is Separator && optMenu.Name.EqualsIgnoreCase("mnuOptOptions"))
					{ 
						// Oculta / muestra el separador
						if (menus.Count == 0)
							optMenu.Visibility = Visibility.Collapsed;
						else
							optMenu.Visibility = Visibility.Visible;
					}
		}

		/// <summary>
		///		Añade los menús de un tipo
		/// </summary>
		private void AddMenus(ItemCollection items, Model.Definitions.MenuModelCollection menus, Model.Definitions.MenuModel.MenuType type)
		{
			foreach (Model.Definitions.MenuModel menu in menus)
				if (menu.Type == type)
				{
					MenuItem optMenu = new MenuItem();

						// Asigna las propiedades
						optMenu.Header = menu.Name;
						optMenu.Icon = new BauMvvm.Views.Tools.ToolsWpf().GetImage(menu.Icon);
						optMenu.Tag = menu;
						// Añade el manejador de eventos
						optMenu.Click += (sender, evntArgs) =>
												{
													if (sender is MenuItem)
													{
														MenuItem option = sender as MenuItem;

															if (option != null && option.Tag is Model.Definitions.MenuModel)
																ViewModel.FileOptionsCommand.Execute(option.Tag as Model.Definitions.MenuModel);
													}
												};
						// Añade el menú
						items.Add(optMenu);
				}
		}

		/// <summary>
		///		Elimina los menús anteriores
		/// </summary>
		private void RemoveMenus(ItemCollection items)
		{
			for (int index = items.Count - 1; index >= 0; index--)
				if (MustRemoveMenu(items[index]))
					items.RemoveAt(index);
				else if (items[index] is MenuItem)
				{
					MenuItem mnuItem = items[index] as MenuItem;

						if (mnuItem.Items != null && mnuItem.Items.Count > 0)
							RemoveMenus(mnuItem.Items);
				}
		}

		/// <summary>
		///		Comprueba si se debe eliminar un menú anterior
		/// </summary>
		private bool MustRemoveMenu(object item)
		{
			MenuItem menu = item as MenuItem;

				return menu != null && menu.Tag is Model.Definitions.MenuModel;
		}

		/// <summary>
		///		Copia los archivos seleccionados desde el navegador en el nodo apropiado del árbol
		/// </summary>
		private void DropExternalFiles(string[] arrfileNames, DependencyObject eventTarget)
		{
			try
			{
				if (arrfileNames != null && arrfileNames.Length > 0)
				{
					BaseNodeViewModel nodeTarget = GetTreeNodeFromEventTarget(eventTarget);

						// Si se ha encontrado algún nodo, copia los archivos en el nodo
						if (nodeTarget != null)
							ViewModel.Copy(nodeTarget, arrfileNames);
				}
			}
			catch (Exception exception)
			{
				SourceEditorPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowMessage($"Excepción al copiar los archivos: {exception.Message}");
			}
		}

		/// <summary>
		///		Corta / copia un nodo sobre otro
		/// </summary>
		private void DropNode(BaseNodeViewModel nodeSource, DependencyObject eventTarget, bool copy)
		{
			if (nodeSource != null)
			{
				BaseNodeViewModel nodeTarget = GetTreeNodeFromEventTarget(eventTarget);

					if (nodeTarget != null)
						ViewModel.Copy(nodeSource, nodeTarget, copy);
			}
		}

		/// <summary>
		///		Obtiene el nodo destino en el árbol a partir de los datos de un evento
		/// </summary>
		private BaseNodeViewModel GetTreeNodeFromEventTarget(DependencyObject eventTarget)
		{
			TreeViewItem trvNode = new BauMvvm.Views.Tools.ToolsWpf().FindAncestor<TreeViewItem>((DependencyObject) eventTarget);

				if (trvNode != null)
					return trvNode.Header as BaseNodeViewModel;
				else
					return null;
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
			if (trvExplorer.DataContext is TreeExplorerViewModel && (sender as TreeView).SelectedItem is MVVM.ViewModels.TreeItems.ITreeViewItemViewModel)
				(trvExplorer.DataContext as TreeExplorerViewModel).SelectedItem = (MVVM.ViewModels.TreeItems.ITreeViewItemViewModel) (sender as TreeView).SelectedItem;
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
				Point actual = e.GetPosition(null);
				Vector difference = _startDrag - actual;

					if (actual.X < trvExplorer.ActualWidth - 50 &&
							(Math.Abs(difference.X) > SystemParameters.MinimumHorizontalDragDistance ||
							 Math.Abs(difference.Y) > SystemParameters.MinimumVerticalDragDistance))
						_dragDropController.InitDragOperation(trvExplorer, trvExplorer.SelectedItem as IHierarchicalViewModel);
			}
		}

		private void trvExplorer_DragEnter(object sender, DragEventArgs e)
		{
			_dragDropController.TreatDragEnter(e);
		}

		private void trvExplorer_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
				DropExternalFiles(e.Data.GetData(DataFormats.FileDrop, true) as string[],
													(DependencyObject) e.OriginalSource);
			else
				DropNode(_dragDropController.GetDragDropFileNode(e.Data) as BaseNodeViewModel,
								 (DependencyObject) e.OriginalSource, (e.KeyStates & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey);
		}

		private void trvExplorer_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			LoadMenus(trvExplorer.ContextMenu);
		}
	}
}