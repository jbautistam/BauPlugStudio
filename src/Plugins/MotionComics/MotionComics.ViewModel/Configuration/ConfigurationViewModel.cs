using System;

namespace Bau.Libraries.MotionComics.ViewModel.Configuration
{
	/// <summary>
	///		Viewmodel para la configuración del documentador de código
	/// </summary>
	public class ConfigurationViewModel : BauMvvm.ViewModels.BaseObservableObject
	{ 
		// Variables privadas
		private bool _showAdorners;

		public ConfigurationViewModel()
		{
			ShowAdorners = MotionComicsViewModel.Instance.ShowAdorners;
		}

		/// <summary>
		///		Comprueba si los datos son correctos
		/// </summary>
		public bool ValidateData(out string error)
		{	
			// Inicializa los argumentos de salida
			error = "";
			// Devuelve el valor que indica si los datos son correctos
			return true;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		public void Save()
		{
			MotionComicsViewModel.Instance.ShowAdorners = ShowAdorners;
		}

		/// <summary>
		///		Indica si se deben mostrar los Adorners sobre los controles
		/// </summary>
		public bool ShowAdorners
		{
			get { return _showAdorners; }
			set { CheckProperty(ref _showAdorners, value); }
		}
	}
}
