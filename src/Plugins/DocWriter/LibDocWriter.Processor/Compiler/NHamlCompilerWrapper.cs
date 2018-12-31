using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.NhamlCompiler.Variables;

namespace Bau.Libraries.LibDocWriter.Processor.Compiler
{
	/// <summary>
	///		Wrapper del compilador de Nhaml
	/// </summary>
	internal class NHamlCompilerWrapper
	{ 
		// Constantes internas con las variables de página
		internal const string VariableContent = "$Content";
		internal const string VariableAdditionalContent = "$AdditionalContent";
		internal const string VariableFullTitle = "$FullTitle";
		internal const string VariableTitle = "$Title";
		internal const string VariableDescription = "$Description";
		internal const string VariableKeyWords = "$KeyWords";
		internal const string VariableMainUrlPage = "$MainUrlPage";
		internal const string VariableMainUrlImage = "$MainUrlImage";
		internal const string VariableMainUrlThumb = "$MainUrlThumbnail";
		// Constantes privadas con las variables de la Web
		private const string VariableWebName = "$WebName";
		private const string VariableWebDescription = "$WebDescription";
		// Constantes privadas con las variables de la página
		private const string VariableUrl = "$Url";
		private const string VariableUrlFull = "$UrlFull";
		private const string VariableUrlImage = "$UrlImage";
		private const string VariableUrlImageFull = "$UrlImageFull";
		private const string VariableUrlThumb = "$UrlThumbnail";
		private const string VariableUrlThumbFull = "$UrlThumbnailFull";
		private const string VariableDate = "$Date";
		private const string VariableDay = "$Day";
		private const string VariableMonth = "$Month";
		private const string VariableMonthName = "$MonthName";
		private const string VariableYear = "$Year";
		private const string Tags = "$Tag";
		// Constantes privadas con las variables de página anterior, siguiente y superior
		private const string UrlNextPage = "$UrlNextPage";
		private const string TitleNextPage = "$TitleNextPage";
		private const string UrlPreviousPage = "$UrlPreviousPage";
		private const string TitlePreviousPage = "$TitlePreviousPage";
		private const string UrlTopPage = "$UrlTopPage";
		private const string TitleTopPage = "$TitleTopPage";
		// Constante para las páginas relacionadas
		private const string VariablePageRelated = "$PageRelated";
		// Constante para el breadCrumb
		private const string VariablePageBreadCrumb = "$BreadCrumb";
		// Variables privadas
		private NhamlCompiler.Compiler _compiler;

		internal NHamlCompilerWrapper(Generator processor, Steps.CompilerData compilerData)
		{
			Processor = processor;
			CompilerData = compilerData;
			_compiler = new NhamlCompiler.Compiler();
		}

		/// <summary>
		///		Compila una cadena
		/// </summary>
		internal string Compile(Pages.SectionSourceModel document, VariablesCollection variables, int maxInstructions)
		{
			return Compile(document.Source.File.DocumentFileName, document.Source.Content, variables, maxInstructions);
		}

		/// <summary>
		///		Compila un documento
		/// </summary>
		internal string Compile(Model.Documents.DocumentModel document, VariablesCollection variables, int maxInstructions)
		{
			return Compile(document.File.DocumentFileName, document.Content, variables, maxInstructions);
		}

