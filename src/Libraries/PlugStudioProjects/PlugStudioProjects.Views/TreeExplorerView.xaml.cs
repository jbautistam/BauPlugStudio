using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.BauMvvm.Views.Forms.Trees;

using Bau.Libraries.PlugStudioProjects.Models;
using Bau.Libraries.PlugStudioProjects.ViewModels;

namespace Bau.Libraries.PlugStudioProjects.Views
{
	/// <summary>
	///		Ventana que muestra el árbol de nodos del proyecto
	/// </summary>
	public partial class TreeExplorerView : UserControl, IFormView
	{ 
		// Variables privadas
		private Point _startDrag;
		private DragDropTreeExplorerController _dragDropController = new DragDropTreeExplorerController();

		public TreeExplorerView(ExplorerProjectViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el viewModel
			trvProject.DataContext = ViewModel = viewModel;
			trvProject.ItemsSource = ViewModel.Children;
			FormView = new BaseFormView(ViewModel);
		}

		/// <summary>
		///		Carga los menús
		/// </summary>
		private void LoadMenus(ContextMenu ctxMenu)
		{
			MenuModelCollection menus = ViewModel.GetMenus();

				// Elimina los menús antiguos
				RemoveMenus(ctxMenu.Items);
				// Añade los menús de opciones
				AddMenus(ctxMenu.Items, menus, MenuModel.MenuType.Command);
				AddMenus(SearchMenu(ctxMenu.Items, "mnuItemsNew"), menus, MenuModel.MenuType.NewFile);
				// Muestra / oculta el separador de las opciones de herramientas
				foreach (Control optMenu in ctxMenu.Items)
					if (optMenu is Separator && optMenu.Name.Equals("mnuOptOptions", StringComparison.CurrentCultureIgnoreCase))
					{ 
						// Oculta / muestra el separador
						if (menus == null || menus.Count == 0)
							optMenu.Visibility = Visibility.Collapsed;
						else
							optMenu.Visibility = Visibility.Visible;
					}
		}

		/// <summary>
		///		Busca un menú por su nombre
		/// </summary>
		private ItemCollection SearchMenu(ItemCollection items, string name)
		{
			// Busca el menú en la colección
			foreach (Control item in items)
				if (item is MenuItem menu && !string.IsNullOrEmpty(menu.Name) && menu.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
					return menu.Items;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Añade los menús de un tipo
		/// </summary>
		private void AddMenus(ItemCollection items, MenuModelCollection menus, MenuModel.MenuType type)
		{
			if (items != null && menus != null)
				foreach (MenuModel menu in menus)
					if (menu.Type == type)
					{
						MenuItem optMenu = new MenuItem();

							// Asigna las propiedades
							optMenu.Header = menu.Text;
							optMenu.Icon = new Bau.Libraries.BauMvvm.Views.Tools.ToolsWpf().GetImage(menu.Icon);
							optMenu.Tag = menu;
							// Añade el manejador de eventos
							optMenu.Click += (sender, evntArgs) =>
													{
														if (sender is MenuItem option && option != null && option.Tag is MenuModel menuItem)
															ViewModel.MenuCommand.Execute(menuItem);
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

				return menu != null && menu.Tag is MenuModel;
		}

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public ExplorerProjectViewModel ViewModel { get; }

		private void trvExplorer_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (trvProject.DataContext is ExplorerProjectViewModel && (sender as TreeView).SelectedItem is ExplorerProjectNodeViewModel node)
				ViewModel.SelectedNode = node;
		}

		private void trvExplorer_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModel.SelectedNode = null;
		}

		private void trvExplorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModel.PropertiesCommand.Execute(null);
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

					if (actual.X < trvProject.ActualWidth - 50 &&
							(Math.Abs(difference.X) > SystemParameters.MinimumHorizontalDragDistance ||
							 Math.Abs(difference.Y) > SystemParameters.MinimumVerticalDragDistance))
						_dragDropController.InitDragOperation(trvProject, trvProject.SelectedItem as IHierarchicalViewModel);
			}
		}

		private void trvExplorer_DragEnter(object sender, DragEventArgs e)
		{
			_dragDropController.TreatDragEnter(e);
		}

		private void trvExplorer_Drop(object sender, DragEventArgs e)
		{
			try
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
				{
					string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

						if (fileNames != null && fileNames.Length > 0)
						{
							TreeViewItem trvNode = new BauMvvm.Views.Tools.ToolsWpf().FindAncestor<TreeViewItem>((DependencyObject) e.OriginalSource);
							IHierarchicalViewModel nodeTarget = null;

								// Si se ha encontrado algún nodo, recoge el objeto ViewModel
								if (trvNode != null)
									nodeTarget = trvNode.Header as IHierarchicalViewModel;
						}
				}
			}
			catch (Exception exception)
			{
				ViewModel.PlugStudioController.ControllerWindow.ShowMessage("Excepción al recoger los archivos: " + exception.Message);
			}
		}

		private void trvExplorer_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			LoadMenus(trvProject.ContextMenu);
		}
	}
}