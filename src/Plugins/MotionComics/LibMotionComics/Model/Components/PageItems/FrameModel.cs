using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems
{
	/// <summary>
	///		Frame / cuadro de una página de Cómic
	/// </summary>
	public class FrameModel : AbstractPageItemModel
	{
		public FrameModel(PageModel page, string key) : base(page, key)
		{
		}

		/// <summary>
		///		Radio X del rectángulo redondeado que se utiliza como borde del frame cuando no se hace la figura
		/// </summary>
		public double? RadiusX { get; set; }

		/// <summary>
		///		Radio Y del rectángulo redondeado que se utiliza como borde del frame cuando no se hace la figura
		/// </summary>
		public double? RadiusY { get; set; }

		/// <summary>
		///		Figura / borde
		/// </summary>
		public ShapeModel Shape { get; set; }

		/// <summary>
		///		Brocha
		/// </summary>
		public Brushes.AbstractBaseBrushModel Brush { get; set; }

		/// <summary>
		///		lápiz
		/// </summary>
		public PenModel Pen { get; set; }

		/// <summary>
		///		Modo de ajuste
		/// </summary>
		public PageModel.StretchMode Stretch { get; set; }

		/// <summary>
		///		Textos
		/// </summary>
		public System.Collections.Generic.List<TextModel> Texts { get; } = new System.Collections.Generic.List<TextModel>();
	}
}
