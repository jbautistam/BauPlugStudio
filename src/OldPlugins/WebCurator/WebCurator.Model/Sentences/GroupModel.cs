using System;

namespace Bau.Libraries.WebCurator.Model.Sentences
{
	/// <summary>
	///		Modelo con los datos de un grupo
	/// </summary>
	public class GroupModel : LibDataStructures.Base.BaseModel
	{
		/// <summary>
		///		Nivel
		/// </summary>
		public string Level { get; set; }

		/// <summary>
		///		Orden del grupo
		/// </summary>
		public int Order { get; set; }

		/// <summary>
		///		Máximo número de frases del grupo
		/// </summary>
		public int Maximum { get; set; }

		/// <summary>
		///		Probabilidad de que se genere un bloque de sentencias con este grupo
		/// </summary>
		public double Probability { get; set; }

		/// <summary>
		///		Frases del grupo
		/// </summary>
		public SentenceModelCollection Sentences { get; } = new SentenceModelCollection();
	}
}
