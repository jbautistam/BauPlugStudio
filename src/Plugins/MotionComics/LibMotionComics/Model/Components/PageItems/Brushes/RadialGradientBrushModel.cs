using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems.Brushes
{
	/// <summary>
	///		Brocha de un gradiante circular
	/// </summary>
	public class RadialGradientBrushModel : AbstractBaseBrushModel
	{
		public RadialGradientBrushModel(string key, string resourceKey) : base(key, resourceKey) { }

		/// <summary>
		///		Centro del radio de la brocha
		/// </summary>
		public Entities.PointModel Center { get; set; }

		/// <summary>
		///		Radio X
		/// </summary>
		public double RadiusX { get; set; }

		/// <summary>
		///		Radio Y
		/// </summary>
		public double RadiusY { get; set; }

		/// <summary>
		///		Origen del gradiante
		/// </summary>
		public Entities.PointModel Origin { get; set; }

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
