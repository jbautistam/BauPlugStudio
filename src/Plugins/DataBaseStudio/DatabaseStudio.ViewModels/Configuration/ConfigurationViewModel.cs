using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Configuration
{
	/// <summary>
	///		ViewModel para la configuración
	/// </summary>
	public class ConfigurationViewModel : BauMvvm.ViewModels.BaseObservableObject
	{   
		// Variables privadas
		private string _pathBase;

		public ConfigurationViewModel()
		{
			PathBase = MainViewModel.Instance.ProjectsPathBase;
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
			MainViewModel.Instance.ProjectsPathBase = PathBase;
		}

		/// <summary>
		///		Nombre del directorio base de proyectos
		/// </summary>
		public string PathBase
		{
			get { return _pathBase; }
			set { CheckProperty(ref  _pathBase, value); }
		}
	}
}
