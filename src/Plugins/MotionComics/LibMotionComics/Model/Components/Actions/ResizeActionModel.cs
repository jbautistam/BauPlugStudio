using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Acción para cambiar el tamaño de un elemento
	/// </summary>
	public class ResizeActionModel : ActionBaseModel
	{
		public ResizeActionModel(TimeLineModel timeLine, string targetKey, double? width, double? height, bool mustAnimate)
							: base(timeLine, targetKey, mustAnimate)
		{
			Width = width;
			Height = height;
		}

		/// <summary>
		///		Ancho
		/// </summary>
		public double? Width { get; }

		/// <summary>
		///		Alto
		/// </summary>
		public double? Height { get; }
	}
}
