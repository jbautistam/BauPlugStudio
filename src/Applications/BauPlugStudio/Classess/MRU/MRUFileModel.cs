using System;

namespace Bau.Applications.BauPlugStudio.Classess.MRU
{
	/// <summary>
	///		Clase para contener el nombre de un archivo abierto en la aplicación
	/// </summary>
	internal class MRUFileModel : Libraries.LibDataStructures.Base.BaseExtendedModel
	{
		internal MRUFileModel(string source, string fileName, string text)
		{
			Name = text;
			Source = source;
			FileName = fileName;
		}

		/// <summary>
		///		Aplicación origen del archivo
		/// </summary>
		internal string Source { get; private set; }

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		internal string FileName { get; private set; }
	}
}
