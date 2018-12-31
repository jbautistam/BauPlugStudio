using System;

namespace Bau.Libraries.LibDataBaseStudio.Model.Reports
{
	/// <summary>
	///		Modelo con los datos de un archivo utilizado como parámetro en la ejecución del informe
	/// </summary>
	public class ReportExecutionFileModel : LibDataStructures.Base.BaseModel
	{
		/// <summary>
		///		Tipo de archivo
		/// </summary>
		public enum FileType
		{
			/// <summary>Imagen</summary>
			Image,
			/// <summary>Estilo</summary>
			Style,
			/// <summary>Fuente</summary>
			Font
		}

		/// <summary>
		///		Tipo de archivo
		/// </summary>
		public FileType IDType { get; set; }

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName { get; set; }
	}
}
