using System;

namespace Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction.Model
{
	/// <summary>
	///		Colección de <see cref="EditorInstructionModel"/>
	/// </summary>
	public class EditorInstructionModelCollection : LibDocWriter.Model.Base.BaseDocWriterModelCollection<EditorInstructionModel>
	{
		/// <summary>
		///		Elimina una instrucción buscando por su nombre
		/// </summary>
		public void RemoveByName(string name)
		{
			EditorInstructionModel located = SearchByName(name);

				if (located != null)
					Remove(located);
		}
	}
}
