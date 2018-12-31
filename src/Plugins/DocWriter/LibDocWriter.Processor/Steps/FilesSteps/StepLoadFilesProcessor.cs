using System;
using System.IO;

using Bau.Libraries.LibDocWriter.Application.Bussiness.Documents;
using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.Processor.Pages;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.FilesSteps
{
	/// <summary>
	///		Procesador para la carga de archivos y la copia al directorio destino
	/// </summary>
	internal class StepLoadFilesProcessor : AbstractBaseSteps
	{
		internal StepLoadFilesProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Carga los archivos del proyecto
		/// </summary>
		internal override void Process()
		{
			bool found = false;

				// Carga los archivos
				LoadRecursive(Processor.Project.File.Path, "");
				// Transforma las referencias
				do
				{ 
					// Indica que no se han encontrado referencias
					found = false;
					// Recorre los archivos modificando las referencias
					for (int index = Data.Files.Count - 1; index >= 0; index--)
						if (Data.Files [index].File.FileType == FileModel.DocumentType.Reference)
						{
							FileTargetModel file;

								// Añade un nuevo archivo
								file = Data.Files.Add(TransformReference(Data.Files [index].File.FullFileName),
																		 Data.Files [index].PathTarget,
																		 Path.GetFileNameWithoutExtension(Data.Files [index].FileNameTarget));
								file.FileNameSource = Path.Combine(Data.Files [index].PathTarget,
																   Path.GetFileNameWithoutExtension(Data.Files [index].FileNameTarget));
								// Elimina el archivo
								Data.Files.RemoveAt(index);
								// Indica que se ha encontrado al menos una referencia
								found = true;
						}
				}
				while (found);
		}

		/// <summary>
		///		Obtiene una instancia de archivo
		/// </summary>
		private void LoadRecursive(string pathSource, string pathTarget)
		{
			string [] files;

				// Obtiene los archivos
				files = Directory.GetFiles(pathSource);
				foreach (string file in files)
					Data.Files.Add(file, pathTarget, Path.GetFileName(file));
				// Obtiene los directorios
				files = Directory.GetDirectories(pathSource);
				foreach (string path in files)
					LoadRecursive(path, Path.Combine(pathTarget, Path.GetFileName(path)));
		}

		/// <summary>
		///		Transforma una referencia
		/// </summary>
		private string TransformReference(string fileName)
		{
			return new ReferenceBussiness().GetFileName(Processor.Solution, Processor.Project, fileName);
		}
	}
}
