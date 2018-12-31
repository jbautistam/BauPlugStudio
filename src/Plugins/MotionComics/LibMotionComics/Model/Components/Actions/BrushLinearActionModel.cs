using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Acción para animar el gradiante lineal
	/// </summary>
	public class BrushLinearActionModel : ActionBaseModel
	{
		public BrushLinearActionModel(TimeLineModel timeLine, string targetKey, Entities.PointModel start,
									  Entities.PointModel end, PageItems.Brushes.LinearGradientBrushModel.Spread spread,
									  bool mustAnimate)
							: base(timeLine, targetKey, mustAnimate)
		{
			Start = start;
			End = end;
			SpreadMethod = spread;
		}

		/// <summary>
		///		Inicio del gradiante
		/// </summary>
		public Entities.PointModel Start { get; }

		/// <summary>
		///		Final
		/// </summary>
		public Entities.PointModel End { get; }

		/// <summary>
		///		Radio X
		/// </summary>
		public PageItems.Brushes.LinearGradientBrushModel.Spread SpreadMethod { get; }
	}
}
