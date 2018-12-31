using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Acción para animar un gradiante circular
	/// </summary>
	public class BrushRadialActionModel : ActionBaseModel
	{
		public BrushRadialActionModel(TimeLineModel timeLine, string targetKey, Entities.PointModel center,
									  Entities.PointModel origin, double? radiusX, double? radiusY,
									  bool mustAnimate)
							: base(timeLine, targetKey, mustAnimate)
		{
			Center = center;
			Origin = origin;
			RadiusX = radiusX;
			RadiusY = radiusY;
		}

		/// <summary>
		///		Centro
		/// </summary>
		public Entities.PointModel Center { get; }

		/// <summary>
		///		Origen
		/// </summary>
		public Entities.PointModel Origin { get; }

		/// <summary>
		///		Radio X
		/// </summary>
		public double? RadiusX { get; }

		/// <summary>
		///		Radio Y
		/// </summary>
		public double? RadiusY { get; }
	}
}
