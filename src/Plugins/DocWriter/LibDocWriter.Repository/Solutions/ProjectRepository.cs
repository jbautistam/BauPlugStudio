using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.Repository.Solutions
{
	/// <summary>
	///		Repository de <see cref="ProjectModel"/>
	/// </summary>
	public class ProjectRepository
	{ 
		// Constantes privadas
		private const string TagWebDefinition = "WebDefinition";
		private const string TagDescription = "Description";
		private const string TagTitle = "Title";
		private const string TagKeyWords = "KeyWords";
		private const string TagWebType = "WebType";
		private const string TagURLBase = "URLBase";
		private const string TagServerFTP = "ServerFTP";
		private const string TagPortFTP = "PortFTP";
		private const string TagUserFTP = "UserFTP";
		private const string TagPasswordFTP = "PasswordFTP";
		private const string TagPathFTP = "PathFTP";
		private const string TagPageMain = "PageMain";
		private const string TagItemsPerCategory = "ItemsPerCategory";
		private const string TagItemsPerSiteMap = "ItemsPerSiteMap";
		private const string TagItemsPerPage = "ItemsPerPage";
		private const string TagMaxWidthImage = "MaxWidthImage";
		private const string TagThumbsWidth = "ThumbsWidth";
		private const string TagAddWebTitle = "AddWebTitle";
		private const string TagNumberParagraphsSummary = "NumberParagraphsSummary";
		private const string TagIDAdvertisingProject = "IDAdvertisingProject";
		private const string TagWebMaster = "Webmaster";
		private const string TagCopyright = "Copyright";
		private const string TagEditor = "Editor";
		private const string TagVariables = "Variables";
		private const string TagCommands = "Commands";

		/// <summary>
		///		Carga los datos de la definición de la web
		/// </summary>
		public ProjectModel Load(SolutionModel solution, string fileName)
		{
			ProjectModel project = new ProjectModel(solution);
			MLFile fileML = new XMLParser().Load(fileName);

				// Si se ha cargado el archivo
				if (fileML != null)
				{
					MLNode nodeML = fileML.Nodes[TagWebDefinition];

						// Asigna el nombre de archivo
						project.File.FullFileName = fileName;
						// Carga el archivo si existe
						if (nodeML != null)
						{ 
							// Carga los datos de los nodos
							project.Description = nodeML.Nodes [TagDescription].Value;
							project.Title = nodeML.Nodes [TagTitle].Value;
							project.KeyWords = nodeML.Nodes [TagKeyWords].Value;
							project.WebType = GetWebType(nodeML.Nodes [TagWebType].Value);
							project.URLBase = nodeML.Nodes [TagURLBase].Value;
							project.PageMain = nodeML.Nodes [TagPageMain].Value;
							project.ItemsPerCategory = nodeML.Nodes [TagItemsPerCategory].Value.GetInt(5);
							project.ItemsPerSiteMap = nodeML.Nodes [TagItemsPerSiteMap].Value.GetInt(50);
							project.MaxWidthImage = nodeML.Nodes [TagMaxWidthImage].Value.GetInt(400);
							project.ThumbsWidth = nodeML.Nodes [TagThumbsWidth].Value.GetInt(200);
							project.AddWebTitle = nodeML.Nodes [TagAddWebTitle].Value.GetBool(true);
							project.ParagraphsSummaryNumber = nodeML.Nodes [TagNumberParagraphsSummary].Value.GetInt(2);
							project.WebMaster = nodeML.Nodes [TagWebMaster].Value;
							project.Copyright = nodeML.Nodes [TagCopyright].Value;
							project.Editor = nodeML.Nodes [TagEditor].Value;
							project.VariablesText = nodeML.Nodes [TagVariables].Value;
							project.PostCompileCommands = nodeML.Nodes [TagCommands].Value;
							// Carga las plantillas
							project.Templates = new Documents.TemplatesArrayRepository().Load(nodeML);
						}
				}
				// Devuelve el proyecto cargado
				return project;
		}

		/// <summary>
		///		Obtiene el tipo de Web
		/// </summary>
		private ProjectModel.WebDefinitionType GetWebType(string type)
		{
			if (type.EqualsIgnoreCase(ProjectModel.WebDefinitionType.EBook.ToString()))
				return ProjectModel.WebDefinitionType.EBook;
			else if (type.EqualsIgnoreCase(ProjectModel.WebDefinitionType.Template.ToString()))
				return ProjectModel.WebDefinitionType.Template;
			else
				return ProjectModel.WebDefinitionType.Web;
		}

		/// <summary>
		///		Graba el documento XML de la definición de la Web
		/// </summary>
		public void Save(ProjectModel project)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagWebDefinition);

				// Añade los datos básicos
				nodeML.Nodes.Add(TagDescription, project.Description);
				nodeML.Nodes.Add(TagTitle, project.Title);
				nodeML.Nodes.Add(TagKeyWords, project.KeyWords);
				nodeML.Nodes.Add(TagWebType, project.WebType.ToString());
				nodeML.Nodes.Add(TagURLBase, project.URLBase);
				// Página principal, elementos por categoría y por etiqueta y si se debe añadir el título de la Web al de la página
				nodeML.Nodes.Add(TagPageMain, project.PageMain);
				nodeML.Nodes.Add(TagItemsPerCategory, project.ItemsPerCategory);
				nodeML.Nodes.Add(TagItemsPerSiteMap, project.ItemsPerSiteMap);
				nodeML.Nodes.Add(TagMaxWidthImage, project.MaxWidthImage);
				nodeML.Nodes.Add(TagThumbsWidth, project.ThumbsWidth);
				nodeML.Nodes.Add(TagAddWebTitle, project.AddWebTitle);
				nodeML.Nodes.Add(TagNumberParagraphsSummary, project.ParagraphsSummaryNumber);
				// Párametros del sitio
				nodeML.Nodes.Add(TagWebMaster, project.WebMaster);
				nodeML.Nodes.Add(TagCopyright, project.Copyright);
				nodeML.Nodes.Add(TagEditor, project.Editor);
				nodeML.Nodes.Add(TagVariables, project.VariablesText);
				// Comandos tras compilación
				nodeML.Nodes.Add(TagCommands, project.PostCompileCommands);
				// Graba las plantillas
				new Documents.TemplatesArrayRepository().AddNodes(project.Templates, nodeML);
				// Graba el archivo
				new XMLWriter().Save(project.File.FullFileName, fileML);
		}
	}
}
