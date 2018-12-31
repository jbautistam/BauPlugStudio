using System;
using System.Collections.Generic;

using Bau.Libraries.LibMediaPlayer.Model;
using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers.Common;

namespace Bau.Libraries.LibMediaPlayer.ViewModel.Controllers.Messengers
{
	/// <summary>
	///		Controlador de mensajes
	/// </summary>
	internal class MediaMessengerController
	{
		// Eventos públicos
		public event EventHandler<EventArguments.ReproductionListEventArguments> Play;

		public MediaMessengerController(MediaPlayerViewModel mainViewModel)
		{
			// Asigna el controlador principal
			MainViewModel = mainViewModel;
			// Asinga los manejadores de events
			MainViewModel.HostController.Messenger.Sent += (sender, evntArgs) => {
																					if (evntArgs != null && evntArgs.MessageSent is MessagePlayMedia message)
																						TreatMessagePlayMedia(message);
																				 };
		}

		/// <summary>
		///		Trata el mensaje de añadir archivos
		/// </summary>
		private void TreatMessagePlayMedia(MessagePlayMedia message)
		{
			if (message.Files.Count > 0)
			{
				MediaAlbumModel album = MainViewModel.MediaManager.File.SearchAlbumByName(message.Group);
				MediaAlbumModel reproductionList = new MediaAlbumModel
																{
																	Name = message.Group
																};

					// Agrega el álbum si no existía
					if (album == null)
						album = MainViewModel.MediaManager.File.Albums.Add(message.Group, string.Empty);
					// Añade los archivos
					foreach (KeyValuePair<string, string> file in message.Files)
					{
						MediaFileModel media = new MediaFileModel
														{
															Album = album
														};

							// Asigna las propiedades
							media.Name = file.Key;
							if (file.Value.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) ||
									file.Value.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
								media.Url = file.Value;
							else
								media.FileName = file.Value;
							// Añade el archivo al álbum
							if (!album.Files.ExistsByFile(media))
								album.Files.Add(media);
							// Añade el archivo a la lista de reproducción
							reproductionList.Files.Add(media);
					}
					// Graba el álbum y lanza el evento de reproducción de lista
					if (reproductionList.Files.Count > 0)
					{
						MediaPlayerViewModel.Instance.MediaManager.Save();
						RaiseEventPlay(reproductionList);
					}
			}
		}

		/// <summary>
		///		Lanza el evento de reproducción de una lista
		/// </summary>
		private void RaiseEventPlay(MediaAlbumModel reproductionList)
		{
			Play?.Invoke(this, new EventArguments.ReproductionListEventArguments(reproductionList));
		}

		/// <summary>
		///		Manejador principal
		/// </summary>
		private MediaPlayerViewModel MainViewModel { get; }
	}
}
