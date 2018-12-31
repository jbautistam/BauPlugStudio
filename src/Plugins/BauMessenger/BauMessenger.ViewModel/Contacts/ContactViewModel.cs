using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibXmppClient.Core;
using Bau.Libraries.LibXmppClient.Users;

namespace Bau.Libraries.BauMessenger.ViewModel.Contacts
{
	/// <summary>
	///		ViewModel para la definición de contactos
	/// </summary>
	public class ContactViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private string _jid, _nickName;

		public ContactViewModel(JabberConnection connection, JabberContact contact)
		{ 
			// Guarda las propiedades
			Connection = connection;
			Contact = contact;
			// Asigna las propiedades del contacto
			if (contact != null)
			{
				Jid = contact.Jid;
				NickName = contact.Name;
			}
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false; // ... supone que los datos no son correctos

				// Comprueba los datos
				if (Jid.IsEmpty())
					BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el código de usuario");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos de la cuenta
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{
				bool isNew = Contact == null;
				bool added = false;

					// Asigna los datos de la cuenta
					Contact = new JabberContact(Connection.Host.Address, Jid, null, NickName);
					// Crea el contacto
					try
					{ 
						// Añade el contacto
						Connection.AddContact(Jid, NickName);
						// Indica que se ha añadido
						added = true;
					}
					catch (Exception exception)
					{
						BauMessengerViewModel.Instance.ControllerWindow.ShowMessage
										  ($"Error al añadir el contacto a la cuenta.{Environment.NewLine}{exception.Message}");
					}
					// Cierra el formulario
					if (added)
					{ 
						// Indica que no ha habido modificaciones
						IsUpdated = false;
						SaveCommand.OnCanExecuteChanged();
						// Cierra el formulario
						RaiseEventClose(true);
					}
			}
		}

		/// <summary>
		///		Código de usuario
		/// </summary>
		public string Jid
		{
			get { return _jid; }
			set { CheckProperty(ref _jid, value); }
		}

		/// <summary>
		///		Nombre
		/// </summary>
		public string NickName
		{
			get { return _nickName; }
			set { CheckProperty(ref _nickName, value); }
		}

		/// <summary>
		///		Conexión
		/// </summary>
		private JabberConnection Connection { get; }

		/// <summary>
		///		Contacto
		/// </summary>
		private JabberContact Contact { get; set; }
	}
}