using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers;
using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers.Common;
using Bau.Libraries.Plugins.ViewModels.Controllers.Processes.EventArguments;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.Menus;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.Views.Host;

namespace Bau.Applications.BauPlugStudio
{
	/// <summary>
	///		Ventana principal de la aplicación
	/// </summary>
	public partial class MainWindow : Window
	{ 
		// Constantes privadas
		private const string StatusDefault = "Preparado";
		// Variables privadas
		private Controls.Notifications.WindowNotifications _wndNotifications = null;
		private Classess.MRU.MRUFileModelCollection _recentFilesUsed = null;

		public MainWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{ 
			// Inicializa los datos globales
			Globals.Initialize(this, dckManager);
			// Asigna el controlador de mensajes
			Globals.HostController.HostViewModelController.Messenger.Sent += (sender, evntArgs) =>
															{
																if (evntArgs != null)
																	TreatMessage(evntArgs.MessageSent);
																else
																	System.Diagnostics.Debug.WriteLine("Esto no debería pasar");
															};
			// Asigna el tratamiento de los mensajes de progreso de la cola de tareas
			Globals.HostController.HostViewModelController.TasksProcessor.Progress += (sender, evntArgs) => TreatTasksProgress(evntArgs);
			Globals.HostController.HostViewModelController.TasksProcessor.EndProcess += (sender, evntArgs) => TreatTaskEndProcess(evntArgs);
			Globals.HostController.HostViewModelController.TasksProcessor.ProgressAction += (sender, evntArgs) => TreatTaskProgressAction(evntArgs);
			Globals.HostController.HostViewModelController.TasksProcessor.ActionProcess += (sender, evntArgs) => TreatActionProcess(evntArgs);
			// Inicializa el registro del WebBrowser
			InitWebBrowser();
			// Inicializa los paneles
			InitDockPanels();
			// Inicializa los plugins
			InitPlugins();
			// Recupera el aspecto de las ventanas
			RestoreLayout();
			// Inicializa el planificador de procesos
			Globals.HostController.HostViewModelController.Scheduler.Start();
			// Inicializa el controlador
			DataContext = ViewModel = Globals.HostController.LayoutController;
			ViewModel.ActiveDocumentChanged += (sender, evntArgs) => TreatActiveDocumentChanged(evntArgs.View);
			// Cambia el título de la ventana y el tema
			Title = Globals.ApplicationName;
			SetDockTheme(null);
			// Muestra los últimos archivos abiertos
			ShowMenuMRUFiles();
		}

		/// <summary>
		///		Inicializa las claves de registro del navegador Web
		/// </summary>
		private void InitWebBrowser()
		{
//#if !DEBUG
//				deberías mirar esto
//#endif
			//const string cnstStrRoot = @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\";
			//string applicationName = AppDomain.CurrentDomain.FriendlyName;
			//Libraries.LibSystem.API.WindowsRegistry objRegistry = new Libraries.LibSystem.API.WindowsRegistry();

			//	try
			//		{ // Asigna el browser determinado
			//				objRegistry.SetValue(Libraries.LibSystem.API.WindowsRegistry.RegistryRoot.CurrentUser,
			//														 cnstStrRoot + "FEATURE_BROWSER_EMULATION",
			//														 applicationName, 8000, Microsoft.Win32.RegistryValueKind.DWord);
			//			// Indica que no se traten las referencias circulares de JavaScript
			//				objRegistry.SetValue(Libraries.LibCommonHelper.API.RegistryApi.RegistryRoot.CurrentUser,
			//														 cnstStrRoot + "FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS",
			//														 applicationName, 0, Microsoft.Win32.RegistryValueKind.DWord);
			//			// Habilita la copia de Url
			//				objRegistry.SetValue(Libraries.LibCommonHelper.API.RegistryApi.RegistryRoot.CurrentUser,
			//														 cnstStrRoot + "FEATURE_ENABLE_SCRIPT_PASTE_URLACTION_IF_PROMPT",
			//														 applicationName, 1, Microsoft.Win32.RegistryValueKind.DWord);
			//		}
			//	catch (Exception exception)
			//		{ System.Diagnostics.Debug.WriteLine("Error al crear la clave de registro: " + exception.Message);
			//		}
		}

