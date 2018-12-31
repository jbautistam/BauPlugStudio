using System;

using Bau.Libraries.DatabaseStudio.Models.Reports;

namespace Bau.Libraries.DatabaseStudio.Application.Bussiness
{
	/// <summary>
	///		Clase de negocio de <see cref="ReportModel"/>
	/// </summary>
	public class ReportBussiness
	{
		/// <summary>
		///		Carga un informe
		/// </summary>
		public ReportModel Load(string fileName)
		{
			return new Repository.ReportRepository().Load(fileName);
		}

		/// <summary>
		///		Graba los datos de una conexión
		/// </summary>
		public void Save(ReportModel report, string fileName)
		{
			new Repository.ReportRepository().Save(report, fileName);
		}
	}
}
