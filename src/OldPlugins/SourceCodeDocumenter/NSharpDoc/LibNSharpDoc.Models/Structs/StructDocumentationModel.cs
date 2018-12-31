using System;

namespace Bau.Libraries.LibNSharpDoc.Models.Structs
{
	/// <summary>
	///		Modelo con los datos de una estructura a documentar
	/// </summary>
	public class StructDocumentationModel
	{ 
		// Enumerados públicos
	  /// <summary>
	  ///		Tipo de ámbito
	  /// </summary>
		public enum ScopeType
		{
			/// <summary>Desconocido</summary>
			Unknown,
			/// <summary>Público</summary>
			Public,
			/// <summary>Privado</summary>
			Private,
			/// <summary>Protegido</summary>
			Protected,
			/// <summary>Interno</summary>
			Internal,
			/// <summary>Ambito global para aquella documentación (como la de índice) que siempre debe salir)</summary>
			Global
		}

		public StructDocumentationModel(StructDocumentationModel parent, ScopeType scope, string name, string type, int order)
		{
			Parent = parent;
			Scope = scope;
			Name = name;
			Type = type;
			Order = order;
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		internal string Debug(int indent)
		{
			string debug = new string('\t', indent);

				// Crea la cadena de depuración
				debug += $"Name: {Name} - Scope: {Scope} Type: {Type} Order: {Order}" + Environment.NewLine;
				// Añade la cadena de depuración de los parámetros
				debug += Parameters.Debug(indent);
				// Añade la cadena de depuración de los hijos
				debug += Childs.Debug(indent + 1);
				// Devuelve la cadena de depuración
				return debug + Environment.NewLine;
		}

		/// <summary>
		///		Estructura padre
		/// </summary>
		public StructDocumentationModel Parent { get; }

		/// <summary>
		///		Ambito del elemento
		/// </summary>
		public ScopeType Scope { get; set; }

		/// <summary>
		///		Nombre
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Tipo
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		///		Orden
		/// </summary>
		public int Order { get; set; }

		/// <summary>
		///		Elementos hijo
		/// </summary>
		public StructDocumentationModelCollection Childs { get; } = new StructDocumentationModelCollection();

		/// <summary>
		///		Parámetros
		/// </summary>
		public StructParameterModelDictionary Parameters { get; } = new StructParameterModelDictionary();
	}
}
