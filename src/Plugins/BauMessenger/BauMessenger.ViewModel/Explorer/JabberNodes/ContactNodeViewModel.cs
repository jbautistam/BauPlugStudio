using System;

using Bau.Libraries.LibXmppClient.Users;

namespace Bau.Libraries.BauMessenger.ViewModel.Explorer.JabberNodes
{
	/// <summary>
	///		Nodo del árbol <see cref="JabberContact"/>
	/// </summary>
	public class ContactNodeViewModel : BaseNodeViewModel
	{   
		// Variables privadas
		private JabberContactStatus.Availability _status = JabberContactStatus.Availability.Offline;

		public ContactNodeViewModel(GroupNodeViewModel parent, JabberContact contact) : base(contact.FullJid, contact.FullName, contact, parent, false)
		{
			Contact = contact;
			Availability = contact.Status.Status;
			SetImage();
		}

		/// <summary>
		///		Obtiene el nodo de conexión del contacto
		/// </summary>
		public override ConnectionNodeViewModel GetConnectionNode()
		{
			return (Parent as GroupNodeViewModel)?.GetConnectionNode();
		}

		/// <summary>
		///		Cambia la imagen
		/// </summary>
		private void SetImage()
		{
			if (Contact.Subscription.Mode != JabberSubscriptionStatus.SubscriptionMode.Both)
				ImageSource = BauMessengerViewModel.Instance.ViewsController.ImageUserPendingRequest;
			else
				switch (Contact.Status.Status)
				{
					case JabberContactStatus.Availability.Away:
							ImageSource = BauMessengerViewModel.Instance.ViewsController.ImageUserStatusAway;
						break;
					case JabberContactStatus.Availability.Chat:
							ImageSource = BauMessengerViewModel.Instance.ViewsController.ImageUserStatusChat;
						break;
					case JabberContactStatus.Availability.Dnd:
							ImageSource = BauMessengerViewModel.Instance.ViewsController.ImageUserStatusDnd;
						break;
					case JabberContactStatus.Availability.Offline:
							ImageSource = BauMessengerViewModel.Instance.ViewsController.ImageUserStatusOffline;
						break;
					case JabberContactStatus.Availability.Online:
							ImageSource = BauMessengerViewModel.Instance.ViewsController.ImageUserStatusOnline;
						break;
					case JabberContactStatus.Availability.Xa:
							ImageSource = BauMessengerViewModel.Instance.ViewsController.ImageUserStatusXa;
						break;
				}
		}

		/// <summary>
		///		Contacto
		/// </summary>
		public JabberContact Contact { get; }

		/// <summary>
		///		Estado del contacto: utiliza una variable privada en lugar de Contact.Status porque puede que los valores del contacto
		///	se hayan cambiado ya desde la librería y entonces Contact.Status != value siempre sería false
		/// </summary>
		public JabberContactStatus.Availability Availability
		{
			get { return _status; }
			set
			{
				if (CheckProperty(ref _status, value))
					SetImage();
			}
		}
	}
}
