using System;

using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Repository.Documents;

namespace Bau.Libraries.LibDocWriter.Application.Bussiness.Documents
{
	/// <summary>
	///		Clase de negocio de <see cref="DocumentModel"/>
	/// </summary>
	public class DocumentBussiness
	{
		/// <summary>
		///		Carga el documento asociado a un archivo
		/// </summary>
		public DocumentModel Load(Model.Solutions.FileModel file)
		{
			return new DocumentRepository().Load(file);
		}

		/// <summary>
		///		Carga el documento asociado a un archivo
		/// </summary>
		public DocumentModel Load(Model.Solutions.ProjectModel project, string fileName)
		{
			return new DocumentRepository().Load(project, fileName);
		}

		/// <summary>
		///		Graba los datos de un documento
		/// </summary>
		public void Save(Model.Solutions.FileModel file, DocumentModel document = null)
		{ 
			// Crea el documento con los datos del archivo
			if (document == null && file.FileType != Model.Solutions.FileModel.DocumentType.File && 
					file.FileType != Model.Solutions.FileModel.DocumentType.Folder)
				document = new DocumentModel(file);
			// Graba el documento
			if (document != null)
				Save(document);
		}

		/// <summary>
		///		Graba los datos de un documento
		/// </summary>
		public void Save(DocumentModel document)
		{
			new DocumentRepository().Save(document);
		}

		/// <summary>
		///		Graba los datos de un documento
		/// </summary>
		public void Save(DocumentModel document, string fileName)
		{
			new DocumentRepository().Save(document, fileName);
		}
	}
}
