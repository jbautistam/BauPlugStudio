using System;

using Bau.Libraries.LibBlogReader.Application.Services.Reader;
using Bau.Libraries.LibBlogReader.Application.Services.EventArguments;
using Bau.Libraries.Plugins.ViewModels.Controllers.Processes;

namespace Bau.Libraries.LibBlogReader.ViewModel.Controllers.Process
{
	/// <summary>
	///		Proceso de descarga de blogs
	/// </summary>
	internal class BlogDownloadProcess : AbstractPlannedProcess
	{   
		// Eventos públicos
		internal event EventHandler<DownloadEventArgs> DownloadProcess;
		internal event EventHandler<BlogEntryDownloadEventArgs> BlogEntryDownload;
		// Variables privadas
		private RssDownload _downloader = null;

		internal BlogDownloadProcess(int minutes, bool enabled = true)
							: base(BlogReaderViewModel.Instance.ModuleName, "BlogReader", "Proceso de descarga de blogs", minutes, enabled)
		{
		}

		/// <summary>
		///		Ejecuta la descarga de blogs
		/// </summary>
		protected override void Execute()
		{
			Downloader.Download(false);
		}

		/// <summary>
		///		Proceso de descarga
		/// </summary>
		private RssDownload Downloader
		{
			get
			{ 
				// Crea el objeto si no existía
				if (_downloader == null)
				{ 
					// Crea el objeto
					_downloader = new RssDownload(BlogReaderViewModel.Instance.BlogManager);
					// Asigna los manejadores de eventos
					_downloader.Process += (sender, evntArgs) => DownloadProcess?.Invoke(this, evntArgs);
					_downloader.BlogEntryDownload += (sender, evntArgs) => BlogEntryDownload?.Invoke(this, evntArgs);
				}
				// Devuelve el objeto de descarga
				return _downloader;
			}
		}
	}
}
