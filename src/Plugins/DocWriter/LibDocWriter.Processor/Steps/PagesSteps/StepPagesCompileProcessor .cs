using System;

using Bau.Libraries.LibDocWriter.Application.Bussiness.Documents;
using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Processor.Pages;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.PagesSteps
{
	/// <summary>
	///		Procesador del paso de carga compilación del contenido de las páginas
	/// </summary>
	internal class StepPagesCompileProcessor : AbstractBaseSteps
	{
		internal StepPagesCompileProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Compila los documentos y lo guarda en temporal
		/// </summary>
		internal override void Process()
		{
			foreach (FileTargetModel file in Data.Files)
				if (file.File.FileType == Model.Solutions.FileModel.DocumentType.Document)
				{
					DocumentModel document = new DocumentBussiness().Load(file.File);

						// Compila los documentos que no sean de categoría y lo graban en el archivo temporal
						if (document.ModeShow == DocumentModel.ShowChildsMode.None)
							try
							{   
								// Compila y graba el documento
								LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.GetDirectoryName(file.GetFullFileNameCompiledTemporal(Processor)));
								LibCommonHelper.Files.HelperFiles.SaveTextFile(file.GetFullFileNameCompiledTemporal(Processor),
																		 Compile(file, document));
							}
							catch (Exception exception)
							{
								Processor.Errors.Add(file.GetFullFileName(Processor),
													 $"Error al compilar: {file.GetFullFileNameCompiledTemporal(Processor)}. Excepción: {exception.Message}");
							}
				}
		}

		/// <summary>
		///		Compila la página
		/// </summary>
		private string Compile(FileTargetModel file, DocumentModel document)
		{
			SectionSourceModel templateArticle = Data.Templates.GetTemplate(document, TemplatesArrayModel.TemplateType.Article);
			SectionSourceModel templateMain = Data.Templates.GetTemplate(document, TemplatesArrayModel.TemplateType.Main);
			string content, fullTitle = document.Title;

				// Obtiene el título completo de la página
				if (Processor.Project.AddWebTitle)
					fullTitle += $" - {Processor.Project.Title}";
				// Compila la página
				content = Data.NhamlCompiler.Compile(document, file.DateUpdate, templateArticle,
													 GetIndexPages(file, document));
				// Compila el contenido con la plantilla principal
				content = templateMain.ReplaceVariablesPageTemplate(content, null, document.Title, document.Title,
																	document.Description, document.KeyWords);
				// Devuelve el valor compilando
				return content;
		}

		/// <summary>
		///		Obtiene las páginas anterior y posterior
		/// </summary>
		private FilesIndexComposition GetIndexPages(FileTargetModel file, DocumentModel document)
		{
			FilesIndexComposition index = new FilesIndexComposition();
			FileTargetModelCollection siblings = Data.Files.GetSiblings(file, false);

				// Asigna los datos
				index.PageTop = Data.Files.GetParent(file);
				index.PagePrevious = siblings.GetPreviousPage(file);
				index.PageNext = siblings.GetNextPage(file);
				index.PagesRelated = GetPagesRelated(file, document);
				index.FilesBreadCrumb = Data.Files.GetParents(file);
				index.Normalize();
				// Devuelve el índice
				return index;
		}

		/// <summary>
		///		Obtiene las páginas relacionadas
		/// </summary>
		private System.Collections.Generic.List<DocumentTargetModel> GetPagesRelated(FileTargetModel file, DocumentModel document)
		{
			System.Collections.Generic.List<DocumentTargetModel> documents = new System.Collections.Generic.List<DocumentTargetModel>();
			FileTargetModelCollection filesRelated = Data.Files.GetPagesRelated(file, document);

				// Carga los archivos y rellena la lista
				foreach (FileTargetModel fileRelated in filesRelated)
					documents.Add(new DocumentTargetModel
											{
												FileTarget = fileRelated,
												Document = new DocumentBussiness().Load(Processor.Project,
																						fileRelated.GetFullFileNameCompiledShort(Processor))
											});
				// Devuelve la colección de archivos
				return documents;
		}
	}
}
