using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.Repository.Solutions
{
	/// <summary>
	///		Repository de <see cref="SolutionCombinedModel"/>
	/// </summary>
	public class SolutionRepository
	{ 
		// Constantes privadas
		private const string TagSolution = "Solution";
		private const string TagName = "Name";
		private const string TagModule = "Module";
		private const string TagType = "Type";
		private const string TagFileName = "FileName";
		private const string TagFolder = "Folder";
		private const string TagProject = "Project";

		/// <summary>
		///		Carga los datos de la solución de un archivo
		/// </summary>
		public SolutionModel Load(ProjectDefinitionModelCollection definitions, string fileName)
		{
			SolutionModel solution = new SolutionModel(fileName);

				// Carga los proyectos
				if (System.IO.File.Exists(fileName))
				{
					MLFile fileML = new XMLParser().Load(fileName);

						foreach (MLNode nodeML in fileML.Nodes)
							if (nodeML.Name == TagSolution)
								foreach (MLNode childML in nodeML.Nodes)
									switch (childML.Name)
									{
										case TagFolder:
												solution.Folders.Add(LoadFolder(solution, definitions, childML));
											break;
										case TagProject:
												solution.Projects.Add(LoadProject(solution, definitions, childML));
											break;
									}
				}
				// Devuelve la solución
				return solution;
		}

		/// <summary>
		///		Carga los datos de una carpeta de proyecto
		/// </summary>
		private SolutionFolderModel LoadFolder(SolutionModel solution, ProjectDefinitionModelCollection definitions, MLNode nodeML)
		{
			SolutionFolderModel folder = new SolutionFolderModel(solution);

				// Asigna las propiedades
				folder.Name = nodeML.Nodes[TagName].Value;
				// Carga los proyectos
				foreach (MLNode childML in nodeML.Nodes)
					if (childML.Name == TagProject)
						folder.Projects.Add(LoadProject(solution, definitions, childML));
				// Devuelve la carpeta
				return folder;
		}

		/// <summary>
		///		Carga los datos de un proyecto
		/// </summary>
		private ProjectModel LoadProject(SolutionModel solution, ProjectDefinitionModelCollection definitions, MLNode nodeML)
		{
			return new ProjectModel(solution,
									definitions.Search(nodeML.Nodes[TagModule].Value,
													   nodeML.Nodes[TagType].Value),
									System.IO.Path.Combine(solution.PathBase, nodeML.Nodes[TagFileName].Value));
		}

		/// <summary>
		///		Graba los datos de una solución
		/// </summary>
		public void Save(SolutionModel solution)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagSolution);

				// Añade las carpetas
				nodeML.Nodes.AddRange(GetNodesFolder(solution.Folders));
				// Añade los proyectos
				nodeML.Nodes.AddRange(GetNodesProject(solution.Projects));
				// Crea el directorio
				LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.GetDirectoryName(solution.FullFileName));
				// Graba el archivo XML
				LibCommonHelper.Files.HelperFiles.SaveTextFile(solution.FullFileName, new XMLWriter().ConvertToString(fileML));
		}

		/// <summary>
		///		Añade los nodos de carpeta
		/// </summary>
		private MLNodesCollection GetNodesFolder(SolutionFolderModelCollection folders)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

				// Añade los nodos
				foreach (SolutionFolderModel folder in folders)
				{
					MLNode nodeML = new MLNode(TagFolder);

						// Añade los nodos de propiedades
						nodeML.Nodes.Add(TagName, folder.Name);
						// Añade los nodos de carpetas y proyectos
						nodeML.Nodes.AddRange(GetNodesFolder(folder.Folders));
						nodeML.Nodes.AddRange(GetNodesProject(folder.Projects));
						// Añade el nodo a la colección
						nodesML.Add(nodeML);
				}
				// Devuelve los nodos
				return nodesML;
		}

		/// <summary>
		///		Añade los nodos de proyecto
		/// </summary>
		private MLNodesCollection GetNodesProject(ProjectsModelCollection projects)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

				// Añade los nodos
				foreach (ProjectModel project in projects)
				{
					MLNode nodeML = new MLNode(TagProject);

						// Asigna las propiedades
						nodeML.Nodes.Add(TagFileName, project.FullFileName);
						nodeML.Nodes.Add(TagModule, project.Definition.Module);
						nodeML.Nodes.Add(TagType, project.Definition.Type);
						// Añade el nodo
						nodesML.Add(nodeML);
				}
				// Devuelve los nodos
				return nodesML;
		}
	}
}