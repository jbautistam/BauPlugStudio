using System;

using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Documents
{
	/// <summary>
	///		Elemento de una lista de páginas
	/// </summary>
	public class PageListItemViewModel : MVVM.ViewModels.ListItems.BaseListItemViewModel
	{
		public PageListItemViewModel(FileModel file)
		{
			File = file;
			Text = file.IDFileName;
		}

		/// <summary>
		///		Archivo
		/// </summary>
		public FileModel File { get; }
	}
}
