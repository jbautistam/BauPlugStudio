using System;

namespace Bau.Libraries.BookLibrary.ViewModel.Controllers
{
	/// <summary>
	///		Argumentos del evento para mostrar una página
	/// </summary>
	public class PageShowEventArgs : EventArgs
	{
		public PageShowEventArgs(string fileName)
		{
			FileName = fileName;
		}

		/// <summary>
		///		Nombre de la página
		/// </summary>
		public string FileName { get; }
	}
}
