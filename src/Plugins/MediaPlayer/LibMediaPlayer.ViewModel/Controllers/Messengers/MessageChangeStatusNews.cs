using System;

using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers;

namespace Bau.Libraries.LibMediaPlayer.ViewModel.Controllers.Messengers
{
	/// <summary>
	///		Mensaje de cambio de estado de archivos multimedia
	/// </summary>
	public class MessageChangeStatusMediaFiles : Message
	{
		public MessageChangeStatusMediaFiles(object content) : base(MediaPlayerViewModel.Instance.ModuleName, "CHANGE_MEDIA_FILE_STATUS", "REFRESH", content) 
		{
		}
	}
}
