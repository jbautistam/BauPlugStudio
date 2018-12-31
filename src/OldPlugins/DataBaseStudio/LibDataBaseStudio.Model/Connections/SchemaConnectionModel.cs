using System;

namespace Bau.Libraries.LibDataBaseStudio.Model.Connections
{
	/// <summary>
	///		Clase con los datos de una conexión
	/// </summary>
	public class SchemaConnectionModel : LibDataStructures.Base.BaseExtendedModel
	{ 
		// Variables privadas
		private string _name;

		/// <summary>
		///		Obtiene la cadena de conexión
		/// </summary>
		public string GetConnectionString()
		{
			if (ConnectToFileDataBase)
				return $"Data Source={Server};AttachDbFilename=\"{DataBaseFileName}\";Connect Timeout={TimeOut};Integrated Security={UseWindowsAuthentification};";
			else
				return $"Server={Server};Uid={User};Pwd={Password};DataBase={DataBase};Integrated Security={UseWindowsAuthentification};Connect TimeOut={TimeOut}";
		}

		/// <summary>
		///		Nombre descriptivo de la conexión
		/// </summary>
		public override string Name
		{
			get
			{ 
				// Crea el nombre si no estaba en memoria
				if (string.IsNullOrEmpty(_name))
					_name = Server + " - " + DataBase;
				// Devuelve el nombre
				return _name;
			}
			set { _name = value; }
		}

		/// <summary>
		///		Nombre del servidor
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		///		Indica si se debe conectar a un archivo de base de datos
		/// </summary>
		public bool ConnectToFileDataBase { get; set; }

		/// <summary>
		///		Base de datos (o archivo de base de datos)
		/// </summary>
		public string DataBase { get; set; }

		/// <summary>
		///		Nombre de archivo de base de datos
		/// </summary>
		public string DataBaseFileName { get; set; }

		/// <summary>
		///		Indica si se va a utilizar autentificación de Windows
		/// </summary>
		public bool UseWindowsAuthentification { get; set; }

		/// <summary>
		///		Tiempo de espera
		/// </summary>
		public int TimeOut { get; set; } = 20;

		/// <summary>
		///		Usuario de SQL Server
		/// </summary>
		public string User { get; set; }

		/// <summary>
		///		Contraseña de SQL Server
		/// </summary>
		public string Password { get; set; }
	}
}
