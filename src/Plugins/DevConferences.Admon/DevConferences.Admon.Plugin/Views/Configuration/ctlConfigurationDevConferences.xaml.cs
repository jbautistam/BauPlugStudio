using System;
using System.Windows.Controls;

using Bau.Libraries.DevConferences.Admon.ViewModel.Configuration;

namespace Bau.Plugins.DevConferences.Admon.Views.Configuration
{
	/// <summary>
	///		Control de configuración
	/// </summary>
	public partial class ctlConfigurationDevConferences : UserControl, Libraries.Plugins.Views.Host.IUserControlConfigurationView
	{
		public ctlConfigurationDevConferences()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = new ConfigurationViewModel();
		}

		/// <summary>
		///		Compureba los datos introducidos
		/// </summary>
		public bool ValidateData(out string error)
		{
			return ViewModel.ValidateData(out error);
		}

		/// <summary>
		///		Graba los datos de configuración
		/// </summary>
		public void Save()
		{
			ViewModel.Save();
		}

		/// <summary>
		///		ViewModel con los datos de configuración
		/// </summary>
		public ConfigurationViewModel ViewModel
		{
			get { return DataContext as ConfigurationViewModel; }
			set { DataContext = value; }
		}
	}
}
