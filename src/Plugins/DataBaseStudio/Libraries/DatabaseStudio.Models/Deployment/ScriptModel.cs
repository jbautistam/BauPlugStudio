using System;

namespace Bau.Libraries.DatabaseStudio.Models.Deployment
{
	/// <summary>
	///		Clase con los datos de generación de un script
	/// </summary>
    public class ScriptModel : LibDataStructures.Base.BaseModel
    {
		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string RelativeFileName { get; set; }

		/// <summary>
		///		Parámetros de generación
		/// </summary>
		public LibDataStructures.Base.BaseModelCollection<ParameterModel> Parameters { get; } = new LibDataStructures.Base.BaseModelCollection<ParameterModel>();
    }
}
