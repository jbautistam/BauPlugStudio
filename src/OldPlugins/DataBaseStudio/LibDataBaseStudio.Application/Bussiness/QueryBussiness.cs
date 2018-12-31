using System;

using Bau.Libraries.LibDataBaseStudio.Model.Queries;

namespace Bau.Libraries.LibDataBaseStudio.Application.Bussiness
{
	/// <summary>
	///		Clase de negocio de <see cref="QueryModel"/>
	/// </summary>
	public class QueryBussiness
	{
		/// <summary>
		///		Carga una consulta
		/// </summary>
		public QueryModel Load(string fileName)
		{
			return new Repository.QueryRepository().Load(fileName);
		}

		/// <summary>
		///		Graba los datos de una consulta
		/// </summary>
		public void Save(QueryModel query, string fileName)
		{
			new Repository.QueryRepository().Save(query, fileName);
		}
	}
}
