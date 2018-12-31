using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems
{
	/// <summary>
	///		Contenido del texto
	/// </summary>
	public class TextContentModel
	{
		public TextContentModel(string language, string text)
		{
			Language = language;
			Text = Normalize(text);
		}

		/// <summary>
		///		Normaliza el texto
		/// </summary>
		private string Normalize(string text)
		{ 
			// Normaliza el texto
			if (!text.IsEmpty())
			{ 
				// Quita los caracteres de espacio
				text = text.ReplaceWithStringComparison("\r\n", " ");
				text = text.ReplaceWithStringComparison("\r", " ");
				text = text.ReplaceWithStringComparison("\n", " ");
				text = text.ReplaceWithStringComparison("\t", " ");
				text = text.TrimIgnoreNull();
				// Quita los espacios dobles
				while (!text.IsEmpty() && text.IndexOf("  ") >= 0)
					text = text.ReplaceWithStringComparison("  ", " ");
			}
			// Devuelve el texto
			return text;
		}

		/// <summary>
		///		Clave del idioma
		/// </summary>
		public string Language { get; }

		/// <summary>
		///		Texto
		/// </summary>
		public string Text { get; }
	}
}
