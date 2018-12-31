using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.WebCurator.Model.WebSites;

namespace Bau.Libraries.WebCurator.Repository.WebSites
{
	/// <summary>
	///		Repository de <see cref="ProjectModel"/>
	/// </summary>
	public class ProjectRepository
	{   
		// Constantes privadas
		private const string TagRoot = "Project";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagProject = "Project";
		private const string TagProjectTargetFile = "FileName";
		private const string TagSectionWithPages = "SectionWithPages";
		private const string TagSectionMenus = "SectionMenus";
		private const string TagSectionTagMenuFileName = "SectionTagMenuFileName";
		private const string TagNumberDocuments = "NumberDocuments";
		private const string TagMaxImageWidth = "MaxImageWidth";
		private const string TagThumbWidth = "ThumbWidth";
		private const string cnstStrHoursBetweenGenerate = "HoursBetweenGenerate";
		private const string TagPathSource = "PathSource";
		private const string TagFileRssSource = "FileRssSource";
		private const string TagFileXMLSentences = "FileXMLSentences";

		/// <summary>
		///		Carga los datos de un proyecto
		/// </summary>
		public ProjectModel Load(string fileName)
		{
			ProjectModel project = new ProjectModel(System.IO.Path.GetDirectoryName(fileName));
			MLFile fileML = new XMLParser().Load(fileName);

				// Carga los datos del proyecto si existe el archivo
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
						{ 
							// Asigna los datos principales
							project.Name = nodeML.Nodes[TagName].Value;
							project.Description = nodeML.Nodes[TagDescription].Value;
							project.NumberDocuments = nodeML.Nodes[TagNumberDocuments].Value.GetInt(1);
							project.MaxImageWidth = nodeML.Nodes[TagMaxImageWidth].Value.GetInt(800);
							project.ThumbWidth = nodeML.Nodes[TagThumbWidth].Value.GetInt(200);
							project.HoursBetweenGenerate = nodeML.Nodes[cnstStrHoursBetweenGenerate].Value.GetInt(24);
							// Asigna los directorios
							foreach (MLNode childML in nodeML.Nodes)
								switch (childML.Name)
								{
									case TagPathSource:
											project.PathImagesSources.Add(childML.Value);
										break;
									case TagFileRssSource:
											project.FilesRssSources.Add(childML.Value);
										break;
									case TagProject:
											project.ProjectsTarget.Add(LoadProject(childML));
										break;
									case TagFileXMLSentences:
											project.FilesXMLSentences.Add(childML.Value);
										break;
								}
						}
				// Devuelve el proyecto
				return project;
		}

		/// <summary>
		///		Carga los datos de un proyecto
		/// </summary>
		private ProjectTargetModel LoadProject(MLNode nodeML)
		{
			ProjectTargetModel project = new ProjectTargetModel();

				// Carga los datos del proyecto
				project.ProjectFileName = nodeML.Nodes[TagProjectTargetFile].Value;
				project.SectionTagMenuFileName = nodeML.Nodes[TagSectionTagMenuFileName].Value;
				// Añade las secciones de categorías
				project.SectionWithPages.AddRange(LoadSections(nodeML, TagSectionWithPages));
				project.SectionMenus.AddRange(LoadSections(nodeML, TagSectionMenus));
				// Devuelve el proyecto
				return project;
		}

		/// <summary>
		///		Carga las secciones de un tipo
		/// </summary>
		private System.Collections.Generic.List<string> LoadSections(MLNode nodeML, string sectionTag)
		{
			System.Collections.Generic.List<string> sections = new System.Collections.Generic.List<string>();

				// Añade las cadenas de sección
				foreach (MLNode childML in nodeML.Nodes)
					if (childML.Name == sectionTag)
						sections.Add(childML.Value);
				// Devuelve la colección de cadenas
				return sections;
		}

		/// <summary>
		///		Graba los datos de un proyecto
		/// </summary>
		public void Save(ProjectModel project)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Añade los datos del proyecto
				nodeML.Nodes.Add(TagName, project.Name);
				nodeML.Nodes.Add(TagDescription, project.Description);
				nodeML.Nodes.Add(TagNumberDocuments, project.NumberDocuments);
				nodeML.Nodes.Add(TagMaxImageWidth, project.MaxImageWidth);
				nodeML.Nodes.Add(TagThumbWidth, project.ThumbWidth);
				nodeML.Nodes.Add(cnstStrHoursBetweenGenerate, project.HoursBetweenGenerate);
				// Añade los datos del proyecto
				foreach (ProjectTargetModel target in project.ProjectsTarget)
					nodeML.Nodes.Add(GetNodeProject(target));
				// Añade los directorios
				foreach (string path in project.PathImagesSources)
					nodeML.Nodes.Add(TagPathSource, path);
				foreach (string file in project.FilesRssSources)
					nodeML.Nodes.Add(TagFileRssSource, file);
				foreach (string xmlFileName in project.FilesXMLSentences)
					nodeML.Nodes.Add(TagFileXMLSentences, xmlFileName);
				// Guarda el archivo
				new XMLWriter().Save(project.FileName, fileML);
		}

		/// <summary>
		///		Obtiene el nodo de un proyecto
		/// </summary>
		private MLNode GetNodeProject(ProjectTargetModel project)
		{
			MLNode nodeML = new MLNode(TagProject);

				// Añade los parámetros del proyecto
				nodeML.Nodes.Add(TagProjectTargetFile, project.ProjectFileName);
				nodeML.Nodes.Add(TagSectionTagMenuFileName, project.SectionTagMenuFileName);
				// Añade las secciones de categorías
				AddNodesSections(nodeML, project.SectionWithPages, TagSectionWithPages);
				AddNodesSections(nodeML, project.SectionMenus, TagSectionMenus);
				// Devuelve el nodo
				return nodeML;
		}

		private void AddNodesSections(MLNode nodeML, System.Collections.Generic.List<string> sections, string tagCategory)
		{
			foreach (string section in sections)
				nodeML.Nodes.Add(tagCategory, section);
		}
	}
}