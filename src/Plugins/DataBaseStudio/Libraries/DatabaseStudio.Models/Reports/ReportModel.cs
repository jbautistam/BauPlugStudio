using System;

using Bau.Libraries.LibDataStructures.Collections;

namespace Bau.Libraries.DatabaseStudio.Models.Reports
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
		///		Parámetros
		/// </summary>
		public ParameterModelCollection Parameters { get; } = new ParameterModelCollection();

		/// <summary>
		///		Parámetros fijos o externos
		/// </summary>
		public ParameterModelCollection FixedParameters { get; } = new ParameterModelCollection();
	}
}
