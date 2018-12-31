using System;

using Bau.Libraries.Plugins.ViewModels;
using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers;
using Bau.Libraries.SourceEditor.Model.Messages;

namespace Bau.Libraries.MotionComics.ViewModel.Controllers
{
	/// <summary>
	///		Controlador de mensajes de MotionComics
	/// </summary>
	internal class MessagesController : BasePluginMessengerController
	{
		public MessagesController() : base(MotionComicsViewModel.Instance, MotionComicsViewModel.Instance.HostController, true)
		{
		}

		/// <summary>
		///		Trata los mensajes del host
		/// </summary>
		public override void TreatMessage(object sender, Message message)
		{
			if (message.Content is MessageRequestPlugin)
				MotionComicsViewModel.Instance.SourceEditorPluginManager.InitPlugin(message.Content as MessageRequestPlugin);
		}
	}
}
