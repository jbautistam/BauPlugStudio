using System;
using System.Windows.Controls;

using Bau.Libraries.TwitterMessenger.ViewModel.Configuration;

namespace Bau.Plugins.TwitterMessenger.Views.Configuration
{
	/// <summary>
	///		Control de configuración de TwitterMessenger
	/// </summary>
	public partial class ctlConfigurationTwitterMessenger : UserControl, Libraries.Plugins.Views.Host.IUserControlConfigurationView
	{
		public ctlConfigurationTwitterMessenger()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = new ConfigurationViewModel();
		}

		/// <summary>
		///		Comprueba los datos introducidos
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
