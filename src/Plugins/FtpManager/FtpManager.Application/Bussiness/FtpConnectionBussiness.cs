using System;

using Bau.Libraries.FtpManager.Model.Connections;

namespace Bau.Libraries.FtpManager.Application.Bussiness
{
	/// <summary>
	///		Clase de negocio de <see cref="FtpConnectionModel"/>
	/// </summary>
	public class FtpConnectionBussiness
	{
		/// <summary>
		///		Carga una conexión
		/// </summary>
		public FtpConnectionModel Load(string fileName)
		{
			return new Repository.FtpConnectionRepository().Load(fileName);
		}

		/// <summary>
		///		Graba los datos de una conexión
		/// </summary>
		public void Save(FtpConnectionModel ftpConnection, string fileName)
		{
			new Repository.FtpConnectionRepository().Save(ftpConnection, fileName);
		}
	}
}
