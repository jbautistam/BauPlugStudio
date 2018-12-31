using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.PageItems
{
	/// <summary>
	///		Clase con los datos de un lápiz
	/// </summary>
	public class PenModel
	{
		/// <summary>
		///		Tipo de inicio / final de línea
		/// </summary>
		public enum LineCap
		{
			/// <summary>Plano. No se extiende al final de la línea</summary>
			Flat,
			/// <summary>Redondo. Un semicírculo al final de la línea</summary>
			Round,
			/// <summary>Cuadrado. Un rectángulo al final de la línea</summary>
			Square,
			/// <summary>Triángulo. Un triángulo al final de la línea</summary>
			Triangle
		}
		/// <summary>
		///		Forma de unir las líneas
		/// </summary>
		public enum LineJoin
		{
			/// <summary>Vértices intermedios</summary>
			Bevel,
			/// <summary>Vértices angulares</summary>
			Miter,
			/// <summary>Vértices redondeados</summary>
			Round
		}

		/// <summary>
		///		Color
		/// </summary>
		public ColorModel Color { get; set; }

		/// <summary>
		///		Ancho
		/// </summary>
		public double Thickness { get; set; }

		/// <summary>
		///		Cap de los puntos
		/// </summary>
		public LineCap CapDots { get; set; }

		/// <summary>
		///		Cap del inicio de la línea
		/// </summary>
		public LineCap StartLineCap { get; set; }

		/// <summary>
		///		Cap del final de la línea
		/// </summary>
		public LineCap EndLineCap { get; set; }

		/// <summary>
		///		Forma en que se unen las línea
		/// </summary>
		public LineJoin JoinMode { get; set; }

		/// <summary>
		///		Desplazamiento del inicio de los puntos
		/// </summary>
		public double? DashOffset { get; set; }

		/// <summary>
		///		Límite sobre el radio de la longitud mediad del StrokeThickness del lápiz. Este valor es siempre un número positivo mayor o igual a 1
		/// </summary>
		public double? MiterLimit { get; set; }

		/// <summary>
		///		Puntos para las líneas
		/// </summary>
		public System.Collections.Generic.List<double> Dots { get; } = new System.Collections.Generic.List<double>();
	}
}
