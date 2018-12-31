using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.LibSourceCodeDocumenter.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="Model.DocumenterProjectModel"/>
	/// </summary>
	internal class DocumenterProjectRepository
	{ 
		// Constantes privadas
		private const string TagRoot = "Project";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagPathTemplates = "PathTemplates";
		private const string TagPathPages = "PathPages";
		private const string TagPathGenerate = "PathGenerate";
		private const string TagShowInternal = "ShowInternal";
		private const string TagShowPrivate = "ShowPrivate";
		private const string TagShowProtected = "ShowProtected";
		private const string TagShowPublic = "ShowPublic";

		/// <summary>
		///		Carga un archivo
		/// </summary>
		internal Model.DocumenterProjectModel Load(string fileName)
		{
			Model.DocumenterProjectModel project = new Model.DocumenterProjectModel();
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

				// Carga los datos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
						{
							project.Name = nodeML.Nodes[TagName].Value;
							project.Description = nodeML.Nodes[TagDescription].Value;
							project.PathTemplates = nodeML.Nodes[TagPathTemplates].Value;
							project.PathPages = nodeML.Nodes[TagPathPages].Value;
							project.PathGenerate = nodeML.Nodes[TagPathGenerate].Value;
							project.ShowInternal = nodeML.Nodes[TagShowInternal].Value.GetBool();
							project.ShowPrivate = nodeML.Nodes[TagShowPrivate].Value.GetBool();
							project.ShowProtected = nodeML.Nodes[TagShowProtected].Value.GetBool();
							project.ShowPublic = nodeML.Nodes[TagShowPublic].Value.GetBool();
						}
				// Devuelve el proyecto
				return project;
		}

		/// <summary>
		///		Graba un archivo
		/// </summary>
		internal void Save(Model.DocumenterProjectModel project, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode objMLRoot = fileML.Nodes.Add(TagRoot);

				// Añade los valores
				objMLRoot.Nodes.Add(TagName, project.Name);
				objMLRoot.Nodes.Add(TagDescription, project.Description);
				objMLRoot.Nodes.Add(TagPathTemplates, project.PathTemplates);
				objMLRoot.Nodes.Add(TagPathPages, project.PathPages);
				objMLRoot.Nodes.Add(TagPathGenerate, project.PathGenerate);
				objMLRoot.Nodes.Add(TagShowInternal, project.ShowInternal);
				objMLRoot.Nodes.Add(TagShowPrivate, project.ShowPrivate);
				objMLRoot.Nodes.Add(TagShowProtected, project.ShowProtected);
				objMLRoot.Nodes.Add(TagShowPublic, project.ShowPublic);
				// Graba el archivo
				new LibMarkupLanguage.Services.XML.XMLWriter().Save(fileName, fileML);
		}
	}
}
