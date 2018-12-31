using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.BookLibrary.ViewModel.Configuration
{
	/// <summary>
	///		ViewModel para la configuración
	/// </summary>
	public class ConfigurationViewModel : BauMvvm.ViewModels.BaseObservableObject
	{   
		// Variables privadas
		private string _pathLibrary;

		public ConfigurationViewModel()
		{
			PathLibrary = BookLibraryViewModel.Instance.PathLibrary;
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
			BookLibraryViewModel.Instance.PathLibrary = PathLibrary;
		}

		/// <summary>
		///		Nombre del archivo de base de datos
		/// </summary>
		public string PathLibrary
		{
			get { return _pathLibrary; }
			set { CheckProperty(ref  _pathLibrary, value); }
		}
	}
}
