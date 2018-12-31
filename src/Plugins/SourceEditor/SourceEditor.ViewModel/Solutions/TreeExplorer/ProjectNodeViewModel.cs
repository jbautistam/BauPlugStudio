using System;

using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.ViewModel.Solutions.TreeExplorer
{
	/// <summary>
	///		Nodo del árbol <see cref="ProjectModel"/>
	/// </summary>
	public class ProjectNodeViewModel : BaseNodeViewModel
	{
		public ProjectNodeViewModel(BaseNodeViewModel parent, ProjectModel project) : base(project.FullFileName, project.Name, parent, true)
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
			Project.Clear();
			// Si tiene nodos propietario, carga los nodos
			if (Project.Definition.OwnerChilds.Count > 0)
				Children.AddRange(new Helpers.OwnerNodeHelper().LoadOwnerNodes(this, Project));
			// Muestra los archivos hijo
			Project.Files.SortByNameType();
			foreach (FileModel file in Project.Files)
				if (file.FileDefinition is Model.Definitions.FileDefinitionModel &&
						Project.Definition.FilesDefinition.CanShow(file))
					Children.Add(new FileNodeViewModel(Project, file, this));
			// Llama al método base
			base.LoadChildren();
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		public ProjectModel Project { get; }
	}
}
