using System;

namespace Bau.Libraries.WebCurator.Model.WebSites
{
	/// <summary>
	///		Resultado de la generación de un proyecto
	/// </summary>
	public class GenerationResultModel
	{ 
		// Contantes públicas
		public const string Extension = "rsml";

		/// <summary>
		///		Fecha de la última generación
		/// </summary>
		public DateTime DateLast { get; set; } = DateTime.Now.AddDays(-1);
	}
}