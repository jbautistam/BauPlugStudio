using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.LibFtpClient;

namespace Bau.Libraries.FtpManager.ViewModel.FtpExplorer.FtpConnectionItems
{
	/// <summary>
	///		Nodo del árbol para un archivo o carpeta
	/// </summary>
	public class FtpFileNodeViewModel : ControlHierarchicalViewModel
	{
		public FtpFileNodeViewModel(FtpFileNodeViewModel parent, FtpTreeExplorerViewModel viewModel, FtpEntry file)
										: base(parent, $"{file.Path}/{file.Name}", file, true)
		{
			ViewModel = viewModel;
			File = file;
			PathName = file.Path.GetFileName();
			ImageSource = "/BauMVVMControls;component/Themes/Images/Folder.png";
		}

		/// <summary>
		///		Carga los hijos del nodo
		/// </summary>
		public override void LoadChildrenData()
		{
			try
			{
				foreach (FtpEntry ftpFile in ViewModel.GetFtpFiles(File))
					if (ftpFile.Name != "." && ftpFile.Name != ".." && ftpFile.Type == FtpEntry.FtpEntryType.Directory)
					{
						FtpFileNodeViewModel nodeChild = new FtpFileNodeViewModel(this, ViewModel, ftpFile);

							// Añade los archivos hijo
							foreach (FtpEntry ftpChild in ViewModel.GetFtpFiles(ftpFile))
								if (ftpChild.Type == FtpEntry.FtpEntryType.File)
									nodeChild.Files.Add(ftpChild);
							// Añade el nodo hijo
							Children.Add(nodeChild);
					}
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine("Excepción: " + exception.Message);
			}
		}

		/// <summary>
		///		ViewModel de la conexión
		/// </summary>
		public FtpTreeExplorerViewModel ViewModel { get; }

		/// <summary>
		///		Archivo del nodo
		/// </summary>
		public FtpEntry File { get; }

		/// <summary>
		///		Nombre del directorio
		/// </summary>
		public string PathName { get; }

		/// <summary>
		///		Archivos del directorio
		/// </summary>
		public System.Collections.Generic.List<FtpEntry> Files { get; } = new System.Collections.Generic.List<FtpEntry>();
	}
}
