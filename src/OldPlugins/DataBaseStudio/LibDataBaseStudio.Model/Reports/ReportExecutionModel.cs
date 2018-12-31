using System;

using Bau.Libraries.LibDataStructures.Collections;

namespace Bau.Libraries.LibDataBaseStudio.Model.Reports
{
	/// <summary>
	///		Parámetros de una conexión
	/// </summary>
	public class ReportExecutionModel : Base.ConnectionItemBase
	{
		/// <summary>
		///		Parámetros
		/// </summary>
		public ParameterModelCollection Parameters { get; } = new ParameterModelCollection();

		/// <summary>
		///		Parámetros fijos o externos
		/// </summary>
		public ParameterModelCollection FixedParameters { get; } = new ParameterModelCollection();

		/// <summary>
		///		Archivos
		/// </summary>
		public ReportExecutionFileModelCollection Files { get; } = new ReportExecutionFileModelCollection();
	}
}
