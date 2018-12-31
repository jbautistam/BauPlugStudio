using System;

namespace Bau.Libraries.LibNSharpDoc.Projects.Models
{
	/// <summary>
	///		Clase con los datos de un proveedor para la generación de documentos
	/// </summary>
	public class ProviderModel
	{
		/// <summary>
		///		ID
		/// </summary>
		public string ID { get; } = Guid.NewGuid().ToString();

		/// <summary>
		///		Tipo de generador
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		///		Parámetros de generación
		/// </summary>
		public LibNSharpDoc.Models.Structs.StructParameterModelDictionary Parameters { get; } = new LibNSharpDoc.Models.Structs.StructParameterModelDictionary();
	}
}
