using System;

using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers;
using Bau.Libraries.SourceEditor.Model.Messages;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Controllers
{
	/// <summary>
	///		Controlador de mensajes de DataBaseStudio
	/// </summary>
	internal class MessagesController : BasePluginMessengerController
	{
		public MessagesController() : base(DataBaseStudioViewModel.Instance, DataBaseStudioViewModel.Instance.HostController, true)
		{
		}

		/// <summary>
		///		Trata los mensajes del host
		/// </summary>
		public override void TreatMessage(object sender, Message message)
		{
			if (message.Content is MessageRequestPlugin)
				DataBaseStudioViewModel.Instance.SourceEditorPluginManager.InitPlugin(message.Content as MessageRequestPlugin);
		}
	}
}
