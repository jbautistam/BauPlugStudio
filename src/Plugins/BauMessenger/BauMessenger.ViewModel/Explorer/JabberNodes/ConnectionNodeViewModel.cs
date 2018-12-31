using System;

using Bau.Libraries.LibXmppClient.Core;
using Bau.Libraries.LibXmppClient.Users;

namespace Bau.Libraries.BauMessenger.ViewModel.Explorer.JabberNodes
{
	/// <summary>
	///		Nodo del árbol <see cref="JabberConnection"/>
	/// </summary>
	public class ConnectionNodeViewModel : BaseNodeViewModel
	{   
		// Variables privadas
		private bool _isConnected;

		public ConnectionNodeViewModel(JabberConnection connection)
								: base($"{connection.Host.Address}_{connection.User.FullJid}",
									   $"{connection.User.FullJid} - {connection.Host.Address}", connection, null, true)
		{
			Connection = connection;
			IsBold = true;
			IsConnected = connection.IsConnected;
			SetImage(connection.IsConnected);
		}

		/// <summary>
		///		Obtiene el nodo de conexión
		/// </summary>
		public override ConnectionNodeViewModel GetConnectionNode()
		{
			return this;
		}

		/// <summary>
		///		Cambia la imagen (la primera vez, IsConnected == false y por tanto IsConnected = false no establece la nueva imagen)
		/// </summary>
		private void SetImage(bool isConnected)
		{
			if (isConnected)
			{
				ImageSource = BauMessengerViewModel.Instance.ViewsController.ImageServerConnected;
				Foreground = BauMvvm.ViewModels.Media.MvvmColor.Olive;
			}
			else
			{
				ImageSource = BauMessengerViewModel.Instance.ViewsController.ImageServerDisconnected;
				Foreground = BauMvvm.ViewModels.Media.MvvmColor.Red;
			}
		}

		/// <summary>
		///		Carga los elementos hijo
		/// </summary>
		public override void LoadChildrenData()
		{
			foreach (JabberGroup group in Connection.Groups)
				Children.Add(new GroupNodeViewModel(this, group));
		}

		/// <summary>
		///		Conexión
		/// </summary>
		public JabberConnection Connection { get; }

		/// <summary>
		///		Indica si está conectado
		/// </summary>
		public bool IsConnected
		{
			get { return _isConnected; }
			set
			{
				if (CheckProperty(ref _isConnected, value))
					SetImage(_isConnected);
			}
		}
	}
}