		/// <summary>
		///		Compila una página
		/// </summary>
		internal string Compile(Model.Documents.DocumentModel document, DateTime createdAt,
														Pages.SectionSourceModel template, Pages.FilesIndexComposition pageIndex)
		{
			VariablesCollection variables = new VariablesCollection();
			string content;

				// Obtiene las variables de proyecto
				variables.AddRange(GetProjectVariables());
				variables.AddRange(GetSectionVariables(Model.Documents.DocumentModel.ScopeType.Web));
				variables.AddRange(GetSectionVariables(Model.Documents.DocumentModel.ScopeType.News));
				variables.AddRange(GetVariablesPage(document, createdAt, null));
				variables.AddRange(GetVariablesIndex(pageIndex));
				// Añade las páginas relacionadas
				variables.AddRange(GetVariablesPagesRelated(pageIndex));
				// Añade las variables de breadCrumb
				variables.AddRange(GetVariablesBreadCrumb(pageIndex));
				// Añade las variables de sección asociadas a la página
				variables.AddRange(GetVariablesSectionPerPage(variables));
				// Compila la página y añade el contenido a la variable
				content = Compile(document.File.DocumentFileName, document.Content, variables, 0);
				variables.Remove(VariableContent);
				variables.Add(VariableContent, content);
				// Compila el contenido del archivo con la plantilla
				if (template != null)
					content = Compile(document.File.DocumentFileName, template.Source.Content, variables, 0);
				else // Añade el error y deja el contenido como estaba
					Processor.Errors.Add(document.File.DocumentFileName, "No se encuentra ninguna plantilla");
				// Devuelve el valor compilado
				return content;
		}

		/// <summary>
		///		Compila una cadena
		/// </summary>
		internal string Compile(string fileName, string content, VariablesCollection variables, int maxInstructions)
		{
			string strParsed = "";

				// Compila el contenido
				if (!content.IsEmpty())
					strParsed = _compiler.Parse(content, variables, maxInstructions, Processor.MustMinimize);
				// Añade los errores
				Processor.Errors.AddRange(fileName, _compiler.LocalErrors);
				// Limpia los errores
				_compiler.LocalErrors.Clear();
				// Devuelve la cadena compilada
				return strParsed;
		}

		/// <summary>
		///		Compila los elementos de una categoría
		/// </summary>
		internal string CompileItemsCategory(Model.Documents.DocumentModel category, Pages.SectionSourceModel template,
											 Pages.FileTargetModelCollection childs,
											 Pages.FilesIndexComposition fileIndex)
		{
			VariablesCollection variables = new VariablesCollection();

				// Obtiene las variables de las páginas
				variables.AddRange(GetVariablesDocumentForCategory(childs));
				// Obtiene las varialbes de proyecto
				variables.AddRange(GetProjectVariables());
				variables.AddRange(GetSectionVariables(Model.Documents.DocumentModel.ScopeType.Web));
				variables.AddRange(GetSectionVariables(Model.Documents.DocumentModel.ScopeType.News));
				variables.AddRange(GetVariablesDocumentForCategory(childs));
				// Obtiene las variables del índice
				variables.AddRange(GetVariablesIndex(fileIndex));
				// Añade las variables de sección para la página
				variables.AddRange(GetVariablesSectionPerPage(variables));
				// Compila el archivo
				return Compile(category.File.DocumentFileName, template.Source.Content, variables, 0);
		}

		/// <summary>
		///		Compila una categoría
		/// </summary>
		internal string CompileCategory(Model.Documents.DocumentModel category, DateTime createdAt,
										string strContentHeader, string strContentItems,
										Pages.SectionSourceModel templateMain)
		{
			VariablesCollection variables = new VariablesCollection();

				// Obtiene las variables de proyecto
				variables.AddRange(GetProjectVariables());
				variables.AddRange(GetSectionVariables(Model.Documents.DocumentModel.ScopeType.Web));
				variables.AddRange(GetSectionVariables(Model.Documents.DocumentModel.ScopeType.News));
				// Obtiene las variables de la página
				variables.AddRange(GetVariablesPage(category, createdAt, null));
				// Quita la variable de contenido
				for (int index = variables.Count - 1; index >= 0; index--)
					if (variables [index].Name.EqualsIgnoreCase(VariableContent))
						variables.RemoveAt(index);
				// Añade las variables de contenido y contenido adicional
				variables.Add(GetVariable(VariableContent, strContentHeader, null));
				variables.Add(GetVariable(VariableAdditionalContent, strContentItems, null));
				// Compila
				return Compile(category.File.FullFileName, templateMain.Source.Content, variables, 0);
		}

