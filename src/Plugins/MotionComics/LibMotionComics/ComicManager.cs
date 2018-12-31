using System;

using Bau.Libraries.LibMotionComic.Model;
using Bau.Libraries.LibMotionComic.Model.Components;
using Bau.Libraries.LibMotionComic.Model.Components.Actions;

namespace Bau.Libraries.LibMotionComic
{
	/// <summary>
	///		Manager de cómics con movimiento
	/// </summary>
	public class ComicManager
	{   
		// Constantes públicas
		public const string Extension = ".cml";

		public ComicManager(string fileName)
		{
			Path = System.IO.Path.GetDirectoryName(fileName);
			FileName = System.IO.Path.GetFileName(fileName);
		}

		public ComicManager(string path, string fileName)
		{
			Path = path;
			FileName = fileName;
		}

		/// <summary>
		///		Carga los datos del archivo y prepara el cómic
		/// </summary>
		public void Load(bool loadFull = true)
		{
			Comic = new Repository.Reader.ComicReaderRepository().Load(Path, FileName, loadFull);
			IndexActualPage = 0;
			IndexActualAction = 0;
		}

		/// <summary>
		///		Obtiene la página anterio
		/// </summary>
		public void MovePagePrevious()
		{
			if (IndexActualPage > 0)
				SetPage(IndexActualPage - 1);
		}

		/// <summary>
		///		Obtiene la siguiente página
		/// </summary>
		public void MoveNexPage()
		{
			if (IndexActualPage < Comic.Pages.Count - 1)
				SetPage(IndexActualPage + 1);
		}

		/// <summary>
		///		Cambia la página
		/// </summary>
		private void SetPage(int newPage)
		{
			IndexActualPage = newPage;
			IndexActualAction = 0;
		}

		/// <summary>
		///		Obtiene la siguiente acción
		/// </summary>
		public TimeLineModel MoveNextAction()
		{
			TimeLineModel timeLine = null;

				// Obtiene la siguiente acción
				if (IndexActualAction < ActualPage.TimeLines.Count)
					timeLine = ActualPage.TimeLines[IndexActualAction++];
				// Devuelve la acción
				return timeLine;
		}

		/// <summary>
		///		Directorio de cómic
		/// </summary>
		public string Path { get; }

		/// <summary>
		///		Nombre del archivo de cómic
		/// </summary>
		public string FileName { get; }

		/// <summary>
		///		Cómic cargado
		/// </summary>
		public ComicModel Comic { get; private set; }

		/// <summary>
		///		Página actual
		/// </summary>
		public PageModel ActualPage
		{
			get
			{
				if (IndexActualPage >= 0 && IndexActualPage < Comic.Pages.Count)
					return Comic.Pages[IndexActualPage];
				else
					return null;
			}
		}

		/// <summary>
		///		Página actual
		/// </summary>
		public int IndexActualPage { get; private set; }

		/// <summary>
		///		Acción actual
		/// </summary>
		public int IndexActualAction { get; private set; }
	}
}
