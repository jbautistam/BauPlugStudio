using System;

using Bau.Libraries.LibDocWriter.Model.Documents;

namespace Bau.Libraries.LibDocWriter.ViewModel.Documents
{
	/// <summary>
	///		ViewModel de <see cref="TemplatesArrayModel"/>
	/// </summary>
	public class TemplateViewModel : BauMvvm.ViewModels.BaseObservableObject
	{ 
		// Variables privadas
		private string _main, _categoryItem, _gallery;
		private string _galleryImage, _article, _siteMap, _categoryHeader, _tag, _news;

		public TemplateViewModel(BauMvvm.ViewModels.BaseObservableObject viewModelParent, TemplatesArrayModel templates)
		{ 
			// Guarda las plantillas
			Templates = templates;
			// Muestra las plantillas
			LoadTemplates(templates);
			// Cambia el viewModelParent
			PropertyChanged += (sender, evntArgs) =>
									{
										if (viewModelParent != null)
											viewModelParent.IsUpdated = true;
									};
		}

		/// <summary>
		///		Carga las plantillas
		/// </summary>
		private void LoadTemplates(TemplatesArrayModel templates)
		{
			Main = templates.Main;
			CategoryItem = templates.CategoryItem;
			Gallery = templates.Gallery;
			GalleryImage = templates.GalleryImage;
			Article = templates.Article;
			SiteMap = templates.SiteMap;
			CategoryHeader = templates.CategoryHeader;
			Tag = templates.Tag;
			News = templates.News;
		}

		/// <summary>
		///		Obtiene las plantillas a partir de los valores
		/// </summary>
		internal TemplatesArrayModel GetTemplates()
		{
			TemplatesArrayModel templates = new TemplatesArrayModel();

				// Pasa los datos
				templates.Main = Main;
				templates.CategoryItem = CategoryItem;
				templates.Gallery = Gallery;
				templates.GalleryImage = GalleryImage;
				templates.Article = Article;
				templates.SiteMap = SiteMap;
				templates.CategoryHeader = CategoryHeader;
				templates.Tag = Tag;
				templates.News = News;
				// Devuelve las plantillas
				return templates;
		}

		/// <summary>
		///		Plantilla principal
		/// </summary>
		public string Main
		{
			get { return _main; }
			set { CheckProperty(ref _main, value); }
		}

		/// <summary>
		///		Plantilla de elemento de categoría
		/// </summary>
		public string CategoryItem
		{
			get { return _categoryItem; }
			set { CheckProperty(ref _categoryItem, value); }
		}

		/// <summary>
		///		Plantilla de galería
		/// </summary>
		public string Gallery
		{
			get { return _gallery; }
			set { CheckProperty(ref _gallery, value); }
		}

		/// <summary>
		///		Plantilla de imagen de galería
		/// </summary>
		public string GalleryImage
		{
			get { return _galleryImage; }
			set { CheckProperty(ref _galleryImage, value); }
		}

		/// <summary>
		///		Plantilla de artículo
		/// </summary>
		public string Article
		{
			get { return _article; }
			set { CheckProperty(ref _article, value); }
		}

		/// <summary>
		///		Plantilla de mapa del sitio
		/// </summary>
		public string SiteMap
		{
			get { return _siteMap; }
			set { CheckProperty(ref _siteMap, value); }
		}

		/// <summary>
		///		Plantilla de lista de etiquetas
		/// </summary>
		public string CategoryHeader
		{
			get { return _categoryHeader; }
			set { CheckProperty(ref _categoryHeader, value); }
		}

		/// <summary>
		///		Plantilla de etiqueta
		/// </summary>
		public string Tag
		{
			get { return _tag; }
			set { CheckProperty(ref _tag, value); }
		}

		/// <summary>
		///		Plantilla de noticias
		/// </summary>
		public string News
		{
			get { return _news; }
			set { CheckProperty(ref _news, value); }
		}

		/// <summary>
		///		Plantillas del ViewModel
		/// </summary>
		public TemplatesArrayModel Templates { get; }
	}
}
