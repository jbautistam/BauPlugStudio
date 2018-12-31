using System;

namespace Bau.Libraries.LibBlogReader.ViewModel.Controllers.Messengers
{
	/// <summary>
	///		Mensaje de cambio de estado de noticias
	/// </summary>
	public class MessageChangeStatusNews : Plugins.ViewModels.Controllers.Messengers.Message
	{
		public MessageChangeStatusNews(object content) : base(BlogReaderViewModel.Instance.ModuleName, "CHANGE_STATUS", "REFRESH", content) 
		{
		}
	}
}
