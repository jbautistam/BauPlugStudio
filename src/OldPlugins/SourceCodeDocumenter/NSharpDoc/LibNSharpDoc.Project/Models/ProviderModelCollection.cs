using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using System.Collections.Generic;

namespace Bau.Libraries.LibNSharpDoc.Projects.Models
{
	/// <summary>
	///		Lista de <see cref="ProviderModel"/>
	/// </summary>
	public class ProviderModelCollection : List<ProviderModel>
	{
		/// <summary>
		///		Elimina un elemento por su clave
		/// </summary>
		public void RemoveByID(string key)
		{
			for (int index = Count - 1; index >= 0; index--)
				if (this[index].ID == key)
					RemoveAt(index);
		}
		/// <summary>
		///		Indice de un elemento
		/// </summary>
		public int IndexOf(string key)
		{ 
			// Busca el índice del elemento
			for (int index = 0; index < Count; index++)
				if (this[index].ID.EqualsIgnoreCase(key))
					return index;
			// Si ha llegado hasta aquí es porque no ha encontrado ningún elemento
			return -1;
		}

		/// <summary>
		///		Elimina un elemento
		/// </summary>
		public void Remove(string key)
		{
			int index = IndexOf(key);

				if (index != -1)
					RemoveAt(index);
		}

		/// <summary>
		///		Obtiene un elemento
		/// </summary>
		public ProviderModel this[string key]
		{
			get
			{
				int index = IndexOf(key);

					if (index < 0)
						return null;
					else
						return this[index];
			}
			set
			{
				int index = IndexOf(key);

					if (index >= 0)
						this[index] = value;
			}
		}
	}
}
