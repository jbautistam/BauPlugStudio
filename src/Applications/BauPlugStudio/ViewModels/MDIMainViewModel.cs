using System;

using Bau.Libraries.BauMvvm.Common.Controllers;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms.Interfaces;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Applications.BauPlugStudio.ViewModels
{
	/// <summary>
	///		ViewModel de la ventana principal de un formulario MDI
	/// </summary>
	public class MDIMainViewModel : BasePaneViewModel
	{ 
		// Eventos
		public event EventHandler ActiveDocumentChanged;
		// Variables privadas
		private AvalonLayout.PaneViewModel _activeDocument;
		private readonly AvalonLayout.EmptyViewModel _emptyViewModel = new AvalonLayout.EmptyViewModel();

		public MDIMainViewModel(Xceed.Wpf.AvalonDock.DockingManager dockManager) : base(false)
		{
			// Inicializa las propiedades
			DockingController = new AvalonLayout.LayoutDockingController(dockManager);
			DockingController.ActiveDocumentChanged += (sender, args) => ActiveDocument = args.ActiveDocument;
			// Inicializa los comandos
			ConfigurationCommand = new BaseCommand("Configuración", parameter => OpenWindowConfigurate());
			PathPluginsCommand = new BaseCommand("Plugins", parameter => OpenWindowPathPlugins());
			SaveAllCommand = new BaseCommand("SaveAll", parameter => ExecuteAction(nameof(SaveAllCommand), parameter),
										     parameter => CanExecuteAction(nameof(SaveAllCommand), parameter));
			CloseAllWindowsCommand = new BaseCommand("CloseAll", parameter => ExecuteAction(nameof(CloseAllWindowsCommand), parameter),
													 parameter => CanExecuteAction(nameof(CloseAllWindowsCommand), parameter));
			HelpIndexCommand = new BaseCommand("HelpIndex", parameter => ExecuteAction(nameof(HelpIndexCommand), parameter));
		}

		/// <summary>
		///		Añade un panel a la ventana principal
		/// </summary>
		public void AddPane(string windowID, string title, System.Windows.Controls.UserControl paneControl, 
							SystemControllerEnums.DockPosition position)
		{
			DockingController.ShowPane(windowID, title, paneControl, position);
		}

		/// <summary>
		///		Añade documentos al administrador de paneles
		/// </summary>
		public void AddDocument(string windowID, string title, System.Windows.Controls.UserControl documentControl)
		{
			DockingController.ShowDocument(windowID, title, documentControl);
		}

		/// <summary>
		///		Obtiene el ViewModel activo
		/// </summary>
		private Libraries.BauMvvm.Views.Forms.IFormView GetActiveViewModel()
		{
			return ActiveDocument?.GetFormView();
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			Libraries.BauMvvm.Views.Forms.IFormView viewModel = GetActiveViewModel();

				if (viewModel != null)
					switch (action)
					{
						case nameof(CloseCommand):
								Close(SystemControllerEnums.ResultType.Yes);
							break;
						case nameof(CloseAllWindowsCommand):
								DockingController.CloseAllDocuments();
							break;
						case nameof(NewCommand):
								if (viewModel is IPaneViewModel)
									(viewModel as IPaneViewModel).NewCommand.Execute(parameter);
							break;
						case nameof(SaveAllCommand):
								DockingController.SaveAllDocuments();
							break;
						case nameof(SaveCommand):
								if (viewModel is IFormViewModel)
									(viewModel as IFormViewModel).SaveCommand.Execute(parameter);
							break;
						case nameof(DeleteCommand):
								if (viewModel is IFormViewModel)
									(viewModel as IFormViewModel).DeleteCommand.Execute(parameter);
							break;
						case nameof(PropertiesCommand):
								if (viewModel is IPaneViewModel)
									(viewModel as IPaneViewModel).PropertiesCommand.Execute(parameter);
							break;
						case nameof(RefreshCommand):
								if (viewModel is IFormViewModel)
									(viewModel as IFormViewModel).RefreshCommand.Execute(parameter);
							break;
						case nameof(HelpIndexCommand):
								Globals.HostController.ShowWebBrowser("http://BauPlugStudio.webs-interesantes.es");
							break;
					}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			Libraries.BauMvvm.Views.Forms.IFormView viewModel = GetActiveViewModel();
			bool canExecute = false;

				// Comprueba si se puede ejecutar el comando
				if (viewModel != null)
					switch (action)
					{
						case nameof(CloseCommand):
						case nameof(SaveAllCommand):
						case nameof(CloseAllWindowsCommand):
						case nameof(HelpIndexCommand):
								canExecute = true;
							break;
						case nameof(NewCommand):
								if (viewModel is IPaneViewModel)
									canExecute = (viewModel as IPaneViewModel).NewCommand.CanExecute(parameter);
							break;
						case nameof(SaveCommand):
								if (viewModel is IFormViewModel)
									canExecute = (viewModel as IFormViewModel).SaveCommand.CanExecute(parameter);
							break;
						case nameof(PropertiesCommand):
								if (viewModel is IPaneViewModel)
									canExecute = (viewModel as IPaneViewModel).PropertiesCommand.CanExecute(parameter);
							break;
						case nameof(DeleteCommand):
								if (viewModel is IFormViewModel)
									canExecute = (viewModel as IFormViewModel).DeleteCommand.CanExecute(parameter);
							break;
						case nameof(RefreshCommand):
								if (viewModel is IFormViewModel)
									canExecute = (viewModel as IFormViewModel).RefreshCommand.CanExecute(parameter);
							break;
					}
				// Ejecuta el comando
				return canExecute;
		}

		/// <summary>
		///		Abre el formulario de configuración
		/// </summary>
		private void OpenWindowConfigurate()
		{
			if (Globals.HostController.HostViewsController.ShowDialog(Globals.HostController.MainWindow,
												  new Views.Tools.Configuration.ConfigurationView()) == SystemControllerEnums.ResultType.Yes)
				Globals.HostController.HostViewModelController.Configuration.Save();
		}

		/// <summary>
		///		Abre el formulario de configuración de directorios de plugins
		/// </summary>
		private void OpenWindowPathPlugins()
		{
			Globals.HostController.HostViewsController.ShowDialog(new Views.Tools.Configuration.PluginPathsView());
		}

		/// <summary>
		///		Comando de configuración de los directorios de plugins
		/// </summary>
		public BaseCommand PathPluginsCommand { get; }

		/// <summary>
		///		Comando de configuración
		/// </summary>
		public BaseCommand ConfigurationCommand { get; }

		/// <summary>
		///		Comando de "Guardar todo"
		/// </summary>
		public BaseCommand SaveAllCommand { get; }

		/// <summary>
		///		Comando para cerrar todas las ventanas
		/// </summary>
		public BaseCommand CloseAllWindowsCommand { get; }

		/// <summary>
		///		Comando para mostrar el índice de la ayuda
		/// </summary>
		public BaseCommand HelpIndexCommand { get; }

		/// <summary>
		///		Documento activo
		/// </summary>
		public AvalonLayout.PaneViewModel ActiveDocument
		{
			get { return _activeDocument; }
			set
			{
				if (CheckObject(ref _activeDocument, value))
				{ 
					// Cambia el documento activo
					_activeDocument = value;
					if (_activeDocument == null)
						_activeDocument = _emptyViewModel;
					// Lanza el evento de cambio de documento
					ActiveDocumentChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		///		Controlador de AvalonDock
		/// </summary>
		public AvalonLayout.LayoutDockingController DockingController { get; }
	}
}