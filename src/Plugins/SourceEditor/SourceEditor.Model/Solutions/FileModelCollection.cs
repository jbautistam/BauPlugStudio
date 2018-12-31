using System;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.SourceEditor.Model.Solutions
{
	/// <summary>
	///		Colección de <see cref="FileModel"/>
	/// </summary>
	public class FileModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<FileModel>
	{
		/// <summary>
		///		Añade un objeto a la colección
		/// </summary>
		public void Add(FileModel parent, string fileName)
		{
			Add(new FileModel(parent, fileName));
		}

		/// <summary>
		///		Ordena los archivos por nombre y tipo
		/// </summary>
		public void SortByNameType(bool ascending = true)
		{
			Sort(new Comparers.FileComparer(ascending));
		}

		/// <summary>
		///		Busca un archivo por su nombre
		/// </summary>
		public FileModel SearchByFileName(string fileName)
		{
			return this.FirstOrDefault<FileModel>(file => file.FullFileName.EqualsIgnoreCase(fileName));
		}

		/// <summary>
		///		Comprueba si existe un archivo por su nombre
		/// </summary>
		public bool ExistsByFileName(string fileName)
		{
			return SearchByFileName(fileName) != null;
		}
	}
}
