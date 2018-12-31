using System;

using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.ViewModel.Solutions.TreeExplorer
{
	/// <summary>
	///		Nodo del árbol para <see cref="OwnerChildModel"/>
	/// </summary>
	public class OwnerNodeViewModel : BaseNodeViewModel
	{
		public OwnerNodeViewModel(FileModel file, OwnerChildModel ownerChild, BaseNodeViewModel parent, bool hasChilds = true)
								: base(file.FullFileName + "_" + ownerChild.GlobalId, ownerChild.Text, parent, hasChilds)
		{
			File = file;
			OwnerChild = ownerChild;
		}

		/// <summary>
		///		Carga los elementos hijo
		/// </summary>
		public override void LoadChildren()
		{   
			// Carga los elementos hijo
			Children.AddRange(new Helpers.OwnerNodeHelper().LoadOwnerNodes(this, File, OwnerChild));
			// Llama al método base
			base.LoadChildren();
		}

		/// <summary>
		///		Archivo padre
		/// </summary>
		public FileModel File { get; }

		/// <summary>
		///		Objeto propietario
		/// </summary>
		public OwnerChildModel OwnerChild { get; }
	}
}
