using System;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.SectionSteps
{
	/// <summary>
	///		Procesador del paso de carga y precompilación de secciones
	/// </summary>
	internal class StepLoadSectionsProcessor : AbstractBaseSteps
	{
		internal StepLoadSectionsProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Carga las secciones y plantillas
		/// </summary>
		internal override void Process()
		{
			NhamlCompiler.Variables.VariablesCollection variables;

				// Obtiene las variables de proyecto
				variables = Data.NhamlCompiler.GetProjectVariables();
				// Carga las secciones y plantillas
				foreach (Pages.FileTargetModel file in Data.Files)
					if (file.File.FileType == Model.Solutions.FileModel.DocumentType.Template ||
							file.File.FileType == Model.Solutions.FileModel.DocumentType.Section)
						Data.Templates.Add(file);
		}
	}
}
