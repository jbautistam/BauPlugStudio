using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Acción para mover un elemento
	/// </summary>
	public class TranslateActionModel : ActionBaseModel
	{
		public TranslateActionModel(TimeLineModel timeLine, string targetKey, double? top, double? left, bool mustAnimate)
							: base(timeLine, targetKey, mustAnimate)
		{
			Top = top;
			Left = left;
		}

		/// <summary>
		///		Posición superior
		/// </summary>
		public double? Top { get; set; }

		/// <summary>
		///		Posición izquierda
		/// </summary>
		public double? Left { get; set; }
	}
}
