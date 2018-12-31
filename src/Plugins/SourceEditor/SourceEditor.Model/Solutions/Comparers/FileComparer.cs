using System;

namespace Bau.Libraries.SourceEditor.Model.Solutions.Comparers
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
			if (first.IsFolder && !second.IsFolder)
				return -1;
			else if (!first.IsFolder && second.IsFolder)
				return 1;
			else
				return first.Name.CompareTo(second.Name);
		}
	}
}
