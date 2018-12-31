using System;

namespace Bau.Libraries.LibDocWriter.Model.Documents
{
	/// <summary>
	///		Clase con los datos de un archivo de referencia
	/// </summary>
	public class ReferenceModel : Base.BaseDocWriterFileModel
	{
		public ReferenceModel(Solutions.FileModel file)
		{
			File = file;
		}

		/// <summary>
		///		Archivo de la referencia
		/// </summary>
		public Solutions.FileModel File { get; }

		/// <summary>
		///		Nombre del proyecto en el que se encuentra la referencia
		/// </summary>
		public string ProjectName { get; set; }

		/// <summary>
		///		Nombre del archivo al que se hace referencia
		/// </summary>
		public string FileNameReference { get; set; }
	}
}
