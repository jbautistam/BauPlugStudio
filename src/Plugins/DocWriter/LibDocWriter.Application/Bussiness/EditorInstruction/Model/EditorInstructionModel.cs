using System;

namespace Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction.Model
{
	/// <summary>
	///		Clase de datos para una instrucción del editor
	/// </summary>
	public class EditorInstructionModel : LibDocWriter.Model.Base.BaseDocWriterModel
	{
		/// <summary>
		///		Código de la instrucción
		/// </summary>
		public string Code { get; set; }
	}
}
