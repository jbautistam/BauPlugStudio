using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMotionComic.Model.Components;
using Bau.Libraries.LibMotionComic.Model.Components.Entities;
using Bau.Libraries.LibMotionComic.Model.Components.PageItems;

namespace Bau.Libraries.LibMotionComic.Repository.Reader
{
	/// <summary>
	///		Repository para funciones comunes
	/// </summary>
	internal class ComicCommonRepository
	{
		/// <summary>
		///		Obtiene el modo de ajuste
		/// </summary>
		internal PageModel.StretchMode GetStretchMode(string value)
		{
			return value.GetEnum(PageModel.StretchMode.Fill);
		}

		/// <summary>
		///		Carga los datos de un punto
		/// </summary>
		internal PointModel GetPoint(string value, double defaultX = 0, double defaultY = 0)
		{
			PointModel point = new PointModel(0, 0);

				// Carga los datos
				if (!value.IsEmpty())
				{
					string[] valueParts = value.Split(',');

						if (valueParts.Length > 0)
							point.X = valueParts[0].GetDouble(defaultX);
						if (valueParts.Length > 1)
							point.Y = valueParts[1].GetDouble(defaultY);
				}
				// Devuelve el punto
				return point;
		}

		/// <summary>
		///		Carga los atributos de un elemento
		/// </summary>
		internal void AssignAttributesPageItem(MLNode nodeML, AbstractPageItemModel item)
		{
			item.Dimensions = AssignDimensions(nodeML);
			item.Visible = nodeML.Attributes[ComicRepositoryConstants.TagVisible].Value.GetBool(true);
			item.Opacity = nodeML.Attributes[ComicRepositoryConstants.TagOpacity].Value.GetDouble(1);
			item.ZIndex = nodeML.Attributes[ComicRepositoryConstants.TagZIndex].Value.GetInt(1);
			if (!item.Visible)
				item.Opacity = 0;
		}

		/// <summary>
		///		Asigna las dimensiones
		/// </summary>
		internal RectangleModel AssignDimensions(MLNode nodeML)
		{
			return new RectangleModel(nodeML.Attributes[ComicRepositoryConstants.TagTop].Value.GetDouble(),
									  nodeML.Attributes[ComicRepositoryConstants.TagLeft].Value.GetDouble(),
									  nodeML.Attributes[ComicRepositoryConstants.TagWidth].Value.GetDouble(),
									  nodeML.Attributes[ComicRepositoryConstants.TagHeight].Value.GetDouble());
		}

		/// <summary>
		///		Obtiene un rectángulo a partir de una cadena de coordenada Top, Left, Width, Height
		/// </summary>
		internal RectangleModel GetRectangle(string value)
		{
			RectangleModel rectangle = null;

				// Obtiene el rectángulo
				if (!value.IsEmpty())
				{
					string[] valueParts = value.Split(',');

						if (valueParts.Length == 4)
							rectangle = new RectangleModel(valueParts[0].GetDouble(), valueParts[1].GetDouble(),
														   valueParts[2].GetDouble(), valueParts[3].GetDouble());
				}
				// Devuelve el rectángulo
				return rectangle;
		}

		/// <summary>
		///		Convierte el modo de relleno
		/// </summary>
		internal Model.Components.PageItems.Brushes.ImageBrushModel.TileType ConvertTile(string value)
		{
			return value.GetEnum(Model.Components.PageItems.Brushes.ImageBrushModel.TileType.None);
		}

		/// <summary>
		///		Obtiene el método de relleno
		/// </summary>
		internal Model.Components.PageItems.Brushes.AbstractBaseBrushModel.Spread GetSpreadMethod(string value)
		{
			return value.GetEnum(Model.Components.PageItems.Brushes.AbstractBaseBrushModel.Spread.Pad);
		}

		/// <summary>
		///		Carga los datos de un color
		/// </summary>
		internal ColorModel GetColor(string color)
		{
			return new ColorModel(color);
		}

		/// <summary>
		///		Normaliza una cadena
		/// </summary>
		internal string Normalize(string value)
		{   
			// Normaliza una cadena
			if (!value.IsEmpty())
			{
				value = value.Replace('\n', ' ');
				value = value.Replace('\r', ' ');
				value = value.Replace('\t', ' ');
				value = value.ReplaceWithStringComparison("  ", " ");
				value = value.TrimIgnoreNull();
			}
			// Devuelve la cadena
			return value;
		}
	}
}
