using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Bau.Libraries.Plugins.Views.Host;
using Bau.Libraries.Plugins.Views.Plugins;

namespace Bau.Applications.BauPlugStudio.Views.Tools.Configuration
{
	/// <summary>
	///		Ventana de configuración
	/// </summary>
	public partial class ConfigurationView : Window
	{   
		// Variables privadas
		private readonly List<UserControl> _controlsConfiguration = new List<UserControl>();

		public ConfigurationView()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa los controles
			InitControls();
		}

		/// <summary>
		///		Inicializa los controles
		/// </summary>
		private void InitControls()
		{
			if (Globals.PluginsManager.Plugins != null)
				foreach (IPluginController plugin in Globals.PluginsManager.Plugins)
				{
					UserControl configuration = plugin.GetConfigurationControl();

						if (configuration != null && configuration is IUserControlConfigurationView)
						{
							TabItem tabControlItem = new TabItem();

								// Añade el control de configuración a la colección interna
								_controlsConfiguration.Add(configuration);
								// Añade el control a la pestaña
								tabControlItem.Header = plugin.Name;
								tabControlItem.Content = configuration;
								// Añade la pestaña al control
								tabControls.Items.Add(tabControlItem);
						}
				}
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = true; // ... supone que los datos son correctos

				// Comprueba los datos
				foreach (UserControl control in _controlsConfiguration)
					if (validate && control is IUserControlConfigurationView)
					{
						IUserControlConfigurationView controlView = control as IUserControlConfigurationView;

							if (controlView != null && !controlView.ValidateData(out string error))
							{ 
								// Muestra el error
								Globals.HostController.ControllerWindow.ShowMessage(error);
								// Indica que la validación no es correcta
								validate = false;
							}
					}
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos de configuración
		/// </summary>
		private void Save()
		{
			if (ValidateData())
			{ 
				// Graba los datos de los controles
				foreach (UserControl control in _controlsConfiguration)
					if (control is IUserControlConfigurationView)
					{
						IUserControlConfigurationView controlView = control as IUserControlConfigurationView;

						if (controlView != null)
							controlView.Save();
					}
				// Cierra el formulario
				DialogResult = true;
				Close();
			}
		}

		private void cmdSave_Click(object sender, RoutedEventArgs e)
		{
			Save();
		}
	}
}
