using System;

using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.Application.Bussiness.Solutions
{
	/// <summary>
	///		Clase de negocio para <see cref="SolutionModel"/>
	/// </summary>
	public class SolutionBussiness
	{
		/// <summary>
		///		Carga los datos de una solución
		/// </summary>
		public SolutionModel Load(Model.Definitions.ProjectDefinitionModelCollection definitions, string fileName)
		{
			return new Repository.SolutionRepository().Load(definitions, fileName);
		}

		/// <summary>
		///		Graba los datos de una solución
		/// </summary>
		public void Save(SolutionModel solution)
		{
			new Repository.SolutionRepository().Save(solution);
		}
	}
}
