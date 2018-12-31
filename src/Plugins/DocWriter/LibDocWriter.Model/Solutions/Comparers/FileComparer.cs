using System;

namespace Bau.Libraries.LibDocWriter.Model.Solutions.Comparers
{
	/// <summary>
	///		Comparador de archivos
	/// </summary>
	internal class FileComparer : LibDataStructures.Tools.Comparers.AbstractBaseComparer<FileModel>
	{
		internal FileComparer(bool ascending = true) : base(ascending) { }

		/// <summary>
		///		Compara dos <see cref="FileModel"/>
		/// </summary>
		protected override int CompareData(FileModel first, FileModel second)
		{
			if (first.FileType == FileModel.DocumentType.Folder && second.FileType != FileModel.DocumentType.Folder)
				return -1;
			else if (first.FileType != FileModel.DocumentType.Folder && second.FileType == FileModel.DocumentType.Folder)
				return 1;
			else if (first.IsDocumentFolder && !second.IsDocumentFolder)
				return -1;
			else if (!first.IsDocumentFolder && second.IsDocumentFolder)
				return 1;
			else
				return first.Title.CompareTo(second.Title);
		}
	}
}
