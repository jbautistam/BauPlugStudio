using System;

using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.Repository.Solutions;

namespace Bau.Libraries.LibDocWriter.Application.Bussiness.Solutions
{
	/// <summary>
	///		Clase de negocio para <see cref="SolutionModel"/>
	/// </summary>
	public class SolutionBussiness
	{
		/// <summary>
		///		Carga los datos de una solución
		/// </summary>
		public SolutionModel Load(string fileName)
		{
			return new SolutionRepository().Load(fileName);
		}

		/// <summary>
		///		Graba los datos de una solución
		/// </summary>
		public void Save(SolutionModel solution)
		{
			new SolutionRepository().Save(solution);
		}
	}
}
