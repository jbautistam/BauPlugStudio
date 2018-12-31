using System;
using System.Windows.Controls;

using Bau.Libraries.LibDocWriter.ViewModel.Configuration;

namespace Bau.Plugins.DocWriter.Views.Configuration
{
	/// <summary>
	///		Control de configuración de DocWriter
	/// </summary>
	public partial class ctlConfigurationDocWriter : UserControl, Libraries.Plugins.Views.Host.IUserControlConfigurationView
	{
		public ctlConfigurationDocWriter()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = new ConfigurationViewModel();
		}

		/// <summary>
		///		Comprueba los datos
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
		///		ViewModel asociado a la ventana
		/// </summary>
		public ConfigurationViewModel ViewModel
		{
			get { return DataContext as ConfigurationViewModel; }
			set { DataContext = value; }
		}
	}
}