using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Transforms
{
	/// <summary>
	///		Transformación de traslación
	/// </summary>
	public class TranslateTransformModel : AbstractTransformModel
	{
		public TranslateTransformModel(double top = 0, double left = 0)
		{
			Top = top;
			Left = left;
		}

		/// <summary>
		///		Punto superior
		/// </summary>
		public double Top { get; set; }

		/// <summary>
		///		Punto izquierdo
		/// </summary>
		public double Left { get; set; }
	}
}