		/// <summary>
		///		Inicializa los paneles colocados a los lados
		/// </summary>
		private void InitDockPanels()
		{
			Globals.HostController.ShowLogPane("LOG_WINDOW", LayoutEnums.DockPosition.Bottomm);
			Globals.HostController.ShowErrorPane("Error_WINDOW", LayoutEnums.DockPosition.Bottomm);
			Globals.HostController.ShowProcessPane("PROCESS_WINDOW", LayoutEnums.DockPosition.Bottomm);
		}

		/// <summary>
		///		Inicializa los plugins
		/// </summary>
		private void InitPlugins()
		{
			Controllers.Plugins.PluginPathModelCollection pluginsPaths = new Controllers.Plugins.PluginPathModelCollection();
			string error = "", errorTotal = "";

				// Carga los directorios de plugins
				pluginsPaths.Load();
				// Inicializa el manejador de plugins
				Globals.PluginsManager.Initialize(typeof(Globals), pluginsPaths.ConvertToList());
				// Inicializa los plugins
				if (Globals.PluginsManager.Plugins != null)
				{ 
					Globals.PluginsManager.InitPlugins(Globals.HostController, out error);
					if (!error.IsEmpty())
						errorTotal = error;
					// Muestra los paneles
					Globals.PluginsManager.ShowPanes(out error);
					if (!error.IsEmpty())
						errorTotal = errorTotal.AddWithSeparator(error, Environment.NewLine);
				}
				// Si hay algún error lo notifica
				if (!errorTotal.IsEmpty())
				{
					Globals.HostController.ControllerWindow.ShowNotification(SystemControllerEnums.NotificationType.Error,
																			 "Error en la inicialización de plugins", errorTotal, TimeSpan.FromSeconds(3));
					Globals.HostController.HostViewModelController.Messenger.SendLog("Ventana principal", MessageLog.LogType.Error,
															 "Error en la inicialización de plugins", errorTotal, null);
				}
		}

		/// <summary>
		///		Asigna el tema al layout
		/// </summary>
		private void SetDockTheme(string theme)
		{ 
			// Cambia la configuración
			if (theme.IsEmpty() || theme.EqualsIgnoreCase("Aero"))
				Globals.HostController.LayoutController.SetTheme(LayoutEnums.Theme.Aero);
			else if (theme.EqualsIgnoreCase("Metro"))
				Globals.HostController.LayoutController.SetTheme(LayoutEnums.Theme.Metro);
			else if (theme.EqualsIgnoreCase("VS2010"))
				Globals.HostController.LayoutController.SetTheme(LayoutEnums.Theme.Vs2010);
			// ... y la guarda
			Properties.Settings.Default.DockTheme = theme;
		}

		/// <summary>
		///		Trata un mensaje
		/// </summary>
		private void TreatMessage(Message message)
		{
			if (message != null)
				Dispatcher.Invoke(new Action(() =>
											{
												if (message is MessageBarProgress progress)
													TreatMessageProgress(progress);
												else if (message is MessageRecentFileUsed recentFileUsed)
													TreatMessageRecentFileUsed(recentFileUsed);
												else if (!message.Action.IsEmpty())
													switch (message.Action.Trim().ToUpper())
													{
														case "OPENPAGE":
																Globals.HostController.ShowWebBrowser(message.Content as string);
															break;
													}
											}
								), null);
		}

		/// <summary>
		///		Trata los eventos de fin de proceso de la cola de tareas
		/// </summary>
		private void TreatTaskEndProcess(EndProcessEventArgs evntArgs)
		{
			if (evntArgs != null)
			{
				string message = evntArgs.Message;

					// Añade los errores al mensaje
					if (evntArgs.Errors != null && evntArgs.Errors.Count > 0)
						foreach (string strItem in evntArgs.Errors)
							message = message.AddWithSeparator(strItem, Environment.NewLine);
					// Muestra el mensaje
					Globals.HostController.ControllerWindow.ShowNotification(SystemControllerEnums.NotificationType.Information,
																			 "Fin de proceso", message, TimeSpan.FromSeconds(3));
			}
		}

		/// <summary>
		///		Trata un evento de acción en un proceso
		/// </summary>
		private void TreatActionProcess(ActionEventArgs evntArgs)
		{
			if (evntArgs != null)
				Globals.HostController.HostViewModelController.Messenger.SendLog(evntArgs.Module, MessageLog.LogType.Information,
														 evntArgs.Message, evntArgs.Action.ToString(), null);
		}

