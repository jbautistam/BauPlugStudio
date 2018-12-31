using System;

using Bau.Libraries.ImageFilters.Helper;
using Bau.Libraries.LibDocWriter.Application.Bussiness.Documents;
using Bau.Libraries.LibDocWriter.Application.Bussiness.Solutions;
using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.Application.Factory
{
	/// <summary>
	///		Factory para la generación de documentos de DocWriter para la compilación NHaml
	/// </summary>
	public class DocWriterFactory
	{ 
		// Variables privadas
		private ProjectModel _project;

		public DocWriterFactory(string targetPath)
		{
			TargetPath = targetPath;
		}

		/// <summary>
		///		Carga un proyecto
		/// </summary>
		public ProjectModel Load(string fileName)
		{
			return new ProjectBussiness().Load(CreateSolution(TargetPath), fileName);
		}

		/// <summary>
		///		Crea el proyecto
		/// </summary>
		public ProjectModel CreateProject()
		{
			ProjectModel project = new ProjectFactory().Create(CreateSolution(TargetPath), null, TargetPath);

				// Graba el proyecto
				new ProjectBussiness().Save(project);
				// Devuelve el proyecto
				return project;
		}

		/// <summary>
		///		Crea una solución
		/// </summary>
		public SolutionModel CreateSolution(string targetPath)
		{
			SolutionModel solution = new SolutionModel();

				// Asigna las propiedades
				solution.FullFileName = System.IO.Path.Combine(targetPath, "Solution.xml");
				// Devuelve la solución
				return solution;
		}

		/// <summary>
		///		Crea una categoría
		/// </summary>
		public DocumentModel CreateCategory(DocumentModel parent, string name, string title,
											string description, string keyWords, string content,
											DocumentModel.ShowChildsMode childMode = DocumentModel.ShowChildsMode.SortByDate,
											bool isRecursive = false, bool showAtRss = false)
		{
			return CreateDocument(parent, name, title,
								  description, keyWords, content, childMode, isRecursive, showAtRss);
		}

		/// <summary>
		///		Crea una página
		/// </summary>
		public DocumentModel CreatePage(DocumentModel parent, string name, string title,
										string description, string keyWords, string content,
										DocumentModel.ShowChildsMode childMode = DocumentModel.ShowChildsMode.None,
										bool isRecursive = false, bool showAtRss = false)
		{
			return CreateDocument(parent, name, title,
								  description, keyWords, content, childMode, isRecursive, showAtRss);
		}

		private DocumentModel CreateDocument(DocumentModel parent,
											 string name, string title, string description,
											 string keyWords, string content,
											 DocumentModel.ShowChildsMode childMode = DocumentModel.ShowChildsMode.SortByDate,
											 bool isRecursive = false, bool showAtRss = false)
		{
			FileModel file = new FileFactory().CreateFile(Project, GetFileParent(parent), name, FileModel.DocumentType.Document);
			DocumentModel document = new DocumentModel(file);

				// Asigna las propiedades al documento
				document.IDScope = DocumentModel.ScopeType.Page;
				document.IsRecursive = isRecursive;
				document.KeyWords = keyWords;
				document.ModeShow = childMode;
				document.ShowAtRSS = showAtRss;
				document.Title = title;
				document.Description = description;
				document.Content = content;
				// Graba el documento
				new DocumentBussiness().Save(document);
				// Devuelve el documento
				return document;
		}

		/// <summary>
		///		Graba una imagen redimensionada y su thumbnail
		/// </summary>
		public void ConvertImage(string fileSource, string fileTarget, int intMaxImageWidth, int intThumbWidth)
		{
			System.Drawing.Image imageResized = null;

				// Graba el thumb de la imagen y la redimensiona si es necesario
				using (System.Drawing.Image image = FiltersHelpers.Load(fileSource))
				{ 
					// Graba el thumb
					FiltersHelpers.SaveThumb(image, intThumbWidth, fileTarget);
					// Redimensiona la imagen (si es necesario) y la graba
					if (image.Width > intMaxImageWidth)
						imageResized = FiltersHelpers.Resize(image, intMaxImageWidth);
					// Libera la imagen
					image.Dispose();
				}
				// Graba la imagen redimensionada
				if (imageResized != null)
					FiltersHelpers.Save(imageResized, fileTarget);
				else
					LibCommonHelper.Files.HelperFiles.CopyFile(fileSource, fileTarget);
		}

		/// <summary>
		///		Obtiene el archivo padre de un documento
		/// </summary>
		private FileModel GetFileParent(DocumentModel parent)
		{
			if (parent == null)
				return Project.File;
			else
				return parent.File;
		}

		/// <summary>
		///		Directorio destino
		/// </summary>
		internal string TargetPath { get; }

		/// <summary>
		///		Proyecto sobre el que se generan los dcoumentos
		/// </summary>
		public ProjectModel Project
		{
			get
			{ 
				// Crea el proyecto si no existía
				if (_project == null)
					_project = CreateProject();
				// Devuelve el proyecto
				return _project;
			}
			set { _project = value; }
		}
	}
}
