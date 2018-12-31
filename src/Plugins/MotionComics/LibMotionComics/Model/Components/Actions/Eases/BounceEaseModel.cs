using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions.Eases
{
	/// <summary>
	///		Rebote de una animación
	/// </summary>
	public class BounceEaseModel : EaseBaseModel
	{
		public BounceEaseModel(ActionBaseModel action, Mode mode, int bounces, double bounciness) : base(action, mode)
		{
			Bounces = bounces;
			Bounciness = bounciness;
		}

		/// <summary>
		///		Número de rebotes
		/// </summary>
		public int Bounces { get; }

		/// <summary>
		///		Escala de amplitud del siguiente rebote
		/// </summary>
		public double Bounciness { get; }
	}
}
