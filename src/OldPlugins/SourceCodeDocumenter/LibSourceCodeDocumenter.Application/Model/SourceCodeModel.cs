using System;

namespace Bau.Libraries.LibSourceCodeDocumenter.Application.Model
{
	/// <summary>
	///		Clase de modelo para un proyecto a documentar
	/// </summary>
	public class SourceCodeModel : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		Nombre del archivo de proyecto
		/// </summary>
		public string SourceFileName { get; set; }
	}
}