		/// <summary>
		///		Obtiene las variables del proyecto
		/// </summary>
		internal VariablesCollection GetProjectVariables()
		{
			VariablesCollection variables = new VariablesCollection();

				// Añade las variables de compilación
				foreach (KeyValuePair<string, string> variable in Processor.Project.ConvertVariables())
					variables.Add(variable.Key, variable.Value);
				// Añade las variables del proyecto
				variables.Add(VariableWebName, Processor.Project.Name);
				variables.Add(VariableWebDescription, Processor.Project.Description);
				// Devuelve la colección de variables
				return variables;
		}

		/// <summary>
		///		Obtiene las variables de documentos para una categoría
		/// </summary>
		internal VariablesCollection GetVariablesDocumentForCategory(Pages.FileTargetModelCollection files)
		{
			List<Pages.DocumentTargetModel> documents = new List<Pages.DocumentTargetModel>();
			Application.Bussiness.Documents.DocumentBussiness bussiness = new Application.Bussiness.Documents.DocumentBussiness();

				// Carga los documentos
				foreach (Pages.FileTargetModel file in files)
				{
					Pages.DocumentTargetModel document = new Pages.DocumentTargetModel();

						// Asigna el archivo destino
						document.FileTarget = file;
						// Carga los datos
						document.Document = bussiness.Load(Processor.Project, file.GetFullFileNameCompiledShort(Processor));
						// Añade el documento a la colección
						documents.Add(document);
				}
				// Obtiene las variables
				return GetVariablesDocumentForCategory(documents);
		}

		/// <summary>
		///		Obtiene las variables de un documento para introducirlas en una categoría
		/// </summary>
		private VariablesCollection GetVariablesDocumentForCategory(List<Pages.DocumentTargetModel> documents)
		{
			VariablesCollection variables = new VariablesCollection();

				// Añade las variables de las páginas para la categoría
				for (int index = 0; index < documents.Count; index++)
					variables.AddRange(GetVariablesPageForCategory(documents [index], index));
				// Devuelve la colección de variables
				return variables;
		}

		/// <summary>
		///		Obtiene las variables de un documento para introducirlas en una categoría
		/// </summary>
		internal VariablesCollection GetVariablesPageForCategory(Pages.DocumentTargetModel document, int? pageIndex)
		{
			VariablesCollection variables = GetVariablesPage(document.Document, document.FileTarget.DateUpdate, pageIndex);

				// Añade las variables de las páginas para la categoría
				if (document != null)
				{ 
					// Añade las variables de la página
					variables.Add(GetVariable(VariableUrl, document.GetUrlSource(), pageIndex));
					variables.Add(GetVariable(VariableUrlFull, document.GetInternetUrl(Processor.Project), pageIndex));
					variables.Add(GetVariable(VariableUrlImageFull, document.GetFullUrlImage(Processor.Project), pageIndex));
					// Añade la variable con el contenido de la página
					variables.Add(GetVariable(VariableContent, document.Document.Content, pageIndex));
				}
				// Devuelve la colección de variables
				return variables;
		}

		/// <summary>
		///		Obtiene las variables de un documento para introducirlas en una categoría
		/// </summary>
		internal VariablesCollection GetVariablesPage(Model.Documents.DocumentModel document, DateTime createdAt, int? pageIndex)
		{
			VariablesCollection variables = new VariablesCollection();

				// Añade las variables de las páginas para la categoría
				if (document != null)
				{ 
					// Añade las variables de la página
					variables.Add(GetVariable(VariableTitle, document.Title, pageIndex));
					variables.Add(GetVariable(VariableKeyWords, document.KeyWords, pageIndex));
					variables.Add(GetVariable(VariableDescription, document.Description, pageIndex));
					variables.Add(GetVariable(VariableUrlImage, document.URLImageSummary, pageIndex));
					if (!document.URLImageSummary.IsEmpty() &&
							System.IO.File.Exists(System.IO.Path.Combine(Processor.Project.File.Path,
												  document.URLThumbImageSummary)))
						variables.Add(GetVariable(VariableUrlThumb, document.URLThumbImageSummary, pageIndex));
					else
						variables.Add(GetVariable(VariableUrlThumb, "", pageIndex));
					variables.Add(GetVariable(VariableDate, string.Format("{0:dd-MM-yyyy}", createdAt), pageIndex));
					variables.Add(GetVariable(VariableDay, createdAt.Day.ToString(), pageIndex));
					variables.Add(GetVariable(VariableMonth, createdAt.Month.ToString(), pageIndex));
					variables.Add(GetVariable(VariableMonthName,
											  System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(createdAt.Month).ToUpperFirstLetter(),
											  pageIndex));
					variables.Add(GetVariable(VariableYear, createdAt.Year.ToString(), pageIndex));
					// Añade las variables con el nombre completo de la página
					if (Processor.Project.AddWebTitle && !Processor.Project.Title.IsEmpty())
						variables.Add(GetVariable(VariableFullTitle,
												  $"{document.Title} - {Processor.Project.Title}",
												  pageIndex));
					else
						variables.Add(GetVariable(VariableFullTitle, document.Title, pageIndex));
					// Añade las variables de las etiquetas
					variables.AddRange(GetVariablesTag(document, pageIndex));
				}
				// Devuelve la colección de variables
				return variables;
		}

