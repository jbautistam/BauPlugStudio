using System;

using Bau.Libraries.LibComicsBooks;
using Bau.Libraries.BookLibrary.Model.Books;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;

namespace Bau.Libraries.BookLibrary.ViewModel.Books.Content.Comic
{
	/// <summary>
	///		ViewModel para ver el contenido de un <see cref="BookModel"/>
	/// </summary>
	public class ComicContentViewModel : BaseContentViewModel
	{   
		// Variables privadas
		private ControlListViewModel _pages;
		private string _fileNameImage;

		public ComicContentViewModel(BookModel book) : base(book) { }

		/// <summary>
		///		Inicializa las propiedades que muestran el cómic
		/// </summary>
		protected override void InitBookView()
		{
			ComicBook comicBook = new ComicBook();
			int pageIndex = 0;

				// Carga el archivo
				comicBook.Load(Book.FileName);
				// Crea la colección de páginas
				ComicPages = new ControlListViewModel();
				// Rellena la colección de páginas
				foreach (ComicPage page in comicBook.Pages)
				{
					BookPageModel bookPage = new BookPageModel();

						// Asigna las propiedades
						bookPage.FileName = bookPage.FileName;
						bookPage.PageNumber = pageIndex++;
						// Añade la página del libro a la lista de página
						ComicPages.Items.Add(new PageListItemViewModel(bookPage));
				}
				// Asigna el manejador de eventos
				comicBook.ComicAction += (sender, comicArgs) =>
											{
												if (comicArgs.Action == EventComicArgs.ActionType.Uncompress)
												{
													int index = comicArgs.Actual - 1;

														// Asigna los nombres de archivo
														if (index < ComicPages.Items.Count && index < comicBook.Pages.Count &&
															ComicPages.Items[index] is PageListItemViewModel page)
														{
															page.Page.FileName = comicBook.Pages[index].FileName;
															page.ThumbFileName = comicBook.Pages[index].FileName;
														}
														// Si ya ha terminado, ordena los nombres
														if (index >= ComicPages.Items.Count - 1)
														{ 
															// Ordena las páginas
															comicBook.Pages.Sort();
															// Asigna los nombres de archivo
															for (int indexPage = 0; indexPage < comicBook.Pages.Count; indexPage++)
																if (ComicPages.Items[indexPage] is PageListItemViewModel pageItem)
																{   
																	pageItem.Page.FileName = comicBook.Pages[indexPage].FileName;
																	pageItem.ThumbFileName = comicBook.Pages[indexPage].FileName;
																}
														}
														// Se coloca en la primera página
														ShowPageReal(0);
												}
											};
				// Descomprime el archivo
				comicBook.Uncompress(PathTarget, true);
		}

		/// <summary>
		///		Muestra la página
		/// </summary>
		protected override void ShowPageReal(int pageIndex)
		{
			if (ComicPages != null && pageIndex >= 0 && pageIndex < ComicPages.Items.Count && ComicPages.Items[pageIndex] is PageListItemViewModel page)
			{
				FileNameImage = page.Page.FileName;
				page.IsSelected = true;
			}
		}

		/// <summary>
		///		Páginas del cómic (no utiliza Pages en el View)
		/// </summary>
		public ControlListViewModel ComicPages
		{
			get { return _pages; }
			set { CheckObject(ref _pages, value); }
		}

		/// <summary>
		///		Nombre del archivo de imagen principal
		/// </summary>
		public string FileNameImage
		{
			get { return _fileNameImage; }
			set { CheckProperty(ref _fileNameImage, value); }
		}
	}
}
