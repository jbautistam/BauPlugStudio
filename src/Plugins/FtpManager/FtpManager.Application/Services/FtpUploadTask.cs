using System;
using System.Collections.Generic;

using Bau.Libraries.FtpManager.Model.Connections;
using Bau.Libraries.LibFtpClient;

namespace Bau.Libraries.FtpManager.Application.Services
{
	/// <summary>
	///		Tarea de subida de archivos
	/// </summary>
	internal class FtpUploadTask
	{ 
		// Variables privadas
		private int _taskId;
		private FtpUploadService _service;
		private FtpUploadService.UploadMode _mode;
		private List<string> _files;
		private FtpConnectionModel _ftpConnection;
		private string _pathLocal;
		private string _pathRemote;

		internal FtpUploadTask(int intTask, FtpUploadService service, FtpConnectionModel ftpConnection, string pathLocal,
							   List<string> files, string pathRemote, FtpUploadService.UploadMode mode)
		{
			_taskId = intTask;
			_service = service;
			_ftpConnection = ftpConnection;
			_pathLocal = pathLocal;
			_files = files;
			_pathRemote = pathRemote;
			_mode = mode;
		}

		/// <summary>
		///		Sube los archivos al servidor FTP
		/// </summary>
		internal void Upload()
		{
			using (FtpClient ftpClient = new FtpClient(ConvertProtocol(_ftpConnection.FtpProtocol),
													   _ftpConnection.Server, _ftpConnection.Port,
													   new System.Net.NetworkCredential(_ftpConnection.User, _ftpConnection.Password), null))
			{
				string lastFtpPath = null;
				IList<FtpEntry> remoteFiles = null;

					// Conecta al servidor
					ftpClient.Connect();
					// Sube los archivos de la colección
					foreach (string file in _files)
					{
						string ftpPath = GetRemotePath(file);

							// Lanza el evento de progreso
							_service.RaiseProgressEvent(_ftpConnection.Name, _taskId, $"Subiendo el archivo {System.IO.Path.GetFileName(file)}",
														_files.IndexOf(file) + 1, _files.Count);
							// Si estamos en otro directorio, obtenemos los archivos
							if (lastFtpPath == null || !ftpPath.Equals(lastFtpPath, StringComparison.CurrentCultureIgnoreCase))
							{ 
								// Crea el directorio remoto
								ftpClient.Commands.MakeDirRecursive(ftpPath);
								// Lista los archivos
								remoteFiles = ftpClient.Commands.List(ftpPath);
								// y guarda el último directorio
								lastFtpPath = ftpPath;
							}
							// Log
							_service.RaiseMessageEvent($"\t{_taskId} - Subiendo el archivo: {System.IO.Path.GetFileName(file)}", false);
							// Sube el archivo
							if (!MustUpload(file, remoteFiles, _mode))
								_service.RaiseMessageEvent($"\t\t{_taskId} - No es necesario subir el archivo", false);
							else
								try
								{ 
									// Borra el archivo en el servidor
									ftpClient.Commands.Delete(file);
									// y lo sube
									ftpClient.Commands.Upload(file, System.IO.Path.Combine(ftpPath, System.IO.Path.GetFileName(file)).Replace("\\", "/"));
								}
								catch (Exception exception)
								{
									_service.RaiseMessageEvent($"\t{_taskId} - Error al subir el archivo {exception.Message}", true);
								}
					}
			}
		}

		/// <summary>
		///		Obtiene el directorio remoto donde se debe dejar un archivo
		/// </summary>
		private string GetRemotePath(string file)
		{
			string path = System.IO.Path.GetDirectoryName(file);

				// Corta el directorio local
				if (path.Length > _pathLocal.Length)
					path = path.Substring(_pathLocal.Length + 1);
				else
					path = "";
				// Quita las barras iniciales
				while (path.StartsWith("\\"))
					path = path.Substring(1);
				// Combina los directorios
				return System.IO.Path.Combine(_pathRemote, path).Replace('\\', '/');
		}

		/// <summary>
		///		Convierte el protocolo
		/// </summary>
		private FtpClient.FtpProtocol ConvertProtocol(FtpConnectionModel.Protocol protocol)
		{
			switch (protocol)
			{
				case FtpConnectionModel.Protocol.Ftp:
					return FtpClient.FtpProtocol.Ftp;
				case FtpConnectionModel.Protocol.FtpEs:
					return FtpClient.FtpProtocol.FtpES;
				case FtpConnectionModel.Protocol.FtpS:
					return FtpClient.FtpProtocol.FtpS;
				case FtpConnectionModel.Protocol.SFtp:
					return FtpClient.FtpProtocol.Ftp;
				default:
					return FtpClient.FtpProtocol.Ftp;
			}
		}

		/// <summary>
		///		Comprueba si se debe subir un archivo
		/// </summary>
		private bool MustUpload(string file, IList<FtpEntry> remoteFiles, FtpUploadService.UploadMode mode)
		{
			System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);

				// Comprueba si ya existe un archivo remoto con el mismo tamaño
				if (mode == FtpUploadService.UploadMode.CheckSize)
					foreach (FtpEntry remoteFile in remoteFiles)
						if (fileInfo.Name.Equals(remoteFile.Name, StringComparison.CurrentCultureIgnoreCase) &&
								fileInfo.Length == remoteFile.Size)
							return false;
				// Si ha llegado hasta aquí es porque se debe subir el archivo
				return true;
		}
	}
}
