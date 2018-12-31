using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Acción para animar el ViewBox o ViewPort de una brocha
	/// </summary>
	public class BrushViewBoxActionModel : ActionBaseModel
	{
		public BrushViewBoxActionModel(TimeLineModel timeLine, string targetKey, Entities.RectangleModel viewBox,
									   Entities.RectangleModel viewPort, PageItems.Brushes.ImageBrushModel.TileType tileMode,
									   bool mustAnimate)
							: base(timeLine, targetKey, mustAnimate)
		{
			ViewBox = viewBox;
			ViewPort = viewPort;
			TileMode = tileMode;
		}

		/// <summary>
		///		ViewBox
		/// </summary>
		public Entities.RectangleModel ViewBox { get; }

		/// <summary>
		///		ViewPort
		/// </summary>
		public Entities.RectangleModel ViewPort { get; }

		/// <summary>
		///		Tipo de relleno
		/// </summary>
		public PageItems.Brushes.ImageBrushModel.TileType TileMode { get; }
	}
}
