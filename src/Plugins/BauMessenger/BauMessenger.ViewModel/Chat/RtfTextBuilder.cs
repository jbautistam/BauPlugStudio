using System;

namespace Bau.Libraries.BauMessenger.ViewModel.Chat
{
	/// <summary>
	///		Clase para creación de texto Rtf
	/// </summary>
	internal class RtfTextBuilder
	{   
		// Enumerados públicos
		public enum RtfColor
		{
			Black,
			Red,
			Green,
			Blue,
			White,
			Gray,
			Olive
		}
		// Variables privadas
		private System.Text.StringBuilder _lastParagraph = null;

		internal RtfTextBuilder()
		{
		}

		/// <summary>
		///		Limpia el contenido
		/// </summary>
		internal void Clear()
		{ 
			// Inicia las fuentes
			Fonts.Clear();
			AddFont("Times New Roman", 0);
			AddFont("Segoe UI", 0);
			// Inicia el cuerpo
			Body.Clear();
		}

		/// <summary>
		///		Añade una fuente
		/// </summary>
		internal void AddFont(string fontName, int charSet = 0)
		{
			Fonts.Add($"fcharset{charSet} {fontName}");
		}

		/// <summary>
		///		Obtiene el texto Rtf almacenado
		/// </summary>
		internal string GetRtfText()
		{
			System.Text.StringBuilder text = new System.Text.StringBuilder(@"{\rtf1\ansi\ansicpg1145\uc1\htmautsp\deff2\deflang1034\lang1034");

				// Añade las fuentes
				text.Append(@"{\fonttbl");
				for (int index = 0; index < Fonts.Count; index++)
					text.Append("{\\f" + index + "\\f" + Fonts[index] + ";}");
				text.Append("}");
				// Añade la tabla de color
				text.Append("{\\colortbl");
				text.Append(GetColor(0, 0, 0)); // black
				text.Append(GetColor(255, 0, 0)); // red
				text.Append(GetColor(0, 255, 0)); // green
				text.Append(GetColor(0, 0, 255)); // blue
				text.Append(GetColor(255, 255, 255)); // white
				text.Append(GetColor(128, 128, 128)); // gray
				text.Append(GetColor(114, 180, 59)); // olive
				text.Append("}");
				// Añade el texto del párrafo
				text.Append(Body);
				// Añade el cierre del RTF
				text.Append("}");
				// Devuelve el texto
				return text.ToString();
		}

		/// <summary>
		///		Obtiene un color
		/// </summary>
		private string GetColor(int red, int green, int blue)
		{
			return $"\\red{red}\\green{green}\\blue{blue};";
		}

		/// <summary>
		///		Inicia un párrafo
		/// </summary>
		internal void AddParagraph()
		{ 
			// Vacía el párrafo anterior
			EndParagraph();
			// Crea un nuevo párrafo
			_lastParagraph = new System.Text.StringBuilder(@"\par\pard" + Environment.NewLine);
		}

		/// <summary>
		///		Cierra el párrafo anterior
		/// </summary>
		internal void EndParagraph()
		{
			if (_lastParagraph != null)
				Body.Append(_lastParagraph);
			_lastParagraph = null;
		}

		/// <summary>
		///		Añade el texto
		/// </summary>
		internal void AddText(string text, bool bold = false, bool italic = false)
		{   
			// Añade el formato
			if (bold)
				_lastParagraph.Append("\\b ");
			if (italic)
				_lastParagraph.Append("\\i ");
			// Cierra el texto
			_lastParagraph.Append(Normalize(text) + " ");
			// Cierra el formato
			if (italic)
				_lastParagraph.Append("\\i0 ");
			if (bold)
				_lastParagraph.Append("\\b0 ");
		}

		/// <summary>
		///		Normaliza una cadena de texto
		/// </summary>
		private string Normalize(string text)
		{ 
			// Acentos
			text = text.Replace("á", "\\'e1");
			text = text.Replace("é", "\\'e9");
			text = text.Replace("í", "\\'ed");
			text = text.Replace("ó", "\\'f3");
			text = text.Replace("ú", "\\'fa");
			// Acentos en mayúsculas
			text = text.Replace("Á", "\\'c1");
			text = text.Replace("É", "\\'c9");
			text = text.Replace("Í", "\\'cd");
			text = text.Replace("Ó", "\\'d3");
			text = text.Replace("Ú", "\\'da");
			// Eñes			
			text = text.Replace("ñ", "\\'f1e");
			text = text.Replace("Ñ", "\\'d1");
			// Símbolos
			text = text.Replace("¿", "\\'bf");
			text = text.Replace("¡", "\\'a1");
			// Devuelve el texto
			return text;
		}

		/// <summary>
		///		Añade un tabulador
		/// </summary>
		internal void AddTab()
		{
			_lastParagraph.Append(@"\tab");
		}

		/// <summary>
		///		Añade un salto de línea
		/// </summary>
		internal void AddNewLine()
		{
			_lastParagraph.Append("\\line");
		}

		/// <summary>
		///		Cambia el color
		/// </summary>
		internal void SetColor(RtfColor color = RtfColor.Black)
		{
			_lastParagraph.Append("\\cf" + ((int) color).ToString() + " ");
		}

		/// <summary>
		///		Fuentes
		/// </summary>
		internal System.Collections.Generic.List<string> Fonts { get; } = new System.Collections.Generic.List<string>();

		/// <summary>
		///		Cuerpo
		/// </summary>
		internal System.Text.StringBuilder Body { get; } = new System.Text.StringBuilder();

		/// <summary>
		///		Obtiene el texto convertido a RTF
		/// </summary>
		internal string RtfText
		{
			get { return GetRtfText(); }
		}
	}
}
