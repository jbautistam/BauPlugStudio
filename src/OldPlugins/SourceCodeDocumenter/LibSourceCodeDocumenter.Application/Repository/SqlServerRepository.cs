using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;

namespace Bau.Libraries.LibSourceCodeDocumenter.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="SqlServerModel"/>
	/// </summary>
	internal class SqlServerRepository
	{   
		// Constantes privadas
		private const string TagRoot = "SqlServer";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagServer = "Server";
		private const string TagDataBase = "DataBase";
		private const string TagUseDataBaseFile = "UseDataBaseFile";
		private const string TagDataBaseFile = "DataBaseFile";
		private const string TagUseWindowsAuthentification = "UseWindowsAuthentification";
		private const string TagUser = "User";
		private const string TagPassword = "Password";

		/// <summary>
		///		Carga los datos de documentación
		/// </summary>
		internal Model.SqlServerModel Load(string fileName)
		{
			Model.SqlServerModel sqlServer = new Model.SqlServerModel();
			MLFile fileML = new XMLParser().Load(fileName);

				// Carga los datos de documentación
				foreach (MLNode nodeML in fileML.Nodes)
					if (nodeML.Name.Equals(TagRoot))
					{
						sqlServer.Name = nodeML.Nodes[TagName].Value;
						sqlServer.Description = nodeML.Nodes[TagDescription].Value;
						sqlServer.Server = nodeML.Nodes[TagServer].Value;
						sqlServer.DataBase = nodeML.Nodes[TagDataBase].Value;
						sqlServer.UseDataBaseFile = nodeML.Nodes[TagUseDataBaseFile].Value.GetBool();
						sqlServer.DataBaseFile = nodeML.Nodes[TagDataBaseFile].Value;
						sqlServer.UseWindowsAuthentification = nodeML.Nodes[TagUseWindowsAuthentification].Value.GetBool();
						sqlServer.User = nodeML.Nodes[TagUser].Value;
						sqlServer.Password = nodeML.Nodes[TagPassword].Value;
					}
				// Devuelve los datos de documentación
				return sqlServer;
		}

		/// <summary>
		///		Graba los datos de una conexión
		/// </summary>
		internal void Save(Model.SqlServerModel sqlServer, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Añade los datos de generación
				nodeML.Nodes.Add(TagName, sqlServer.Name);
				nodeML.Nodes.Add(TagDescription, sqlServer.Description);
				nodeML.Nodes.Add(TagServer, sqlServer.Server);
				nodeML.Nodes.Add(TagDataBase, sqlServer.DataBase);
				nodeML.Nodes.Add(TagUseDataBaseFile, sqlServer.UseDataBaseFile);
				nodeML.Nodes.Add(TagDataBaseFile, sqlServer.DataBaseFile);
				nodeML.Nodes.Add(TagUseWindowsAuthentification, sqlServer.UseWindowsAuthentification);
				nodeML.Nodes.Add(TagUser, sqlServer.User);
				nodeML.Nodes.Add(TagPassword, sqlServer.Password);
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}
	}
}