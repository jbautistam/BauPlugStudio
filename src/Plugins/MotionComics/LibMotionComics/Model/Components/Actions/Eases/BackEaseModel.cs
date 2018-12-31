using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions.Eases
{
	/// <summary>
	///		Vuelve una animación hacia atrás
	/// </summary>
	public class BackEaseModel : EaseBaseModel
	{
		public BackEaseModel(ActionBaseModel action, Mode mode, double amplitude) : base(action, mode)
		{
			Amplitude = amplitude;
		}

		/// <summary>
		///		Amplitud
		/// </summary>
		public double Amplitude { get; }
	}
}
