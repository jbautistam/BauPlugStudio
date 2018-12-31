using System;

namespace Bau.Libraries.WebCurator.Model.Sentences
{
	/// <summary>
	///		Clase con los datos de un sinónimo
	/// </summary>
	public class SynonymousModel
	{
		public SynonymousModel(string name)
		{
			Name = name;
		}

		/// <summary>
		///		Nombre del sinónimo
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Sinónimos
		/// </summary>
		public System.Collections.Generic.List<string> Values { get; set; } = new System.Collections.Generic.List<string>();
	}
}
