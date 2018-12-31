using System;

using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Processor.Pages;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.SectionSteps
{
	/// <summary>
	///		Procesador del paso de compilación de las secciones Web
	/// </summary>
	internal class StepSectionsWebProcessor : AbstractBaseSteps
	{   
		// Constantes privadas
		private const string VariablePrefix = "~~¬¬";

		internal StepSectionsWebProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Compila la secciones Web y las plantillas
		/// </summary>
		internal override void Process()
		{ 
			// Compila las secciones Web
			CompileSectionsWeb();
			// Compila las plantillas
			CompileTemplates();
		}

		/// <summary>
		///		Compila las secciones Web
		/// </summary>
		private void CompileSectionsWeb()
		{
			NhamlCompiler.Variables.VariablesCollection variables = new NhamlCompiler.Variables.VariablesCollection();

				// Obtiene las variables de proyecto
				variables.AddRange(Data.NhamlCompiler.GetProjectVariables());
				// Añade las variables de secciones de noticias
				variables.AddRange(Data.NhamlCompiler.GetSectionVariables(DocumentModel.ScopeType.News));
				// Añade como variables los nombres de sección (puesto que al no estar compiladas aún, no tendremos el contenido y la
				// variable está a null)
				foreach (SectionSourceModel section in Data.Templates)
					if (section.Source.IDScope == DocumentModel.ScopeType.Web)
						variables.Add(section.GetNameSection(), VariablePrefix + section.GetNameSection() + VariablePrefix);
				// Compila las secciones Web
				foreach (SectionSourceModel section in Data.Templates)
					if (section.Source.IDScope == DocumentModel.ScopeType.Web)
						section.Source.Content = Data.NhamlCompiler.Compile(section, variables, 0);
				// Reemplaza en el contenido compilado las variables falsas anteriores
				foreach (SectionSourceModel section in Data.Templates)
					if (section.Source.IDScope == DocumentModel.ScopeType.Web)
						foreach (SectionSourceModel sectionFake in Data.Templates)
							if (sectionFake.Source.IDScope == DocumentModel.ScopeType.Web)
								section.Source.Content = section.Source.Content.Replace(VariablePrefix + sectionFake.GetNameSection() + VariablePrefix,
																						sectionFake.Source.Content);
		}

		/// <summary>
		///		Compila las plantillas
		/// </summary>
		private void CompileTemplates()
		{
			NhamlCompiler.Variables.VariablesCollection variables = new NhamlCompiler.Variables.VariablesCollection();

				// Obtiene las variables de proyecto
				variables.AddRange(Data.NhamlCompiler.GetProjectVariables());
				// Añade las variables de secciones de noticias y Web
				variables.AddRange(Data.NhamlCompiler.GetSectionVariables(DocumentModel.ScopeType.News));
				variables.AddRange(Data.NhamlCompiler.GetSectionVariables(DocumentModel.ScopeType.Web));
				// Añade las variables con los datos explícitos de la página
				variables.Add(Compiler.NHamlCompilerWrapper.VariableMainUrlPage, SectionSourceModel.VariableMainUrlPage);
				variables.Add(Compiler.NHamlCompilerWrapper.VariableMainUrlImage, SectionSourceModel.VariableMainUrlImage);
				variables.Add(Compiler.NHamlCompilerWrapper.VariableContent, SectionSourceModel.VariableContent);
				variables.Add(Compiler.NHamlCompilerWrapper.VariableAdditionalContent, SectionSourceModel.VariableAdditionalContent);
				variables.Add(Compiler.NHamlCompilerWrapper.VariableTitle, SectionSourceModel.VariableTitle);
				variables.Add(Compiler.NHamlCompilerWrapper.VariableFullTitle, SectionSourceModel.VariableFullTitle);
				variables.Add(Compiler.NHamlCompilerWrapper.VariableKeyWords, SectionSourceModel.VariableKeywords);
				variables.Add(Compiler.NHamlCompilerWrapper.VariableDescription, SectionSourceModel.VariableDescription);
				// Compila las secciones Web
				foreach (SectionSourceModel template in Data.Templates)
					if (template.FileTarget.File.FileType == Model.Solutions.FileModel.DocumentType.Template)
						template.ContentCompiled = Data.NhamlCompiler.Compile(template, variables, 0);
		}
	}
}
