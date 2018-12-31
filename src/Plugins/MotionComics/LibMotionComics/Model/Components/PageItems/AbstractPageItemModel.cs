using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems
{
	/// <summary>
	///		Clase base para los elementos de una página
	/// </summary>
	public abstract class AbstractPageItemModel : AbstractComponentModel
	{
		public AbstractPageItemModel(PageModel page, string key) : base(key)
		{
			Page = page;
		}

		/// <summary>
		///		Página a la que se asocia el elemento
		/// </summary>
		public PageModel Page { get; }

		/// <summary>
		///		Posición
		/// </summary>
		public Entities.RectangleModel Dimensions { get; set; } = new Entities.RectangleModel(null, null, null, null);

		/// <summary>
		///		Visibilidad
		/// </summary>
		public bool Visible { get; set; }

		/// <summary>
		///		ZIndex
		/// </summary>
		public int ZIndex { get; set; }

		/// <summary>
		///		Opacidad
		/// </summary>
		public double Opacity { get; set; }

		/// <summary>
		///		Angulo
		/// </summary>
		public double Angle { get; set; }

		/// <summary>
		///		Origen de las rotaciones X
		/// </summary>
		public double OriginX { get; set; } = 0.5;

		/// <summary>
		///		Origen de las rotaciones Y
		/// </summary>
		public double OriginY { get; set; } = 0.5;

		/// <summary>
		///		Escala X
		/// </summary>
		public double ScaleX { get; set; } = 1;

		/// <summary>
		///		Escala Y
		/// </summary>
		public double ScaleY { get; set; } = 1;
	}
}
