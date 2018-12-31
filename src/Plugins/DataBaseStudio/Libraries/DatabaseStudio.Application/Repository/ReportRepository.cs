using System;

using Bau.Libraries.LibDataStructures.Collections;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.DatabaseStudio.Models.Reports;

namespace Bau.Libraries.DatabaseStudio.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="ReportModel"/>
	/// </summary>
	internal class ReportRepository : BaseRepository
	{   
		// Constantes privadas
		private const string TagRoot = "Report";
		[Obsolete("Esto hay que quitarlo")]
		private const string TagRootOld = "Documenter";
		private const string TagId = "Id";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagReportDefinition = "ReportDefinition";
		private const string TagParameter = "Parameter";
		private const string TagFixedParameter = "FixedParameter";
		private const string TagParameterName = "Name";
		private const string TagParameterValue = "Value";

		/// <summary>
		///		Carga los datos de un informe
		/// </summary>
		internal ReportModel Load(string fileName)
		{
			ReportModel report = new ReportModel();
			MLFile fileML = LoadFile(fileName);

				// Carga los datos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name.Equals(TagRoot) || nodeML.Name.Equals(TagRootOld))
						{ 
							// Carga las propiedades
							report.GlobalId = nodeML.Nodes[TagId].Value;
							report.Name = nodeML.Nodes[TagName].Value;
							report.Description = nodeML.Nodes[TagDescription].Value;
							report.ReportDefinition = NormalizeContentLoad(nodeML.Nodes[TagReportDefinition].Value);
							// Carga los parámetros de ejecución
							report.Parameters.AddRange(LoadParameterList(nodeML, TagParameter));
							report.FixedParameters.AddRange(LoadParameterList(nodeML, TagFixedParameter));
						}
				// Devuelve el informe
				return report;
		}

		/// <summary>
		///		Carga los parámetros de un tipo
		/// </summary>
		private ParameterModelCollection LoadParameterList(MLNode nodeML, string tag)
		{
			ParameterModelCollection parameters = new ParameterModelCollection();

				// Carga los parámetros
				foreach (MLNode parameterML in nodeML.Nodes)
					if (parameterML.Name == tag)
						parameters.Add(parameterML.Nodes[TagParameterName].Value,
									   parameterML.Nodes[TagParameterValue].Value);
				// Devuelve la colección de parámetros
				return parameters;
		}

		/// <summary>
		///		Graba los datos de un informe
		/// </summary>
		internal void Save(ReportModel report, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Añade los nodos
				nodeML.Nodes.Add(TagId, report.GlobalId);
				nodeML.Nodes.Add(TagName, report.Name);
				nodeML.Nodes.Add(TagDescription, report.Description);
				nodeML.Nodes.Add(TagReportDefinition, NormalizeContentSave(report.ReportDefinition));
				// Añade los parámetros
				nodeML.Nodes.AddRange(GetParameterNodes(report.Parameters, TagParameter));
				nodeML.Nodes.AddRange(GetParameterNodes(report.FixedParameters, TagFixedParameter));
				// Graba el archivo
				SaveFile(fileName, fileML);
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
