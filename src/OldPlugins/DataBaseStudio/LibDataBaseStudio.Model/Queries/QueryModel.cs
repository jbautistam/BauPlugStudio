using System;

namespace Bau.Libraries.LibDataBaseStudio.Model.Queries
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

		/// <summary>
		///		Ultima conexión seleccionada
		/// </summary>
		public string LastConnectionGuid { get; set; }
	}
}
