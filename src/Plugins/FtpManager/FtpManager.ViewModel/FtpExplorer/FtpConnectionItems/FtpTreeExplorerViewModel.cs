using System;
using System.Collections.Generic;

using Bau.Libraries.LibFtpClient;
using Bau.Libraries.FtpManager.Model.Connections;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.Trees;

namespace Bau.Libraries.FtpManager.ViewModel.FtpExplorer.FtpConnectionItems
{
	/// <summary>
	///		ViewModel para el árbol de directorios y la lista de archivos de una conexión FTP
	/// </summary>
	public class FtpTreeExplorerViewModel : TreeViewModel<FtpFileNodeViewModel>
	{   
		// Variables privadas
		private FtpClient _ftpClient = null;
		private FtpEntry _ftpSelectedPath = null;
		private string _sourcePath;

		public FtpTreeExplorerViewModel(FtpConnectionModel ftpConnection)
		{
			FtpConnection = ftpConnection;
			SourcePath = "/";
			ChangeUpdated = false;
			// Inicializa los comandos
			DeleteCommand = new BaseCommand(parameter => Delete(), parameter => SelectedNode != null)
									.AddListener(this, nameof(SelectedNode));
			// Tratamiento del evento "SelectedNode"
			PropertyChanged += (sender, evntArgs) =>
										{
											if (evntArgs.PropertyName == nameof(SelectedNode))
												ChangeSelectedPath();
										};
		}

		/// <summary>
		///		Conecta al servidor FTP
		/// </summary>
		internal void Connect()
		{ 
			// Conecta al servidor
			_ftpClient.Connect();
			// Obtiene la lista
			SourcePath = _ftpClient.Commands.GetActualPath();
			SelectedPath = null;
		}

		/// <summary>
		///		Obtiene las entradas de un directorio Ftp
		/// </summary>
		internal List<FtpEntry> GetFtpFiles(FtpEntry ftpFile)
		{
			if (ftpFile.Type == FtpEntry.FtpEntryType.Directory)
				return GetFtpFiles(ftpFile.Path.Path);
			else
				return new List<FtpEntry>();
		}

		/// <summary>
		///		Obtiene las entradas de un directorio FTP
		/// </summary>
		internal List<FtpEntry> GetFtpFiles(string ftpPath)
		{
			List<FtpEntry> objColFtpFiles = new List<FtpEntry>();

				// Conecta al servidor si es necesario
				if (!FtpClient.IsConnected)
					Connect();
				// Obtiene los archivos
				if (FtpClient.IsConnected)
					objColFtpFiles.AddRange(FtpClient.Commands.List(ftpPath));
				// Devuelve la colección de archivos
				return objColFtpFiles;
		}

		/// <summary>
		///		Cambia el elemento seleccionado
		/// </summary>
		private void ChangeSelectedPath()
		{
			FtpFileNodeViewModel node = GetSelectedNode();

				// Obtiene el directorio y archivo
				if (node != null)
					SelectedPath = node.File;
				else
					SelectedPath = null;
		}

		/// <summary>
		///		Carga los nodos
		/// </summary>
		protected override void LoadNodesData()
		{
			FtpEntry ftpPathSource = new FtpEntry(new FtpPath("/"), SourcePath, 0, FtpEntry.FtpEntryType.Directory, DateTime.Now, null);
			FtpFileNodeViewModel node = new FtpFileNodeViewModel(null, this, ftpPathSource);

				// Añade los archivos
				foreach (FtpEntry ftpFile in GetFtpFiles(ftpPathSource))
					if (ftpFile.Name != "." && ftpFile.Name != ".." && ftpFile.Type == FtpEntry.FtpEntryType.File)
						node.Files.Add(ftpFile);
		}

		/// <summary>
		///		Carga la lista de archivos
		/// </summary>
		private void LoadFiles()
		{
			FtpFileNodeViewModel node = GetSelectedNode();

				// Limpia los elementos
				Files.Items.Clear();
				// Añade los archivos
				if (node != null)
					foreach (FtpEntry file in node.Files)
						if (file.Type == FtpEntry.FtpEntryType.File)
							Files.Items.Add(new FtpFileListItemViewModel(file));
		}

		/// <summary>
		///		Obtiene el nodo seleccionado en el árbol
		/// </summary>
		private FtpFileNodeViewModel GetSelectedNode()
		{
			if (SelectedNode != null && SelectedNode is FtpFileNodeViewModel node)
				return node;
			else
				return null;
		}

		/// <summary>
		///		Conexión
		/// </summary>
		public FtpConnectionModel FtpConnection { get; }

		/// <summary>
		///		Cliente de FTP
		/// </summary>
		private FtpClient FtpClient
		{
			get
			{ 
				// Crea el servidor FTP	
				if (_ftpClient == null)
				{
					FtpClient.FtpProtocol protocol = FtpClient.FtpProtocol.Ftp;

						// Asigna el protocolo
						switch (FtpConnection.FtpProtocol)
						{
							case FtpConnectionModel.Protocol.FtpS:
									protocol = FtpClient.FtpProtocol.FtpS;
								break;
							case FtpConnectionModel.Protocol.FtpEs:
									protocol = FtpClient.FtpProtocol.FtpES;
								break;
						}
						// Crea el cliente
						_ftpClient = new FtpClient(protocol, FtpConnection.Server, FtpConnection.Port,
												   new System.Net.NetworkCredential(FtpConnection.User, FtpConnection.Password), null);
				}
				// Devuelve el servidor FTP
				return _ftpClient;
			}
		}

		/// <summary>
		///		Lista de archivos
		/// </summary>
		public BauMvvm.ViewModels.Forms.ControlItems.ControlListViewModel Files { get; } = new BauMvvm.ViewModels.Forms.ControlItems.ControlListViewModel();

		/// <summary>
		///		Inicializa los comandos
		/// </summary>
		private void Delete()
		{ 
			FtpManagerViewModel.Instance.ControllerWindow.ShowMessage("Borrar el archivo / directorio");
		}

		/// <summary>
		///		Directorio base del árbol
		/// </summary>
		public string SourcePath
		{
			get { return _sourcePath; }
			set
			{
				if (CheckProperty(ref _sourcePath, value))
					LoadNodes();
			}
		}

		/// <summary>
		///		Directorio seleccionado
		/// </summary>
		public FtpEntry SelectedPath
		{
			get { return _ftpSelectedPath; }
			set
			{
				if (CheckObject(ref _ftpSelectedPath, value))
					LoadFiles();
			}
		}

		///// <summary>
		/////		Archivo seleccionado
		///// </summary>
		//public string SelectedFile
		//{ get { return strSelectedFile; }
		//	set { CheckProperty(ref strSelectedFile, value); }
		//}

		///// <summary>
		/////		Indica si se deben mostrar los archivos en el árbol
		///// </summary>
		//public bool ShowFiles 
		//{ get { return blnShowFiles; } 
		//	set 
		//		{ if (CheckProperty(ref blnShowFiles, value))
		//				Refresh();
		//		}
		//}

		/// <summary>
		///		Comando para copiar archivos
		/// </summary>
		public BaseCommand CopyCommand { get; }

		/// <summary>
		///		Comando para borrar archivos / directorios
		/// </summary>
		public BaseCommand DeleteCommand { get; }
	}
}
