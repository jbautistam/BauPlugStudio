using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibMediaPlayer.ViewModel.Configuration
{
	/// <summary>
	///		ViewModel para la configuración
	/// </summary>
	public class ConfigurationViewModel : BauMvvm.ViewModels.BaseObservableObject
	{   
		// Variables privadas
		private string _pathFiles;

		public ConfigurationViewModel()
		{
			PathFiles = MediaPlayerViewModel.Instance.PathFiles;
		}

		/// <summary>
		///		Comprueba si los datos son correctos
		/// </summary>
		public bool ValidateData(out string error)
		{ 
			// Inicializa los argumentos de salida
			error = "";
			// Devuelve el valor que indica si los datos son correctos
			return error.IsEmpty();
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		public void Save()
		{
			MediaPlayerViewModel.Instance.PathFiles = PathFiles;
		}

		/// <summary>
		///		Directorio a partir del que se encuentran los archivos multimedia
		/// </summary>
		public string PathFiles
		{
			get { return _pathFiles; }
			set { CheckProperty(ref _pathFiles, value); }
		}
	}
}
