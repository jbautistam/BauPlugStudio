using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.WebCurator.Model.Sentences
{
	/// <summary>
	///		Clase con los datos de un archivo de sentencias
	/// </summary>
	public class FileSentencesModel : LibDataStructures.Base.BaseExtendedModel
	{ 
		// Constantes privadas
		public const string Extension = "sxml";
		// Enumerados públicos
		/// <summary>
		///		Tipo de página
		/// </summary>
		public enum PageType
		{
			/// <summary>Página de categoría</summary>
			Category,
			/// <summary>Página normal</summary>
			Page
		}

		/// <summary>
		///		Selecciona los datos de página de un tipo
		/// </summary>
		public PageDefinitionModel SelectPage(PageType pageIndex)
		{
			if (pageIndex == PageType.Category)
				return CategoryDefinition;
			else
				return PageDefinition;
		}

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public override string Name
		{
			get
			{
				if (FileName.IsEmpty())
					return "Sin archivo";
				else
					return System.IO.Path.GetFileName(FileName);
			}
			set { base.Name = value; }
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		///		Sinónimos
		/// </summary>
		public SynonymousModelCollection Synonymous { get; } = new SynonymousModelCollection();

		/// <summary>
		///		Definición de sentencias para una categoría
		/// </summary>
		public PageDefinitionModel CategoryDefinition { get; } = new PageDefinitionModel();

		/// <summary>
		///		Definición de sentencias para una página
		/// </summary>
		public PageDefinitionModel PageDefinition { get; } = new PageDefinitionModel();
	}
}
