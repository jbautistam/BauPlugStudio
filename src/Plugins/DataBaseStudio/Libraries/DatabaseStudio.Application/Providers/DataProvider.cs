using System;
using System.Collections.Generic;
using System.Data;

using Bau.Libraries.Aggregator.Providers.Base;
using Bau.Libraries.Aggregator.Providers.SqLite;
using Bau.Libraries.Aggregator.Providers.SQLServer;

using Bau.Libraries.DatabaseStudio.Models.Connections;

namespace Bau.Libraries.DatabaseStudio.Application.Providers
{
	/// <summary>
	///		Proveedor de datos
	/// </summary>
    public class DataProvider
    {
		/// <summary>
		///		Carga el esquema de una conexión
		/// </summary>
		public LibDbProviders.Base.Schema.SchemaDbModel LoadSchema(AbstractConnectionModel connection)
		{
			IDataProvider provider = GetProvider(connection);

				// Carga el esquema del proveedor
				if (provider == null)
					return new LibDbProviders.Base.Schema.SchemaDbModel();
				else
					return provider.LoadSchema();
		}

		/// <summary>
		///		Carga datos de una conexión
		/// </summary>
		public DataTable LoadData(AbstractConnectionModel connection, string command, Dictionary<string, object> parameters, 
								  int pageIndex, int pageSize, out long records)
		{
			IDataProvider provider = GetProvider(connection);

				// Inicializa los argumentos de salida
				records = 0;
				// Carga los datos
				if (provider == null)
					return new DataTable();
				else
					return provider.LoadData(GetProviderDataBaseCommand(command, parameters), pageIndex, pageSize, out records);
		}

		/// <summary>
		///		Obtiene el proveedor de una conexión abstracta
		/// </summary>
		private IDataProvider GetProvider(AbstractConnectionModel connection)
		{
			switch (connection)
			{
				case DatabaseConnectionModel dbConnection:
					return GetProvider(dbConnection);
				default:
					return null;
			}
		}

		/// <summary>
		///		Obtiene un comando para una base de datos
		/// </summary>
		private DataProviderCommand GetProviderDataBaseCommand(string sql, Dictionary<string, object> parameters)
		{
			DataProviderCommand command = new DataProviderCommand();

				// Añade el comando
				command.Sentences.Add("Sentence", sql);
				command.Parameters = parameters;
				// Devuelve el comando
				return command;
		}

		/// <summary>
		///		Obtiene el esquema de una conexión
		/// </summary>
		private IDataProvider GetProvider(DatabaseConnectionModel connection)
		{
			switch (connection.Type)
			{
				case DatabaseConnectionModel.DataBaseType.SqLite:
					return new ScriptsSqLiteProvider(connection.Name, connection.Type.ToString(), connection.FileName);
				case DatabaseConnectionModel.DataBaseType.SqlServer:
					return new ScriptsSqlServerProvider(connection.Name, connection.Type.ToString(), connection.Server,
														connection.Port, connection.DataBase, connection.User,
														connection.Password, connection.IntegratedSecurity);
				default:
					return null;
			}
		}
	}
}
