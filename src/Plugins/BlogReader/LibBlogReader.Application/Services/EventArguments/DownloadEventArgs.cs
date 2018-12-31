using System;

namespace Bau.Libraries.LibBlogReader.Application.Services.EventArguments
{
	/// <summary>
	///		Eventos de descarga de blogs
	/// </summary>
	public class DownloadEventArgs : EventArgs
	{
		/// <summary>Tipo de acción de descarga</summary>
		public enum ActionType
		{
			/// <summary>Inicio de la descarga</summary>
			StartDownload,
			/// <summary>Inicio de la descarga de un blog</summary>
			StartDownloadBlog,
			/// <summary>Final de la descarga de un blog</summary>
			EndDownloadBlog,
			/// <summary>Final de descarga</summary>
			EndDownload,
			/// <summary>Error en la descarga de un blog</summary>
			ErrorDonwloadBlog
		}

		public DownloadEventArgs(ActionType action, string description)
		{
			Action = action;
			Description = description;
		}

		/// <summary>
		///		Acción ejecutada
		/// </summary>
		public ActionType Action { get; }

		/// <summary>
		///		Descripción de la acción
		/// </summary>
		public string ActionDescription
		{
			get
			{
				switch (Action)
				{
					case ActionType.StartDownload:
						return "Inicio de descarga";
					case ActionType.StartDownloadBlog:
						return "Inicio de descarga del blog";
					case ActionType.EndDownloadBlog:
						return "Fin de descarga del blog";
					case ActionType.EndDownload:
						return "Fin de descarga";
					case ActionType.ErrorDonwloadBlog:
						return "Error en descarga";
					default:
						return "Acción desconocida";
				}
			}
		}

		/// <summary>
		///		Descripción de la acción
		/// </summary>
		public string Description { get; }
	}
}
