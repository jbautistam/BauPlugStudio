using System;

namespace Bau.Libraries.LibDocWriter.Processor.Steps
{
	/// <summary>
	///		Datos de compilación
	/// </summary>
	internal class CompilerData
	{
		internal CompilerData(Generator processor)
		{
			Files = new Pages.FileTargetModelCollection(processor);
			Templates = new Pages.SectionSourceModelCollection(processor);
			NhamlCompiler = new Compiler.NHamlCompilerWrapper(processor, this);
		}

		/// <summary>
		///		Archivos del proyecto
		/// </summary>
		internal Pages.FileTargetModelCollection Files { get; }

		/// <summary>
		///		Plantillas y secciones del proyecto
		/// </summary>
		internal Pages.SectionSourceModelCollection Templates { get; }

		/// <summary>
		///		Compilador de Nhaml
		/// </summary>
		internal Compiler.NHamlCompilerWrapper NhamlCompiler { get; }
	}
}
