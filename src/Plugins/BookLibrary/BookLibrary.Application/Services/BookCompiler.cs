using System;
using System.IO;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.BookLibrary.Model.Books;
using Bau.Libraries.LibDocWriter.Application.Factory;
using Bau.Libraries.LibDocWriter.Application.Services;
using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibEBook.Formats.eBook;

namespace Bau.Libraries.BookLibrary.Application.Services
{
	/// <summary>
	///		Compilador de libro en NHTML
	/// </summary>
	public class BookCompiler
	{
		/// <summary>
		///		Compila el libro
		/// </summary>
		public bool Compile(BookModel book, string title, string description, string keyWords, string content, string pathTarget,
							out string error)
		{
			bool compiled = false;
			string pathTemp = Path.Combine(Path.GetTempPath(), HelperFiles.Normalize(book.Name));

				// Inicializa los argumentos de salida
				error = "";
				// Compila el libro
				try
				{   
					// Compila el libro
					Compile(new LibEBook.BookFactory().Load(LibEBook.BookFactory.BookType.ePub, book.FileName, pathTemp),
							pathTemp, title, description, keyWords, content, pathTarget);
					// Borra el directorio temporal
					HelperFiles.KillPath(pathTemp);
					// Indica que se ha compilado el libro
					compiled = true;
				}
				catch (Exception exception)
				{
					error = "Error en la compilación. " + exception.Message;
				}
				// Devuelve el valor que indica si se ha compilado correctamente
				return compiled;
		}

		/// <summary>
		///		Compila el contenido del libro
		/// </summary>
		private void Compile(Book book, string pathTemp, string title, string description,
							 string keyWords, string content, string pathTarget)
		{
			DocWriterFactory writerFactory = new DocWriterFactory(pathTarget);
			ProjectModel project = writerFactory.CreateProject();
			DocumentModel documentRoot;

				// Crea el documento raíz
				documentRoot = CreateRoot(title, description, keyWords, content, writerFactory);
				// Crea los documentos de las páginas
				foreach (IndexItem page in book.Index)
					CreatePage(title, pathTemp, writerFactory, documentRoot, page);
				// Crea el índice y graba de nuevo el documento raíz
				documentRoot.Content += Environment.NewLine + GetIndex(project, documentRoot);
				new LibDocWriter.Application.Bussiness.Documents.DocumentBussiness().Save(documentRoot);
				// Copia las imágenes
				CopyImages(book, pathTemp, documentRoot.File.FullFileName);
				// Borra el archivo de proyecto y el archivo de solución
				HelperFiles.KillFile(project.File.FullFileName);
				HelperFiles.KillFile(project.Solution.FullFileName);
		}

		/// <summary>
		///		Obtiene el índice del documento
		/// </summary>
		private string GetIndex(ProjectModel project, DocumentModel documentRoot)
		{
			NhamlBuilder builder = new NhamlBuilder();
			FilesModelCollection files = GetFilesChild(documentRoot);

				// Crea la lista con el índice
				builder.Indent = 0;
				builder.AddTag("br");
				// Obtiene el índice
				CreateIndex(builder, project, files);
				// Devuelve el contenido
				return builder.ToString();
		}

		/// <summary>
		///		Obtiene el índice
		/// </summary>
		private void CreateIndex(NhamlBuilder builder, ProjectModel project, FilesModelCollection files)
		{
			if (GetCountDocuments(files) > 0)
			{ 
				// Abre la lista
				builder.AddTag("ul");
				builder.Indent++;
				// Crea los elementos de la lista
				foreach (FileModel file in files)
					if (file.FileType == FileModel.DocumentType.Document)
					{
						DocumentModel document = new LibDocWriter.Application.Bussiness.Documents.DocumentBussiness().Load(file);

							// Añade el título
							builder.AddTag("li", $"#a {{href=\"{project.Name}\\{document.File.IDFileName}\" }} {document.Title} #");
							// Añade los elementos hijo
							CreateIndex(builder, project, GetFilesChild(document));
					}
				// Cierra la lista
				builder.Indent--;
			}
		}

		/// <summary>
		///		Obtiene el número de documentos de una colección de archivos
		/// </summary>
		private int GetCountDocuments(FilesModelCollection files)
		{
			int count = 0;

				// Obtiene el número de documentos entre los archivos
				if (files != null)
					foreach (FileModel file in files)
						if (file.FileType == FileModel.DocumentType.Document)
							count++;
				// Devuelve el número de documentos
				return count;
		}

		/// <summary>
		///		Obtiene los archivos hijo de un documento
		/// </summary>
		private FilesModelCollection GetFilesChild(DocumentModel document)
		{
			return new LibDocWriter.Application.Bussiness.Solutions.FileBussiness().Load(document.File);
		}

