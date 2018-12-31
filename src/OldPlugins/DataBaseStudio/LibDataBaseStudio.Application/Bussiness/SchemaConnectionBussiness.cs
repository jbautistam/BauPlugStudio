using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDbProviders.SqlServer;
using Bau.Libraries.LibDataBaseStudio.Model.Connections;
using Bau.Libraries.LibDataBaseStudio.Model.Base;

namespace Bau.Libraries.LibDataBaseStudio.Application.Bussiness
{
	/// <summary>
	///		Clase de negocio de <see cref="SchemaConnectionModel"/>
	/// </summary>
	public class SchemaConnectionBussiness
	{   
		// Constantes públicas
		public const string ExtensionConnection = "sch";

		/// <summary>
		///		Carga una conexión
		/// </summary>
		public SchemaConnectionModel Load(string fileName)
		{
			return new Repository.SchemaConnectionRepository().Load(fileName);
		}

		/// <summary>
		///		Graba los datos de una conexión
		/// </summary>
		public void Save(SchemaConnectionModel connection, string fileName)
		{
			new Repository.SchemaConnectionRepository().Save(connection, fileName);
		}

		/// <summary>
		///		Carga las conexiones que existen en un directorio recursivamente
		/// </summary>
		public SchemaConnectionModelCollection LoadByPath(string projectPath)
		{
			System.Collections.Generic.List<string> files = new System.Collections.Generic.List<string>();
			SchemaConnectionModelCollection connections = new SchemaConnectionModelCollection();

				// Carga recursivamente los archivos de un directorio
				files = LibCommonHelper.Files.HelperFiles.ListRecursive(projectPath);
				// Carga las conexiones en la lista
				foreach (string fileName in files)
					if (fileName.EndsWith(ExtensionConnection, StringComparison.CurrentCultureIgnoreCase))
					{
						SchemaConnectionModel connection = new Repository.SchemaConnectionRepository().Load(fileName);

							if (!connection.Name.IsEmpty())
								connections.Add(connection);
					}
				// Devuelve la colección de conexiones
				return connections;
		}

		/// <summary>
		///		Carga las conexiones asociadas a un elemento con conexiones
		/// </summary>
		public SchemaConnectionModelCollection LoadByConnectionItem(string projectPath, ConnectionItemBase connectionItem)
		{
			SchemaConnectionModelCollection connections = LoadByPath(projectPath);

				// Quita las conexiones que no están asociadas al elemento
				if (connectionItem != null)
					for (int index = connections.Count - 1; index >= 0; index--)
						if (!connectionItem.ExistsConnection(connections[index]))
							connections.RemoveAt(index);
				// Devuelve la colección
				return connections;
		}

		/// <summary>
		///		Ejecuta una consulta sobre la conexión
		/// </summary>
		public System.Data.DataTable ProcessSelect(SchemaConnectionModel connection, string sql)
		{
			System.Data.DataTable table = null;

				// Carga los datos
				using (SqlServerProvider repository = new SqlServerProvider(new SqlServerConnectionString(connection.GetConnectionString())))
				{ 
					// Abre la conexión
					repository.Open();
					// Carga los datos
					table = repository.GetDataTable(sql, null, System.Data.CommandType.Text);
				}
				// Devuelve la tabla cargada
				return table;
		}
	}
}
