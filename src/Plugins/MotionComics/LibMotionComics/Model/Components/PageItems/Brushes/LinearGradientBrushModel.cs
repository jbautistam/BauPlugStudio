using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems.Brushes
{
	/// <summary>
	///		Brocha de un gradiante linear
	/// </summary>
	public class LinearGradientBrushModel : AbstractBaseBrushModel
	{
		public LinearGradientBrushModel(string key, string resourceKey) : base(key, resourceKey) { }

		/// <summary>
		///		Punto inicial del gradiante
		/// </summary>
		public Entities.PointModel Start { get; set; }

		/// <summary>
		///		Punto final del gradiante
		/// </summary>
		public Entities.PointModel End { get; set; }

		/// <summary>
		///		Forma en que se reflejan los gradiantes
		/// </summary>
		public Spread SpreadMethod { get; set; }

		/// <summary>
		///		Puntos de parada del gradiante
		/// </summary>
		public System.Collections.Generic.List<GradientStopModel> Stops { get; } = new System.Collections.Generic.List<GradientStopModel>();
	}
}
