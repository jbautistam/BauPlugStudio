using System;
using System.Collections.Generic;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.DevConference.Application.Models
{
	/// <summary>
	///		Colección de <see cref="BaseModel"/>
	/// </summary>
    public abstract class BaseModelCollection<TypeData> : List<TypeData> where TypeData : BaseModel
    {
		/// <summary>
		///		Busca un <see cref="TypeData"/> en la colección
		/// </summary>
		public TypeData Search(string id)
		{
			if (this == null)
				return null;
			else
				return this.FirstOrDefault(item => item.Id.EqualsIgnoreCase(id));
		}

		/// <summary>
		///		Busca un <see cref="TypeData"/> por Id o por título
		/// </summary>
		public TypeData Search(string id, string title)
		{
			TypeData found = Search(id);

				// Si no se ha encontrado, busca por título
				if (found == null)
					found = this.FirstOrDefault(item => item.Title.EqualsIgnoreCase(title));
				// Devuelve el elemento encontrado
				return found;
		}

		/// <summary>
		///		Ordena los elementos
		/// </summary>
		public virtual void SortByTitle(bool ascending = true)
		{
			Sort((first, second) => (ascending ? 1 : -1) * first.Title.CompareTo(second.Title));
		}

		/// <summary>
		///		Borra un elemento por su Id
		/// </summary>
		public void DeleteById(string id)
		{
			for (int index = Count - 1; index >= 0; index--)
				if (this[index].Id.EqualsIgnoreCase(id))
					RemoveAt(index);
		}
    }
}
