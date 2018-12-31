using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.DatabaseStudio.Models.Queries;

namespace Bau.Libraries.DatabaseStudio.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="QueryModel"/>
	/// </summary>
	internal class QueryRepository : BaseRepository
	{   
		// Constantes privadas
		private const string TagRoot = "Query";
		private const string TagGlobalId = "GlobalId";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagSQL = "Sql";

		/// <summary>
		///		Carga los datos de una consulta
		/// </summary>
		internal QueryModel Load(string fileName)
		{
			QueryModel report = new QueryModel();
			MLFile fileML = LoadFile(fileName);

				// Carga los datos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name.Equals(TagRoot))
						{ 
							// Carga las propiedades
							report.GlobalId = nodeML.Nodes[TagGlobalId].Value;
							report.Name = nodeML.Nodes[TagName].Value;
							report.Description = nodeML.Nodes[TagDescription].Value;
							report.SQL = NormalizeContentLoad(nodeML.Nodes[TagSQL].Value);
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

				// Añade los nodos
				nodeML.Nodes.Add(TagGlobalId, query.GlobalId);
				nodeML.Nodes.Add(TagName, query.Name);
				nodeML.Nodes.Add(TagDescription, query.Description);
				nodeML.Nodes.Add(TagSQL, NormalizeContentSave(query.SQL));
				// Graba el archivo
				SaveFile(fileName, fileML);
		}
	}
}
