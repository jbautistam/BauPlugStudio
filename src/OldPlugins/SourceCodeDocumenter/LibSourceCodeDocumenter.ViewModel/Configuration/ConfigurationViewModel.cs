using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Configuration
{
	/// <summary>
	///		Viewmodel para la configuración del documentador de código
	/// </summary>
	public class ConfigurationViewModel : BauMvvm.ViewModels.BaseObservableObject
	{ 
		// Variables privadas
		private string _compilerFileName;

		public ConfigurationViewModel()
		{
			CompilerFileName = SourceCodeDocumenterViewModel.Instance.CompilerFileName;
		}

		/// <summary>
		///		Comprueba si los datos son correctos
		/// </summary>
		public bool ValidateData(out string error)
		{ 
			// Inicializa los argumentos de salida
			error = "";
			// Comprueba el directorio de datos
			if (CompilerFileName.IsEmpty())
				error = "Seleccione el nombre del ejecutable que realiza la compilación";
			else if (!System.IO.File.Exists(CompilerFileName))
				error = "No se encuentra el archivo de compilación";
			// Devuelve el valor que indica si los datos son correctos
			return error.IsEmpty();
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		public void Save()
		{
			SourceCodeDocumenterViewModel.Instance.CompilerFileName = CompilerFileName;
		}

		/// <summary>
		///		Nombre del ejecutable de compilación
		/// </summary>
		public string CompilerFileName
		{
			get { return _compilerFileName; }
			set { CheckProperty(ref _compilerFileName, value); }
		}
	}
}
