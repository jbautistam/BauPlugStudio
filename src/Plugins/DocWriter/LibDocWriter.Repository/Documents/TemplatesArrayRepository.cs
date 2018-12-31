using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibDocWriter.Model.Documents;

namespace Bau.Libraries.LibDocWriter.Repository.Documents
{
	/// <summary>
	///		Repository de <see cref="TemplatesArrayModel"/>
	/// </summary>
	internal class TemplatesArrayRepository
	{ 
		// Constantes privadas
		private const string TagTemplateCategoryItem = "TemplateCategoryItem";
		private const string TagTemplateGallery = "TemplateGallery";
		private const string TagTemplateGalleryImage = "TemplateGalleryImage";
		private const string TagTemplateSiteMap = "TemplateSiteMap";
		private const string TagTemplateArticle = "Article";
		private const string TagTemplateCategoryHeader = "CategoryHeader";
		private const string TagTemplateTag = "TemplateTag";
		private const string TagTemplateMainPage = "TemplateMainPage";
		private const string TagTemplateNews = "TemplateNews";

		/// <summary>
		///		Carga los datos
		/// </summary>
		internal TemplatesArrayModel Load(MLNode nodeML)
		{
			TemplatesArrayModel templates = new TemplatesArrayModel();

				// Carga las plantillas
				foreach (MLNode childML in nodeML.Nodes)
					switch (childML.Name)
					{
						case TagTemplateCategoryItem:
								templates.CategoryItem = childML.Value;
							break;
						case TagTemplateGallery:
								templates.Gallery = childML.Value;
							break;
						case TagTemplateGalleryImage:
								templates.GalleryImage = childML.Value;
							break;
						case TagTemplateSiteMap:
								templates.SiteMap = childML.Value;
							break;
						case TagTemplateArticle:
								templates.Article = childML.Value;
							break;
						case TagTemplateCategoryHeader:
								templates.CategoryHeader = childML.Value;
							break;
						case TagTemplateTag:
								templates.Tag = childML.Value;
							break;
						case TagTemplateMainPage:
								templates.Main = childML.Value;
							break;
						case TagTemplateNews:
								templates.News = childML.Value;
							break;
					}
				// Devuelve las plantillas
				return templates;
		}

		/// <summary>
		///		Obtiene la cadena XML
		/// </summary>
		internal void AddNodes(TemplatesArrayModel template, MLNode nodeML)
		{
			nodeML.Nodes.Add(TagTemplateCategoryItem, template.CategoryItem);
			nodeML.Nodes.Add(TagTemplateGallery, template.Gallery);
			nodeML.Nodes.Add(TagTemplateGalleryImage, template.GalleryImage);
			nodeML.Nodes.Add(TagTemplateSiteMap, template.SiteMap);
			nodeML.Nodes.Add(TagTemplateArticle, template.Article);
			nodeML.Nodes.Add(TagTemplateCategoryHeader, template.CategoryHeader);
			nodeML.Nodes.Add(TagTemplateTag, template.Tag);
			nodeML.Nodes.Add(TagTemplateMainPage, template.Main);
			nodeML.Nodes.Add(TagTemplateNews, template.News);
		}
	}
}
