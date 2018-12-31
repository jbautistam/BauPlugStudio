using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Acción para un zoom de un elemento
	/// </summary>
	public class ZoomActionModel : ActionBaseModel
	{
		public ZoomActionModel(TimeLineModel timeLine, string targetKey, 
							   double originX, double originY, double scaleX, double scaleY, bool mustAnimate) 
							: base(timeLine, targetKey, mustAnimate) 
		{ 
			OriginX = originX;
			OriginY = originY;
			ScaleX = scaleX;
			ScaleY = scaleY;
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
		///		Escala para el eje X
		/// </summary>
		public double ScaleX { get; }

		/// <summary>
		///		Escala para el eje Y
		/// </summary>
		public double ScaleY { get; }
	}
}
