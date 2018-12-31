using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Parámetros básicos de animación
	/// </summary>
	public class AnimationParameters
	{
		/// <summary>
		///		Inicio de la animación
		/// </summary>
		public int Start { get; set; }

		/// <summary>
		///		Duración de la animación
		/// </summary>
		public int Duration { get; set; }

		/// <summary>
		///		Ratio de aceleración (0..1)
		/// </summary>
		public double? AccelerationRatio { get; set; }

		/// <summary>
		///		Ratio de deceleración (0..1)
		/// </summary>
		public double? DecelerationRatio { get; set; }

		/// <summary>
		///		Ratio de velocidad (0..1)
		/// </summary>
		public double? SpeedRatio { get; set; }
	}
}
