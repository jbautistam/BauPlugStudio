using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibNSharpDoc.Models.Structs
{
	/// <summary>
	///		Diccionario de parámetros de una estructura
	/// </summary>
	public class StructParameterModelDictionary
	{
		/// <summary>
		///		Añade la cadena de depuración
		/// </summary>
		internal string Debug(int indent)
		{
			string debug = "";

				// Añade los parámetros
				if (Parameters.Count > 0)
				{ 
					// Añade la cabecera
					debug += new string('\t', indent) + "<!-- BeginParameters -->" + Environment.NewLine;
					// Añade el valor del parámetros
					foreach (KeyValuePair<string, StructParameterModel> parameter in Parameters)
						debug += parameter.Value.Debug(indent + 1);
					// Añade el fin de la cadena de parámetros
					debug += new string('\t', indent) + "<!-- EndParameters -->" + Environment.NewLine;
				}
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Obtiene una cadena con la descripción de los parámetros
		/// </summary>
		public string GetFullParameters()
		{
			string parameters = "";

				// Obtiene los valores
				foreach (KeyValuePair<string, StructParameterModel> parameter in Parameters)
					parameters = parameters.AddWithSeparator($"{parameter.Value.Key} = {parameter.Value.Value?.ToString()}", ";");
				// Devuelve la cadena
				return parameters;
		}

		/// <summary>
		///		Añade un parámetro al diccionario
		/// </summary>
		public StructParameterModel Add(string key, object value, string referenceKey = null)
		{
			return Add(new StructParameterModel(key, value, referenceKey));
		}

		/// <summary>
		///		Añade un parámetro al diccionario
		/// </summary>
		public StructParameterModel Add(StructParameterModel parameter)
		{   
			// Comprueba si es nulo antes de añadir
			if (parameter != null)
			{
				string key = NormalizeKey(parameter.Key);

					// Añade el parámetro
					if (Parameters.ContainsKey(key))
						Parameters[key] = parameter;
					else
						Parameters.Add(key, parameter);
			}
			// Devuelve el parámetro añadido
			return parameter;
		}

		/// <summary>
		///		Obtiene el valor de un parámetro
		/// </summary>
		public string GetValue(string key, string defaultValue = "")
		{
			if (Parameters.TryGetValue(NormalizeKey(key), out StructParameterModel parameter))
				return parameter.Value?.ToString();
			else
				return defaultValue;
		}

		/// <summary>
		///		Obtiene un parámetro por su clave
		/// </summary>
		public StructParameterModel Search(string key)
		{
			// Obtiene el parámetro
			if (!Parameters.TryGetValue(NormalizeKey(key), out StructParameterModel parameter))
				parameter = new StructParameterModel(key, "", null);
			// Devuelve el parámetro
			return parameter;
		}

		/// <summary>
		///		Normaliza la clave
		/// </summary>
		private string NormalizeKey(string name)
		{
			return name.TrimIgnoreNull().ToUpper();
		}

		/// <summary>
		///		Diccionario de parámetros
		/// </summary>
		public Dictionary<string, StructParameterModel> Parameters { get; set; } = new Dictionary<string, StructParameterModel>();
	}
}
