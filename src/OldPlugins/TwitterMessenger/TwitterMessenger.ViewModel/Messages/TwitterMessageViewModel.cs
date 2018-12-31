using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibTwitter;
using Bau.Libraries.BauMvvm.ViewModels;

namespace Bau.Libraries.TwitterMessenger.ViewModel.Messages
{
	/// <summary>
	///		ViewModel para el control de envío de mensajes
	/// </summary>
	public class TwitterMessageViewModel : BaseObservableObject
	{ 
		// Variables privadas
		private string _message;
		private TwitterAccount _account;

		public TwitterMessageViewModel(TwitterAccount account)
		{
			Account = account;
			SendCommand = new BaseCommand("Enviar", parameter => Send(parameter),
													parameter => Account != null && !Message.IsEmpty())
							.AddListener(this, nameof(IsUpdated));

		}

		/// <summary>
		///		Envía el mensaje
		/// </summary>
		private void Send(object parameter)
		{
			TwitterMessengerViewModel.Instance.ControllerWindow.ShowMessage("Enviar");
		}

		/// <summary>
		///		Cuenta de Twitter
		/// </summary>
		public TwitterAccount Account
		{
			get { return _account; }
			set { CheckObject(ref _account, value); }
		}

		/// <summary>
		///		Mensaje a enviar
		/// </summary>
		public string Message
		{
			get { return _message; }
			set { CheckProperty(ref _message, value); }
		}

		/// <summary>
		///		Comando de envío
		/// </summary>
		public BaseCommand SendCommand { get; }
	}
}
