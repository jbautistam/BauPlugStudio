using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems.Brushes
{
	/// <summary>
	///		Fondo de imagen
	/// </summary>
	public class ImageBrushModel : AbstractBaseBrushModel
	{
		/// <summary>
		///		Modo de repetición de la imagen en el fondo
		/// </summary>
		public enum TileType
		{
			/// <summary>
			/// The same as Tile except that alternate columns of tiles are flipped horizontally. The base tile itself is not flipped.
			/// </summary>
			FlipX,
			/// <summary>
			/// The combination of FlipX and FlipY. The base tile itself is not flipped.
			/// </summary>
			FlipXY,
			/// <summary>
			/// The same as Tile except that alternate rows of tiles are flipped vertically. The base tile itself is not flipped.
			/// </summary>
			FlipY,
			/// <summary>
			/// The base tile is drawn but not repeated. The remaining area is transparent
			/// </summary>
			None,
			/// <summary>
			/// The base tile is drawn and the remaining area is filled by repeating the base tile. The right edge of one tile meets the left edge of the next, and similarly for the bottom and top edges.
			/// </summary>
			Tile
		}

		public ImageBrushModel(string key, string resourceKey, string fileName,
							   Entities.RectangleModel viewBox, Entities.RectangleModel viewPort,
							   TileType tile, PageModel.StretchMode stretch)
							: base(key, resourceKey)
		{
			FileName = fileName;
			ViewPort = viewPort;
			ViewBox = viewBox;
			Stretch = stretch;
			TileMode = tile;
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName { get; }

		/// <summary>
		///		Rectángulo de la vista de imagen (se aplican como relativos, es decir, 0,0,1,1 es toda la imagen)
		/// </summary>
		public Entities.RectangleModel ViewPort { get; }

		/// <summary>
		///		ViewBox del relleno
		/// </summary>
		public Entities.RectangleModel ViewBox { get; }

		/// <summary>
		///		Ajuste
		/// </summary>
		public PageModel.StretchMode Stretch { get; }

		/// <summary>
		///		Modo de repetición de las imágenes en el fondo
		/// </summary>
		public TileType TileMode { get; }
	}
}
