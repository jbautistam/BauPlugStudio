using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions.Eases
{
	/// <summary>
	///		Función exponencial asociada a una animación
	/// </summary>
	public class ExponentialEaseModel : EaseBaseModel
	{
		public ExponentialEaseModel(ActionBaseModel action, Mode mode, double exponent)
						: base(action, mode)
		{
			Exponent = exponent;
		}

		/// <summary>
		///		Exponente
		/// </summary>
		public double Exponent { get; }
	}
}
