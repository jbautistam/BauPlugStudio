using System;

namespace Bau.Libraries.LibNSharpDoc.Models.Interfaces
{
	/// <summary>
	///		Interface para los intérpretes de archivos en estructuras de documentación
	/// </summary>
	public interface IDocumenter
	{
		/// <summary>
		///		Interpreta un archivo
		/// </summary>
		Structs.StructDocumentationModel Parse(Structs.StructParameterModelDictionary parameters);
	}
}
