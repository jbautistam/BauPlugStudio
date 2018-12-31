using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems
{
	/// <summary>
	///		Clase con los datos de una figura
	/// </summary>
	public class FigureModel
	{
		/// <summary>
		///		Cadena de datos de la figura
		/// </summary>
		public string Data { get; set; }

		/// <summary>
		///		Modo de relleno
		/// </summary>
		public ShapeModel.FillRule FillMode { get; set; }

		/// <summary>
		///		Fondo
		/// </summary>
		public Brushes.AbstractBaseBrushModel Brush { get; set; }

		/// <summary>
		///		Transformaciones de la figura
		/// </summary>
		public Transforms.TransformModelCollection Transforms { get; } = new Transforms.TransformModelCollection();
	}
}
