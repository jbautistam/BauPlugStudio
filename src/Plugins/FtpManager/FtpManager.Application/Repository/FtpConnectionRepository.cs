using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.FtpManager.Model.Connections;

namespace Bau.Libraries.FtpManager.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="FtpConnectionModel"/>
	/// </summary>
	internal class FtpConnectionRepository
	{   
		// Constantes privadas
		private const string TagRoot = "Connections";
		private const string TagConnection = "Connection";
		private const string TagGlobalId = "GlobalId";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagServer = "Server";
		private const string TagPort = "Port";
		private const string TagProtocol = "Protocol";
		private const string TagUser = "User";
		private const string TagPassword = "Password";
		private const string TagTimeOut = "TimeOut";

		/// <summary>
		///		Carga los datos de esquema de un archivo de conexión
		/// </summary>
		internal FtpConnectionModel Load(string fileName)
		{
			FtpConnectionModel connection = new FtpConnectionModel();
			MLFile fileML = new XMLParser().Load(fileName);

				// Carga los datos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name.Equals(TagConnection))
						{ 
							// Carga las propiedades de la conexión
							connection.GlobalId = nodeML.Attributes[TagGlobalId].Value;
							connection.Server = nodeML.Attributes[TagServer].Value;
							connection.FtpProtocol = (FtpConnectionModel.Protocol) nodeML.Attributes[TagProtocol].Value.GetInt(0);
							connection.Port = nodeML.Attributes[TagPort].Value.GetInt(20);
							connection.TimeOut = nodeML.Attributes[TagTimeOut].Value.GetInt(100);
							// Carga los nodos
							connection.Name = nodeML.Nodes[TagName].Value;
							connection.Description = nodeML.Nodes[TagDescription].Value;
							connection.User = nodeML.Nodes[TagUser].Value;
							connection.Password = nodeML.Nodes[TagPassword].Value;
						}
				// Devuelve la conexión
				return connection;
		}

		/// <summary>
		///		Graba los datos de una conexión
		/// </summary>
		internal void Save(FtpConnectionModel connection, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagConnection);

				// Crea el directorio
				HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileName));
				// Añade los atributos de la conexión
				nodeML.Attributes.Add(TagGlobalId, connection.GlobalId);
				nodeML.Attributes.Add(TagServer, connection.Server);
				nodeML.Attributes.Add(TagProtocol, (int) connection.FtpProtocol);
				nodeML.Attributes.Add(TagPort, connection.Port);
				nodeML.Attributes.Add(TagTimeOut, connection.TimeOut);
				// Añade los nodos de la conexión
				nodeML.Nodes.Add(TagName, connection.Name);
				nodeML.Nodes.Add(TagDescription, connection.Description);
				nodeML.Nodes.Add(TagServer, connection.Server);
				nodeML.Nodes.Add(TagUser, connection.User);
				nodeML.Nodes.Add(TagPassword, connection.Password);
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}
	}
}