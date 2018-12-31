using System;

using Bau.Libraries.BookLibrary.Model.Books;

namespace Bau.Libraries.BookLibrary.ViewModel.Books.Content.Comic
{
	/// <summary>
	///		ViewModel para mostrar una página de un cómic en un ListView
	/// </summary>
	public class PageListItemViewModel : BauMvvm.ViewModels.Forms.ControlItems.ControlItemViewModel
	{ 
		// Variables privadas
		private string _thumbFileName;

		public PageListItemViewModel(BookPageModel page) : base((page.PageNumber + 1).ToString(), page)
		{
			Page = page;
		}

		/// <summary>
		///		Nombre del archivo de thumbs
		/// </summary>
		public string ThumbFileName
		{
			get { return _thumbFileName; }
			set { CheckProperty(ref _thumbFileName, value); }
		}

		/// <summary>
		///		Número de página
		/// </summary>
		public int PageNumber
		{
			get { return Page.PageNumber + 1; }
		}

		/// <summary>
		///		Página
		/// </summary>
		public BookPageModel Page { get; }
	}
}
