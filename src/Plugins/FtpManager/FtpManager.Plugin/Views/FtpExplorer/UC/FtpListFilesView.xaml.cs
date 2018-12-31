using System;
using System.Windows;
using System.Windows.Controls;

using Bau.Libraries.FtpManager.ViewModel.FtpExplorer.FtpConnectionItems;

namespace Bau.Plugins.FtpManager.Views.FtpExplorer.UC
{
	/// <summary>
	///		Control de usuario para mostrar una lista de archivos de una conexión FTP
	/// </summary>
	public partial class FtpListFilesView : UserControl
	{ // Propiedades
			public static readonly DependencyProperty SourcePathProperty = 
								DependencyProperty.Register(nameof(SourcePath), typeof(string), typeof(FtpListFilesView),
																						new FrameworkPropertyMetadata("C:\\", 
																																					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public FtpListFilesView()
		{ InitializeComponent();
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		public void InitControl(FtpTreeExplorerViewModel objViewModel)
		{ ViewModelData = objViewModel;
			lswFiles.DataContext = objViewModel;
			lswFiles.ItemsSource = objViewModel.ListItems;
		}

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public FtpFileListViewModel ViewModelData { get; private set; }

		/// <summary>
		///		Directorio origen
		/// </summary>
		public string SourcePath
		{	get { return (string) GetValue(SourcePathProperty); }
			set 
				{ SetValue(SourcePathProperty, value); 
					ViewModelData.SourcePath = value;
				}
		}
	}
}
