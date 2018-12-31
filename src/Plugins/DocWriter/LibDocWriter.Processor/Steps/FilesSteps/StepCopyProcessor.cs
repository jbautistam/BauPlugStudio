using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.LibDocWriter.Processor.Pages;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.FilesSteps
{
	/// <summary>
	///		Procesador para la copia de archivos al directorio destino
	/// </summary>
	internal class StepCopyProcessor : AbstractBaseSteps
	{
		internal StepCopyProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Copia los archivos del proyecto que no precisan compilación
		/// </summary>
		internal override void Process()
		{
			foreach (FileTargetModel file in Data.Files)
				if (MustCopy(file))
				{
					string fileName = file.GetFullFileName(Processor);

						// Crea el directorio destino
						HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileName));
						// Copia el archivo
						HelperFiles.CopyFile(file.File.FullFileName, fileName);
				}
		}

		/// <summary>
		///		Comprueba si debe copiar el archivo
		/// </summary>
		private bool MustCopy(FileTargetModel file)
		{
			return (file.File.FileType == Model.Solutions.FileModel.DocumentType.Image ||
							  file.File.FileType == Model.Solutions.FileModel.DocumentType.File) &&
				   !file.File.Extension.EqualsIgnoreCase(".scss") &&
				   !file.File.FileName.EqualsIgnoreCase(Model.Solutions.ProjectModel.FileName);
		}
	}
}
