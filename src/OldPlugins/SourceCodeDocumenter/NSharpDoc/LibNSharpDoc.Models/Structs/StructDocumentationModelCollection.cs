using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibNSharpDoc.Models.Structs
{
	/// <summary>
	///		Colección de <see cref="StructDocumentationModel"/>
	/// </summary>
	public class StructDocumentationModelCollection : List<StructDocumentationModel>
	{
		/// <summary>
		///		Añade una estructura a la lista
		/// </summary>
		public StructDocumentationModel Add(StructDocumentationModel parent, StructDocumentationModel.ScopeType scope,
											string name, string type, int order)
		{
			StructDocumentationModel structDocumentation = new StructDocumentationModel(parent, scope, name, type, order);

			// Añade la estructura
			Add(structDocumentation);
			// Devuelve la estructura añadida
			return structDocumentation;
		}

		/// <summary>
		///		Depura la colección
		/// </summary>
		public string Debug(int indent = 0)
		{
			string debug = "";

				// Obtiene la cadena de depuración de las estructuras
				foreach (StructDocumentationModel structDocumentation in this)
					debug += structDocumentation.Debug(indent);
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Ordena las estructuras por nombre
		/// </summary>
		public void SortByName()
		{
			Sort((first, second) => first.Name.CompareTo(second.Name));
		}
	}
}
