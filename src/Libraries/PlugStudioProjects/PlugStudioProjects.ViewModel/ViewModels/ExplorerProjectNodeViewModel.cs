using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.PlugStudioProjects.Models;

namespace Bau.Libraries.PlugStudioProjects.ViewModels
{
	/// <summary>
	///		Nodo del explorador de proyectos
	/// </summary>
	public class ExplorerProjectNodeViewModel : ControlHierarchicalViewModel
	{
		// Variables privadas
		private string _icon;

		public ExplorerProjectNodeViewModel(ExplorerProjectViewModel treeViewModel, IHierarchicalViewModel parent, 
											string text, ProjectItemDefinitionModel itemDefinition, 
											object tag = null, bool lazyLoad = true, 
											BauMvvm.ViewModels.Media.MvvmColor foreground = null) 
							: base(parent, text, tag, lazyLoad, foreground)
		{
			TreeViewModel = treeViewModel;
			ItemDefinition = itemDefinition;
		}

		/// <summary>
		///		Obtiene el nombre de archivo del nodo
		/// </summary>
		public string GetNodeFileName(string pathBase)
		{
			if (ItemDefinition.Type == ProjectItemDefinitionModel.ItemType.Project || Parent == null)
				return pathBase;
			else if (ItemDefinition.Type == ProjectItemDefinitionModel.ItemType.Folder || ItemDefinition.Type == ProjectItemDefinitionModel.ItemType.File)
				return Tag?.ToString();
			else
				return string.Empty;
		}

		/// <summary>
		///		Carga los elementos hijo
		/// </summary>
		public override void LoadChildrenData()
		{
			TreeViewModel.LoadNodes(this);
		}

		/// <summary>
		///		Arbol al que pertenece el nodo
		/// </summary>
		private ExplorerProjectViewModel TreeViewModel { get; }

		/// <summary>
		///		Definición del elemento asociado al nodo
		/// </summary>
		public ProjectItemDefinitionModel ItemDefinition { get; }

		/// <summary>
		///		Icono del elemento
		/// </summary>
		public string Icon
		{
			get { return _icon; }
			set { CheckProperty(ref _icon, value); }
		}
	}
}
