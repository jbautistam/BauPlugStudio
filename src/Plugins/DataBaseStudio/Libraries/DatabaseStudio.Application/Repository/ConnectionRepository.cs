using System;
using System.Collections.Generic;

using Bau.Libraries.DatabaseStudio.Models.Connections;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.DatabaseStudio.Application.Repository
{
	/// <summary>
	///		Repositorio para las conexiones a base de datos
	/// </summary>
	internal class ConnectionRepository : BaseRepository
	{
		// Constantes privadas
		private const string TagRoot = "DbScriptParameters";
		private const string TagDataBaseConnection = "DataBaseConnection";
		private const string TagType = "Type";
		private const string TagServer = "Server";
		private const string TagPort = "Port";
		private const string TagUser = "User";
		private const string TagPassword = "Password";
		private const string TagIntegratedSecurity = "IntegratedSecurity";
		private const string TagDataBase = "Database";
		private const string TagFilename = "Filename";
		private const string TagConnectionString = "Connectionstring";
		private const string TagParameters = "Parameters";

		/// <summary>
		///		Carga el archivo de parámetros
		/// </summary>
		internal (List<DatabaseConnectionModel> connections, string parameters) Load(string fileName)
		{
			(List<DatabaseConnectionModel> connections, string parameters) result = (new List<DatabaseConnectionModel>(), string.Empty);
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

				// Lee los datos
				if (fileML != null)
					foreach (MLNode rootML in fileML.Nodes)
						if (rootML.Name == TagRoot)
							foreach (MLNode nodeML in rootML.Nodes)
								switch (nodeML.Name)
								{
									case TagDataBaseConnection:
											result.connections.Add(LoadConnection(nodeML));
										break;
									case TagParameters:
											result.parameters = nodeML.Value;
										break;
								}
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Carga las conexiones
		/// </summary>
		internal ConnectionModelCollection LoadConnections(MLNode rootML)
		{
			ConnectionModelCollection connections = new ConnectionModelCollection();

				// Carga las conexiones
				foreach (MLNode nodeML in rootML.Nodes)
					switch (nodeML.Name)
					{
						case TagDataBaseConnection:
								connections.Add(LoadConnection(nodeML));
							break;
					}
				// Devuelve las conexiones
				return connections;
		}

		/// <summary>
		///		Carga los datos de una conexión
		/// </summary>
		private DatabaseConnectionModel LoadConnection(MLNode rootML)
		{
			DatabaseConnectionModel connection = new DatabaseConnectionModel();

				// Asigna las propiedades
				LoadBase(rootML, connection);
				connection.Type = rootML.Attributes[TagType].Value.GetEnum(DatabaseConnectionModel.DataBaseType.Odbc);
				connection.Server = rootML.Attributes[TagServer].Value;
				connection.Port = rootML.Attributes[TagPort].Value.GetInt(0);
				connection.User = rootML.Attributes[TagUser].Value;
				connection.Password = rootML.Attributes[TagPassword].Value;
				connection.IntegratedSecurity = rootML.Attributes[TagIntegratedSecurity].Value.GetBool();
				connection.DataBase = rootML.Attributes[TagDataBase].Value;
				connection.FileName = rootML.Nodes[TagFilename].Value;
				connection.ConnectionString = rootML.Nodes[TagConnectionString].Value;
				// Devuelve la conexión creada
				return connection;
		}

		/// <summary>
		///		Carga el archivo de parámetros
		/// </summary>
		internal void Save(string fileName, List<DatabaseConnectionModel> connections, string parameters)
		{
			MLFile fileML = new MLFile();
			MLNode rootML = fileML.Nodes.Add(TagRoot);

				// Añade las conexiones
				foreach (DatabaseConnectionModel connection in connections)
					rootML.Nodes.Add(GetMLConnection(connection));
				// Añade los parámetros
				rootML.Nodes.Add(TagParameters, parameters);
				// Graba el archivo
				LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileName));
				new LibMarkupLanguage.Services.XML.XMLWriter().Save(fileName, fileML);
		}

		/// <summary>
		///		Obtiene los nodos de las conexiones
		/// </summary>
		internal MLNodesCollection GetMLConnections(ConnectionModelCollection connections)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

				// Agrega los nodos
				foreach (AbstractConnectionModel baseConnection in connections)
					switch (baseConnection)
					{
						case DatabaseConnectionModel connection:
								nodesML.Add(GetMLConnection(connection));
							break;
					}
				// Devuelve la colección de nodos
				return nodesML;
		}

		/// <summary>
		///		Obtiene los datos de un nodo para una conexión
		/// </summary>
		private MLNode GetMLConnection(DatabaseConnectionModel connection)
		{
			MLNode rootML = GetMLNodeBase(TagDataBaseConnection, connection);

				// Asigna las propiedades
				 rootML.Attributes.Add(TagType, connection.Type.ToString());
				 rootML.Attributes.Add(TagServer, connection.Server);
				 rootML.Attributes.Add(TagPort, connection.Port);
				 rootML.Attributes.Add(TagUser, connection.User);
				 rootML.Attributes.Add(TagPassword, connection.Password);
				 rootML.Attributes.Add(TagIntegratedSecurity, connection.IntegratedSecurity);
				 rootML.Attributes.Add(TagDataBase, connection.DataBase);
				 rootML.Nodes.Add(TagFilename, connection.FileName);
				 rootML.Nodes.Add(TagConnectionString, connection.ConnectionString);
				// Devuelve el nodo de conexión
				return rootML;
		}
	}
}
