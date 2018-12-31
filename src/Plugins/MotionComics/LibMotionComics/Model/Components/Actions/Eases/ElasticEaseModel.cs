using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions.Eases
{
	/// <summary>
	///		Función elástica asociada a una animación
	/// </summary>
	public class ElasticEaseModel : EaseBaseModel
	{
		public ElasticEaseModel(ActionBaseModel action, Mode mode, int oscillations, double springiness)
						: base(action, mode)
		{
			Oscillations = oscillations;
			Springiness = springiness;
		}

		/// <summary>
		///		Número de oscilaciones
		/// </summary>
		public int Oscillations { get; }

		/// <summary>
		///		Escala de elasticidad
		/// </summary>
		public double Springiness { get; }
	}
}
