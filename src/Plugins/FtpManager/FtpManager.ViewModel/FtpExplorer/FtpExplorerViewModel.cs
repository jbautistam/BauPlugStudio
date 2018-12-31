using System;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.FtpManager.Model.Connections;

namespace Bau.Libraries.FtpManager.ViewModel.FtpExplorer
{
	/// <summary>
	///		ViewModel para una ventana de explorador de un servidor FTP
	/// </summary>
	public class FtpExplorerViewModel : BaseFormViewModel
	{
		public FtpExplorerViewModel(FtpConnectionModel ftpConnection)
		{ 
			FtpTreeExplorerViewModel = new FtpConnectionItems.FtpTreeExplorerViewModel(ftpConnection);
		}

		/// <summary>
		///		Conecta al servidor FTP
		/// </summary>
		public void Connect()
		{
			FtpTreeExplorerViewModel.Connect();
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				default:
					return false;
			}
		}

		/// <summary>
		///		ViewModel con el árbol de directorios de la conexión
		/// </summary>
		public FtpConnectionItems.FtpTreeExplorerViewModel FtpTreeExplorerViewModel { get; }
	}
}
