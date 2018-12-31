using System;

namespace Bau.Libraries.LibDocWriter.Processor.Steps
{
	/// <summary>
	///		Clase base para los pasos del proceso de compilación
	/// </summary>
	internal abstract class AbstractBaseSteps
	{
		internal AbstractBaseSteps(Generator processor, CompilerData compilerData)
		{
			Processor = processor;
			Data = compilerData;
		}

		/// <summary>
		///		Procesa el paso
		/// </summary>
		internal abstract void Process();

		/// <summary>
		///		Datos de compilación
		/// </summary>
		internal CompilerData Data { get; }

		/// <summary>
		///		Procesador
		/// </summary>
		internal Generator Processor { get; }
	}
}
