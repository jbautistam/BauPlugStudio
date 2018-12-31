using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction.Model;

namespace Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction.Repository
{
	/// <summary>
	///		Repository para <see cref="EditorInstructionModel"/>
	/// </summary>
	internal class EditorInstructionRepository
	{ 
		// Constantes privadas
		private const string TagRoot = "Instructions";
		private const string TagInstruction = "Instruction";
		private const string TagName = "Name";
		private const string TagCode = "Code";

		/// <summary>
		///		Carga las instrucciones de un archivo
		/// </summary>
		internal EditorInstructionModelCollection Load(string fileName)
		{
			EditorInstructionModelCollection instructions = new EditorInstructionModelCollection();
			MLFile fileML = new XMLParser().Load(fileName);

				// Carga los datos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
							foreach (MLNode InstructionML in nodeML.Nodes)
								if (InstructionML.Name == TagInstruction)
								{
									EditorInstructionModel instruction = new EditorInstructionModel();

										// Asigna los datos
										instruction.Name = InstructionML.Nodes [TagName].Value;
										instruction.Code = InstructionML.Nodes [TagCode].Value;
										// Añade la instrucción a la colección
										instructions.Add(instruction);
								}
				// Ordena las instrucciones por nombre
				instructions.SortByName();
				// Devuelve la colección de instrucciones
				return instructions;
		}

		/// <summary>
		///		Graba las instrucciones en un archivo
		/// </summary>
		internal void Save(string fileName, EditorInstructionModelCollection instructions)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Añade las instrucciones
				foreach (EditorInstructionModel instruction in instructions)
				{
					MLNode instructionML = nodeML.Nodes.Add(TagInstruction);

						// Añade los campos de la instrucción
						instructionML.Nodes.Add(TagName, instruction.Name);
						instructionML.Nodes.Add(TagCode, instruction.Code);
				}
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}
	}
}
