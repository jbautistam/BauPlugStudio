using System;

using Bau.Libraries.LibSmallCssCompiler;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.AdditionalSteps
{
	/// <summary>
	///		Paso de compilación de los archivos SmallCss
	/// </summary>
	internal class StepCompileScssProcessor : AbstractBaseSteps
	{
		internal StepCompileScssProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Compila los archivos SmallCss
		/// </summary>
		internal override void Process()
		{
			System.Collections.Generic.List<Tuple<string, string, string>> filesCompiled = new System.Collections.Generic.List<Tuple<string, string, string>>();

				// Primero copia los archivos
				for (int index = Data.Files.Count - 1; index >= 0; index--)
					if (Data.Files [index].File.FullFileName.EndsWith(".scss", StringComparison.CurrentCultureIgnoreCase))
					{
						Pages.FileTargetModel file = Data.Files [index];

							// Crea el directorio de destino
							LibCommonHelper.Files.HelperFiles.MakePath(file.GetFullPath(Processor));
							// Copia el archivo origen en el destino
							LibCommonHelper.Files.HelperFiles.CopyFile(file.File.FullFileName, file.GetFullFileName(Processor));
					}
				// Después los compila
				for (int index = Data.Files.Count - 1; index >= 0; index--)
					if (Data.Files [index].File.FullFileName.EndsWith(".scss", StringComparison.CurrentCultureIgnoreCase))
					{
						Pages.FileTargetModel file = Data.Files [index];

							// Sólo se compila lo que no comienza por "_"
							if (!file.File.FileName.StartsWith("_"))
							{ 
								// Crea el directorio de destino
								LibCommonHelper.Files.HelperFiles.MakePath(file.GetFullPath(Processor));
								// Compila el archivo
								Compile(file.GetFullFileName(Processor), file.GetFullPath(Processor));
								// Añade el archivo generado a la colección
								filesCompiled.Add(new Tuple<string, string, string>(file.File.FullFileName, file.PathTarget,
																					System.IO.Path.GetFileNameWithoutExtension(file.FileNameTarget) + ".css"));
							}
					}
				// Elimina los archivos intermedios
				for (int index = Data.Files.Count - 1; index >= 0; index--)
					if (Data.Files [index].File.FullFileName.EndsWith(".scss", StringComparison.CurrentCultureIgnoreCase))
					{
						string fileName = Data.Files [index].GetFullFileName(Processor);

							// Elimina el archivo copiado al destino (sustituyendo la extensión por .scss)
							LibCommonHelper.Files.HelperFiles.KillFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileName),
																 System.IO.Path.GetFileNameWithoutExtension(fileName) + ".scss"));
							// Elimina el archivo de la colección (aunque no se haya compilado porque comienza por _)
							Data.Files.RemoveAt(index);
					}
				// ... y por último añade los archivos compilados a la colección de archivos
				foreach (Tuple<string, string, string> fileCompiled in filesCompiled)
					Data.Files.Add(fileCompiled.Item1, fileCompiled.Item2, fileCompiled.Item3);
		}

		/// <summary>
		///		Compila un archivo SmallCss
		/// </summary>
		private void Compile(string fileName, string path)
		{
			SmallCssCompiler compiler = new SmallCssCompiler(fileName, path, Processor.MustMinimize, false);

				compiler.Compile();
		}
	}
}
