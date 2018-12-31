using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Acción para cambiar la visibilidad de un objeto
	/// </summary>
	public class SetVisibilityActionModel : ActionBaseModel
	{
		public SetVisibilityActionModel(TimeLineModel timeLine, string key, bool visible, double? opacity, bool mustAnimate)
										: base(timeLine, key, mustAnimate)
		{
			if (opacity == null)
			{
				if (visible)
					Opacity = 1;
				else
					Opacity = 0;
			}
			else
				Opacity = opacity ?? 1;
		}

		/// <summary>
		///		Opacidad del objeto
		/// </summary>
		public double Opacity { get; }
	}
}
