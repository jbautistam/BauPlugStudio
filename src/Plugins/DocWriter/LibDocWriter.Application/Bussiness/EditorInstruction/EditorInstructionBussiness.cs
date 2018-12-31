using System;

namespace Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction
{
	/// <summary>
	///		Clase de negocio de <see cref="EditorInstructionModel"/>
	/// </summary>
	public class EditorInstructionBussiness
	{
		/// <summary>
		///		Carga las instrucciones de un archivo XML
		/// </summary>
		public Model.EditorInstructionModelCollection Load(string fileName)
		{
			return new Repository.EditorInstructionRepository().Load(fileName);
		}

		/// <summary>
		///		Graba las instrucciones en un archivo
		/// </summary>
		public void Save(string fileName, Model.EditorInstructionModelCollection instructions)
		{
			new Repository.EditorInstructionRepository().Save(fileName, instructions);
		}
	}
}
