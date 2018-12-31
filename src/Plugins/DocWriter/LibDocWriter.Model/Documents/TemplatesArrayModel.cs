using System;

namespace Bau.Libraries.LibDocWriter.Model.Documents
{
	/// <summary>
	///		Array de referencias a plantillas
	/// </summary>
	public class TemplatesArrayModel
	{
		/// <summary>
		///		Tipo de plantilla
		/// </summary>
		public enum TemplateType
		{
			/// <summary>Plantilla principal</summary>
			Main,
			/// <summary>Elemento de categoría</summary>
			CategoryItem,
			/// <summary>Galería de imágenes</summary>
			Gallery,
			/// <summary>Imagen de galería</summary>
			GalleryImage,
			/// <summary>Artículo</summary>
			Article,
			/// <summary>Mapa del sitio</summary>
			SiteMap,
			/// <summary>Cabecera de categoría</summary>
			CategoryHeader,
			/// <summary>Etiqueta</summary>
			Tag,
			/// <summary>Noticias</summary>
			News
		}

		/// <summary>
		///		Obtiene la plantilla
		/// </summary>
		public string GetTemplate(TemplateType type)
		{
			switch (type)
			{
				case TemplateType.CategoryItem:
					return CategoryItem;
				case TemplateType.Gallery:
					return Gallery;
				case TemplateType.GalleryImage:
					return GalleryImage;
				case TemplateType.Article:
					return Article;
				case TemplateType.SiteMap:
					return SiteMap;
				case TemplateType.CategoryHeader:
					return CategoryHeader;
				case TemplateType.Tag:
					return Tag;
				case TemplateType.News:
					return News;
				default:
					return Main;
			}
		}

		/// <summary>
		///		Plantilla principal
		/// </summary>
		public string Main { get; set; }

		/// <summary>
		///		Plantilla de elemento de categorío
		/// </summary>
		public string CategoryItem { get; set; }

		/// <summary>
		///		Plantilla de galería
		/// </summary>
		public string Gallery { get; set; }

		/// <summary>
		///		Plantilla de imagen de galería
		/// </summary>
		public string GalleryImage { get; set; }

		/// <summary>
		///		Plantilla de artículo
		/// </summary>
		public string Article { get; set; }

		/// <summary>
		///		Plantilla de mapa del sitio
		/// </summary>
		public string SiteMap { get; set; }

		/// <summary>
		///		Plantilla de cabecera de una categoría / etiqueta
		/// </summary>
		public string CategoryHeader { get; set; }

		/// <summary>
		///		Plantilla de etiqueta
		/// </summary>
		public string Tag { get; set; }

		/// <summary>
		///		Plantilla de noticias
		/// </summary>
		public string News { get; set; }
	}
}