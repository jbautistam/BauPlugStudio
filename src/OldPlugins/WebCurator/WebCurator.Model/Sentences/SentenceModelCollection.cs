using System;

namespace Bau.Libraries.WebCurator.Model.Sentences
{
	/// <summary>
	///		Colección de <see cref="SentenceModel"/>
	/// </summary>
	public class SentenceModelCollection : LibDataStructures.Base.BaseModelCollection<SentenceModel>
	{
		/// <summary>
		///		Añade una nueva sentencia
		/// </summary>
		public void Add(string sentence)
		{
			Add(new SentenceModel { Sentence = sentence });
		}

		/// <summary>
		///		Obtiene una sentencia aleatoriamente
		/// </summary>
		public SentenceModel GetRandom(Random rnd)
		{
			if (Count == 0)
				return new SentenceModel();
			else
				return this[rnd.Next(Count)];
		}
	}
}
