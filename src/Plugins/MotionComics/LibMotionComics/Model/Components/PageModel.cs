using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibMotionComic.Model.Components
{
	/// <summary>
	///		Clase con los datos de una página de cómic
	/// </summary>
	public class PageModel
	{ 
		// Enumerados públicos
		/// <summary>
		///		Modo de ajuste
		/// </summary>
		public enum StretchMode
		{
			None,
			Fill,
			Uniform,
			UniformToFill
		}

		public PageModel(ComicModel comic)
		{
			Comic = comic;
		}

		/// <summary>
		///		Cómic al que pertenece la página
		/// </summary>
		public ComicModel Comic { get; }

		/// <summary>
		///		Fondo
		/// </summary>
		public PageItems.Brushes.AbstractBaseBrushModel Brush { get; set; }

		/// <summary>
		///		Contenido
		/// </summary>
		public List<PageItems.AbstractPageItemModel> Content { get; } = new List<PageItems.AbstractPageItemModel>();

		/// <summary>
		///		Contenido de la página
		/// </summary>
		public List<Actions.TimeLineModel> TimeLines { get; } = new List<Actions.TimeLineModel>();
	}
}
