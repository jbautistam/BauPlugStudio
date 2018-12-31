using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Acción para rotar un elemento
	/// </summary>
	public class RotateActionModel : ActionBaseModel
	{
		public RotateActionModel(TimeLineModel timeLine, string targetKey, double originX, double originY, double angle, bool mustAnimate)
							: base(timeLine, targetKey, mustAnimate)
		{
			OriginX = originX;
			OriginY = originY;
			Angle = angle;
		}

		/// <summary>
		///		Punto de origen de la rotación (X)
		/// </summary>
		public double OriginX { get; }

		/// <summary>
		///		Punto de origen de la rotación (Y)
		/// </summary>
		public double OriginY { get; }

		/// <summary>
		///		Angulo de rotación
		/// </summary>
		public double Angle { get; }
	}
}
