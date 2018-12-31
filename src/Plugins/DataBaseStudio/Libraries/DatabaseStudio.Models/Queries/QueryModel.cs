using System;

namespace Bau.Libraries.DatabaseStudio.Models.Queries
{
	/// <summary>
	///		Clase de modelo para una consulta SQL
	/// </summary>
	public class QueryModel : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		SQL de la consulta
		/// </summary>
		public string SQL { get; set; }
	}
}
