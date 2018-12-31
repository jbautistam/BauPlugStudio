using System;

using Bau.Libraries.LibMediaPlayer.Model;

namespace Bau.Libraries.LibMediaPlayer.ViewModel.Controllers.EventArguments
{
	/// <summary>
	///		Argumentos de los eventos de una lista de reproducción
	/// </summary>
	internal class ReproductionListEventArguments : EventArgs
	{
		internal ReproductionListEventArguments(MediaAlbumModel album)
		{
			Album = album;
		}

		/// <summary>
		///		Album asociado a la lista de reproducción
		/// </summary>
		internal MediaAlbumModel Album { get; }
	}
}
