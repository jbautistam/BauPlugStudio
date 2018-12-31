using System;

using Bau.Libraries.LibHelper.Extensors;

namespace Bau.Libraries.BauMessenger.ViewModel.Configuration
{
	/// <summary>
	///		ViewModel para la configuración
	/// </summary>
	public class ConfigurationViewModel : MVVM.ViewModels.BaseViewModel
	{ // Variables privadas
			private string strOAuthConsumerKey, strOAuthConsumerSecret;
		
		public ConfigurationViewModel()
		{ OAuthConsumerKey = BauMessengerViewModel.Instance.OAuthConsumerKey;
			OAuthConsumerSecret = BauMessengerViewModel.Instance.OAuthConsumerSecret;
		}

		/// <summary>
		///		Comprueba si los datos son correctos
		/// </summary>
		public bool ValidateData(out string strError)
		{ // Inicializa el valor de salida
				strError = "";
			// Devuelve el valor que indica si los datos son correctos
				return strError.IsEmpty();
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		public void Save()
		{ BauMessengerViewModel.Instance.OAuthConsumerKey = OAuthConsumerKey;
			BauMessengerViewModel.Instance.OAuthConsumerSecret = OAuthConsumerSecret;
		}

		/// <summary>
		///		Clave de OAuth
		/// </summary>
		public string OAuthConsumerKey
		{ get { return strOAuthConsumerKey; }
			set
				{ if (strOAuthConsumerKey != value)
						{ strOAuthConsumerKey = value;
							OnPropertyChanged("OAuthConsumerKey");
						}
				}
		}

		/// <summary>
		///		Secret de OAuth
		/// </summary>
		public string OAuthConsumerSecret
		{ get { return strOAuthConsumerSecret; }
			set
				{ if (strOAuthConsumerSecret != value)
						{ strOAuthConsumerSecret = value;
							OnPropertyChanged("OAuthConsumerSecret");
						}
				}
		}
	}
}
