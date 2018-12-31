using System;

using Bau.Libraries.DatabaseStudio.Models;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.DatabaseStudio.Application.Repository
{
	/// <summary>
	///		Repository para <see cref="ProjectModel"/>
	/// </summary>
	internal class ProjectRepository : BaseRepository
    {
		// Constantes privadas
		private const string TagRoot = "DBProject";

		/// <summary>
		///		Carga los datos de un proyecto
		/// </summary>
		internal ProjectModel Load(string fileName)
		{
			ProjectModel project = new ProjectModel();
			MLFile fileML = LoadFile(fileName);

				// Asigna el nombre de proyecto (aunque no haya cargado nada)
				project.Name = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(fileName));
				// Carga los datos del proyecto
				if (fileML != null)
					foreach (MLNode rootML in fileML.Nodes)
						if (rootML.Name == TagRoot)
						{
							// Carga los datos básicos del proyecto
							LoadBase(rootML, project);
							// Carga las conexiones y modelos de distribución
							project.Connections.AddRange(new ConnectionRepository().LoadConnections(rootML));
							project.Deployments.AddRange(new DeploymentRepository().Load(project, rootML));
						}
				// Devuelve el proyecto cargado
				return project;
		}

		/// <summary>
		///		Graba los datos de un proyecto
		/// </summary>
		internal void Save(ProjectModel project, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode rootML = fileML.Nodes.Add(TagRoot);

				// Rellena un nodo con los datos básicos
				FillNodeBase(rootML, project);
				// Añade los nodos de conexión y distribuciones
				rootML.Nodes.AddRange(new ConnectionRepository().GetMLConnections(project.Connections));
				rootML.Nodes.AddRange(new DeploymentRepository().GetMLNodes(project.Deployments));
				// Graba el archivo
				SaveFile(fileName, fileML);
		}
	}
}
