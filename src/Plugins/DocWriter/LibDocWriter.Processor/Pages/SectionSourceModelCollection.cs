using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Application.Bussiness.Documents;
using Bau.Libraries.LibDocWriter.Model.Documents;

namespace Bau.Libraries.LibDocWriter.Processor.Pages
{
	/// <summary>
	///		Colección de <see cref="SectionSourceModel"/>
	/// </summary>
	internal class SectionSourceModelCollection : List<SectionSourceModel>
	{
		internal SectionSourceModelCollection(Generator processor)
		{
			Processor = processor;
			DocumentBussinessManager = new DocumentBussiness();
		}

		/// <summary>
		///		Añade un documento a la colección
		/// </summary>
		internal void Add(FileTargetModel file)
		{
			Add(new SectionSourceModel(file, DocumentBussinessManager.Load(file.File)));
		}

		/// <summary>
		///		Depuración de las secciones cargadas
		/// </summary>
		#if DEBUG
		internal void Debug()
		{
			foreach (SectionSourceModel document in this)
			{
				System.Diagnostics.Debug.WriteLine($"{document.Source.Title} - {document.Source.File.FileType} - {document.Source.IDScope}");
				System.Diagnostics.Debug.WriteLine(document.Source.Content);
				System.Diagnostics.Debug.WriteLine("--------------------------------------------------------------------------------");
				System.Diagnostics.Debug.WriteLine("");
			}
		}
		#endif

		/// <summary>
		///		Obtiene la plantilla de un documento de determinado tipo
		/// </summary>
		internal SectionSourceModel GetTemplate(DocumentModel document, TemplatesArrayModel.TemplateType intTemplateType)
		{
			string template = document.Templates.GetTemplate(intTemplateType);

				// Si no se ha recogido ninguna plantilla se obtiene el tipo de plantilla del proyecto
				if (template.IsEmpty())
					template = Processor.Project.Templates.GetTemplate(intTemplateType);
				// Si se ha encontrado alguna plantilla, se busca entre las plantillas cargadas
				if (!template.IsEmpty())
					foreach (SectionSourceModel section in this)
						if (section.FileTarget.FileNameSource.EqualsIgnoreCase(template))
							return section;
				// Si ha llegado hasta aquí es porque no ha encontrado ninguna plantilla
				return null;
		}

		/// <summary>
		///		Procesador al que se asocia la colección de documentos
		/// </summary>
		internal Generator Processor { get; }

		/// <summary>
		///		Objeto de negocio de archivos
		/// </summary>
		private DocumentBussiness DocumentBussinessManager { get; }
	}
}
