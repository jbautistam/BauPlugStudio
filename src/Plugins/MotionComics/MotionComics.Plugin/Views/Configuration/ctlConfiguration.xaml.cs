using System;
using System.Windows.Controls;

using Bau.Libraries.MotionComics.ViewModel.Configuration;

namespace Bau.Plugins.MotionComics.Views.Configuration
{
	/// <summary>
	///		Control de configuración del plugin
	/// </summary>
	public partial class ctlConfiguration : UserControl, Libraries.Plugins.Views.Host.IUserControlConfigurationView
	{
		public ctlConfiguration()
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
