using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibMotionComic.Model.Components
{
	/// <summary>
	///		Diccionario de componentes
	/// </summary>
	public class ResourcesDictionary : Dictionary<string, AbstractComponentModel>
	{
		/// <summary>
		///		Añade un elemento con la clave normalizada
		/// </summary>
		public new void Add(string key, AbstractComponentModel component)
		{
			base.Add(ComputeKey(key), component);
		}

		/// <summary>
		///		Obtiene un componente del diccionario
		/// </summary>
		public AbstractComponentModel Search(string key)
		{
			if (TryGetValue(ComputeKey(key), out AbstractComponentModel component))
				return component;
			else
				return null;
		}

		/// <summary>
		///		Calcula una clave normalizada
		/// </summary>
		private string ComputeKey(string key)
		{
			return key?.ToUpperInvariant();
		}
	}
}
