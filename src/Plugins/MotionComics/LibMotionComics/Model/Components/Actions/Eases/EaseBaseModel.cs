using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions.Eases
{
	/// <summary>
	///		Base para los objetos que modifican una animación
	/// </summary>
	public abstract class EaseBaseModel
	{
		/// <summary>
		///		Modo en que se aplica la modificación de la animación
		/// </summary>
		public enum Mode
		{
			/// <summary>Al principio</summary>
			EaseIn,
			/// <summary>Al final</summary>
			EaseOut,
			/// <summary>Al principio y al final</summary>
			EaseInOut
		}

		public EaseBaseModel(ActionBaseModel action, Mode mode)
		{
			Action = action;
			EaseMode = mode;
		}

		/// <summary>
		///		Acción a la que se asocia
		/// </summary>
		public ActionBaseModel Action { get; }

		/// <summary>
		///		Modo de aplicación
		/// </summary>
		public Mode EaseMode { get; }
	}
}
