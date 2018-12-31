using System;
using System.IO;
using System.Threading.Tasks;

using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.LibMediaPlayer.Model;

namespace Bau.Libraries.LibMediaPlayer.Application.Bussiness
{
	/// <summary>
	///		Clase de descarga de archivos
	/// </summary>
	public class MediaFilesDownloader
	{
		// Eventos públicos
		public event EventHandler<EventArguments.MediaFileDownloadEventArgs> MediaFileDownload;
		public event EventHandler EndDownload;

		public MediaFilesDownloader(string pathBase, MediaFileModelCollection files)
		{
			PathBase = pathBase;
			Files = files;
		}

		/// <summary>
		///		Descarga los archivos
		/// </summary>
		public void DownloadFiles()
		{
			Task.Run(() => DownloadFilesAsync());
		}

		/// <summary>
		///		Descarga los archivos
		/// </summary>
		public async Task DownloadFilesAsync()
		{
			var webClient = new LibCommonHelper.Communications.HttpWebClient();

				// Descarga los eventos
				foreach (MediaFileModel file in Files)
				{
					string fileName = GetDownloadFileName(file);

						// Descarga el archivo
						if (!string.IsNullOrEmpty(fileName) && !File.Exists(fileName))
						{
							// Lanza el evento de descarga
							RaiseEvent(file, Files.IndexOf(file), Files.Count);
							// Crea el directorio
							HelperFiles.MakePath(Path.GetDirectoryName(fileName));
							// Descarga el archivo
							try
							{
								// Descarga el archivo
								await webClient.DownloadFileAsync(file.Url, fileName);
								// Asigna el nombre del archivo descargado
								file.FileName = fileName;
							}
							catch {}
						}
				}
				// Lanza el evento de fin
				EndDownload?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		///		Obtiene el nombre del archivo de descarga
		/// </summary>
		private string GetDownloadFileName(MediaFileModel file)
		{
			string fileName = string.Empty;

				// Calcula el nombre del archivo
				if (!string.IsNullOrEmpty(file.Url))
				{
					// Obtiene el nombre de archivo
					fileName = Path.Combine(PathBase, HelperFiles.Normalize(file.Album.Name), 
											HelperFiles.Normalize(Path.GetFileName(file.Url)));
					// Ajusta el nombre de archivo
					fileName = HelperFiles.GetConsecutiveFileName(Path.GetDirectoryName(fileName), Path.GetFileName(fileName));
				}
				// Devuelve el nombre de archivo
				return fileName;
		}

		/// <summary>
		///		Lanza el evento de descarga de una entrada de un blog
		/// </summary>
		private void RaiseEvent(MediaFileModel file, int actual, int total)
		{
			MediaFileDownload?.Invoke(this, new EventArguments.MediaFileDownloadEventArgs(file, actual, total));
		}

		/// <summary>
		///		Directorio base
		/// </summary>
		private string PathBase { get; }

		/// <summary>
		///		Archivos a descargar
		/// </summary>
		private MediaFileModelCollection Files { get; }
	}
}
