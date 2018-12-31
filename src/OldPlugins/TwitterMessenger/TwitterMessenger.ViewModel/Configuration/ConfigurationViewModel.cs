using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.TwitterMessenger.ViewModel.Configuration
{
	/// <summary>
	///		ViewModel para la configuración
	/// </summary>
	public class ConfigurationViewModel : BauMvvm.ViewModels.BaseObservableObject
	{ 
		// Variables privadas
		private string _oAuthConsumerKey, _oAuthConsumerSecret;

		public ConfigurationViewModel()
		{
			OAuthConsumerKey = TwitterMessengerViewModel.Instance.OAuthConsumerKey;
			OAuthConsumerSecret = TwitterMessengerViewModel.Instance.OAuthConsumerSecret;
		}

		/// <summary>
		///		Comprueba si los datos son correctos
		/// </summary>
		public bool ValidateData(out string error)
		{ 
			// Inicializa el valor de salida
			error = "";
			// Devuelve el valor que indica si los datos son correctos
			return error.IsEmpty();
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		public void Save()
		{
			TwitterMessengerViewModel.Instance.OAuthConsumerKey = OAuthConsumerKey;
			TwitterMessengerViewModel.Instance.OAuthConsumerSecret = OAuthConsumerSecret;
		}

		/// <summary>
		///		Clave de OAuth
		/// </summary>
		public string OAuthConsumerKey
		{
			get { return _oAuthConsumerKey; }
			set { CheckProperty(ref _oAuthConsumerKey, value); }
		}

		/// <summary>
		///		Secret de OAuth
		/// </summary>
		public string OAuthConsumerSecret
		{
			get { return _oAuthConsumerSecret; }
			set { CheckProperty(ref _oAuthConsumerSecret, value); }
		}
	}
}
