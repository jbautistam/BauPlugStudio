using System;

using Bau.Libraries.LibDocWriter.Application.Bussiness.Documents;
using Bau.Libraries.LibDocWriter.Model.Documents;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.PagesSteps
{
	/// <summary>
	///		Procesador del paso de carga de datos básicos de las páginas y precompilación del contenido
	/// </summary>
	internal class StepPagesPrecompileProcessor : AbstractBaseSteps
	{
		internal StepPagesPrecompileProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Carga los documentos, inicializa sus datos básicos y compila en corto
		/// </summary>
		internal override void Process()
		{
			NhamlCompiler.Variables.VariablesCollection variables;

				// Obtiene las variables de proyecto
				variables = Data.NhamlCompiler.GetProjectVariables();
				// Carga las secciones y plantillas
				foreach (Pages.FileTargetModel file in Data.Files)
					if (file.File.FileType == Model.Solutions.FileModel.DocumentType.Document)
					{
						DocumentModel document = new DocumentBussiness().Load(file.File);

							// Inicializa el nombre de archivo destino con el nombre de la carpeta
							// y sube el directorio un nivel
							file.FileNameTarget = System.IO.Path.GetFileName(file.PathTarget) + ".htm";
							if (file.FileNameTarget.EndsWith(Processor.Project.PageMain, StringComparison.CurrentCultureIgnoreCase))
								file.PathTarget = System.IO.Path.GetDirectoryName(file.PathTarget);
							// Inicializa los datos básicos del documento
							file.Title = document.Title;
							file.FileNameImage = document.URLImageSummary;
							file.FileNameThumbnail = document.URLThumbImageSummary;
							file.ShowMode = document.ModeShow;
							file.IsRecursive = document.IsRecursive;
							file.ShowAtRss = document.ShowAtRSS;
							// Compila el documento en corto si no es una categoría
							Compile(document, file, variables);
					}
		}

		/// <summary>
		///		Compila un documento
		/// </summary>
		private void Compile(DocumentModel document, Pages.FileTargetModel file, NhamlCompiler.Variables.VariablesCollection variables)
		{   
			// Compila 
			document.Content = Data.NhamlCompiler.Compile(document, variables, Processor.Project.ParagraphsSummaryNumber);
			// Graba el documento en corto
			try
			{
				string fileName = file.GetFullFileNameCompiledShort(Processor);

					// Crea el directorio
					LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileName));
					// Graba el documento
					new DocumentBussiness().Save(document, fileName);
			}
			catch (Exception exception)
			{
				Processor.Errors.Add(file.GetFullFileName(Processor), $"Error al grabar el archivo {document.File.IDFileName}: {exception.Message}");
			}
		}
	}
}