		/// <summary>
		///		Trata el mensaje de progreso de una acción
		/// </summary>
		private void TreatTaskProgressAction(ProgressActionEventArgs evntArgs)
		{
			if (evntArgs != null)
				Globals.HostController.HostViewModelController.Messenger.SendProgress
									(evntArgs.ID, evntArgs.Module, evntArgs.Action, evntArgs.Process,
									 evntArgs.Actual, evntArgs.Total, null);
		}

		/// <summary>
		///		Trata las tareas de progreso
		/// </summary>
		private void TreatTasksProgress(ProgressEventArgs evntArgs)
		{
			if (evntArgs != null)
				TreatProgress(evntArgs.Actual, evntArgs.Total, evntArgs.Message);
		}

		/// <summary>
		///		Trata el mensaje de progreso
		/// </summary>
		private void TreatMessageProgress(MessageBarProgress message)
		{
			if (message != null)
				TreatProgress(message.Actual, message.Total, message.Message);
		}

		/// <summary>
		///		Trata información de progreso
		/// </summary>
		private void TreatProgress(long actual, long total, string message)
		{
			Dispatcher.Invoke(new Action(() =>
										{
											if (actual >= total)
											{
												prgProgress.Visibility = Visibility.Hidden;
												lblStatus.Text = StatusDefault;
											}
											else
											{
												prgProgress.Visibility = Visibility.Visible;
												lblStatus.Text = message;
												prgProgress.Maximum = total;
												prgProgress.Value = actual;
											}
										}
							), null);
		}

		/// <summary>
		///		Trata el mensaje de último archivo utilizado
		/// </summary>
		private void TreatMessageRecentFileUsed(MessageRecentFileUsed message)
		{
			if (message?.Type == MessageRecentFileUsed.ActionType.Open)
			{
				// Añade el archivo
				RecentFilesUsed.Add(message.Source, message.FileName, message.Text);
				// Graba el archivo
				new Classess.MRU.MRUFileRepository().Save(RecentFilesUsed);
				// Actualiza el menú
				ShowMenuMRUFiles();
			}
		}

		/// <summary>
		///		Trata el evento de modificación del documento activo
		/// </summary>
		private void TreatActiveDocumentChanged(Libraries.BauMvvm.Views.Forms.IFormView formView)
		{
			bool treated = false;

				// Si hay un documento activo, añade los menús abrir, nuevo y adicional
				if (formView != null && formView.FormView.ViewModel is Libraries.BauMvvm.ViewModels.Forms.Interfaces.IFormViewModel viewModel && 
					viewModel != null)
				{
					// Añade los menús
					AddMenus(mnuFilesOpen, viewModel.MenuCompositionData.Menus.Select(MenuGroupViewModel.TargetMenuType.MainMenu,
																					  MenuGroupViewModel.TargetMainMenuItemType.FileOpenItems));
					AddMenus(mnuFilesNew, viewModel.MenuCompositionData.Menus.Select(MenuGroupViewModel.TargetMenuType.MainMenu,
																					 MenuGroupViewModel.TargetMainMenuItemType.FileNewItems));
					AddMenusBetween(mnuFiles, mnuFileAdditionalStart, mnuFileAdditionalEnd,
									viewModel.MenuCompositionData.Menus.Select(MenuGroupViewModel.TargetMenuType.MainMenu,
																			   MenuGroupViewModel.TargetMainMenuItemType.FileAdditionalItems));
					// Indica que se han tratado los menús
					treated = true;
				}
				// Si no se han tratado los menús, oculta los sobrantes
				if (!treated)
					mnuFileAdditionalEnd.Visibility = Visibility.Collapsed;
		}

		/// <summary>
		///		Añade los menús de un grupo a una opción
		/// </summary>
		private void AddMenus(MenuItem mnuItem, MenuGroupViewModelCollection groups)
		{ 
			// Borra los elementos del menú
			mnuItem.Items.Clear();
			// Añade los menús del grupo
			foreach (MenuGroupViewModel group in groups)
				foreach (MenuItemViewModel menu in group.MenuItems)
					mnuItem.Items.Add(CreateMenu(menu));
			// Cambia los estados del menú
			mnuItem.IsEnabled = mnuItem.Items.Count != 0;
		}

