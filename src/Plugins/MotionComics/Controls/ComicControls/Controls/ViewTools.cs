using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Bau.Libraries.LibMotionComic.Model;
using Bau.Libraries.LibMotionComic.Model.Components;
using Bau.Libraries.LibMotionComic.Model.Components.Entities;
using Bau.Libraries.LibMotionComic.Model.Components.PageItems;
using Bau.Libraries.LibMotionComic.Model.Components.PageItems.Brushes;

namespace Bau.Controls.ComicControls.Controls
{
	/// <summary>
	///		Herramientas para las vistas
	/// </summary>
	internal static class ViewTools
	{
		/// <summary>
		///		Convierte un ajuste
		/// </summary>
		internal static Stretch Convert(PageModel.StretchMode stretch)
		{
			switch (stretch)
			{
				case PageModel.StretchMode.None:
					return Stretch.None;
				case PageModel.StretchMode.Uniform:
					return Stretch.Uniform;
				case PageModel.StretchMode.UniformToFill:
					return Stretch.UniformToFill;
				default:
					return Stretch.Fill;
			}
		}

		/// <summary>
		///		Asigna el lápiz a una figura
		/// </summary>
		internal static void AssignPen(Shape shape, PenModel pen)
		{
			if (pen != null)
			{ 
				// Asigna los datos del lápiz
				shape.Stroke = new SolidColorBrush(Color.FromArgb(pen.Color.Alpha, pen.Color.Red, pen.Color.Green, pen.Color.Blue));
				shape.StrokeThickness = pen.Thickness;
				// Añade los puntos
				foreach (double dblDot in pen.Dots)
					shape.StrokeDashArray.Add(dblDot);
				shape.StrokeDashCap = Convert(pen.CapDots);
				if (pen.DashOffset != null)
					shape.StrokeDashOffset = pen.DashOffset ?? 0;
				if (pen.MiterLimit != null)
					shape.StrokeMiterLimit = pen.MiterLimit ?? 1;
				shape.StrokeStartLineCap = Convert(pen.StartLineCap);
				shape.StrokeEndLineCap = Convert(pen.EndLineCap);
				shape.StrokeLineJoin = Convert(pen.JoinMode);
			}
		}

		/// <summary>
		///		Convierte el modo de la línea
		/// </summary>
		private static PenLineCap Convert(PenModel.LineCap lineCap)
		{
			switch (lineCap)
			{
				case PenModel.LineCap.Round:
					return PenLineCap.Round;
				case PenModel.LineCap.Square:
					return PenLineCap.Square;
				case PenModel.LineCap.Triangle:
					return PenLineCap.Triangle;
				default:
					return PenLineCap.Flat;
			}
		}

		/// <summary>
		///		Convierte el modo de unión de las líneas
		/// </summary>
		private static PenLineJoin Convert(PenModel.LineJoin lineJoin)
		{
			switch (lineJoin)
			{
				case PenModel.LineJoin.Bevel:
					return PenLineJoin.Bevel;
				case PenModel.LineJoin.Round:
					return PenLineJoin.Round;
				default:
					return PenLineJoin.Miter;
			}
		}

		/// <summary>
		///		Convierte un modelo de brocha en una brocha de WPF
		/// </summary>
		internal static Brush GetBrush(AbstractPageItemModel pageItem, AbstractBaseBrushModel brush)
		{
			if (pageItem.Page == null)
				return null;
			else
				return GetBrush(pageItem.Page, brush);
		}

		/// <summary>
		///		Convierte un modelo de brocha en una brocha de WPF
		/// </summary>
		internal static Brush GetBrush(PageModel page, AbstractBaseBrushModel brush)
		{
			if (brush == null)
				return null;
			else if (brush is SolidBrushModel)
				return ConvertSolidBrush(page, brush as SolidBrushModel);
			else if (brush is ImageBrushModel)
				return ConvertImageBrush(page, brush as ImageBrushModel);
			else if (brush is RadialGradientBrushModel)
				return ConvertRadialBrush(page, brush as RadialGradientBrushModel);
			else if (brush is LinearGradientBrushModel)
				return ConvertLinearBrush(page, brush as LinearGradientBrushModel);
			else
				return null;
		}

