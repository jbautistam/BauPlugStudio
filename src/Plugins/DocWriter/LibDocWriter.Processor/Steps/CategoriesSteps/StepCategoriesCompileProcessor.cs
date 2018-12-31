using System;

using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Processor.Pages;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.CategoriesSteps
{
	/// <summary>
	///		Procesador del paso de carga y compilación de categorías
	/// </summary>
	internal class StepCategoriesCompileProcessor : AbstractBaseStepsCategories
	{
		internal StepCategoriesCompileProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

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
			return file.File.FileType == Model.Solutions.FileModel.DocumentType.Document &&
				   file.ShowMode != DocumentModel.ShowChildsMode.None;
		}

		/// <summary>
		///		Obtiene los elementos hijo de la categoría
		/// </summary>
		protected override FileTargetModelCollection GetChilds(FileTargetModel file, DocumentModel document)
		{
			return Data.Files.GetChildsCategory(file, file.IsRecursive);
		}

		/// <summary>
		///		Obtiene la plantilla principal
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
