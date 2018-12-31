using System;
using System.Windows.Controls;

using Bau.Libraries.WebCurator.ViewModel.Configuration;

namespace Bau.Libraries.WebCurator.Plugin.Views.Configuration
{
	/// <summary>
	///		Control de configuración de WebCurator
	/// </summary>
	public partial class ctlConfigurationWebCurator : UserControl, Libraries.Plugins.Views.Host.IUserControlConfigurationView
	{
		public ctlConfigurationWebCurator()
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

