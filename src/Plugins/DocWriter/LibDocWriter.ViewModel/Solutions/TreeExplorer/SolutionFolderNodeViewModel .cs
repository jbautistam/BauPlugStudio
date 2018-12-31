using System;

using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions.TreeExplorer
{
	/// <summary>
	///		Nodo del árbol <see cref="SolutionFolderModel"/>
	/// </summary>
	public class SolutionFolderNodeViewModel : BaseNodeViewModel
	{
		public SolutionFolderNodeViewModel(SolutionFolderModel folder) : base(folder.GlobalId, folder.Name, null, true)
		{
			Folder = folder;
			IsBold = true;
		}

		/// <summary>
		///		Carga los hijos del nodo
		/// </summary>
		public override void LoadChildren()
		{ 
			// Ordena los archivos hijo
			Folder.Folders.SortByName();
			Folder.Projects.SortByName();
			// Muestra las carpetas hija
			foreach (SolutionFolderModel folder in Folder.Folders)
				Children.Add(new SolutionFolderNodeViewModel(folder));
			// Muestra los projectos hijo
			foreach (ProjectModel project in Folder.Projects)
				Children.Add(new ProjectNodeViewModel(this, project));
			// Llama al método base
			base.LoadChildren();
		}

		/// <summary>
		///		Carpeta
		/// </summary>
		public SolutionFolderModel Folder { get; }
	}
}
