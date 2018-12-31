using System;

using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Processor.Pages;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.CategoriesSteps
{
	/// <summary>
	///		Procesador del paso de carga y compilación de etiquetas
	/// </summary>
	internal class StepTagsCompileProcessor : AbstractBaseStepsCategories
	{
		internal StepTagsCompileProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Compila las categorías
		/// </summary>
		internal override void Process()
		{
			base.ProcessCategories(Processor.Project.ItemsPerCategory);
		}

		/// <summary>
		///		Comprueba si un archivo se debe procesar como una categoría para esta paso
		/// </summary>
		protected override bool CheckIsCategory(FileTargetModel file)
		{
			return file.File.FileType == Model.Solutions.FileModel.DocumentType.Tag;
		}

		/// <summary>
		///		Preprocesa el archivo
		/// </summary>
		protected override void Preprocess(FileTargetModel file, DocumentModel category)
		{   
			// Inicializa el nombre de archivo destino con el nombre de la carpeta
			file.FileNameTarget = System.IO.Path.GetFileName(file.PathTarget) + ".htm";
			// Inicializa los datos básicos del documento
			file.Title = category.Title;
			file.FileNameImage = category.URLImageSummary;
			file.FileNameThumbnail = category.URLThumbImageSummary;
			file.ShowMode = category.ModeShow;
			file.IsRecursive = true;
			file.ShowAtRss = false;
			// Llama al método base
			base.Preprocess(file, category);
		}

		/// <summary>
		///		Obtiene los elementos hijo de la categoría
		/// </summary>
		protected override FileTargetModelCollection GetChilds(FileTargetModel file, DocumentModel document)
		{
			return Data.Files.GetChildsTag(file, document);
		}

		/// <summary>
		///		Obtiene la plantilla de los elementos
		/// </summary>
		protected override SectionSourceModel GetTemplateItems(DocumentModel document)
		{
			return Data.Templates.GetTemplate(document, TemplatesArrayModel.TemplateType.CategoryItem);
		}

		/// <summary>
		///		Nombre del documento para una página
		/// </summary>
		protected override string GetDocumentFileName(FileTargetModel category, int pageIndex)
		{
			if (pageIndex == 0)
				return category.DocumentFileName;
			else
				return $"{category.DocumentFileName} ?.?_ {pageIndex}"; // ... evita los errores cuando en los textos ponga Seccion 2 1
		}
	}
}
