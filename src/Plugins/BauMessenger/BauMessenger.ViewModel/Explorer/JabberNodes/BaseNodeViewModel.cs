using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;

namespace Bau.Libraries.BauMessenger.ViewModel.Explorer.JabberNodes
{
	/// <summary>
	///		Clase base para los nodos del árbol <see cref="PaneTreeConnectionViewModel"/>
	/// </summary>
	public abstract class BaseNodeViewModel : ControlHierarchicalViewModel
	{
		// Variables privadas
		private string _nodeId;

		public BaseNodeViewModel(string nodeID, string text, object tag, ControlHierarchicalViewModel parent = null, bool lazyLoad = true)
								: base(parent, text, tag, lazyLoad)
		{
			NodeId = nodeID;
		}

		/// <summary>
		///		Id del nodo
		/// </summary>
		public string NodeId
		{
			get { return _nodeId; }
			set { CheckProperty(ref _nodeId, value); }
		}

		/// <summary>
		///		Obtiene el nodo de la conexión
		/// </summary>
		public abstract ConnectionNodeViewModel GetConnectionNode();
	}
}
