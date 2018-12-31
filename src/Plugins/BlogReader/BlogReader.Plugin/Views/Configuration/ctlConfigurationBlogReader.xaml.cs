using System;
using System.Windows.Controls;

using Bau.Libraries.LibBlogReader.ViewModel.Configuration;

namespace Bau.Plugins.BlogReader.Views.Configuration
{
	/// <summary>
	///		Control de configuración de BlogReader
	/// </summary>
	public partial class ctlConfigurationBlogReader : UserControl, Libraries.Plugins.Views.Host.IUserControlConfigurationView
	{
		public ctlConfigurationBlogReader()
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