		/// <summary>
		///		Crea el documento raíz
		/// </summary>
		private DocumentModel CreateRoot(string title, string description, string keyWords, string content,
										 DocWriterFactory writerFactory)
		{
			return writerFactory.CreateCategory(null, title, title, description,
												keyWords, content, DocumentModel.ShowChildsMode.None, false, true);
		}

		/// <summary>
		///		Crea el documento de las páginas
		/// </summary>
		private void CreatePage(string strBookTitle, string pathTemp, DocWriterFactory writerFactory, DocumentModel documentRoot, IndexItem page)
		{
			string fileName = GetFileName(pathTemp, page);

				// Crea la página
				if (File.Exists(fileName))
				{
					string content = ConvertPageToNhtml(HelperFiles.LoadTextFile(fileName));
					DocumentModel document;

						// Si hay algo que meter en el documento ...
						if (!content.IsEmpty())
							document = writerFactory.CreatePage(documentRoot,
																$"{page.PageNumber:000}. {page.Name}",
																strBookTitle + " - " + page.Name,
																documentRoot.Description + " - " + page.Name,
																documentRoot.KeyWords, content);
						else
							document = documentRoot;
						// Crea los documentos de las páginas hija
						if (page.Items != null && page.Items.Count > 0)
							foreach (IndexItem item in page.Items)
								CreatePage(strBookTitle, pathTemp, writerFactory, document, item);
				}
		}

		/// <summary>
		///		Convierte el contenido de una página a NHTML
		/// </summary>
		private string ConvertPageToNhtml(string html)
		{ 
			// Convierte la página
			if (!html.IsEmpty())
			{
				int index = html.IndexOf("<body", StringComparison.CurrentCultureIgnoreCase);

					// Quita lo que hay antes de la etiqueta body
					if (index >= 0)
					{
						html = html.From(index + 1);
						index = html.IndexOf(">");
						if (index >= 0)
							html = html.From(index + 1);
					}
					// Quita todo lo que hay después del último </body>
					index = html.IndexOf("</body", StringComparison.CurrentCultureIgnoreCase);
					if (index >= 0)
						html = html.Left(index);
					// Añade las etiquetas de código
					if (!html.IsEmpty())
					{ // Quita los ../ del código
						while (html.IndexOf("\"../") >= 0)
							html = html.Replace("\"../", "\"");
						// Añade las etiquetas de código
						html = "<%code%>" + Environment.NewLine + html.TrimIgnoreNull() + Environment.NewLine + "<%end%>";
					}
			}
			// Devuelve el resultado
			return html;
		}

		/// <summary>
		///		Obtiene el nombre de archivo de una página
		/// </summary>
		private string GetFileName(string pathTemp, IndexItem page)
		{
			return Path.Combine(pathTemp, page.URL).Replace("/", "\\");
		}

		/// <summary>
		///		Copia las imágenes de un libro
		/// </summary>
		private void CopyImages(Book book, string pathTemp, string pathTarget)
		{
			foreach (PageFile file in book.Files)
				if (!file.FileName.IsEmpty() && IsImage(file.FileName))
				{
					string fileNameTarget = Path.Combine(pathTarget, RemoveFirstPath(file.FileName));

						// Crea el directorio destino
						HelperFiles.MakePath(Path.GetDirectoryName(fileNameTarget));
						// Copia el archivo
						HelperFiles.CopyFile(Path.Combine(pathTemp, file.FileName), fileNameTarget);
				}
		}

		/// <summary>
		///		Elimina el primer directorio
		/// </summary>
		private string RemoveFirstPath(string fileName)
		{
			string[] files;
			string target = "";

				// Corta el nombre de archivo por el separador
				if (fileName.IndexOf('\\') >= 0)
					files = fileName.Split('\\');
				else
					files = fileName.Split('/');
				// Quita el primer directorio
				if (files != null && files.Length > 1)
					for (int index = 1; index < files.Length; index++)
						target = target.AddWithSeparator(files[index], "\\", false);
				// Devuelve la cadena sin el primer directorio
				return target;
		}

		/// <summary>
		///		Comprueba si un archivo es de imagen
		/// </summary>
		private bool IsImage(string fileName)
		{
			string[] extensions = { "bmp", "jpg", "gif", "png", "tif", "tiff" };

				// Comprueba si el archivo es una imagen
				foreach (string extension in extensions)
					if (fileName.EndsWith("." + extension, StringComparison.CurrentCultureIgnoreCase))
						return true;
				// Si ha llegado hasta aquí es porque no es una imagen
				return false;
		}

	}
}
