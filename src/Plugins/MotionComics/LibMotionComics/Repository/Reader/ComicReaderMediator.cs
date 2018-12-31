using System;

namespace Bau.Libraries.LibMotionComic.Repository.Reader
{
	/// <summary>
	///		Clase de mediación para la lectura de archivos
	/// </summary>
	internal class ComicReaderMediator
	{
		internal ComicReaderMediator()
		{
			ResourcesRepository = new ComicResourcesRepository(this);
			PagesRepository = new ComicPageRepository(this);
			BrushesRepository = new ComicBrushesRepository(this);
			TimeLineRepository = new ComicTimeLineRepository(this);
			ShapesRepository = new ComicShapesRepository(this);
			TransformRepository = new ComicTransformsRepository(this);
		}

		/// <summary>
		///		Repository para recursos
		/// </summary>
		internal ComicResourcesRepository ResourcesRepository { get; }

		/// <summary>
		///		Repository para páginas
		/// </summary>
		internal ComicPageRepository PagesRepository { get; }

		/// <summary>
		///		Repository para brochas
		/// </summary>
		internal ComicBrushesRepository BrushesRepository { get; }

		/// <summary>
		///		Repository para timeline y acciones
		/// </summary>
		internal ComicTimeLineRepository TimeLineRepository { get; }

		/// <summary>
		///		Repository para figuras
		/// </summary>
		internal ComicShapesRepository ShapesRepository { get; }

		/// <summary>
		///		Repository para fuciones comunes de lectura
		/// </summary>
		internal ComicCommonRepository CommonRepository { get; } = new ComicCommonRepository();

		/// <summary>
		///		Repository para las transformaciones
		/// </summary>
		internal ComicTransformsRepository TransformRepository { get; }
	}
}