using System;

namespace Bau.Libraries.LibSourceCodeDocumenter.Application.Bussiness
{
	/// <summary>
	///		Clase de negocio de <see cref="Model.SqlServerModel"/>
	/// </summary>
	public class SqlServerBussiness
	{
		/// <summary>
		///		Carga una conexión
		/// </summary>
		public Model.SqlServerModel Load(string fileName)
		{
			return new Repository.SqlServerRepository().Load(fileName);
		}

		/// <summary>
		///		Graba los datos de una conexión
		/// </summary>
		public void Save(Model.SqlServerModel sqlServer, string fileName)
		{
			new Repository.SqlServerRepository().Save(sqlServer, fileName);
		}
	}
}
