using System;

using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions.TreeExplorer
{
	/// <summary>
	///		Nodo del árbol <see cref="ProjectModel"/>
	/// </summary>
	public class ProjectNodeViewModel : BaseNodeViewModel
	{
		public ProjectNodeViewModel(BaseNodeViewModel parent, ProjectModel project) : base(project.File.FullFileName, project.Name, parent, true)
		{
			Project = project;
			IsBold = true;
		}

		/// <summary>
		///		Carga los hijos del nodo
		/// </summary>
		public override void LoadChildren()
		{ 
			// Carga los archivos hijo
			Project.File.Files.Clear();
			Project.File.Files.AddRange(new Application.Bussiness.Solutions.FileBussiness().Load(Project));
			// Ordena los archivos hijo
			Project.File.Files.SortByNameType();
			// Muestra los archivos hijo
			foreach (FileModel file in Project.File.Files)
				if (file.FileName != ProjectModel.FileName) // ... se salta el archivo de proyecto
					Children.Add(new FileNodeViewModel(file, this));
			// Llama al método base
			base.LoadChildren();
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		public ProjectModel Project { get; }
	}
}
