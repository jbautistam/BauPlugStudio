using System;
using System.Windows.Controls;

using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Plugins.DocWriter.Views.UC
{
	/// <summary>
	///		Control para mostrar las plantillas asociadas a un documento
	/// </summary>
	public partial class ctlTemplates : UserControl
	{
		public ctlTemplates()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Asigna el proyecto a un control de búsqueda de páginas
		/// </summary>
		private void AssignProject(ctlSearchPage udtPage, ProjectModel project)
		{
			udtPage.Project = project;
			udtPage.FileType = FileModel.DocumentType.Template;
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		public ProjectModel Project
		{
			get { return udtTemplateMain.Project; }
			set
			{
				AssignProject(udtTemplateMain, value);
				AssignProject(udtTemplateCategoryItem, value);
				AssignProject(udtTemplateArticle, value);
				AssignProject(udtTemplateCategoryHeader, value);
				AssignProject(udtTemplateSiteMap, value);
				AssignProject(udtTemplateGallery, value);
				AssignProject(udtTemplateGalleryImage, value);
				AssignProject(udtTemplateTag, value);
				AssignProject(udtTemplateNews, value);
			}
		}
	}
}
