using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Acción para cambiar la visibilidad de un objeto
	/// </summary>
	public class SetZIndexModel : ActionBaseModel
	{
		public SetZIndexModel(TimeLineModel timeLine, string key, bool mustAnimate, int intZIndex)
										: base(timeLine, key, mustAnimate)
		{
			ZIndex = intZIndex;
		}

		/// <summary>
		///		ZIndex del control
		/// </summary>
		public int ZIndex { get; }
	}
}
