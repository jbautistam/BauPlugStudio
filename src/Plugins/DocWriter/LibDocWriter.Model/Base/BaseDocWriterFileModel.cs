using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibDocWriter.Model.Base
{
	/// <summary>
	///		Clase base con los datos de un archivo
	/// </summary>
	public abstract class BaseDocWriterFileModel : Base.BaseDocWriterModel
	{
		/// <summary>
		///		Normaliza un nombre de archivo
		/// </summary>
		public string NormalizeFileName(string fileName, bool blnWithAccents = false)
		{
			return LibCommonHelper.Files.HelperFiles.Normalize(fileName, blnWithAccents);
		}

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public override string Name
		{
			get
			{
				if (!FullFileName.IsEmpty())
					return System.IO.Path.GetFileNameWithoutExtension(FullFileName);
				else
					return base.Name;
			}
			set { base.Name = value; }
		}

		/// <summary>
		///		Nombre completo del archivo
		/// </summary>
		public string FullFileName { get; set; }

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName
		{
			get
			{
				if (!FullFileName.IsEmpty())
					return System.IO.Path.GetFileName(FullFileName);
				else
					return null;
			}
		}

		/// <summary>
		///		Extensión del archivo
		/// </summary>
		public string Extension
		{
			get
			{
				if (!FullFileName.IsEmpty())
					return System.IO.Path.GetExtension(FullFileName);
				else
					return null;
			}
		}

		/// <summary>
		///		Directorio
		/// </summary>
		public string Path
		{
			get
			{
				if (!FullFileName.IsEmpty())
					return System.IO.Path.GetDirectoryName(FullFileName);
				else
					return null;
			}
		}
	}
}