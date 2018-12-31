using System;

namespace Bau.Libraries.LibBlogReader.Application.Services.EventArguments
{
	/// <summary>
	///		Argumentos del evento de descarga de un elemento de un blog
	/// </summary>
	public class BlogEntryDownloadEventArgs : EventArgs
	{
		public BlogEntryDownloadEventArgs(Model.EntryModel blogEntry)
		{
			BlogEntry = blogEntry;
		}

		/// <summary>
		///		Entrada de blog descargada
		/// </summary>
		public Model.EntryModel BlogEntry { get; }
	}
}
