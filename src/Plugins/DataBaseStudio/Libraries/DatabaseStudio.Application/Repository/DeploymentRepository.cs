using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.DatabaseStudio.Models;
using Bau.Libraries.DatabaseStudio.Models.Connections;
using Bau.Libraries.DatabaseStudio.Models.Deployment;
using Bau.Libraries.LibDataStructures.Base;

namespace Bau.Libraries.DatabaseStudio.Application.Repository
{
	/// <summary>
	///		Repository para <see cref="DeploymentModel"/>
	/// </summary>
    internal class DeploymentRepository : BaseRepository
    {
		// Constantes privadas
		private const string TagRoot = "Deployment";
		private const string TagParameter = "Parameter";
		private const string TagConnection = "Connection";
		private const string TagKey = "Key";
		private const string TagId = "Id";
		private const string TagPathScriptsTarget = "PathScripts";
		private const string TagPathFilesTarget = "PathFiles";
		private const string TagType = "Type";
		private const string TagScript = "Script";
		private const string TagFile = "File";
		private const string TagFormatReportType = "ReportFormat";

		/// <summary>
		///		Carga los modos de distribución 
		/// </summary>
		internal DeploymentModelCollection Load(ProjectModel project, MLNode rootML)
		{
			DeploymentModelCollection deployments = new DeploymentModelCollection();

				// Carga los datos
				foreach (MLNode nodeML in rootML.Nodes)
					if (nodeML.Name == TagRoot)
					{
						DeploymentModel deployment = new DeploymentModel();

							// Carga los datos
							LoadBase(nodeML, deployment);
							deployment.PathScriptsTarget = nodeML.Nodes[TagPathScriptsTarget].Value;
							deployment.PathFilesTarget = nodeML.Nodes[TagPathFilesTarget].Value;
							// Carga las conexiones y parámetros
							foreach (MLNode childML in nodeML.Nodes)
								switch (childML.Name)
								{
									case TagConnection:
											if (!string.IsNullOrEmpty(childML.Attributes[TagKey].Value) &&
													!string.IsNullOrEmpty(childML.Attributes[TagId].Value))
												deployment.Connections.Add(childML.Attributes[TagKey].Value, 
																		   project.Connections.Search(childML.Attributes[TagId].Value) as DatabaseConnectionModel);
										break;
									case TagScript:
											deployment.Scripts.Add(LoadScript(childML));
										break;
									case TagFormatReportType:
											deployment.ReportFormatTypes.Add(childML.Value.GetEnum(DeploymentModel.ReportFormat.Xml));
										break;
								}
							// Carga los parámetros
							LoadParameters(nodeML, deployment.Parameters);
							// Añade los datos de la distribución
							deployments.Add(deployment);
					}
				// Devuelve la colección de distribuciones
				return deployments;
		}

		/// <summary>
		///		Carga los parámetros
		/// </summary>
		private void LoadParameters(MLNode rootML, BaseModelCollection<ParameterModel> parameters)
		{
			foreach (MLNode nodeML in rootML.Nodes)
				if (nodeML.Name == TagParameter)
				{
					ParameterModel parameter = new ParameterModel();

						// Asigna las propiedades
						parameter.GlobalId = nodeML.Attributes[TagId].Value;
						parameter.Type = nodeML.Attributes[TagType].Value.GetEnum(ParameterModel.ParameterType.String);
						parameter.Value = nodeML.Value;
						// Añade el parámetro
						parameters.Add(parameter);
				}
		}

		/// <summary>
		///		Carga los datos de un script
		/// </summary>
		private ScriptModel LoadScript(MLNode rootML)
		{
			ScriptModel script = new ScriptModel();

				// Añade las propiedades
				script.RelativeFileName = rootML.Attributes[TagFile].Value;
				LoadParameters(rootML, script.Parameters);
				// Devuelve el script
				return script;
		}

		/// <summary>
		///		Obtiene los nodos de distribución
		/// </summary>
		internal MLNodesCollection GetMLNodes(DeploymentModelCollection deployments)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

				// Crea los nodos
				foreach (DeploymentModel deployment in deployments)
				{
					MLNode rootML = GetMLNodeBase(TagRoot, deployment);

						// Añade los nodos de propiedades
						rootML.Nodes.Add(TagPathScriptsTarget, deployment.PathScriptsTarget);
						rootML.Nodes.Add(TagPathFilesTarget, deployment.PathFilesTarget);
						// Añade los datos de las conexiones
						foreach (KeyValuePair<string, DatabaseConnectionModel> item in deployment.Connections)
						{
							MLNode nodeML = rootML.Nodes.Add(TagConnection);

								// Añade los datos
								nodeML.Attributes.Add(TagKey, item.Key);
								nodeML.Attributes.Add(TagId, item.Value.GlobalId);
						}
						// Añade los parámetros
						rootML.Nodes.AddRange(GetMLParameters(deployment.Parameters));
						// Añade los scripts
						rootML.Nodes.AddRange(GetMLScripts(deployment.Scripts));
						// Añade los nodos con los tipos de salida
						foreach (DeploymentModel.ReportFormat format in deployment.ReportFormatTypes)
							rootML.Nodes.Add(TagFormatReportType, format.ToString());
						// Añade el nodo a la colección
						nodesML.Add(rootML);
				}
				// Devuelve la colección de nodos
				return nodesML;
		}

		/// <summary>
		///		Obtiene los nodos de parámetros
		/// </summary>
		private MLNodesCollection GetMLParameters(BaseModelCollection<ParameterModel> parameters)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

				// Añade los parámetros
				foreach (ParameterModel parameter in parameters)
				{
					MLNode nodeML = nodesML.Add(TagParameter);

						// Añade las propiedades
						nodeML.Attributes.Add(TagId, parameter.GlobalId);
						nodeML.Attributes.Add(TagType, parameter.Type.ToString());
						nodeML.Value = parameter.Value;
				}
				// Devuelve la colección de parámetros
				return nodesML;
		}

		/// <summary>
		///		Obtiene los nodos de scripts
		/// </summary>
		private MLNodesCollection GetMLScripts(BaseModelCollection<ScriptModel> scripts)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

				// Añade los scripts
				foreach (ScriptModel script in scripts)
				{
					MLNode nodeML = nodesML.Add(TagScript);

						// Añade las propiedades
						nodeML.Attributes.Add(TagFile, script.RelativeFileName);
						// Añade los parámetros
						nodeML.Nodes.AddRange(GetMLParameters(script.Parameters));
				}
				// Devuelve los nodos
				return nodesML;
		}
    }
}
