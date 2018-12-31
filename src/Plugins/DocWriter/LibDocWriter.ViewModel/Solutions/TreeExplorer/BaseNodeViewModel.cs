using System;

using Bau.Libraries.MVVM.ViewModels.TreeItems;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions.TreeExplorer
{
	/// <summary>
	///		Clase base para los nodos del árbol <see cref="TreeExplorerViewModel"/>
	/// </summary>
	public abstract class BaseNodeViewModel : TreeViewItemViewModel
	{
		public BaseNodeViewModel(string nodeID, string text, ITreeViewItemViewModel parent = null, bool lazyLoadChildren = true)
										: base(nodeID, text, parent, lazyLoadChildren)
		{
		}
	}
}
