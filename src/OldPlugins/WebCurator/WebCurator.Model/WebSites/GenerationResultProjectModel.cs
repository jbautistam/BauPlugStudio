using System;
using System.Collections.Generic;

namespace Bau.Libraries.WebCurator.Model.WebSites
{
	/// <summary>
	///		Resultado de la generación de un proyecto
	/// </summary>
	public class GenerationResultProjectModel
	{   
		// Contantes públicas
		public const string Extension = "rsml";

		/// <summary>
		///		Archivos de imagen origen
		/// </summary>
		public List<string> ImagesSource { get; set; } = new List<string>();
	}
}
