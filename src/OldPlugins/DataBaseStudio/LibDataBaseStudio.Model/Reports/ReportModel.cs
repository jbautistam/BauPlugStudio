using System;

namespace Bau.Libraries.LibDataBaseStudio.Model.Reports
{
	/// <summary>
	///		Clase de modelo para un informe
	/// </summary>
	public class ReportModel : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		XML del Informe
		/// </summary>
		public string ReportDefinition { get; set; }

		/// <summary>
		///		Nombre del parámetro utilizado en la última ejecución
		/// </summary>
		public string LastExecutionParameterName { get; set; }

		/// <summary>
		///		Código del tipo de salida de la última ejecución
		/// </summary>
		public int LastExecutionOuputMode { get; set; }

		/// <summary>
		///		Nombre de archivo de la última ejecución
		/// </summary>
		public string LastExecutionFileName { get; set; }

		/// <summary>
		///		Parámetros asociados al informe
		/// </summary>
		public ReportExecutionModelCollection ExecutionParameters { get; } = new ReportExecutionModelCollection();
	}
}
