using System;
using System.Windows.Controls;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.FtpManager.ViewModel.FtpExplorer;

namespace Bau.Plugins.FtpManager.Views.FtpExplorer
{
	/// <summary>
	///		Ventana para mostrar un explroador de una conexión FTP
	/// </summary>
	public partial class FtpExplorerView : UserControl, IFormView
	{
		public FtpExplorerView(Libraries.FtpManager.Model.Connections.FtpConnectionModel ftpConnection)
		{   
			// Inicializa los componentes
			InitializeComponent();
			// Asigna la clase del ViewModel
			ViewModel = new FtpExplorerViewModel(ftpConnection);
			udtFtpConnection.ViewModelData = ViewModel.FtpTreeExplorerViewModel;
			FormView = new BaseFormView(ViewModel);
		}

		/// <summary>
		///		ViewModel asociado a la ventana
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel asociado a la ventana
		/// </summary>
		public FtpExplorerViewModel ViewModel { get; }
	}
}
