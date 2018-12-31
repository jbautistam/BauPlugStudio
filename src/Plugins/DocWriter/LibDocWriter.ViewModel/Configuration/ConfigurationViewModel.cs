using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibDocWriter.ViewModel.Configuration
{
	/// <summary>
	///		ViewModel para la configuración
	/// </summary>
	public class ConfigurationViewModel : BauMvvm.ViewModels.BaseObservableObject
	{   
		// Variables privadas
		private string _pathData, _pathGeneration;
		private bool _minimize, _saveBeforeCompile, _openExternalWebBrowser;

		public ConfigurationViewModel()
		{
			PathData = DocWriterViewModel.Instance.PathData;
			PathGeneration = DocWriterViewModel.Instance.PathGeneration;
			Minimize = DocWriterViewModel.Instance.Minimize;
			SaveBeforeCompile = DocWriterViewModel.Instance.SaveBeforeCompile;
			OpenExternalWebBrowser = DocWriterViewModel.Instance.OpenExternalWebBrowser;
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
			DocWriterViewModel.Instance.PathData = PathData;
			DocWriterViewModel.Instance.PathGeneration = PathGeneration;
			DocWriterViewModel.Instance.Minimize = Minimize;
			DocWriterViewModel.Instance.SaveBeforeCompile = SaveBeforeCompile;
			DocWriterViewModel.Instance.OpenExternalWebBrowser = OpenExternalWebBrowser;
		}

		/// <summary>
		///		Directorio donde se encuentran los proyectos
		/// </summary>
		public string PathData
		{
			get { return _pathData; }
			set { CheckProperty(ref _pathData, value); }
		}

		/// <summary>
		///		Directorio donde se generan los proyectos
		/// </summary>
		public string PathGeneration
		{
			get { return _pathGeneration; }
			set { CheckProperty(ref _pathGeneration, value); }
		}

		/// <summary>
		///		Indica si se debe minimizar el resultado de la compilación
		/// </summary>
		public bool Minimize
		{
			get { return _minimize; }
			set { CheckProperty(ref _minimize, value); }
		}

		/// <summary>
		///		Indica si se debe grabar antes de compilar
		/// </summary>
		public bool SaveBeforeCompile
		{
			get { return _saveBeforeCompile; }
			set { CheckProperty(ref _saveBeforeCompile, value); }
		}

		/// <summary>
		///		Indica si se debe abrir el resultado de la compilación en un navegador externo
		/// </summary>
		public bool OpenExternalWebBrowser
		{
			get { return _openExternalWebBrowser; }
			set { CheckProperty(ref _openExternalWebBrowser, value); }
		}
	}
}
