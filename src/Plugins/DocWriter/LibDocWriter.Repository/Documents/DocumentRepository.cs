using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.LibDocWriter.Model.Documents;

namespace Bau.Libraries.LibDocWriter.Repository.Documents
{
	/// <summary>
	///		Repository de <see cref="DocumentModel"/>
	/// </summary>
	public class DocumentRepository
	{ 
		// Constantes privadas
		private const string TagRoot = "Document";
		private const string TagID = "ID";
		private const string TagType = "Type";
		private const string TagScope = "Scope";
		private const string TagContent = "Content";
		private const string TagDateCreate = "DateCreate";
		private const string TagTitle = "Title";
		private const string TagDescription = "Description";
		private const string TagKeyWords = "KeyWords";
		private const string TagURLImageSummary = "ImageSummary";
		private const string TagShowAtRSS = "ShowAtRSS";
		private const string TagShowChilds = "ShowChilds";
		private const string TagPageTag = "Tag";
		private const string TagPageChildPage = "ChildPage";
		private const string TagIsRecursive = "IsRecursive";

		/// <summary>
		///		Carga los datos del documento
		/// </summary>
		public DocumentModel Load(Model.Solutions.FileModel file)
		{
			DocumentModel document = new DocumentModel(file);

				// Carga el archivo si existe
				Load(file.Project, document, file.DocumentFileName);
				// Obtiene el tipo de documento
				document.File.FileType = file.GetFileTypeByExtension();
				// Devuelve el documento
				return document;
		}

		/// <summary>
		///		Carga los datos del documento
		/// </summary>
		public DocumentModel Load(Model.Solutions.ProjectModel project, string fileName)
		{
			DocumentModel document = new DocumentModel(new Model.Solutions.FileModel(project, fileName));

				// Carga el archivo
				Load(project, document, fileName);
				// Devuelve el documento
				return document;
		}

		/// <summary>
		///		Carga los datos del documento
		/// </summary>
		private void Load(Model.Solutions.ProjectModel project, DocumentModel document, string fileName)
		{
			MLFile fileML = new XMLParser().Load(fileName);
				
				// Carga los nodos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot || nodeML.Name == "Template" || nodeML.Name == "Section" || nodeML.Name == "TemplatePage" || nodeML.Name == "Feed")
						{ 
							// Carga los datos del documento
							document.GlobalId = nodeML.Nodes [TagID].Value;
							document.IsRecursive = nodeML.Attributes [TagIsRecursive].Value.GetBool(false);
							document.Title = nodeML.Nodes [TagTitle].Value;
							document.Description = nodeML.Nodes [TagDescription].Value;
							document.KeyWords = nodeML.Nodes [TagKeyWords].Value;
							document.Content = nodeML.Nodes [TagContent].Value;
							document.ShowAtRSS = nodeML.Nodes [TagShowAtRSS].Value.GetBool(false);
							document.URLImageSummary = nodeML.Nodes [TagURLImageSummary].Value;
							document.ModeShow = (DocumentModel.ShowChildsMode) nodeML.Nodes [TagShowChilds].Value.GetInt((int) DocumentModel.ShowChildsMode.None);
							document.IDScope = (DocumentModel.ScopeType) nodeML.Nodes [TagScope].Value.GetInt((int) DocumentModel.ScopeType.Unknown);
							document.DateNew = nodeML.Nodes [TagDateCreate].Value.GetDateTime(DateTime.Now);
							// Carga las plantillas
							document.Templates = new TemplatesArrayRepository().Load(nodeML);
							// Carga los hijos
							document.Tags.AddRange(LoadFiles(nodeML, TagPageTag, project));
							document.ChildPages.AddRange(LoadFiles(nodeML, TagPageChildPage, project));
						}
		}

		/// <summary>
		///		Crea un archivo con el nombre pasado como parámetro
		/// </summary>
		private Model.Solutions.FilesModelCollection LoadFiles(MLNode nodeML, string tag, Model.Solutions.ProjectModel project)
		{
			Model.Solutions.FilesModelCollection files = new Model.Solutions.FilesModelCollection(project);

				// Carga los nodos
				foreach (MLNode childML in nodeML.Nodes)
					if (childML.Name == tag)
					{
						Model.Solutions.FileModel file = new Model.Solutions.FileModel(project);

							// Asigna el nombre de archivo
							file.FullFileName = System.IO.Path.Combine(project.File.Path, childML.Value);
							// Añade el archivo a la colección
							files.Add(file);
					}
				// Devuelve el archivo
				return files;
		}

		/// <summary>
		///		Graba los datos de un documento
		/// </summary>
		public void Save(DocumentModel document)
		{
			Save(document, document.File.DocumentFileName);
		}

		/// <summary>
		///		Graba los datos de un documento
		/// </summary>
		public void Save(DocumentModel document, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Asigna los datos del nodo
				nodeML.Attributes.Add(TagIsRecursive, document.IsRecursive);
				nodeML.Nodes.Add(TagID, document.GlobalId);
				nodeML.Nodes.Add(TagTitle, document.Title);
				nodeML.Nodes.Add(TagDescription, document.Description);
				nodeML.Nodes.Add(TagKeyWords, document.KeyWords);
				nodeML.Nodes.Add(TagContent, document.Content);
				nodeML.Nodes.Add(TagShowAtRSS, document.ShowAtRSS);
				nodeML.Nodes.Add(TagURLImageSummary, document.URLImageSummary);
				nodeML.Nodes.Add(TagShowChilds, (int) document.ModeShow);
				nodeML.Nodes.Add(TagType, (int) document.File.FileType);
				nodeML.Nodes.Add(TagScope, (int) document.IDScope);
				nodeML.Nodes.Add(TagDateCreate, document.DateNew);
				// Añade las plantillas
				new TemplatesArrayRepository().AddNodes(document.Templates, nodeML);
				// Añade los archivos asociados
				nodeML.Nodes.AddRange(GetFileNodes(TagPageTag, document.Tags));
				nodeML.Nodes.AddRange(GetFileNodes(TagPageChildPage, document.ChildPages));
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}

		/// <summary>
		///		Obtiene los nodos de archivo
		/// </summary>
		private MLNodesCollection GetFileNodes(string tag, Model.Solutions.FilesModelCollection files)
		{
			MLNodesCollection nodes = new MLNodesCollection();

				// Añade los archivos a los nodos
				foreach (Model.Solutions.FileModel file in files)
					nodes.Add(tag, file.IDFileName);
				// Devuelve la colección de nodos
				return nodes;
		}
	}
}
