using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems.Brushes
{
	/// <summary>
	///		Fondo sólido
	/// </summary>
	public class SolidBrushModel : AbstractBaseBrushModel
	{
		public SolidBrushModel(string key, string resourceKey, string color) : base(key, resourceKey)
		{
			Color = new ColorModel(color);
		}

		/// <summary>
		///		Color del fondo
		/// </summary>
		public ColorModel Color { get; }
	}
}
