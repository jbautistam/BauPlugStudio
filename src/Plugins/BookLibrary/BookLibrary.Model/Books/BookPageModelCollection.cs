using System;

namespace Bau.Libraries.BookLibrary.Model.Books
{
	/// <summary>
	///		Colección de <see cref="BookPageModel"/>
	/// </summary>
	public class BookPageModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<BookPageModel>
	{
		/// <summary>
		///		Calcula el número de cada página (comienza desde 1)
		/// </summary>
		public void NormalizePages()
		{
			int pageIndex = 1;

				NormalizePages(ref pageIndex);
		}

		/// <summary>
		///		Calcula el número de página
		/// </summary>
		internal void NormalizePages(ref int pageIndex)
		{
			foreach (BookPageModel page in this)
			{ 
				// Asigna el número de página
				page.PageNumber = pageIndex++;
				// Asigna el número a las páginas hija
				page.Pages.NormalizePages(ref pageIndex);
			}
		}

		/// <summary>
		///		Cuenta las páginas
		/// </summary>
		public int CountPages()
		{
			int pageIndex = 0;

				// Cuenta las páginas
				foreach (BookPageModel page in this)
				{ 
					// Cuenta esta página
					pageIndex++;
					// Cuenta las páginas hija
					pageIndex += page.Pages.CountPages();
				}
				// Devuelve el número de páginas
				return pageIndex;
		}

		/// <summary>
		///		Busca la página X
		/// </summary>
		public BookPageModel SearchPage(int pageIndex)
		{
			BookPageModel pageSearched = null;

				// Busca la página en la colección
				foreach (BookPageModel page in this)
					if (page.PageNumber == pageIndex)
						pageSearched = page;
				// Busca la página en las hijas
				if (pageSearched == null)
					foreach (BookPageModel page in this)
						if (pageSearched == null)
							pageSearched = page.Pages.SearchPage(pageIndex);
				// Devuelve la página encontrada
				return pageSearched;
		}
	}
}
