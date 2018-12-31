using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Clase base para las acciones
	/// </summary>
	public abstract class ActionBaseModel
	{
		public ActionBaseModel(TimeLineModel timeLine, string targetKey, bool mustAnimate)
		{
			TimeLine = timeLine;
			TargetKey = targetKey;
			MustAnimate = mustAnimate;
		}

		/// <summary>
		///		Línea de tiempo a la que se asocia la acción
		/// </summary>
		public TimeLineModel TimeLine { get; }

		/// <summary>
		///		Clave del destino
		/// </summary>
		public string TargetKey { get; }

		/// <summary>
		///		Indica si se debe animar la acción
		/// </summary>
		public bool MustAnimate { get; }

		/// <summary>
		///		Parámetros de animación
		/// </summary>
		public AnimationParameters Parameters { get; } = new AnimationParameters();

		/// <summary>
		///		Funciones asociadas a la animación
		/// </summary>
		public System.Collections.Generic.List<Eases.EaseBaseModel> Eases { get; } = new System.Collections.Generic.List<Eases.EaseBaseModel>();
	}
}
