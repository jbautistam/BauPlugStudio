using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.Trees;
using Bau.Libraries.LibXmppClient.Core;
using Bau.Libraries.LibXmppClient.EventArguments;

namespace Bau.Libraries.BauMessenger.ViewModel.Explorer.JabberNodes
{
	/// <summary>
	///		Arbol de conexiones
	/// </summary>
	public class TreeConnectionViewModel : TreeViewModel<BaseNodeViewModel>
	{
		/// <summary>
		///		Carga los nodos del árbol
		/// </summary>
		protected override void LoadNodesData()
		{
			// Limpia los nodos
			Children.Clear();
			// Carga los datos. Lo hace dentro de un try porque en ocasiones ha dado problemas la carga de la DLL de Sharp.Xmpp.dll
			//! Si da errores de carga, puede que sea porque en modo Release, la librería Sharp.Xmpp.dll tiene que estar marcada como "Any Cpu"
			try
			{ 
				// Carga la configuración
				BauMessengerViewModel.Instance.BauMessenger.LoadConfiguration();
				// Carga los nodos
				foreach (JabberConnection connection in BauMessengerViewModel.Instance.BauMessenger.ManagerJabber.Connections)
					Children.Add(new ConnectionNodeViewModel(connection));
			}
			catch (Exception exception)
			{
				BauMessengerViewModel.Instance.ControllerWindow.ShowMessage($"Excepción en la carga de conexiones: {exception.Message}");
			}
		}

		/// <summary>
		///		Trata el cambio de estado de un contacto
		/// </summary>
		internal void TreatChangedStatus(ChangedStatusEventArgs evntArgs)
		{
			foreach (BaseNodeViewModel nodeConnection in Children)
				if ((nodeConnection as ConnectionNodeViewModel)?.Connection.Host.EqualsTo(evntArgs.Connection.Host) ?? true)
					foreach (BaseNodeViewModel nodeGroup in nodeConnection.Children)
						if (nodeGroup is GroupNodeViewModel)
							foreach (BaseNodeViewModel nodeContact in nodeGroup.Children)
								if (nodeContact is ContactNodeViewModel node && node.Contact.EqualsTo(evntArgs.Contact))
									node.Availability = evntArgs.NewStatus.Status;
		}

		/// <summary>
		///		Obtiene la conexión seleccionada
		/// </summary>
		internal JabberConnection GetSelectedConnection()
		{
			if (SelectedNode is ConnectionNodeViewModel)
				return (SelectedNode as ConnectionNodeViewModel)?.Connection;
			else
				return null;
		}

		/// <summary>
		///		Obtiene el nodo seleccionado
		/// </summary>
		internal BaseNodeViewModel GetSelectedNode()
		{
			if (SelectedNode is BaseNodeViewModel node)
				return node;
			else
				return null;
		}

		/// <summary>
		///		Obtiene el contacto seleccionado
		/// </summary>
		internal ContactNodeViewModel GetSelectedContact()
		{
			if (SelectedNode is ContactNodeViewModel node)
				return node;
			else
				return null;
		}

		/// <summary>
		///		Obtiene el nodo de grupo seleccionado
		/// </summary>
		internal GroupNodeViewModel GetSelectedGroup()
		{
			if (SelectedNode is GroupNodeViewModel node)
				return node;
			else
				return null;
		}
	}
}
