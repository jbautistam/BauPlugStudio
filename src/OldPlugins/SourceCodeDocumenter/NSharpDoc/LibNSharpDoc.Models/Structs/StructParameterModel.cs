using System;

namespace Bau.Libraries.LibNSharpDoc.Models.Structs
{
	/// <summary>
	///		Parámetro de una estructura
	/// </summary>
	public class StructParameterModel
	{
		public StructParameterModel(string key, object value, string referenceKey)
		{
			Key = key;
			Value = value;
			ReferenceKey = referenceKey;
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		internal string Debug(int indent)
		{
			string debug = new string('\t', indent);

				// Añade los datos del parámetro
				debug += $"Key: {Key} - Value: {Value} - ReferenceKey: {ReferenceKey}" + Environment.NewLine;
				debug += Parameters.Debug(indent + 1);
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Clave
		/// </summary>
		public string Key { get; }

		/// <summary>
		///		Valor
		/// </summary>
		public object Value { get; }

		/// <summary>
		///		Clave de referencia
		/// </summary>
		public string ReferenceKey { get; }

		/// <summary>
		///		Referencia de estructura
		/// </summary>
		public StructDocumentationModel Reference { get; set; }

		/// <summary>
		///		Parámetros de la referencia
		/// </summary>
		public StructParameterModelDictionary Parameters { get; } = new StructParameterModelDictionary();
	}
}