		/// <summary>
		///		Convierte una definición de brocha
		/// </summary>
		private static AbstractBaseBrushModel GetBrushDefinition(PageModel page, string resourceKey)
		{
			AbstractBaseBrushModel resource = page.Comic.Resources.Search(resourceKey) as AbstractBaseBrushModel;

				if (resource != null && resource is AbstractBaseBrushModel)
					return resource;
				else
					return null;
		}


		/// <summary>
		///		Obtiene una brocha de un color sólido
		/// </summary>
		private static SolidColorBrush ConvertSolidBrush(PageModel page, SolidBrushModel brush)
		{   
			// Obtiene la brocha a partir del nombre del recurso
			if (!string.IsNullOrEmpty(brush.ResourceKey))
				brush = GetBrushDefinition(page, brush.ResourceKey) as SolidBrushModel;
			// Obtiene la brocha
			if (brush?.Color != null && !brush.Color.IsEmpty)
				return new SolidColorBrush(Color.FromArgb(brush.Color.Alpha, brush.Color.Red, brush.Color.Green, brush.Color.Blue));
			else
				return null;
		}

		/// <summary>
		///		Obtiene una brocha de imagen
		/// </summary>
		private static Brush ConvertImageBrush(PageModel page, ImageBrushModel brush)
		{
			ImageBrush background = null;
			string fileName = GetFileName(page.Comic, brush.ResourceKey, brush.FileName);

				// Asigna las propiedades al fondo
				if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
				{ 
					// Genera el fondo
					background = new ImageBrush();
					// Asigna la imagen
					background.ImageSource = LoadImage(fileName);
					background.Stretch = Convert(brush.Stretch);
					background.TileMode = Convert(brush.TileMode);
					// Asigna el viewport de la imagen
					if (brush.ViewPort != null)
					{
						background.Viewport = ConvertToRect(brush.ViewPort);
						background.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
					}
					// Asigna el ViewBox de la imagen
					if (brush.ViewBox != null)
					{
						background.Viewbox = ConvertToRect(brush.ViewBox);
						background.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
					}
				}
				// Asigna el fondo
				return background;
		}

		/// <summary>
		///		Convierte un rectángulo
		/// </summary>
		internal static Rect ConvertToRect(RectangleModel rectangle)
		{
			return new Rect(rectangle.LeftDefault, rectangle.TopDefault, rectangle.WidthDefault, rectangle.HeightDefault);
		}

		/// <summary>
		///		Convierte el modo de relleno
		/// </summary>
		internal static TileMode Convert(ImageBrushModel.TileType tileMode)
		{
			switch (tileMode)
			{
				case ImageBrushModel.TileType.FlipX:
					return TileMode.FlipX;
				case ImageBrushModel.TileType.FlipXY:
					return TileMode.FlipXY;
				case ImageBrushModel.TileType.FlipY:
					return TileMode.FlipY;
				case ImageBrushModel.TileType.Tile:
					return TileMode.Tile;
				default:
					return TileMode.None;
			}
		}

		/// <summary>
		///		Carga una imagen
		/// </summary>
		internal static BitmapImage LoadImage(ImageModel image)
		{
			return LoadImage(GetFileName(image.Page.Comic, image.ResourceKey, image.FileName));
		}

		/// <summary>
		///		Carga una imagen
		/// </summary>
		private static BitmapImage LoadImage(string fileName)
		{
			BitmapImage image = null;

				// Carga la imagen
				if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
				{ 
					// Crea un nuevo bitmap
					image = new BitmapImage();
					// Carga la imagen
					image.BeginInit();
					image.UriSource = new Uri(fileName, UriKind.Relative);
					image.CacheOption = BitmapCacheOption.OnLoad; // ... carga la imagen en este momento, no la deja en caché
					image.EndInit();
				}
				else
				{ 
					// Crea un nuevo bitmap
					image = new BitmapImage();
					// Carga la imagen
					image.BeginInit();
					image.UriSource = new Uri("pack://application:,,,/ComicControls;component/Assets/DefaultCover.png", UriKind.Absolute);
					image.CacheOption = BitmapCacheOption.OnLoad; // ... carga la imagen en este momento, no la deja en caché
					image.EndInit();
				}
				// Devuelve la imagen cargada
				return image;
		}

