using System;

namespace Bau.Libraries.LibDocWriter.Application.Services
{
	/// <summary>
	///		Clase de ayuda para generación de Nhaml
	/// </summary>
	public class NhamlBuilder
	{ 
		// Variables privadas
		private System.Text.StringBuilder _builder;

		public NhamlBuilder()
		{
			Clear();
		}

		/// <summary>
		///		Limpia el contenido
		/// </summary>
		public void Clear()
		{
			_builder = new System.Text.StringBuilder();
			Indent = 0;
		}

		/// <summary>
		///		Añade una etiqueta con su texto
		/// </summary>
		public void AddTag(string tag)
		{
			AddTag(tag, null);
		}

		/// <summary>
		///		Añade una etiqueta con su texto
		/// </summary>
		public void AddTag(string tag, string text)
		{ 
			// Añade la indentación
			_builder.Append(new string('\t', Indent));
			// Añade la etiqueta
			if (!tag.StartsWith("%") && !tag.StartsWith("<%"))
				_builder.Append("%");
			_builder.Append(tag);
			// Añade el texto
			if (!string.IsNullOrEmpty(text))
				_builder.Append(" " + text);
			// Añade un salto de línea
			_builder.Append(Environment.NewLine);
		}

		/// <summary>
		///		Indentación
		/// </summary>
		public int Indent { get; set; }

		/// <summary>
		///		Obtiene la cadena HTML
		/// </summary>
		public override string ToString()
		{
			return _builder.ToString();
		}
	}
}