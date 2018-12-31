using System;
using System.Collections.ObjectModel;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Projects.Deployments.Scripts
{
	/// <summary>
	///		Nodo del árbol de scripts de una distribución
	/// </summary>
	public class TreeScriptNodeViewModel : ControlHierarchicalViewModel
	{
		// Variables privadas
		private string _icon;

		public TreeScriptNodeViewModel(TreeScriptNodeViewModel parent, string fileName) : base(parent, string.Empty, fileName, false)
		{
			if (!string.IsNullOrEmpty(fileName))
				Text = System.IO.Path.GetFileName(fileName);
			else
				Text = "Sin archivo";
			FileName = fileName;
		}

		/// <summary>
		///		Obtiene los nodos hijo cambiando el tipo
		/// </summary>
		internal ObservableCollection<TreeScriptNodeViewModel> GetChildrensChecked()
		{
			ObservableCollection<TreeScriptNodeViewModel> children = new ObservableCollection<TreeScriptNodeViewModel>();

				// Convierte los nodos
				foreach (IHierarchicalViewModel node in Children)
					if (node.IsChecked && node is TreeScriptNodeViewModel scriptNode)
						children.Add(scriptNode);
				// Devuelve la colección de nodos
				return children;
		}

		/// <summary>
		///		Nombre de arcivo
		/// </summary>
		public string FileName { get; }

		/// <summary>
		///		Icono asociado al nodo
		/// </summary>
		public string Icon 
		{
			get { return _icon; }
			set { CheckProperty(ref _icon, value); }
		}
	}
}
