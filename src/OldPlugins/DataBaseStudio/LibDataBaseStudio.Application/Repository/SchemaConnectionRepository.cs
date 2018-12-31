using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.LibDataBaseStudio.Model.Connections;

namespace Bau.Libraries.LibDataBaseStudio.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="SchemaConnection"/>
	/// </summary>
	internal class SchemaConnectionRepository
	{   
		// Constantes privadas
		private const string TagRoot = "Connections";
		private const string TagConnection = "Connection";
		private const string TagGlobalId = "GlobalId";
		private const string TagName = "Name";
		private const string TagServer = "Server";
		private const string TagDataBase = "DataBase";
		private const string TagConnectToFileDataBase = "ConnectToFileDataBase";
		private const string TagDataBaseFileName = "DataBaseFileName";
		private const string TagUser = "User";
		private const string TagPassword = "Password";
		private const string TagUseWindowsAuthentification = "UseWindowsAuthentification";
		private const string TagTimeOut = "TimeOut";

		/// <summary>
		///		Carga los datos de esquema de un archivo de conexión
		/// </summary>
		internal SchemaConnectionModel Load(string fileName)
		{
			SchemaConnectionModel connection = new SchemaConnectionModel();
			MLFile fileML = new XMLParser().Load(fileName);

				// Carga la conexión
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name.Equals(TagConnection))
						{ 
							connection.GlobalId = nodeML.Nodes[TagGlobalId].Value;
							connection.Name = nodeML.Nodes[TagName].Value;
							connection.Server = nodeML.Nodes[TagServer].Value;
							connection.DataBase = nodeML.Nodes[TagDataBase].Value;
							connection.ConnectToFileDataBase = nodeML.Nodes[TagConnectToFileDataBase].Value.GetBool();
							connection.DataBaseFileName = nodeML.Nodes[TagDataBaseFileName].Value;
							connection.User = nodeML.Nodes[TagUser].Value;
							connection.Password = nodeML.Nodes[TagPassword].Value;
							connection.UseWindowsAuthentification = nodeML.Nodes[TagUseWindowsAuthentification].Value.GetBool();
							connection.TimeOut = nodeML.Nodes[TagTimeOut].Value.GetInt(100);
						}
				// Devuelve la conexión
				return connection;
		}

		/// <summary>
		///		Graba los datos de una conexión
		/// </summary>
		internal void Save(SchemaConnectionModel connection, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagConnection);

				// Crea el directorio
				HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileName));
				// Añade los nodos
				nodeML.Nodes.Add(TagGlobalId, connection.GlobalId);
				nodeML.Nodes.Add(TagName, connection.Name);
				nodeML.Nodes.Add(TagServer, connection.Server);
				nodeML.Nodes.Add(TagDataBase, connection.DataBase);
				nodeML.Nodes.Add(TagConnectToFileDataBase, connection.ConnectToFileDataBase);
				nodeML.Nodes.Add(TagDataBaseFileName, connection.DataBaseFileName);
				nodeML.Nodes.Add(TagUser, connection.User);
				nodeML.Nodes.Add(TagPassword, connection.Password);
				nodeML.Nodes.Add(TagUseWindowsAuthentification, connection.UseWindowsAuthentification);
				nodeML.Nodes.Add(TagTimeOut, connection.TimeOut);
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}
	}
}