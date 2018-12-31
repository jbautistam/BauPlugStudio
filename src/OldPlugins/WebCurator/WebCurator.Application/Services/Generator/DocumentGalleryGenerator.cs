using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.LibDocWriter.Application.Bussiness.Documents;
using Bau.Libraries.LibDocWriter.Application.Factory;
using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.WebCurator.Application.Services.Generator
{
	/// <summary>
	///		Generador de documentos a partir de una galería de imágenes
	/// </summary>
	internal class DocumentGalleryGenerator
	{ 
		// Constantes privadas
		internal const string ExtensionUrl = ".urll"; // ... la extensión no es .url porque Windows transforma los archivos .url en accesos directos a archivos
		private const string StartTag = "<!--Start index-->";
		// Variables privadas
		private Random _rnd = new Random();

		internal DocumentGalleryGenerator(ProjectCompiler compiler, Model.WebSites.GenerationResultProjectModel result)
		{
			Compiler = compiler;
			Result = result;
		}

		/// <summary>
		///		Genera los documentos de un proyecto
		/// </summary>
		internal void Generate(Model.WebSites.ProjectTargetModel projectTarget, int intNumberDocuments)
		{
			DocWriterFactory writerFactory = new DocWriterFactory(Path.GetDirectoryName(projectTarget.ProjectFileName));
			List<string> sources = GetFilesNotUsed(Compiler.FilesImagesSources, Result.ImagesSource);

				// Crea los documentos necesarios
				if (File.Exists(projectTarget.ProjectFileName) && sources.Count > 0)
				{ 
					// Asigna el proyecto al factory
					writerFactory.Project = CreateProject(writerFactory, projectTarget.ProjectFileName);
					// Obtiene los documentos a partir de imágenes
					for (int index = 0; index < intNumberDocuments; index++)
					{
						string imageSource = SearchSource(sources, Result.ImagesSource);

							if (!imageSource.IsEmpty())
							{
								string pathourceImage = GetPathBaseImage(imageSource);

									if (!pathourceImage.IsEmpty())
									{
										string category = GetPathCategory(pathourceImage, imageSource);

											if (!category.IsEmpty())
											{
												// Obtiene las sentencias
												ComputeSentencesPage(imageSource, category, Model.Sentences.FileSentencesModel.PageType.Page,
																	 out string fileNameTarget, out string title, out string description,
																	 out string keyWords, out string body);
												// Crea la página
												CreatePage(projectTarget.ProjectFileName, writerFactory, category, imageSource,
														   fileNameTarget, title, description, keyWords, body);
												// Añade el archivo de imagen a los resultados
												Result.ImagesSource.Add(imageSource);
											}
									}
						}
					}
					// Crea los archivos adicionales
					CompileIndexFiles(projectTarget, writerFactory.Project);
				}
		}

		/// <summary>
		///		Obtiene los archivos no utilizados 
		/// </summary>
		private List<string> GetFilesNotUsed(List<string> sourceFiles, List<string> usedFiles)
		{
			List<string> targets = new List<string>();

				// Recorre los archivos origen y comprueba que no se hayan utilizado ya
				foreach (string source in sourceFiles)
				{
					bool found = false;

						// Comprueba si ya se ha utilizado
						foreach (string used in usedFiles)
							if (source.EqualsIgnoreCase(used) || (source + ExtensionUrl).EqualsIgnoreCase(used) ||
									source.EqualsIgnoreCase(used + ExtensionUrl))
								found = true;
						// Si no se ha utilizado, se añade
						if (!found)
							targets.Add(source);
				}
				// Devuelve la colección de archivos no utilizados
				return targets;
		}

		/// <summary>
		///		Calcula las sentencias de una página
		/// </summary>
		private void ComputeSentencesPage(string fileName, string category, Model.Sentences.FileSentencesModel.PageType pageIndexType,
										  out string fileNameTarget, out string title,
										  out string description, out string keyWords, out string body)
		{ 
			// Añade las variables al compilador
			Compiler.SentencesGenerator.ClearVariables();
			Compiler.SentencesGenerator.AddVariable("@File", GetVariableFileName(fileName));
			Compiler.SentencesGenerator.AddVariable("@Category", category);
			// Obtiene el título (que también será el nombre) del documento
			title = Compiler.SentencesGenerator.Compute(pageIndexType, FilesSentencesGenerator.SentenceType.Title).TrimIgnoreNull();
			fileNameTarget = GetDocumentFileName(title);
			// Crea la página
			description = Compiler.SentencesGenerator.Compute(pageIndexType, FilesSentencesGenerator.SentenceType.Description).TrimIgnoreNull();
			keyWords = Compiler.SentencesGenerator.Compute(pageIndexType, FilesSentencesGenerator.SentenceType.KeyWords).TrimIgnoreNull();
			body = Compiler.SentencesGenerator.Compute(pageIndexType, FilesSentencesGenerator.SentenceType.Body).TrimIgnoreNull();
		}

		/// <summary>
		///		Crea o carga un proyecto
		/// </summary>
		private ProjectModel CreateProject(DocWriterFactory writerFactory, string projectTarget)
		{
			ProjectModel project;

				// Carga o crea un proyecto
				if (File.Exists(projectTarget))
					project = writerFactory.Load(projectTarget);
				else
				{ 
					// Crea el proyecto
					project = writerFactory.CreateProject();
					// Graba el proyecto
					new LibDocWriter.Application.Bussiness.Solutions.ProjectBussiness().Save(project);
				}
				// Devuelve el proyecto creado
				return project;
		}

		/// <summary>
		///		Obtiene el nombre del archivo a partir del título quitando todas
		///	aquellas palabras con menos de tres caracteres
		/// </summary>
		private string GetDocumentFileName(string title)
		{
			string fileName = "";
			string[] titleParts = title.Split(' ');

				// Añade al nombre del archivo todas las palabras con más de tres caracteres
				for (int index = 0; index < titleParts.Length; index++)
					if (titleParts[index].Length > 3 || index == titleParts.Length - 1)
						fileName = fileName.AddWithSeparator(titleParts[index], " ", false);
				// Normaliza el nombre de archivo
				fileName = HelperFiles.Normalize(fileName, false);
				// Devuelve el nombre del archivo
				return fileName.TrimIgnoreNull();
		}

		/// <summary>
		///		Crea una página
		/// </summary>
		private void CreatePage(string projectTarget, DocWriterFactory writerFactory,
								string category, string imageSource, string name, string title,
								string description, string keyWords, string content)
		{
			string pathDocument = Path.Combine(Path.GetDirectoryName(projectTarget), category);
			DocumentModel document = writerFactory.CreatePage(GetDocumentCategory(writerFactory,
																				  Path.GetDirectoryName(projectTarget),
																				  category),
															  name, title, description, keyWords, content,
															  DocumentModel.ShowChildsMode.None, false, true);

				// Crea el directorio
				HelperFiles.MakePath(pathDocument);
				// Crea la imagen
				if (imageSource.EndsWith(ExtensionUrl, StringComparison.CurrentCultureIgnoreCase))
					document.URLImageSummary = LoadNameImageUrl(imageSource);
				else
				{
					string pathImage = Path.Combine(pathDocument, name);
					string fileImageTarget = Path.Combine(pathImage, Path.GetFileName(imageSource));

						// Crea el directorio
						HelperFiles.MakePath(pathImage);
						// Crea la imagen
						writerFactory.ConvertImage(imageSource, fileImageTarget, Compiler.Project.MaxImageWidth, Compiler.Project.ThumbWidth);
						// Asigna la URL de la imagen al documento
						document.URLImageSummary = fileImageTarget.Substring(writerFactory.Project.File.Path.Length + 1);
						// Transforma la imagen en un archivo de Url
						ConvertImageUrlFile(writerFactory.Project, imageSource, document.URLImageSummary);
				}
				// Graba el documento	
				new DocumentBussiness().Save(document);
		}

		/// <summary>
		///		Carga el nombre de un archivo de Url
		/// </summary>
		private string LoadNameImageUrl(string imageSource)
		{
			string urlImage = "";

				// Carga el texto de la imagen
				try
				{
					urlImage = HelperFiles.LoadTextFile(imageSource);
				}
				catch { }
				// Devuelve el texto con la Url de la imagen
				return urlImage;
		}

		/// <summary>
		///		Convierte la Url de la imagen
		/// </summary>
		private void ConvertImageUrlFile(ProjectModel project, string imageSource, string urlImage)
		{
			if (!project.URLBase.IsEmpty())
			{
				string url = project.URLBase;

					// Crea la URL
					url = Path.Combine(url, urlImage).Replace("\\", "/");
					// Crea el archivo de texto
					HelperFiles.SaveTextFile(imageSource + ExtensionUrl, url);
					// Borra el archivo original
					HelperFiles.KillFile(imageSource);
			}
		}

		/// <summary>
		///		Crea un documento de categoría si no existía
		/// </summary>
		private DocumentModel GetDocumentCategory(DocWriterFactory writerFactory, string pathBase, string category)
		{
			string pathCategory = Path.Combine(pathBase, category);
			string fileNameCategory = Path.Combine(pathCategory, "Document.dcx");
			DocumentModel categoryDocument;

				// Obtiene el documento de la categoría
				if (!Directory.Exists(pathCategory) || !File.Exists(fileNameCategory))
				{
					// Obtiene las sentencias
					ComputeSentencesPage(category, category, Model.Sentences.FileSentencesModel.PageType.Category,
															 out string fileNameTarget, out string title, out string description,
															 out string keyWords, out string body);
					// Crea el documento de categoría
					categoryDocument = writerFactory.CreateCategory(null, category, title,
																	description, keyWords, body,
																	DocumentModel.ShowChildsMode.SortByDate,
																	true, false);
					// Graba el documento de categoría
					new DocumentBussiness().Save(categoryDocument);
				}
				else
					categoryDocument = new DocumentBussiness().Load(new FileModel(writerFactory.Project, fileNameCategory));
				// Devuelve el documento de categoría
				return categoryDocument;
		}

		/// <summary>
		///		Obtiene el directorio base de una imagen
		/// </summary>
		private string GetPathBaseImage(string imageSource)
		{ 
			// Busca el directorio base de la imagen
			if (!imageSource.IsEmpty())
				foreach (string pathource in Compiler.Project.PathImagesSources)
					if (imageSource.StartsWith(pathource, StringComparison.CurrentCultureIgnoreCase))
						return pathource;
			// Si ha llegado hasta aquí es porque no ha encontrado el directorio origen
			return null;
		}

		/// <summary>
		///		Obtiene el directorio de una categoría: el primer directorio después del base
		/// </summary>
		private string GetPathCategory(string pathourceImage, string imageSource)
		{
			string path = imageSource.Mid(pathourceImage.Length + 1, imageSource.Length);

				// Obtiene el primer directorio
				if (!path.IsEmpty())
				{
					string[] paths = path.Split('\\');

						if (paths != null && paths.Length > 0)
							path = paths[0];
						else
							path = null;
				}
				// Devuelve el directorio
				return path;
		}

		/// <summary>
		///		Obtiene el valor de una variable con el nombre de archivo
		/// </summary>
		private string GetVariableFileName(string imageSource)
		{
			string value = imageSource;

				// Obtiene el nombre de archivo sin extensión
				if (value.EndsWith(ExtensionUrl, StringComparison.CurrentCultureIgnoreCase))
					value = System.IO.Path.GetFileNameWithoutExtension(value);
				value = System.IO.Path.GetFileNameWithoutExtension(value);
				// Cambia los - y _ por espacios
				value = value.Replace('_', ' ');
				value = value.Replace('-', ' ');
				// Devuelve el valor
				return value.TrimIgnoreNull();
		}

		/// <summary>
		///		Obtiene la imagen origen 
		/// </summary>
		private string SearchSource(List<string> files, List<string> used)
		{
			string source = null;

				// Busca la imagen origen
				if (files != null && files.Count > 0)
				{
					int loops = 0;

						do
						{
							source = files[_rnd.Next(files.Count)];
							if (used.FirstOrDefault<string>(file => file.EqualsIgnoreCase(source)) != null)
								source = null;
						}
						while (source.IsEmpty() && loops++ < 10);
				}
				// Devuelve la imagen
				return source;
		}

		/// <summary>
		///		Compila los archivos de índices
		/// </summary>
		private void CompileIndexFiles(Model.WebSites.ProjectTargetModel projectTarget, ProjectModel project)
		{
			FilesModelCollection files = GetFilesCategories(project, Path.GetDirectoryName(projectTarget.ProjectFileName));

				// Añade las categorías al índice
				AddCategoriasPagesIndex(projectTarget, project, files);
				// Añade las opciones al menú principal
				AddCategoriesTopMenu(projectTarget, project, files);
		}

		/// <summary>
		///		Obtiene los archivos de categorías
		/// </summary>
		private FilesModelCollection GetFilesCategories(ProjectModel project, string pathBase)
		{
			FilesModelCollection files = new FilesModelCollection(project);
			string[] paths;

				// Obtiene los directorios
				paths = Directory.GetDirectories(pathBase);
				// Crea la colección de archivos
				foreach (string path in paths)
					if (File.Exists(Path.Combine(path, "Document.dcx")))
						files.Add(new FileModel(project, path));
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Añade las categorías al índice
		/// </summary>
		private void AddCategoriasPagesIndex(Model.WebSites.ProjectTargetModel projectTarget, ProjectModel project, FilesModelCollection filesCategories)
		{
			foreach (string indexPage in projectTarget.SectionWithPages)
				if (File.Exists(indexPage))
				{
					DocumentModel document = new DocumentBussiness().Load(new FileModel(project, indexPage));

						// Limpia las páginas hija del índice
						document.ChildPages.Clear();
						// Añade el directorio
						foreach (FileModel fileCategory in filesCategories)
							document.ChildPages.Add(fileCategory);
						// Graba el índice
						new DocumentBussiness().Save(document);
				}
		}

		/// <summary>
		///		Añade las categorías al menú principal
		/// </summary>
		private void AddCategoriesTopMenu(Model.WebSites.ProjectTargetModel projectTarget, ProjectModel project, FilesModelCollection filesCategories)
		{
			foreach (string sectionCategory in projectTarget.SectionMenus)
				if (File.Exists(sectionCategory))
				{
					DocumentModel document = new DocumentBussiness().Load(new FileModel(project, sectionCategory));
					int tabs = CountTabsBefore(document.Content, StartTag);
					string menu = "";

						// Añade las categorías al menú
						foreach (FileModel file in filesCategories)
							menu += AddTabsBefore($"%li #a {{ href=\"{file.FileName}\" title = \"Categoría {file.FileName}\" }} {file.FileName} #" + Environment.NewLine, tabs);
						// Obtiene el contenido del menú
						document.Content = ReplaceIndexContent(document.Content, menu, tabs);
						// Graba el índice
						new DocumentBussiness().Save(document);
				}
		}

		/// <summary>
		///		Añade tabuladores por delante a una cadena
		/// </summary>
		private string AddTabsBefore(string content, int tabs)
		{ 
			// Añade los tabuladores por delante
			for (int index = 0; index < tabs; index++)
				content = "\t" + content;
			// Devuelve el contenido con los tabuladores
			return content;
		}

		/// <summary>
		///		Reemplaza el contenido del archivo entre las etiquetas <!--Start index--> y <!--End index-->
		///	por el texto del menú
		/// </summary>
		private string ReplaceIndexContent(string content, string strMenu, int tabs)
		{
			int indexOfStart = content.IndexOf(StartTag, StringComparison.CurrentCultureIgnoreCase) + StartTag.Length;
			string result = strMenu; // ... por si acaso no hay contenido

				// Obtiene la posición donde se debe colocar el texto
				if (indexOfStart >= 0)
				{
					int indexOfEnd = content.IndexOf("<!--End index-->", StringComparison.CurrentCultureIgnoreCase);

						// Añade al resultado el contenido hasta el inicio
						result = content.Left(indexOfStart) + Environment.NewLine + strMenu + Environment.NewLine;
						// Añade el contenido desde el final
						if (indexOfEnd >= 0)
							result += AddTabsBefore(content.Substring(indexOfEnd), tabs);
				}
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Cuenta el número de tabuladores antes de la cadena Start index
		/// </summary>
		private int CountTabsBefore(string content, string tag)
		{
			int tabs = 0;
			int startIndex = content.IndexOf(tag, StringComparison.CurrentCultureIgnoreCase);

				// Cuenta el número de tabuladores antes de la etiqueta
				while (startIndex > 0 && content[startIndex - 1] == '\t')
				{
					tabs++;
					startIndex--;
				}
				// Devuelve el número de tabuladores
				return tabs;
		}

		/// <summary>
		///		Compilador
		/// </summary>
		private ProjectCompiler Compiler { get; }

		/// <summary>
		///		Resultado de la compilación
		/// </summary>
		private Model.WebSites.GenerationResultProjectModel Result { get; }
	}
}