		/// <summary>
		///		Obtiene las variables de etiqueta
		/// </summary>
		private VariablesCollection GetVariablesTag(Model.Documents.DocumentModel document, int? pageIndex)
		{
			VariablesCollection variables = new VariablesCollection();
			int tagIndex = 0;

				// Añade las variables de la etiqueta
				if (pageIndex == null)
					foreach (Model.Solutions.FileModel tag in document.Tags)
					{
						Variable variable = GetVariable(Tags, "", tagIndex);

							// Añade los miembros de la variable
							variable.Members.Add(VariableTitle, tag.Title);
							variable.Members.Add(VariableUrl, tag.IDFileName);
							// Añade la variable a la colección
							variables.Add(variable);
							// Incrementa el índice de la etiqueta
							tagIndex++;
					}
				else
				{
					Variable variable = GetVariable(Tags, "", pageIndex);

						// Añade los miembros de la etiqueta
						foreach (Model.Solutions.FileModel tag in document.Tags)
						{ 
							// Añade los miembros de la variable
							variable.Members.Add(VariableTitle, tag.Title, tagIndex + 1);
							variable.Members.Add(VariableUrl, tag.IDFileName, tagIndex + 1);
							// Incrementa el índice de la etiqueta
							tagIndex++;
						}
						// Añade la variable a la colección
						variables.Add(variable);
				}
				// Devuelve la colección de variables
				return variables;
		}

		/// <summary>
		///		Obtiene las variables de página superior, anterior y siguiente
		/// </summary>
		private VariablesCollection GetVariablesIndex(Pages.FilesIndexComposition filesIndex)
		{
			VariablesCollection variables = new VariablesCollection();

				// Crea un objeto de índice vacío si no existía ninguno
				if (filesIndex == null)
					filesIndex = new Pages.FilesIndexComposition();
				// Añade las variables de índice
				AddVariablesPageIndex(variables, TitleTopPage, UrlTopPage, filesIndex.PageTop);
				AddVariablesPageIndex(variables, TitlePreviousPage, UrlPreviousPage, filesIndex.PagePrevious);
				AddVariablesPageIndex(variables, TitleNextPage, UrlNextPage, filesIndex.PageNext);
				// Devuelve la colección de variables
				return variables;
		}

		/// <summary>
		///		Añade las variables índice de la página (siguiente, anterior o superior)
		/// </summary>
		private void AddVariablesPageIndex(VariablesCollection variables, string nameVariableTitle, string nameVariableUrl,
										   Pages.FileTargetModel page)
		{
			if (page != null && !page.DocumentFileName.IsEmpty())
			{
				variables.Add(nameVariableTitle, page.Title);
				variables.Add(nameVariableUrl, page.DocumentFileName);
			}
			else
			{
				variables.Add(nameVariableTitle, "null");
				variables.Add(nameVariableUrl, "null");
			}
		}

