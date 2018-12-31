using System;

using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;

namespace Bau.Libraries.LibDocWriter.Repository.Solutions
{
	/// <summary>
	///		Repository de <see cref="SolutionModel"/>
	/// </summary>
	public class SolutionRepository
	{
		// Constantes privadas
		private const string TagFilesRoot = "Files";
		internal const string TagFolder = "Folder";
		internal const string TagFile = "File";
		private const string TagName = "Name";
		private const string TagDescription = "Description";

		/// <summary>
		///		Carga los datos de una solución
		/// </summary>
		public SolutionModel Load(string fileName)
		{
			SolutionModel solution = new SolutionModel();
			MLFile fileML = new XMLParser().Load(fileName);

				// Asigna las propiedades
				solution.FullFileName = fileName;
				// Carga los proyectos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name.Equals(TagFilesRoot))
							foreach (MLNode childML in nodeML.Nodes)
								switch (childML.Name)
								{
									case TagFile:
											solution.Projects.Add(LoadProject(solution, childML.Value));
										break;
									case TagFolder:
											solution.Folders.Add(LoadFolder(solution, childML));
										break;
								}
				// Devuelve la solución
				return solution;
		}

		/// <summary>
		///		Carga los datos de la definición de la web
		/// </summary>
		private SolutionFolderModel LoadFolder(SolutionModel solution, MLNode nodeML)
		{
			SolutionFolderModel folder = new SolutionFolderModel(solution);

			// Carga los datos del nodo
			foreach (MLNode childML in nodeML.Nodes)
				switch (childML.Name)
				{
					case TagName:
							folder.Name = childML.Value;
						break;
					case TagDescription:
							folder.Description = childML.Value;
						break;
					case TagFolder:
							folder.Folders.Add(LoadFolder(solution, childML));
						break;
					case TagFile:
							folder.Projects.Add(LoadProject(solution, childML.Value));
						break;
				}
			// Devuelve la carpeta cargada
			return folder;
		}

		/// <summary>
		///		Obtiene el nombre de archivo de proyecto
		/// </summary>
		private ProjectModel LoadProject(SolutionModel solution, string relativeFileName)
		{
			if (System.IO.File.Exists(relativeFileName))
				return new ProjectRepository().Load(solution, relativeFileName);
			else
				return new ProjectRepository().Load(solution, LibCommonHelper.Files.HelperFiles.CombinePath(solution.Path, relativeFileName));
		}

		/// <summary>
		///		Graba los datos de una solución
		/// </summary>
		public void Save(SolutionModel solution)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagFilesRoot);

				// Añade las carpetas
				nodeML.Nodes.AddRange(GetXMLFolderNodes(solution, solution.Folders));
				// Añade los proyectos
				nodeML.Nodes.AddRange(GetXMLProjectNodes(solution, solution.Projects));
				// Graba el archivo
				new XMLWriter().Save(solution.FullFileName, fileML);
		}

		/// <summary>
		///		Obtiene una serie de nodos para las carpetas
		/// </summary>
		private MLNodesCollection GetXMLFolderNodes(SolutionModel solution, SolutionFolderModelCollection folders)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

			// Añade los nodos de las carpetas
			foreach (SolutionFolderModel folder in folders)
			{
				MLNode nodeML = new MLNode(TagFolder);

					// Añade los nodos con los datos de la carpeta
					nodeML.Nodes.Add(TagName, folder.Name);
					nodeML.Nodes.Add(TagDescription, folder.Description);
					// Añade los nodos con las subcarpetas
					nodeML.Nodes.AddRange(GetXMLFolderNodes(solution, folder.Folders));
					// Añade los nodos con los proyectos
					nodeML.Nodes.AddRange(GetXMLProjectNodes(solution, folder.Projects));
					// Añade el nodo a la colección
					nodesML.Add(nodeML);
			}
			// Devuelve la colección de nodos
			return nodesML;
		}

		/// <summary>
		///		Obtiene los nodos de los projectos
		/// </summary>
		internal MLNodesCollection GetXMLProjectNodes(SolutionModel solution, ProjectsModelCollection projects)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

				// Rellena los nodos
				foreach (ProjectModel project in projects)
					nodesML.Add(SolutionRepository.TagFile, GetPathRelative(solution, project));
				// Devuelve la colección de nodos
				return nodesML;
		}

		/// <summary>
		///		Obtiene la ruta relativa entre la solución y el proyecto
		/// </summary>
		private string GetPathRelative(SolutionModel solution, ProjectModel project)
		{
			return System.IO.Path.Combine(LibCommonHelper.Files.HelperFiles.GetPathRelative(solution.Path, project.File.Path),
										  project.File.FileName);
		}
	}
}