		/// <summary>
		///		Añade opciones de menú entre dos opciones
		/// </summary>
		private void AddMenusBetween(MenuItem mnuParent, Separator mnuStart, Separator mnuEnd, MenuGroupViewModelCollection groups)
		{
			int startIndex = mnuParent.Items.IndexOf(mnuStart);
			int indexEnd = mnuParent.Items.IndexOf(mnuEnd);

				// Borra las opciones de menú que se hubiesen creado anteriormente
				DeleteMenusBetween(mnuParent, startIndex, indexEnd);
				// Añade las opciones de menú
				foreach (MenuGroupViewModel group in groups)
					foreach (MenuItemViewModel menu in group.MenuItems)
					{ 
						// Inserta la opción de menú
						mnuParent.Items.Insert(startIndex + 1, CreateMenu(menu));
						// Incrementa la posición
						startIndex++;
					}
				// Oculta el separador final si no se ha añadido nada
				if (groups.Count == 0)
					mnuEnd.Visibility = Visibility.Collapsed;
				else
					mnuEnd.Visibility = Visibility.Visible;
		}

		/// <summary>
		///		Borra los menús entre dos separadores
		/// </summary>
		private void DeleteMenusBetween(MenuItem mnuParent, int startIndex, int indexEnd)
		{
			for (int index = indexEnd - 1; index > startIndex; index--)
				mnuParent.Items.RemoveAt(index);
		}

		/// <summary>
		///		Crea una opción de menú o un separador
		/// </summary>
		private Control CreateMenu(MenuItemViewModel menu)
		{
			if (menu.IsSeparator)
				return new Separator();
			else
				return CreateMenu(menu.Text, menu.Icon, menu.Command, menu);
		}

		/// <summary>
		///		Crea una opción de menú
		/// </summary>
		private MenuItem CreateMenu(string text, string icon, ICommand command, object tag = null)
		{
			MenuItem mnuNewItem = new MenuItem();

				// Asigna las propiedades
				mnuNewItem.Header = text;
				mnuNewItem.Icon = new Libraries.BauMvvm.Views.Tools.ToolsWpf().GetImage(icon);
				mnuNewItem.Tag = tag;
				// Añade el comando
				mnuNewItem.Command = command;
				// Devuelve la opción de menú creada
				return mnuNewItem;
		}

		/// <summary>
		///		Muestra el menú de archivos abiertos
		/// </summary>
		private void ShowMenuMRUFiles()
		{
			System.Collections.Generic.List<string> applications = RecentFilesUsed.GetApplications();

				// Borra los últimos menús
				mnuFileLastFiles.Items.Clear();
				// Recorre las aplicaciones en la colección mostrándolas en el menú
				foreach (string application in applications)
				{
					MenuItem mnuApplication = CreateMenu(application, null, null, null);

						// Añade los archivos al menú de aplicación
						for (int index = RecentFilesUsed.Count - 1; index >= 0; index--)
							if (RecentFilesUsed[index].Source.EqualsIgnoreCase(application))
							{
								MenuItem mnuMru = CreateMenu($"{RecentFilesUsed.Count - index} {RecentFilesUsed[index].Name}",
															 null, null, RecentFilesUsed[index]);

									// Añade el menú al menú de aplicación
									mnuApplication.Items.Add(mnuMru);
									// Añade el manejador de menús
									mnuMru.Click += (sender, evntArgs) =>
														{
															Classess.MRU.MRUFileModel file = (sender as MenuItem).Tag as Classess.MRU.MRUFileModel;

																if (file != null && !file.FileName.IsEmpty())
																{
																	if (!System.IO.File.Exists(file.FileName))
																		Globals.HostController.ControllerWindow.ShowMessage("No se encuentra el archivo");
																	else
																		Globals.HostController.HostViewModelController.Messenger.SendRecentFileClicked(file.Source, file.FileName);
																}
														};
							}
						// Añade el menú de aplicación a la opción principal
						mnuFileLastFiles.Items.Add(mnuApplication);
				}
				// Desactiva el menú si no hay ningún archivo
				mnuFileLastFiles.IsEnabled = mnuFileLastFiles.Items.Count > 0;
		}

