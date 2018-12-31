using System;

using Bau.Libraries.LibDataStructures.Collections;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.LibDataBaseStudio.Model.Reports;

namespace Bau.Libraries.LibDataBaseStudio.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="ReportModel"/>
	/// </summary>
	internal class ReportRepository : ConnectionItemRepositoryBase
	{   
		// Constantes privadas
		private const string TagRoot = "Documenter";
		private const string TagGlobalId = "GlobalId";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagReportDefinition = "ReportDefinition";
		private const string TagLastExecutionParameterName = "LastExecutionParameterName";
		private const string TagLastExecutionOuputMode = "LastExecutionOuputMode";
		private const string TagLastExecutionFileName = "LastExecutionFileName";
		private const string TagParametersRoot = "ParametersRoot";
		private const string TagParameter = "Parameter";
		private const string TagFixedParameter = "FixedParameter";
		private const string TagImageParameter = "ImageParameter";
		private const string TagParameterName = "Name";
		private const string TagParameterValue = "Value";
		private const string TagFile = "File";
		private const string TagFileType = "Type";

		/// <summary>
		///		Carga los datos de un informe
		/// </summary>
		internal ReportModel Load(string fileName)
		{
			ReportModel report = new ReportModel();
			MLFile fileML = new XMLParser().Load(fileName);
			HelperRepository helper = new HelperRepository();

				// Carga los datos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name.Equals(TagRoot))
						{ 
							// Carga las propiedades
							report.GlobalId = nodeML.Nodes[TagGlobalId].Value;
							report.Name = nodeML.Nodes[TagName].Value;
							report.Description = nodeML.Nodes[TagDescription].Value;
							report.ReportDefinition = helper.NormalizeContentLoad(nodeML.Nodes[TagReportDefinition].Value);
							report.LastExecutionParameterName = nodeML.Nodes[TagLastExecutionParameterName].Value;
							report.LastExecutionOuputMode = nodeML.Nodes[TagLastExecutionOuputMode].Value.GetInt(0);
							report.LastExecutionFileName = nodeML.Nodes[TagLastExecutionFileName].Value;
							// Carga los parámetros de ejecución
							report.ExecutionParameters.AddRange(LoadExecutionParameters(nodeML));
						}
				// Devuelve el informe
				return report;
		}

		/// <summary>
		///		Carga los parámetros de ejecución
		/// </summary>
		private ReportExecutionModelCollection LoadExecutionParameters(MLNode nodeML)
		{
			ReportExecutionModelCollection executions = new ReportExecutionModelCollection();

				// Carga los parámetros de ejecución
				foreach (MLNode childML in nodeML.Nodes)
					if (childML.Name == TagParametersRoot)
					{
						ReportExecutionModel execution = new ReportExecutionModel();

							// Carga el nombre y la descripción
							execution.Name = childML.Nodes[TagName].Value;
							execution.Description = childML.Nodes[TagDescription].Value;
							// Carga las cadenas de conexión
							execution.ConnectionsGuid.AddRange(base.LoadConnections(childML));
							// Carga los parámetros adicionales
							execution.Parameters.AddRange(LoadParameterList(childML, TagParameter));
							execution.FixedParameters.AddRange(LoadParameterList(childML, TagFixedParameter));
							execution.Files.AddRange(LoadFiles(childML));
							// Añade el parámetro a la colección
							executions.Add(execution);
					}
				// Devuelve los parámetros de ejecución
				return executions;
		}

		/// <summary>
		///		Carga los parámetros de un tipo
		/// </summary>
		private ParameterModelCollection LoadParameterList(MLNode nodeML, string tag)
		{
			ParameterModelCollection parameters = new ParameterModelCollection();

				// Carga los parámetros
				foreach (MLNode objMLParameter in nodeML.Nodes)
					if (objMLParameter.Name == tag)
						parameters.Add(objMLParameter.Nodes[TagParameterName].Value,
									   objMLParameter.Nodes[TagParameterValue].Value);
				// Devuelve la colección de parámetros
				return parameters;
		}

		/// <summary>
		///		Carga los archivos
		/// </summary>
		private ReportExecutionFileModelCollection LoadFiles(MLNode nodeML)
		{
			ReportExecutionFileModelCollection files = new ReportExecutionFileModelCollection();

				// Recorre los nodos
				foreach (MLNode childML in nodeML.Nodes)
					if (childML.Name == TagFile)
					{
						ReportExecutionFileModel file = new ReportExecutionFileModel();

							// Añade las propiedades
							file.GlobalId = childML.Attributes[TagGlobalId].Value;
							file.IDType = ConvertFileType(childML.Attributes[TagFileType].Value);
							file.FileName = childML.Value;
							// Añade el archivo
							files.Add(file);
					}
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Convierte un tipo de archivo
		/// </summary>
		private ReportExecutionFileModel.FileType ConvertFileType(string type)
		{
			return type.GetEnum(ReportExecutionFileModel.FileType.Style);
		}

		/// <summary>
		///		Graba los datos de un informe
		/// </summary>
		internal void Save(ReportModel report, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Crea el directorio
				HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileName));
				// Añade los nodos
				nodeML.Nodes.Add(TagGlobalId, report.GlobalId);
				nodeML.Nodes.Add(TagName, report.Name);
				nodeML.Nodes.Add(TagDescription, report.Description);
				nodeML.Nodes.Add(TagReportDefinition, new HelperRepository().NormalizeContentSave(report.ReportDefinition));
				nodeML.Nodes.Add(TagLastExecutionParameterName, report.LastExecutionParameterName);
				nodeML.Nodes.Add(TagLastExecutionOuputMode, report.LastExecutionOuputMode);
				nodeML.Nodes.Add(TagLastExecutionFileName, report.LastExecutionFileName);
				// Graba los parámetros de ejecución
				foreach (ReportExecutionModel execution in report.ExecutionParameters)
				{
					MLNode executionML = nodeML.Nodes.Add(TagParametersRoot);

						// Añade los datos del parámetro de ejecución
						executionML.Nodes.Add(TagName, execution.Name);
						executionML.Nodes.Add(TagDescription, execution.Description);
						// Añade las conexiones
						executionML.Nodes.Add(base.GetConnectionsNodes(execution));
						// Añade los parámetros
						executionML.Nodes.AddRange(GetParameterNodes(execution.Parameters, TagParameter));
						executionML.Nodes.AddRange(GetParameterNodes(execution.FixedParameters, TagFixedParameter));
						// Añade los archivos
						foreach (ReportExecutionFileModel file in execution.Files)
							executionML.Nodes.Add(GetNode(file));
				}
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}

		/// <summary>
		///		Obtiene el nodo de archivo
		/// </summary>
		private MLNode GetNode(ReportExecutionFileModel file)
		{
			MLNode nodeML = new MLNode(TagFile);

				// Añade los atributos
				nodeML.Attributes.Add(TagGlobalId, file.GlobalId);
				nodeML.Attributes.Add(TagFileType, file.IDType.ToString());
				nodeML.Value = file.FileName;
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene los nodos de los parámetros
		/// </summary>
		private MLNodesCollection GetParameterNodes(ParameterModelCollection parameters, string tag)
		{
			MLNodesCollection parametersML = new MLNodesCollection();

				// Crea la colección de nodos
				foreach (ParameterModel parameter in parameters)
				{
					MLNode parameterML = new MLNode(tag);

						// Crea los valores del nodo
						parameterML.Nodes.Add(TagParameterName, parameter.ID);
						parameterML.Nodes.Add(TagParameterValue, parameter.Value?.ToString());
						// Añade el parámetro
						parametersML.Add(parameterML);
				}
				// Devuelve la colección de nodos
				return parametersML;
		}
	}
}
