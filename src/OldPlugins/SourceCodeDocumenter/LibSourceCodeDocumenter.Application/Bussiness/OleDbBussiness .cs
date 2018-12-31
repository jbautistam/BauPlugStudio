using System;

namespace Bau.Libraries.LibSourceCodeDocumenter.Application.Bussiness
{
	/// <summary>
	///		Clase de negocio de <see cref="Model.OleDbModel"/>
	/// </summary>
	public class OleDbBussiness
	{
		/// <summary>
		///		Carga una conexión
		/// </summary>
		public Model.OleDbModel Load(string fileName)
		{
			return new Repository.OleDbRepository().Load(fileName);
		}

		/// <summary>
		///		Graba los datos de una conexión
		/// </summary>
		public void Save(Model.OleDbModel oleDb, string fileName)
		{
			new Repository.OleDbRepository().Save(oleDb, fileName);
		}
	}
}
