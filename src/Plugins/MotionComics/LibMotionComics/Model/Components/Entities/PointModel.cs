using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Entities
{
	/// <summary>
	///		Clase con los datos de un punto
	/// </summary>
	public class PointModel
	{
		public PointModel(double x, double y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		///		Coordenada X
		/// </summary>
		public double X { get; set; }

		/// <summary>
		///		Coordenada Y
		/// </summary>
		public double Y { get; set; }
	}
}
