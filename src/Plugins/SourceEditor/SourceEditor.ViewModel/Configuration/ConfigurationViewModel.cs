using System;

namespace Bau.Libraries.SourceEditor.ViewModel.Configuration
{
	/// <summary>
	///		ViewModel para la configuración
	/// </summary>
	public class ConfigurationViewModel : BauMvvm.ViewModels.BaseObservableObject
	{   
		// Variables privadas
		private string pathData;

		public ConfigurationViewModel()
		{
			PathData = SourceEditorViewModel.Instance.PathData;
		}

		/// <summary>
		///		Comprueba si los datos son correctos
		/// </summary>
		public bool ValidateData(out string error)
		{ 
			// Inicializa los argumentos de salida
			error = "";
			// Comprueba el directorio de datos
			if (string.IsNullOrEmpty(PathData))
				error = "Seleccione el directorio donde se guardan las soluciones";
			// Devuelve el valor que indica si los datos son correctos
			return string.IsNullOrEmpty(error);
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		public void Save()
		{
			SourceEditorViewModel.Instance.PathData = PathData;
		}

		/// <summary>
		///		Directorio donde se encuentran los proyectos
		/// </summary>
		public string PathData
		{
			get { return pathData; }
			set { CheckProperty(ref pathData, value); }
		}
	}
}
