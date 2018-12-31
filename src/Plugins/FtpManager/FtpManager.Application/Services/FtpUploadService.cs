using System;
using System.Collections.Generic;

using Bau.Libraries.FtpManager.Model.Connections;

namespace Bau.Libraries.FtpManager.Application.Services
{
	/// <summary>
	///		Servicio para subir el contenido de un directorio
	/// </summary>
	public class FtpUploadService
	{ 
		// Eventos públicos
		public event EventHandler<EventArguments.UploadEventArgs> UploadMessage;
		public event EventHandler<EventArguments.ProgressEventArgs> ProgressMessage;
		// Enumerados
		/// <summary>
		///		Modo de subida
		/// </summary>
		public enum UploadMode
		{
			/// <summary>Sube todos los archivos</summary>
			All,
			/// <summary>Comprueba el tamaño antes de subir los archivos</summary>
			CheckSize
		}

		/// <summary>
		///		Sube los archivos de un directorio
		/// </summary>
		public void Upload(FtpConnectionModel ftpConnection, string pathLocal, string pathRemote, int processors, UploadMode mode)
		{
			List<string> files = LoadRecursive(pathLocal);

				// Normaliza el número de procesadores
				if (processors < 1)
					processors = 1;
				// Procesa los archivos
				if (files.Count > 0)
				{
					int filesPerProcessor = (int) Math.Ceiling(((double) files.Count) / ((double) processors));
					int start = 0, taskId = 0;

						// Ordena los archivos
						files.Sort();
						// Crea las tareas
						while (start < files.Count)
						{ 
							// Crea la tarea de subida
							CreateTaskUpload(++taskId, ftpConnection, pathLocal, pathRemote, Split(files, start, filesPerProcessor), mode);
							// Incrementa el inicio
							start += filesPerProcessor;
						}
				}
		}

		/// <summary>
		///		Crea una tarea de subida de archivos
		/// </summary>
		private void CreateTaskUpload(int taskId, FtpConnectionModel ftpConnection, string pathLocal, string pathRemote, List<string> files, UploadMode mode)
		{
			if (files.Count > 0)
			{
				FtpUploadTask ftpUpload = new FtpUploadTask(taskId, this, ftpConnection, pathLocal, files, pathRemote, mode);
				System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(() => ftpUpload.Upload());

					try
					{
						task.Start();
					}
					catch (Exception exception)
					{
						RaiseMessageEvent($"Error al generar el proceso de subida de archivos: {exception.Message}", true);
					}
			}
		}

		/// <summary>
		///		Parte una colección
		/// </summary>
		private List<string> Split(List<string> files, int start, int filesPerProcessor)
		{
			List<string> targets = new List<string>();

				// Añade los archivos
				for (int index = 0; index < filesPerProcessor && start + index < files.Count; index++)
					targets.Add(files[start + index]);
				// Devuelve la colección de archivos
				return targets;
		}

		/// <summary>
		///		Carga recursivamente una colección de archivos de un directorio
		/// </summary>
		private static List<string> LoadRecursive(string pathource)
		{
			List<string> targetFiles = new List<string>();

				// Si se trata de un archivo, no se busca por directorios
				if (System.IO.File.Exists(pathource))
					targetFiles.Add(pathource);
				else
				{
					string[] files;

						// Obtiene los archivos
						files = System.IO.Directory.GetFiles(pathource);
						foreach (string file in files)
							targetFiles.Add(file);
						// Obtiene los directorios
						files = System.IO.Directory.GetDirectories(pathource);
						foreach (string path in files)
							targetFiles.AddRange(LoadRecursive(System.IO.Path.Combine(pathource, System.IO.Path.GetFileName(path))));
				}
				// Devuelve la colección de archivos
				return targetFiles;
		}

		/// <summary>
		///		Lanza un evento con el mensaje
		/// </summary>
		internal void RaiseMessageEvent(string message, bool error)
		{
			UploadMessage?.Invoke(this, new EventArguments.UploadEventArgs(message, error));
		}

		/// <summary>
		///		Lanza un evento de progreso
		/// </summary>
		internal void RaiseProgressEvent(string strConnection, int taskId, string message, int progress, int maximum)
		{
			ProgressMessage?.Invoke(this, new EventArguments.ProgressEventArgs(strConnection, taskId, message, progress, maximum));
		}
	}
}
