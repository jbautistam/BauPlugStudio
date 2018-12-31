using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Entities
{
	/// <summary>
	///		Clase con los datos de un rectángulo
	/// </summary>
	public class RectangleModel
	{
		public RectangleModel(double? top = null, double? left = null,
							  double? width = null, double? height = null)
		{
			Top = top;
			Left = left;
			Width = width;
			Height = height;
		}

		/// <summary>
		///		Obtiene un rectángulo escalado
		/// </summary>
		public RectangleModel Scale(double comicWidth, double comicHeight, double viewPortWidth, double viewPortHeight)
		{
			return new RectangleModel((TopDefault / comicHeight) * viewPortHeight,
									  (LeftDefault / comicWidth) * viewPortWidth,
									  (WidthDefault / comicWidth) * viewPortWidth,
									  (HeightDefault / comicHeight) * viewPortHeight);
		}

		/// <summary>
		///		Restringe un valor superior
		/// </summary>
		public double GetNotNullTop(double? top, double defaultValue = 0)
		{
			return Top ?? top ?? defaultValue;
		}

		/// <summary>
		///		Restringe el valor de izquierda
		/// </summary>
		public double GetNotNullLeft(double? left, double defaultValue = 0)
		{
			return Left ?? left ?? defaultValue;
		}

		/// <summary>
		///		Restringe el ancho
		/// </summary>
		public double GetNotNullWidth(double? width, double defaultValue = 0)
		{
			return Width ?? width ?? defaultValue;
		}

		/// <summary>
		///		Restringe la altura
		/// </summary>
		public double GetNotNullHeight(double? height, double defaultValue = 0)
		{
			return Height ?? height ?? defaultValue;
		}

		/// <summary>
		///		Superior
		/// </summary>
		public double? Top { get; set; }

		/// <summary>
		///		Valor predeterminado para el punto superior
		/// </summary>
		public double TopDefault
		{
			get { return Top ?? 0; }
		}

		/// <summary>
		///		Izquierda
		/// </summary>
		public double? Left { get; set; }

		/// <summary>
		///		Valor predeterminado para el punto izquierdo
		/// </summary>
		public double LeftDefault
		{
			get { return Left ?? 0; }
		}

		/// <summary>
		///		Ancho
		/// </summary>
		public double? Width { get; set; }

		/// <summary>
		///		Valor predeterminado para el ancho
		/// </summary>
		public double WidthDefault
		{
			get { return Width ?? 100; }
		}

		/// <summary>
		///		Alto
		/// </summary>
		public double? Height { get; set; }

		/// <summary>
		///		Valor predeterminado para el alto
		/// </summary>
		public double HeightDefault
		{
			get { return Height ?? 100; }
		}
	}
}
