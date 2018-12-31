using System;

using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions.TreeExplorer
{
	/// <summary>
	///		Nodo del árbol para <see cref="FileModel"/>
	/// </summary>
	public class FileNodeViewModel : BaseNodeViewModel
	{
		public FileNodeViewModel(FileModel file, BaseNodeViewModel parent)
										: base(file.FullFileName, file.Title, parent, file.IsFolder || file.IsDocumentFolder)
		{
			File = file;
		}

		/// <summary>
		///		Carga los hijos del nodo
		/// </summary>
		public override void LoadChildren()
		{ 
			// Carga los archivos hijo
			File.Files.Clear();
			File.Files.AddRange(new Application.Bussiness.Solutions.FileBussiness().Load(File));
			// Ordena los archivos hijo
			File.Files.SortByNameType();
			// Muestra los archivos hijo
			foreach (FileModel file in File.Files)
				Children.Add(new FileNodeViewModel(file, this));
			// Llama al método base
			base.LoadChildren();
		}

		/// <summary>
		///		Archivo
		/// </summary>
		public FileModel File { get; }

		/// <summary>
		///		Tipo de archivo
		/// </summary>
		public FileModel.DocumentType FileType
		{
			get { return File.FileType; }
		}
	}
}
