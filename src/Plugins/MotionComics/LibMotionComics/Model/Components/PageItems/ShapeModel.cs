using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems
{
	/// <summary>
	///		Conjunto de figuras
	/// </summary>
	public class ShapeModel : AbstractPageItemModel
	{
		/// <summary>
		///		Enumerado con los modos de relleno
		/// </summary>
		public enum FillRule
		{
			/// <summary>No se rellena</summary>
			None,
			NonZero,
			EvenOdd
		}

		public ShapeModel(PageModel page, string key) : base(page, key) { }

		/// <summary>
		///		Clave del componente
		/// </summary>
		public string ComponentKey { get; set; }

		/// <summary>
		///		Modo de relleno
		/// </summary>
		public FillRule FillMode { get; set; }

		/// <summary>
		///		Transformaciones aplicadas a la figura
		/// </summary>
		public Transforms.TransformModelCollection Transforms { get; } = new Transforms.TransformModelCollection();

		/// <summary>
		///		Figuras de la forma
		/// </summary>
		public System.Collections.Generic.List<FigureModel> Figures { get; } = new System.Collections.Generic.List<FigureModel>();
	}
}
