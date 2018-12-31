using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Plugins.ViewModels;
using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers;
using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers.Common;
using Bau.Libraries.Plugins.ViewModels.Controllers.Settings;
using Bau.Libraries.SourceEditor.Model.Messages;

namespace Bau.Libraries.FtpManager.ViewModel.Controllers
{
	/// <summary>
	///		Controlador de mensajes de FtmPanager
	/// </summary>
	internal class MessagesController : BasePluginMessengerController
	{
		public MessagesController() : base(FtpManagerViewModel.Instance, FtpManagerViewModel.Instance.HostController, true)
		{
		}

		/// <summary>
		///		Trata los mensajes del host
		/// </summary>
		public override void TreatMessage(object sender, Message message)
		{
			if (message.Content is MessageRequestPlugin)
				FtpManagerViewModel.Instance.SourceEditorPluginManager.InitPlugin(message.Content as MessageRequestPlugin);
			else if (message is MessageCommand)
				ExecuteCommand(message as MessageCommand);
		}

		/// <summary>
		///		Ejecuta un comando
		/// </summary>
		private void ExecuteCommand(MessageCommand messageCommand)
		{
			if (messageCommand != null && messageCommand.TargetPlugin.EqualsIgnoreCase(FtpManagerViewModel.Instance.ModuleName))
			{
				if (messageCommand.Action.EqualsIgnoreCase("Upload"))
					ExecuteUpload(messageCommand.Parameters);
			}
		}

		/// <summary>
		///		Sube el contenido a un servidor FTP
		/// </summary>
		private void ExecuteUpload(ParametersModelCollection parameters)
		{
			new Services.UploadProcessService().Process(parameters);
		}
	}
}
