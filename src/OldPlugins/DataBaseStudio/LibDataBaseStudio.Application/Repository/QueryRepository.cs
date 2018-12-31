using System;

using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.LibDataBaseStudio.Model.Queries;

namespace Bau.Libraries.LibDataBaseStudio.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="QueryModel"/>
	/// </summary>
	internal class QueryRepository : ConnectionItemRepositoryBase
	{   
		// Constantes privadas
		private const string TagRoot = "Query";
		private const string TagGlobalId = "GlobalId";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagSQL = "Sql";
		private const string TagConnection = "Connection";

		/// <summary>
		///		Carga los datos de una consulta
		/// </summary>
		internal QueryModel Load(string fileName)
		{
			QueryModel report = new QueryModel();
			HelperRepository helper = new HelperRepository();
			MLFile fileML = new XMLParser().Load(fileName);

				// Carga los datos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name.Equals(TagRoot))
						{ 
							// Carga las propiedades
							report.GlobalId = nodeML.Nodes[TagGlobalId].Value;
							report.Name = nodeML.Nodes[TagName].Value;
							report.Description = nodeML.Nodes[TagDescription].Value;
							report.SQL = helper.NormalizeContentLoad(nodeML.Nodes[TagSQL].Value);
							report.LastConnectionGuid = nodeML.Nodes[TagConnection].Value;
						}
				// Devuelve el informe
				return report;
		}

		/// <summary>
		///		Graba los datos de una consulta
		/// </summary>
		internal void Save(QueryModel query, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Crea el directorio
				HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileName));
				// Añade los nodos
				nodeML.Nodes.Add(TagGlobalId, query.GlobalId);
				nodeML.Nodes.Add(TagName, query.Name);
				nodeML.Nodes.Add(TagDescription, query.Description);
				nodeML.Nodes.Add(TagSQL, new HelperRepository().NormalizeContentSave(query.SQL));
				nodeML.Nodes.Add(TagConnection, query.LastConnectionGuid);
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}
	}
}
