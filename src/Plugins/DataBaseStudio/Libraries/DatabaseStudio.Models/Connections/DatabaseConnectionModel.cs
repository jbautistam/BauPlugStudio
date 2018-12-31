using System;

namespace Bau.Libraries.DatabaseStudio.Models.Connections
{
	/// <summary>
	///		Conexión a base de datos
	/// </summary>
	public class DatabaseConnectionModel : AbstractConnectionModel
	{
		/// <summary>
		///		Tipo de conexión a base de datos
		/// </summary>
		public enum DataBaseType
		{
			/// <summary>Conexión por ODBC</summary>
			Odbc,
			/// <summary>Conexión por SQL Server</summary>
			SqlServer,
			/// <summary>Conexión por MySql</summary>
			MySql,
			/// <summary>Conexión con PostgreSql</summary>
			PostgreSql,
			/// <summary>Conexión a base de datos SqLite</summary>
			SqLite
		}

		/// <summary>
		///		Comprueba si una conexión es de servidor
		/// </summary>
		public static bool CheckIsServerConnection(DataBaseType? type)
		{
			return type == DataBaseType.MySql || type == DataBaseType.SqlServer || type == DataBaseType.PostgreSql;
		}

		/// <summary>
		///		Comprueba si un tipo de conexión admite seguridad integrada
		/// </summary>
		public static bool CheckCanUserIntegratedSecurity(DataBaseType? type)
		{
			return type == DataBaseType.SqlServer || type == DataBaseType.PostgreSql;
		}

		/// <summary>
		///		Obtiene el puerto predeterminado
		/// </summary>
		public static int GetDefaultPort(DataBaseType? type)
		{
			switch (type)
			{
				//case DataBaseType.Db2:
				//	return 50_000;
				case DataBaseType.MySql:
					return 3_306;
				case DataBaseType.PostgreSql:
					return 5_432;
				default:
					return 0;
			}
		}

		/// <summary>
		///		Indica si utiliza una cadena de conexión
		/// </summary>
		public static bool CheckUseConnectionString(DataBaseType? type)
		{
			return type == DataBaseType.Odbc;
		}

		/// <summary>
		///		Tipo de base de datos
		/// </summary>
		public DataBaseType Type { get; set; }

		/// <summary>
		///		Indica si es una conexión con un servidor asociado (Sql Server, MySql...)
		/// </summary>
		public bool IsServerConnection
		{
			get { return CheckIsServerConnection(Type); }
		}

		/// <summary>
		///		Indica si utiliza una cadena de conexión
		/// </summary>
		public bool UseConnectionString
		{
			get { return CheckUseConnectionString(Type); }
		}

		/// <summary>
		///		Indica si se puede utilizar seguridad integrada para este tipo de conexión
		/// </summary>
		public bool CanUseIntegratedSecurity
		{
			get { return CheckCanUserIntegratedSecurity(Type); }
		}

		/// <summary>
		///		Puerto predeterminado
		/// </summary>
		public int DefaultPort
		{
			get { return GetDefaultPort(Type); }
		}

		/// <summary>
		///		Servidor
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		///		Número de puerto
		/// </summary>
		public int Port { get; set; }

		/// <summary>
		///		Usuario
		/// </summary>
		public string User { get; set; }

		/// <summary>
		///		Contraseña
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		///		Indica si se utiliza seguridad integrada
		/// </summary>
		public bool IntegratedSecurity { get; set; }

		/// <summary>
		///		Base de datos
		/// </summary>
		public string DataBase { get; set; }

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		///		Cadena de conexión
		/// </summary>
		public string ConnectionString { get; set; }
	}
}
