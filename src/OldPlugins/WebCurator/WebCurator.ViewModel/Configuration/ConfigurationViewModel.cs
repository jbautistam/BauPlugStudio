using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.WebCurator.ViewModel.Configuration
{
	/// <summary>
	///		ViewModel para la configuración
	/// </summary>
	public class ConfigurationViewModel : Bau.Libraries.BauMvvm.ViewModels.BaseObservableObject
	{   
		// Variables privadas
		private string _pathLibrary, _pathGenerate;
		private bool _autoStart;
		private int _minutesBetweenCompile;

		public ConfigurationViewModel()
		{
			PathLibrary = WebCuratorViewModel.Instance.PathLibrary;
			PathGenerate = WebCuratorViewModel.Instance.PathGeneration;
			AutoStart = WebCuratorViewModel.Instance.AutoStartCompile;
			MinutesBetweenCompile = WebCuratorViewModel.Instance.MinutesBetweenCompile;
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
			WebCuratorViewModel.Instance.PathLibrary = PathLibrary;
			WebCuratorViewModel.Instance.PathGeneration = PathGenerate;
			WebCuratorViewModel.Instance.AutoStartCompile = AutoStart;
			WebCuratorViewModel.Instance.MinutesBetweenCompile = MinutesBetweenCompile;
		}

		/// <summary>
		///		Nombre del directorio donde se encuentran los archivos
		/// </summary>
		public string PathLibrary
		{
			get { return _pathLibrary; }
			set { CheckProperty(ref _pathLibrary, value); }
		}

		/// <summary>
		///		Directorio de compilación de los sitios web autogenerados
		/// </summary>
		public string PathGenerate
		{
			get { return _pathGenerate; }
			set { CheckProperty(ref _pathGenerate, value); }
		}

		/// <summary>
		///		Indica si se debe iniciar la compilación cuando se arranca la aplicación 
		/// </summary>
		public bool AutoStart
		{
			get { return _autoStart; }
			set { CheckProperty(ref _autoStart, value); }
		}

		/// <summary>
		///		Minutos entre compilaciones de código
		/// </summary>
		public int MinutesBetweenCompile
		{
			get { return _minutesBetweenCompile; }
			set { CheckProperty(ref _minutesBetweenCompile, value); }
		}
	}
}
