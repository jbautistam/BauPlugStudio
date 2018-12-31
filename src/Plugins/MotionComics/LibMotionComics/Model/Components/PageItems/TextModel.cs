using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems
{
	/// <summary>
	///		Clase con los datos de un texto
	/// </summary>
	public class TextModel : AbstractPageItemModel
	{
		public TextModel(PageModel page, string key) : base(page, key) { }

		/// <summary>
		///		Textos
		/// </summary>
		public TextContentModelCollection Texts { get; } = new TextContentModelCollection();

		/// <summary>
		///		Nombre de la fuente
		/// </summary>
		public string FontName { get; set; }

		/// <summary>
		///		Tamaño del texto
		/// </summary>
		public double Size { get; set; }

		/// <summary>
		///		Indica si es texto en negrita
		/// </summary>
		public bool IsBold { get; set; }

		/// <summary>
		///		Indica si es texto en cursiva
		/// </summary>
		public bool IsItalic { get; set; }

		/// <summary>
		///		Color del texto
		/// </summary>
		public ColorModel Color { get; set; }

		/// <summary>
		///		Transformaciones del texto
		/// </summary>
		public Transforms.TransformModelCollection Transforms { get; } = new Transforms.TransformModelCollection();
	}
}