		/// <summary>
		///		Obtiene el nombre de archivo
		/// </summary>
		private static string GetFileName(ComicModel comic, string resourceKey, string fileName)
		{
			if (!string.IsNullOrEmpty(resourceKey))
				return (comic.Resources.Search(resourceKey) as ImageModel)?.FileName;
			else
				return fileName;
		}

		/// <summary>
		///		Crea una brocha con un gradiante lineal
		/// </summary>
		private static LinearGradientBrush ConvertLinearBrush(PageModel page, LinearGradientBrushModel brush)
		{
			LinearGradientBrush background = new LinearGradientBrush();

				// Obtiene la brocha a partir del nombre del recurso
				if (!string.IsNullOrEmpty(brush.ResourceKey))
					brush = GetBrushDefinition(page, brush.ResourceKey) as LinearGradientBrushModel;
				// Si realmente se ha encontrado una brocha ...
				if (brush != null)
				{ 
					// Asigna los datos de la brocha
					background.StartPoint = Convert(brush.Start);
					background.EndPoint = Convert(brush.End);
					background.SpreadMethod = Convert(brush.SpreadMethod);
					// Asigna los puntos
					AddGradientStops(background, brush.Stops);
				}
				// Devuelve la brocha
				return background;
		}

		/// <summary>
		///		Crea una brocha con un gradiante circular
		/// </summary>
		private static RadialGradientBrush ConvertRadialBrush(PageModel page, RadialGradientBrushModel brush)
		{
			RadialGradientBrush background = new RadialGradientBrush();

				// Obtiene la brocha a partir del nombre del recurso
				if (!string.IsNullOrEmpty(brush.ResourceKey))
					brush = GetBrushDefinition(page, brush.ResourceKey) as RadialGradientBrushModel;
				// Si realmente se ha encontrado una brocha
				if (brush != null)
				{ 
					// Asigna los datos de la brocha
					background.Center = Convert(brush.Center);
					background.RadiusX = brush.RadiusX;
					background.RadiusY = brush.RadiusY;
					background.GradientOrigin = Convert(brush.Origin);
					// Asigna los puntos
					AddGradientStops(background, brush.Stops);
				}
				// Devuelve la brocha
				return background;
		}

		/// <summary>
		///		Añade los puntos de parada del gradiante
		/// </summary>
		private static void AddGradientStops(GradientBrush background, List<GradientStopModel> stops)
		{
			foreach (GradientStopModel stop in stops)
				background.GradientStops.Add(new GradientStop(Convert(stop.Color), stop.Offset));
		}

		/// <summary>
		///		Convierte el método de relleno
		/// </summary>
		internal static GradientSpreadMethod Convert(AbstractBaseBrushModel.Spread spread)
		{
			switch (spread)
			{
				case AbstractBaseBrushModel.Spread.Reflect:
					return GradientSpreadMethod.Reflect;
				case AbstractBaseBrushModel.Spread.Repeat:
					return GradientSpreadMethod.Repeat;
				default:
					return GradientSpreadMethod.Pad;
			}
		}

		/// <summary>
		///		Convierte una regla de relleno de una geometría
		/// </summary>
		internal static FillRule Convert(ShapeModel.FillRule fillMode)
		{
			switch (fillMode)
			{
				case ShapeModel.FillRule.NonZero:
					return FillRule.Nonzero;
				default:
					return FillRule.EvenOdd;
			}
		}

		/// <summary>
		///		Convierte un color
		/// </summary>
		internal static Color Convert(ColorModel color)
		{
			if (color == null || color.IsEmpty)
				return Color.FromArgb(255, 255, 255, 255);
			else
				return Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
		}

		/// <summary>
		///		Convierte un punto
		/// </summary>
		internal static Point Convert(PointModel point)
		{
			return new Point(point.X, point.Y);
		}
	}
}
