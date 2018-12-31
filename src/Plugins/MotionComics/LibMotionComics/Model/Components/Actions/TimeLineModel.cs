using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Modelo para una línea de tiempo de acciones de una página
	/// </summary>
	public class TimeLineModel
	{
		public TimeLineModel(PageModel page, bool mustAnimate)
		{
			Page = page;
			MustAnimate = mustAnimate;
		}

		/// <summary>
		///		Página en la que se define el TimeLine
		/// </summary>
		public PageModel Page { get; }

		/// <summary>
		///		Indica si se debe animar
		/// </summary>
		public bool MustAnimate { get; }

		/// <summary>
		///		Parámetros
		/// </summary>
		public AnimationParameters Parameters { get; } = new AnimationParameters();

		/// <summary>
		///		Acciones
		/// </summary>
		public List<ActionBaseModel> Actions { get; } = new List<ActionBaseModel>();
	}
}
