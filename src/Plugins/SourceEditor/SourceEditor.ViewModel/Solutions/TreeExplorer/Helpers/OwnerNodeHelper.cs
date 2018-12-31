using System;

using Bau.Libraries.MVVM.ViewModels.TreeItems;
using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.ViewModel.Solutions.TreeExplorer.Helpers
{
	/// <summary>
	///		Clase de ayuda para el tratamiento de los nodos <see cref="OwnerNodeViewModel"/>
	/// </summary>
	internal class OwnerNodeHelper
	{
		/// <summary>
		///		Obtiene los nodos propietario 
		/// </summary>
		internal TreeViewItemsViewModelCollection LoadOwnerNodes(BaseNodeViewModel parent, FileModel file)
		{
			return LoadOwnerNodes(parent, file, null, file.FileDefinition.OwnerChilds);
		}

		/// <summary>
		///		Obtiene los nodos correspondientes a las definiciones hija de un objeto propietario
		/// </summary>
		internal TreeViewItemsViewModelCollection LoadOwnerNodes(BaseNodeViewModel parent, FileModel file, OwnerChildModel ownerChild)
		{
			return LoadOwnerNodes(parent, file, ownerChild, ownerChild.Definition.OwnerChilds);
		}

		/// <summary>
		///		Obtiene los nodos correspondientes a las definiciones de objetos propietario
		/// </summary>
		private TreeViewItemsViewModelCollection LoadOwnerNodes(BaseNodeViewModel parent, FileModel file, OwnerChildModel owner,
																OwnerObjectDefinitionModelCollection ownerDefinitions)
		{
			TreeViewItemsViewModelCollection nodes = new TreeViewItemsViewModelCollection();

				// Obtiene los nodos
				if (owner != null && owner.ObjectChilds != null && owner.ObjectChilds.Count > 0)
					nodes = LoadTreeNodes(parent, file, owner.ObjectChilds);
				else
				{
					// Normaliza los elementos hijo
					if (ownerDefinitions == null || ownerDefinitions.Count == 0)
					{
						if (file is ProjectModel)
							ownerDefinitions = (file as ProjectModel)?.Definition.OwnerChilds;
						else
							ownerDefinitions = file.FileDefinition.OwnerChilds;
					}
					// Carga los nodos
					foreach (OwnerObjectDefinitionModel ownerDefinition in ownerDefinitions)
					{
						if (ownerDefinition.IsRootNode)
							nodes.Add(new OwnerNodeViewModel(file, new OwnerChildModel(file.FullFileName + "_" + ownerDefinition.GlobalId,
																					   file, ownerDefinition.Name,
																					   ownerDefinition, ownerDefinition.OwnerChilds.Count > 0),
																					   parent));
						else
							nodes = LoadTreeNodes(parent, file,
												  SourceEditorViewModel.Instance.MessagesController.LoadOwnerChilds(file, owner));
					}
				}
				// Devuelve la colección de nodos
				return nodes;
		}

		/// <summary>
		///		Carga los nodos del árbol
		/// </summary>
		private TreeViewItemsViewModelCollection LoadTreeNodes(BaseNodeViewModel parent, FileModel file,
															   OwnerChildModelCollection childs)
		{
			TreeViewItemsViewModelCollection nodes = new TreeViewItemsViewModelCollection();

				// Crea los nodos
				if (childs != null)
					foreach (OwnerChildModel ownerChild in childs)
						nodes.Add(new OwnerNodeViewModel(file, ownerChild, parent, ownerChild.HasChilds));
				// Devuelve la colección de nodos
				return nodes;
		}
	}
}