		/// <summary>
		///		Obtiene las variables asociadas a las secciones
		/// </summary>
		internal VariablesCollection GetSectionVariables(Model.Documents.DocumentModel.ScopeType idScope)
		{
			VariablesCollection variables = new VariablesCollection();

				// Asigna las variables
				foreach (Pages.SectionSourceModel source in CompilerData.Templates)
					if (source.Source.File.FileType == Model.Solutions.FileModel.DocumentType.Section &&
							source.Source.IDScope == idScope)
						variables.Add(source.GetNameSection(), source.Source.Content);
				// Devuelve la colección de variables
				return variables;
		}

		/// <summary>
		///		Obtiene las variables asociadas a secciones de página
		/// </summary>
		private VariablesCollection GetVariablesSectionPerPage(VariablesCollection variablesPage)
		{
			VariablesCollection variables = new VariablesCollection();

				// Asigna las variables
				foreach (Pages.SectionSourceModel source in CompilerData.Templates)
					if (source.Source.File.FileType == Model.Solutions.FileModel.DocumentType.Section &&
							source.Source.IDScope == Model.Documents.DocumentModel.ScopeType.Page)
						variables.Add(source.GetNameSection(),
									  Compile(source.FileTarget.File.DocumentFileName, source.Source.Content, variablesPage, 0));
				// Devuelve la colección de variables
				return variables;
		}

		/// <summary>
		///		Obtiene las variables necesarias para crear el BreadCrumb
		/// </summary>
		private VariablesCollection GetVariablesBreadCrumb(Pages.FilesIndexComposition fileIndex)
		{
			VariablesCollection variables = new VariablesCollection();

				// Añade las variables 
				if (fileIndex != null && fileIndex.FilesBreadCrumb != null)
					for (int pageIndex = 0; pageIndex < fileIndex.FilesBreadCrumb.Count; pageIndex++)
					{
						Variable variable = new Variable(VariablePageBreadCrumb, ValueBase.GetInstance(""), pageIndex + 1);

							// Añade los miembros
							variable.Members.Add(VariableTitle, ValueBase.GetInstance(fileIndex.FilesBreadCrumb [pageIndex].Title));
							variable.Members.Add(VariableUrl, ValueBase.GetInstance(fileIndex.FilesBreadCrumb [pageIndex].DocumentFileName)); 
							// Añade la variable a la colección
							variables.Add(variable);
					}
				// Devuelve la colección de variables
				return variables;
		}

		/// <summary>
		///		Obtiene las variables con las páginas relacionadas
		/// </summary>
		private VariablesCollection GetVariablesPagesRelated(Pages.FilesIndexComposition fileIndex)
		{
			VariablesCollection variables = new VariablesCollection();

				// Recorre la colección de páginas relacionadas
				if (fileIndex != null && fileIndex.PagesRelated != null)
					for (int pageIndex = 0; pageIndex < fileIndex.PagesRelated.Count; pageIndex++)
					{
						Pages.DocumentTargetModel page = fileIndex.PagesRelated [pageIndex];
						Variable variable = new Variable(VariablePageRelated, ValueBase.GetInstance(""), pageIndex + 1);

							// Añade los miembros
							variable.Members.Add(VariableTitle, ValueBase.GetInstance(page.Document.Title));
							variable.Members.Add(VariableUrl, ValueBase.GetInstance(page.GetUrlSource()));
							variable.Members.Add(VariableUrlImage, ValueBase.GetInstance(page.Document.URLImageSummary));
							variable.Members.Add(VariableUrlThumb, ValueBase.GetInstance(page.Document.URLThumbImageSummary));
							// Añade la variable a la colección
							variables.Add(variable);
					}
				// Devuelve la colección de variables
				return variables;
		}

		/// <summary>
		///		Obtiene una variable (indexada o no)
		/// </summary>
		private Variable GetVariable(string name, string value, int? index)
		{
			if (index == null)
				return new Variable(name, ValueBase.GetInstance(value));
			else
				return new Variable(name, ValueBase.GetInstance(value), (index ?? 0) + 1);
		}

		/// <summary>
		///		Procesador
		/// </summary>
		private Generator Processor { get; set; }

		/// <summary>
		///		Datos de compilación
		/// </summary>
		private Steps.CompilerData CompilerData { get; set; }
	}
}
