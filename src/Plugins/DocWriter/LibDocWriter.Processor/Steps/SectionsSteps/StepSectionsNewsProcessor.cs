using System;

using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Processor.Pages;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.SectionSteps
{
	/// <summary>
	///		Procesador del paso de compilación de las secciones de noticias
	/// </summary>
	internal class StepSectionsNewsProcessor : AbstractBaseSteps
	{
		internal StepSectionsNewsProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Carga los documentos, inicializa sus datos básicos y compila en corto
		/// </summary>
		internal override void Process()
		{
			foreach (SectionSourceModel section in Data.Templates)
				if (section.Source.IDScope == DocumentModel.ScopeType.News)
				{
					FileTargetModelCollection childs = Data.Files.GetChilds(section.Source.ChildPages, true, true);

						// Compila el contenido de la sección
						section.Source.Content = Compile(section, childs);
				}
		}

		/// <summary>
		///		Compila una sección
		/// </summary>
		private string Compile(SectionSourceModel section, FileTargetModelCollection childs)
		{
			SectionSourceModel template = Data.Templates.GetTemplate(section.Source, TemplatesArrayModel.TemplateType.News);
			NhamlCompiler.Variables.VariablesCollection variables = Data.NhamlCompiler.GetVariablesDocumentForCategory(childs);
			string content;

				// Ordena los archivos hijo
				childs.SortByDate(false);
				// Dependiendo de si la sección tiene o no una plantilla
				if (template != null)
				{ 
					// Compila las páginas
					content = Data.NhamlCompiler.Compile(section.FileTarget.File.DocumentFileName,
														 template.Source.Content, variables, 0);
					// Crea las variables para el contenido de la sección
					variables.Clear();
					variables.Add(Compiler.NHamlCompilerWrapper.VariableContent, content);
					// Compila la sección
					content = Data.NhamlCompiler.Compile(section.FileTarget.File.DocumentFileName,
														 section.Source.Content, variables, 0);
				}
				else
					content = Data.NhamlCompiler.Compile(section.Source, variables, 0);
				// Devuelve el valor compilado
				return content;
		}
	}
}
