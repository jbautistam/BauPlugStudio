using System;

namespace Bau.Libraries.WebCurator.Model.Sentences
{
	/// <summary>
	///		Clase para los datos de frases de una página / categoría
	/// </summary>
	public class PageDefinitionModel
	{
		/// <summary>
		///		Compacta los datos de la página
		/// </summary>
		internal void Compact(PageDefinitionModel source)
		{ 
			// Compacta las sentencias básicas
			Titles.AddRange(source.Titles);
			Descriptions.AddRange(source.Descriptions);
			KeyWords.AddRange(source.KeyWords);
			// Compacta los grupos
			Groups.Compact(source.Groups);
		}

		/// <summary>
		///		Títulos
		/// </summary>
		public SentenceModelCollection Titles { get; } = new SentenceModelCollection();

		/// <summary>
		///		Descripciones
		/// </summary>
		public SentenceModelCollection Descriptions { get; } = new SentenceModelCollection();

		/// <summary>
		///		Palabras clave
		/// </summary>
		public SentenceModelCollection KeyWords { get; } = new SentenceModelCollection();

		/// <summary>
		///		Grupos de frases para el cuerpo
		/// </summary>
		public GroupModelCollection Groups { get; } = new GroupModelCollection();
	}
}
