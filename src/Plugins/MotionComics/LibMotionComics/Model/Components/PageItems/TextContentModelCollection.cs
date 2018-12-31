using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems
{
	/// <summary>
	///		Colección de textos
	/// </summary>
	public class TextContentModelCollection : System.Collections.Generic.List<TextContentModel>
	{
		/// <summary>
		///		Obtiene el texto de un idioma
		/// </summary>
		public string GetText(string language, string languageDefault)
		{ 
			// Obtiene el texto adecuado al idioma
			foreach (TextContentModel text in this)
				if (text.Language.EqualsIgnoreCase(language))
					return text.Text;
			// Obtiene el texto adecuado al idioma predeterminado
			foreach (TextContentModel text in this)
				if (text.Language.EqualsIgnoreCase(languageDefault))
					return text.Text;
			// Si ha llegado hasta aquí devuelve el primer texto (o nada)
			if (Count > 0)
				return this[0].Text;
			else
				return "";
		}
	}
}
