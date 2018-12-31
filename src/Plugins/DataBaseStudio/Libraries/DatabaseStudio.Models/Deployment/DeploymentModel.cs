using System;
using System.Collections.Generic;

namespace Bau.Libraries.DatabaseStudio.Models.Deployment
{
	/// <summary>
	///		Clase con los datos de un modelo de distribución de los scripts
	/// </summary>
    public class DeploymentModel : LibDataStructures.Base.BaseExtendedModel
    {
		/// <summary>
		///		Formatos de salida de los informes
		/// </summary>
		public enum ReportFormat
		{
			/// <summary>Xml</summary>
			Xml,
			/// <summary>Html (sin conversión)</summary>
			Html,
			/// <summary>Html (con conversión)</summary>
			HtmlConverted,
			/// <summary>Pdf</summary>
			Pdf
		}

		/// <summary>
		///		Directorio destino de los script
		/// </summary>
		public string PathScriptsTarget { get; set; }

		/// <summary>
		///		Directorio de generación de archivos
		/// </summary>
		public string PathFilesTarget { get; set; }

		/// <summary>
		///		Conexiones
		/// </summary>
		public Dictionary<string, Connections.DatabaseConnectionModel> Connections { get; } = new Dictionary<string, Connections.DatabaseConnectionModel>();

		/// <summary>
		///		Parámetros
		/// </summary>
		public LibDataStructures.Base.BaseModelCollection<ParameterModel> Parameters { get; } = new LibDataStructures.Base.BaseModelCollection<ParameterModel>();

		/// <summary>
		///		Scripts
		/// </summary>
		public LibDataStructures.Base.BaseModelCollection<ScriptModel> Scripts { get; } = new LibDataStructures.Base.BaseModelCollection<ScriptModel>();

		/// <summary>
		///		Formatos de salida de los informes
		/// </summary>
		public List<ReportFormat> ReportFormatTypes { get; } = new List<ReportFormat>();
    }
}
