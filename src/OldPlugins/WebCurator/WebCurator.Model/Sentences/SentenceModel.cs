using System;

namespace Bau.Libraries.WebCurator.Model.Sentences
{
	/// <summary>
	///		Clase base para las frases
	/// </summary>
	public class SentenceModel : LibDataStructures.Base.BaseModel
	{
		/// <summary>
		///		Frase
		/// </summary>
		public string Sentence { get; set; }
	}
}
