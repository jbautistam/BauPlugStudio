using System;

namespace Bau.Libraries.DatabaseStudio.Models.Deployment
{
	/// <summary>
	///		Datos de un parámetro
	/// </summary>
    public class ParameterModel : LibDataStructures.Base.BaseModel
    {
		/// <summary>
		///		Tipo de parámetro
		/// </summary>
		public enum ParameterType
		{
			/// <summary>Cadena</summary>
			String,
			/// <summary>Fecha</summary>
			DateTime,
			/// <summary>Entero</summary>
			Integer,
			/// <summary>Decimal</summary>
			Decimal,
			/// <summary>Lógico</summary>
			Boolean
		}

		/// <summary>
		///		Tipo de parámetro
		/// </summary>
		public ParameterType Type { get; set; } = ParameterType.String;

		/// <summary>
		///		Valor
		/// </summary>
		public string Value { get; set; }
    }
}
