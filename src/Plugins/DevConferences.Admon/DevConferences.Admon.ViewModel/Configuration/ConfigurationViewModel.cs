using System;

namespace Bau.Libraries.DevConferences.Admon.ViewModel.Configuration
{
	/// <summary>
	///		ViewModel para la configuración
	/// </summary>
	public class ConfigurationViewModel : BauMvvm.ViewModels.BaseObservableObject
	{ 
		// Variables privadas
		private string pathBase;

		public ConfigurationViewModel()
		{
			PathTracks = DevConferencesViewModel.Instance.PathTracks;
		}

		/// <summary>
		///		Comprueba si los datos son correctos
		/// </summary>
		public bool ValidateData(out string error)
		{ 
			// Inicializa el valor de salida
			error = "";
			// Comprueba los datos
			if (string.IsNullOrEmpty(PathTracks) || !System.IO.Directory.Exists(PathTracks))
				error = "Seleccione el directorio donde se almacenan los archivos";
			// Devuelve el valor que indica si los datos son correctos
			return string.IsNullOrEmpty(error);
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		public void Save()
		{
			DevConferencesViewModel.Instance.PathTracks = PathTracks;
		}

		/// <summary>
		///		Directorio de los canales
		/// </summary>
		public string PathTracks
		{
			get { return pathBase; }
			set { CheckProperty(ref pathBase, value); }
		}
	}
}
