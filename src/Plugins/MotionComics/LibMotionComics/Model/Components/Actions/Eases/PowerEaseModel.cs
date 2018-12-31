using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions.Eases
{
	/// <summary>
	///		Función potencia asociada a una animación
	/// </summary>
	public class PowerEaseModel : EaseBaseModel
	{
		public PowerEaseModel(ActionBaseModel action, Mode mode, double power) : base(action, mode) 
		{ 
			Power = power;
		}

		/// <summary>
		///		Potencia
		/// </summary>
		public double Power { get; }
	}
}
