using System;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibDocWriter.Model.Solutions
{
	/// <summary>
	///		Colección de <see cref="FileModel"/>
	/// </summary>
	public class FilesModelCollection : Base.BaseDocWriterModelCollection<FileModel>
	{
		public FilesModelCollection(ProjectModel project)
		{
			Project = project;
		}

		/// <summary>
		///		Añade un archivo
		/// </summary>
		public new void Add(FileModel file)
		{
			FileModel oldFile = Search(file.GlobalId);

				// Si existe ya un archivo con ese ID le cambia los datos
				if (oldFile != null)
					oldFile.FullFileName = file.FullFileName;
				else
					base.Add(file);
		}

		/// <summary>
		///		Obtiene los archivos
		/// </summary>
		public string GetFilesIDs()
		{
			string files = "";

				// Añade los archivos
				foreach (FileModel file in this)
					files = files.AddWithSeparator(file.IDFileName, Environment.NewLine);
				// Devuelve los archivos
				return files;
		}

		/// <summary>
		///		Ordena los archivos por nombre y tipo (primero las carpetas y después los archivos)
		/// </summary>
		public void SortByNameType(bool ascending = true)
		{
			Sort(new Comparers.FileComparer(ascending));
		}

		/// <summary>
		///		Busca el primer elemento con un nombre de archivo
		/// </summary>
		public FileModel SearchByFullFileName(string strFullFileName)
		{
			return this.FirstOrDefault(file => file.FullFileName.EqualsIgnoreCase(strFullFileName));
		}

		/// <summary>
		///		Comprueba si existe un archivo con el mismo nombre
		/// </summary>
		public bool ExistsByFullFileName(string strFullFileName)
		{
			return SearchByFullFileName(strFullFileName) != null;
		}

		/// <summary>
		///		Busca el primer elemento con un nombre de archivo
		/// </summary>
		public FileModel SearchByIDFileName(string idFileName)
		{
			return this.FirstOrDefault(file => file.IDFileName.EqualsIgnoreCase(idFileName));
		}

		/// <summary>
		///		Comprueba si existe un archivo con el mismo nombre
		/// </summary>
		public bool ExistsByIDFileName(string idFileName)
		{
			return SearchByIDFileName(idFileName) != null;
		}

		/// <summary>
		///		Muestra los elementos de la colección
		/// </summary>
		public void Debug()
		{
			foreach (FileModel file in this)
				System.Diagnostics.Debug.WriteLine(file.FileName);
		}

		/// <summary>
		///		Proyecto al que están asociados los archivos
		/// </summary>
		public ProjectModel Project { get; }
	}
}