		/// <summary>
		///		Abre el formulario de configuración
		/// </summary>
		private void OpenWindowConfigurate()
		{
			if (Globals.HostController.HostViewsController.ShowDialog(new Views.Tools.Configuration.ConfigurationView()) == SystemControllerEnums.ResultType.Yes)
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
		///		Recupera el layout de las ventanas
		/// </summary>
		private void RestoreLayout()
		{
			string fileName = GetFileNameLayout();

			// Recupera el layout
			//if (System.IO.File.Exists(fileName))
			//  new Xceed.Wpf.AvalonDock.Layout.Serialization.XmlLayoutSerializer(dckManager).Deserialize(fileName);
		}

		/// <summary>
		///		Nombre del archivo de layout
		/// </summary>
		private string GetFileNameLayout()
		{
			return System.IO.Path.Combine(Globals.HostController.HostViewModelController.Configuration.PathBaseData, "MainWindowLayout.xml");
		}

		/// <summary>
		///		Graba la configuración
		/// </summary>
		private void SaveConfiguration()
		{ 
			// Graba el layout de ventanas
			//new Xceed.Wpf.AvalonDock.Layout.Serialization.XmlLayoutSerializer(dckManager).Serialize(GetFileNameLayout());
			Properties.Settings.Default.Save();
			// Graba la configuración
			Globals.HostController.HostViewModelController.Configuration.Save();
		}

		/// <summary>
		///		Sale de la aplicación
		/// </summary>
		private void ExitApplication()
		{ 
			// Graba la configuración
			SaveConfiguration();
			// Cierra todos los documentos
			Globals.HostController.LayoutController.CloseAllDocuments();
			// Cierra la lista de notificaciones
			if (_wndNotifications != null)
				_wndNotifications.Close();
			// Cierra la aplicación
			//	Close();
		}

		/// <summary>
		///		Muestra una notificación
		/// </summary>
		internal void ShowNotification(Controls.Notifications.NotificationModel.NotificationType type, string message, string title, string urlImage = null)
		{
			Dispatcher.Invoke(new Action(() => WindowNotificationsManager.AddNotification(type, title, message, urlImage)), null);
		}

		/// <summary>
		///		Manager para la ventana de notificaciones
		/// </summary>
		private Controls.Notifications.WindowNotifications WindowNotificationsManager
		{
			get
			{ // Crea la ventana de notificaciones si no existía
				if (_wndNotifications == null)
				{
					Dispatcher.Invoke(new Action(() =>
												{ 
												// Crea la ventana
												_wndNotifications = new Controls.Notifications.WindowNotifications();
												// Coloca la ventana
												_wndNotifications.Top = SystemParameters.WorkArea.Top + 20;
												_wndNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - 380;
												}
									  ), null);
				}
				// Devuelve la ventana
				return _wndNotifications;
			}
		}

		/// <summary>
		///		Ultimos archivos abiertos
		/// </summary>
		private Classess.MRU.MRUFileModelCollection RecentFilesUsed
		{
			get
			{ 
				// Obtiene los últimos archivos utilizados
				if (_recentFilesUsed == null)
					_recentFilesUsed = new Classess.MRU.MRUFileRepository().Load();
				// Devuelve los últimos archivos utilizados
				return _recentFilesUsed;
			}
		}

		/// <summary>
		///		ViewModel con el layout de la ventana principal
		/// </summary>
		public IHostPluginsLayoutController ViewModel { get; private set; }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			InitForm();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ExitApplication();
		}

		private void mnuSetTheme_Click(object sender, RoutedEventArgs e)
		{
			MenuItem mnuTheme = sender as MenuItem;

				if (mnuTheme != null && mnuTheme.Tag != null)
					SetDockTheme(mnuTheme.Tag as string);
		}

		private void mnuExit_Click(object sender, RoutedEventArgs e)
		{
			ExitApplication();
		}

		private void mnuPlugins_Click(object sender, RoutedEventArgs e)
		{
			OpenWindowPathPlugins();
		}

		private void mnuConfiguration_Click(object sender, RoutedEventArgs e)
		{
			OpenWindowConfigurate();
		}

		private void mnuSaveAll_Click(object sender, RoutedEventArgs e)
		{
			Globals.HostController.LayoutController.SaveAllDocuments();
		}

		private void mnuCloseAllWindows_Click(object sender, RoutedEventArgs e)
		{
			Globals.HostController.LayoutController.CloseAllDocuments();
		}

		private void mnuHelp_Click(object sender, RoutedEventArgs e)
		{
			Globals.HostController.ShowWebBrowser("http://jbautistam.com");
		}
	}
}
