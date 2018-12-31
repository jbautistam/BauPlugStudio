using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems
{
	/// <summary>
	///		Clase con los datos de un color
	/// </summary>
	public class ColorModel
	{
		public ColorModel(string color)
		{
			Color = color;
			Convert(color);
		}

		/// <summary>
		///		Convierte una cadena de color
		/// </summary>
		private void Convert(string color)
		{ 
			// Inicializa las propiedades
			Alpha = 255;
			Red = 0;
			Green = 0;
			Blue = 0;
			// Quita los códigos adicionales
			while (!string.IsNullOrEmpty(color) && color.StartsWith("#"))
				color = color.Substring(1);
			// Convierte la cadena
			if (!string.IsNullOrEmpty(color))
			{ // Añade el canal Alpha al color
				if (color.Length < 8)
					color = "FF" + color;
				// Añade la cadena final al color
				while (color.Length < 8)
					color += "00";
				// Convierte los colores
				Alpha = ConvertByte(color.Substring(0, 2));
				Red = ConvertByte(color.Substring(2, 2));
				Green = ConvertByte(color.Substring(4, 2));
				Blue = ConvertByte(color.Substring(6, 2));
			}
		}

		/// <summary>
		///		Convierte una cadena en un byte
		/// </summary>
		private byte ConvertByte(string part)
		{
			return byte.Parse(part, System.Globalization.NumberStyles.HexNumber);
		}

		/// <summary>
		///		Cadena con el color
		/// </summary>
		public string Color { get; }

		/// <summary>
		///		Canal alpha del color
		/// </summary>
		public byte Alpha { get; private set; }

		/// <summary>
		///		Canal rojo del color
		/// </summary>
		public byte Red { get; private set; }

		/// <summary>
		///		Canal verde del color
		/// </summary>
		public byte Green { get; private set; }

		/// <summary>
		///		Canal azul del color
		/// </summary>
		public byte Blue { get; private set; }

		/// <summary>
		///		Indica si un color está vacío
		/// </summary>
		public bool IsEmpty
		{
			get { return string.IsNullOrWhiteSpace(Color); }
		}
	}
}
