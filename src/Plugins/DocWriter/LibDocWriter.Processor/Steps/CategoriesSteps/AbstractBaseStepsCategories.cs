using System;

using Bau.Libraries.LibDocWriter.Application.Bussiness.Documents;
using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Processor.Pages;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.CategoriesSteps
{
	/// <summary>
	///		Clase base para la compilación de categorías, etiquetas, etc...
	/// </summary>
	internal abstract class AbstractBaseStepsCategories : AbstractBaseSteps
	{
		internal AbstractBaseStepsCategories(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Procesa las categorías
		/// </summary>
		protected void ProcessCategories(int intItemsPerPage)
		{ 
			// Ordena los elementos por directorio / nombre
			Data.Files.SortByFullFileName(true);
			// Recorre los elementos al revés, comenzando por el final para compilar primero las categorías internas
			for (int index = Data.Files.Count - 1; index >= 0; index--)
			{
				FileTargetModel file = Data.Files [index];

					if (CheckIsCategory(file))
					{
						DocumentModel category = new DocumentBussiness().Load(file.File);

							// Preprocesa el archivo si es necesario
							Preprocess(file, category);
							// Compila las páginas y las añade a la colección
							Data.Files.AddRange(Compile(file, category, GetChilds(file, category), intItemsPerPage));
							// Elimina la categoría
							Data.Files.RemoveAt(index);
				}
			}
		}

		/// <summary>
		///		Preprocesa el archivo si es necesario (hace los cambios para el directorio destino y demás...
		/// </summary>
		protected virtual void Preprocess(FileTargetModel file, DocumentModel category) { }

		/// <summary>
		///		Comprueba si un archivo se debe procesar como una categoría para esta paso
		/// </summary>
		protected abstract bool CheckIsCategory(FileTargetModel file);

		/// <summary>
		///		Obtiene los archivos hijo de una categoría
		/// </summary>
		protected abstract FileTargetModelCollection GetChilds(FileTargetModel file, DocumentModel document);

		/// <summary>
		///		Crea las páginas de una categoría
		/// </summary>
		private FileTargetModelCollection Compile(FileTargetModel fileCategory, DocumentModel category,
												  FileTargetModelCollection childs, int intItemsPerPage)
		{
			FileTargetModelCollection filesCompiled = new FileTargetModelCollection(Processor);
			SectionSourceModel templateMain = Data.Templates.GetTemplate(category, TemplatesArrayModel.TemplateType.Main);
			SectionSourceModel templateHeader = Data.Templates.GetTemplate(category, TemplatesArrayModel.TemplateType.CategoryHeader);

				// Si no hay ninguna plantilla añade el error
				if (templateHeader == null)
					Processor.Errors.Add(fileCategory.File.DocumentFileName, "No se encuentra ninguna plantilla para la cabecera de la categoría");
				else
				{
					SectionSourceModel templateItems = GetTemplateItems(category);

					if (templateItems == null)
						Processor.Errors.Add(fileCategory.File.DocumentFileName, "No se encuentra ninguna plantilla para los elementos de la categoría");
					else
					{
						string contentHeader = Data.NhamlCompiler.Compile(category, fileCategory.DateUpdate, templateHeader, null);
						int pageIndex = 0;
						int numberPages = CountPages(childs.Count, intItemsPerPage);

						if (childs.Count == 0)
						{
							string content;

								// Compila con la cabecera de la plantilla
								content = templateMain.ReplaceVariablesPageTemplate(contentHeader, null,
																					category.Title, category.Title,
																					category.Description, category.KeyWords);

								// Crea el archivo
								filesCompiled.Add(CreateFile(fileCategory, 0));
								// Graba en el archivo compilado el contenido de la cabecera
								Save(filesCompiled [filesCompiled.Count - 1], content);
						}
						else
						{ 
							// Ordena los archivos
							if (fileCategory.ShowMode == DocumentModel.ShowChildsMode.SortByDate)
								childs.SortByDate(false);
							else
								childs.SortByName(true);
							// Compila el contenido 
							for (int top = 0; top < childs.Count; top += intItemsPerPage, pageIndex++)
							{
								string content;
								FilesIndexComposition fileIndex = new FilesIndexComposition();

									// Obtiene las páginas de índice
									fileIndex.PageTop = Data.Files.GetParent(fileCategory);
									fileIndex.PagePrevious = GetPage(fileCategory, pageIndex - 1, numberPages);
									fileIndex.PageNext = GetPage(fileCategory, pageIndex + 1, numberPages);
									// Normaliza el índice
									fileIndex.Normalize();
									// Compila los elementos de la categoría
									content = Data.NhamlCompiler.CompileItemsCategory(category, templateItems,
																					  childs.Slice(top, intItemsPerPage),
																					  fileIndex);
									// Compila con la cabecera de la plantilla
									content = templateMain.ReplaceVariablesPageTemplate(contentHeader, content, category.Title, category.Title,
																						category.Description, category.KeyWords);
									// Crea el archivo
									filesCompiled.Add(CreateFile(fileCategory, pageIndex));
									// Graba el archivo compilado
									Save(filesCompiled [filesCompiled.Count - 1], content);
							}
						}
					}
				}
				// Devuelve la colección de páginas
				return filesCompiled;
		}

		/// <summary>
		///		Cuenta el número de páginas
		/// </summary>
		private int CountPages(int intItems, int intItemsPerPage)
		{
			int pageIndex = 0;

				// Cuenta el número de página
				while (intItems > 0)
				{
					pageIndex++;
					intItems -= intItemsPerPage;
				}
				// Devuelve el número de páginas
				return pageIndex;
		}

		/// <summary>
		///		Obtiene la plantilla de los elementos
		/// </summary>
		protected abstract SectionSourceModel GetTemplateItems(DocumentModel document);

		/// <summary>
		///		Obtiene un FileTargetModel "falso" para indicar la página siguiente o anterior
		/// </summary>
		private FileTargetModel GetPage(FileTargetModel fileCategory, int pageIndex, int totalPages)
		{
			if (pageIndex < 0 || pageIndex > totalPages - 1)
				return null;
			else
				return CreateFile(fileCategory, pageIndex);
		}

		/// <summary>
		///		Crea un archivo para la categoría
		/// </summary>
		private FileTargetModel CreateFile(FileTargetModel fileCategory, int pageIndex)
		{
			FileTargetModel file = fileCategory.Clone();

				// Clona los datos básicos
				file.ShowMode = DocumentModel.ShowChildsMode.SortByDate;
				file.DateUpdate = fileCategory.DateUpdate.AddMinutes(pageIndex);
				file.FileNameImage = fileCategory.FileNameImage;
				file.FileNameThumbnail = fileCategory.FileNameThumbnail;
				file.PreviousTitle = fileCategory.Title;
				file.Title = $"Página {pageIndex + 1}";
				file.Page = pageIndex;
				// Cambia el nombre del archivo destino
				if (pageIndex > 0)
					file.FileNameTarget = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(file.FileNameTarget),
																 System.IO.Path.GetFileNameWithoutExtension(file.FileNameTarget) + " " + pageIndex +
																		System.IO.Path.GetExtension(file.FileNameTarget));
				file.FileNameSource = GetDocumentFileName(fileCategory, pageIndex);
				// Devuelve el archivo creado
				return file;
		}

		/// <summary>
		///		Nombre del documento para una página
		/// </summary>
		protected abstract string GetDocumentFileName(FileTargetModel category, int pageIndex);

		/// <summary>
		///		Graba el archivo temporal
		/// </summary>
		private void Save(FileTargetModel file, string content)
		{
			string fileName = file.GetFullFileNameCompiledTemporal(Processor);

				// Crea el directorio
				LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileName));
				// Graba el archivo
				LibCommonHelper.Files.HelperFiles.SaveTextFile(fileName, content);
		}
	}
}
