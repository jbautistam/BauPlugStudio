using System;

using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.ViewModel.Solutions.TreeExplorer
{
	/// <summary>
	///		Nodo del árbol para <see cref="FileModel"/>
	/// </summary>
	public class FileNodeViewModel : BaseNodeViewModel
	{
		public FileNodeViewModel(ProjectModel project, FileModel file, BaseNodeViewModel parent)
										: base(file.FullFileName, file.Name, parent, file.IsFolder || file.FileDefinition.OwnerChilds.Count > 0)
		{
			Project = project;
			File = file;
			if (!file.IsFolder && (project.Definition.FilesDefinition.SearchByExtension(file.Extension) == null ||
																project.Definition.FilesDefinition.MustShowExtension(file)))
				Text += "." + file.Extension;
		}

		/// <summary>
		///		Carga los hijos del nodo
		/// </summary>
		public override void LoadChildren()
		{ 
			// Carga los archivos hijo
			File.Clear();
			// Ordena los archivos hijo
			File.Files.SortByNameType();
			// Muestra los archivos hijo
			foreach (FileModel file in File.Files)
				if (Project.Definition.FilesDefinition.CanShow(file))
					Children.Add(new FileNodeViewModel(Project, file, this));
			// Si tiene nodos propietario, carga los nodos
			if (File.FileDefinition.OwnerChilds.Count > 0)
				Children.AddRange(new Helpers.OwnerNodeHelper().LoadOwnerNodes(this, File));
			// Llama al método base
			base.LoadChildren();
		}

		/// <summary>
		///		Proyecto al que pertenece el archivo
		/// </summary>
		public ProjectModel Project { get; }

		/// <summary>
		///		Archivo
		/// </summary>
		public FileModel File { get; }
	}
}
