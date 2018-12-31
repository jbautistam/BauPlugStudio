using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDataBaseStudio.Model.Connections;
using Bau.Libraries.LibDbProviders.SqlServer;
using Bau.Libraries.LibDbProviders.Base.Schema;

namespace Bau.Libraries.LibDataBaseStudio.Application.Services
{
	/// <summary>
	///		Servicio para lectura de un esquema
	/// </summary>
	public class SchemaReader
	{
		/// <summary>
		///		Obtiene el esquema de una conexión
		/// </summary>
		public SchemaDbModel GetSchema(string fileName)
		{
			return GetSchema(new Bussiness.SchemaConnectionBussiness().Load(fileName));
		}

		/// <summary>
		///		Obtiene el esquema de una conexión
		/// </summary>
		public SchemaDbModel GetSchema(SchemaConnectionModel schemaConnection)
		{
			using (SqlServerProvider connection = new SqlServerProvider(GetConnectionString(schemaConnection)))
			{
				// Abre la conexión
				connection.Open();
				// Carga el esquema
				return connection.GetSchema();
			}
		}

		/// <summary>
		///		Obtiene una lista de tablas
		/// </summary>
		public List<TableDbModel> GetTables(SchemaConnectionModel schemaConnection)
		{
			return GetSchema(schemaConnection).Tables;
		}

		/// <summary>
		///		Obtiene una lista de vistas
		/// </summary>
		public List<TableDbModel> GetViews(SchemaConnectionModel schemaConnection)
		{
			return GetSchema(schemaConnection).Views;
		}

		/// <summary>
		///		Obtiene los procedimientos almacenados de una conexión
		/// </summary>
		public List<RoutineDbModel> GetStoredProcedures(SchemaConnectionModel schemaConnection)
		{
			return GetSchema(schemaConnection).Routines;
		}

		/// <summary>
		///		Obtiene la cadena de conexión de SQL Server
		/// </summary>
		private SqlServerConnectionString GetConnectionString(SchemaConnectionModel schemaConnection)
		{
			SqlServerConnectionString connectionString = new SqlServerConnectionString(schemaConnection.Server, schemaConnection.User,
																					   schemaConnection.Password, schemaConnection.DataBase,
																					   schemaConnection.UseWindowsAuthentification);

				// Asigna el resto de las propiedades
				if (!schemaConnection.DataBaseFileName.IsEmpty())
					connectionString.DataBaseFile = schemaConnection.DataBaseFileName;
				// Devuelve la cadena de conexión
				return connectionString;
		}
	}
}
