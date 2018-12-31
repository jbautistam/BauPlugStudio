using System;
using System.Windows;

using Bau.Libraries.LibMediaPlayer.ViewModel.Blogs;

namespace Bau.Plugins.MediaPlayer.Views
{
	/// <summary>
	///		Formulario para mantenimiento de un <see cref="FolderModel"/>
	/// </summary>
	public partial class FolderView : Window
	{
		public FolderView(FolderViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			DataContext = viewModel;
			viewModel.Close += (sender, result) =>
											{
												DialogResult = result.IsAccepted;
												Close();
											};
		}
	}
}