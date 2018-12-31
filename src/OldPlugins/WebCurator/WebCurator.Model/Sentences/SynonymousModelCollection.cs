using System;
using System.Collections.Generic;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.WebCurator.Model.Sentences
{
	/// <summary>
	///		Colección de <see cref="SynonymousModel"/>
	/// </summary>
	public class SynonymousModelCollection : List<SynonymousModel>
	{
		/// <summary>
		///		Busca un sinónimo por su nombre
		/// </summary>
		public SynonymousModel Search(string name)
		{ 
			// Quita el identificador del sinónimo
			if (!name.IsEmpty() && (name.StartsWith("~") || name.StartsWith("@")))
				name = name.Substring(1);
			// Devuelve el primer sinónimo que encuentra
			return this.FirstOrDefault<SynonymousModel>(synonymous => synonymous.Name.EqualsIgnoreCase(name));
		}

		/// <summary>
		///		Obtiene un sinónimo
		/// </summary>
		public string SearchSynonymous(Random rnd, string name)
		{
			SynonymousModel synonymous = Search(name);

				if (synonymous != null && synonymous.Values.Count > 0)
					return synonymous.Values[rnd.Next(synonymous.Values.Count)].TrimIgnoreNull();
				else
					return "";
		}
	}
}
