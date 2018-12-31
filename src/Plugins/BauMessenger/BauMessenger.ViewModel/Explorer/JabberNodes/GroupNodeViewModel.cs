using System;

using Bau.Libraries.LibXmppClient.Users;

namespace Bau.Libraries.BauMessenger.ViewModel.Explorer.JabberNodes
{
	/// <summary>
	///		Nodo del árbol <see cref="JabberGroup"/>
	/// </summary>
	public class GroupNodeViewModel : BaseNodeViewModel
	{
		public GroupNodeViewModel(ConnectionNodeViewModel parent, JabberGroup group)
								: base($"{group.Type.ToString()}_{group.Name}", group.Name, group, parent, true)
		{
			Group = group;
			IsBold = true;
			ImageSource = BauMessengerViewModel.Instance.ViewsController.ImageGroup;
			Foreground = BauMvvm.ViewModels.Media.MvvmColor.Navy;
		}

		/// <summary>
		///		Obtiene el nodo de conexión
		/// </summary>
		public override ConnectionNodeViewModel GetConnectionNode()
		{
			return (Parent as ConnectionNodeViewModel)?.GetConnectionNode();
		}

		/// <summary>
		///		Carga los elementos hijo
		/// </summary>
		public override void LoadChildrenData()
		{ 
			foreach (System.Collections.Generic.KeyValuePair<string, JabberContact> contact in Group.Contacts)
				Children.Add(new ContactNodeViewModel(this, contact.Value));
		}

		/// <summary>
		///		Grupo
		/// </summary>
		public JabberGroup Group { get; }
	}
}
