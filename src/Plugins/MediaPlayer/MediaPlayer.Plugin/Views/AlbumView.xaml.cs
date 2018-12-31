using System;
using System.Windows;

using Bau.Libraries.LibMediaPlayer.ViewModel.Blogs;

namespace Bau.Plugins.MediaPlayer.Views
{
	/// <summary>
	///		Formulario para mantenimiento de un <see cref="AlbumViewModel"/>
	/// </summary>
	public partial class BlogView : Window
	{
		public BlogView(AlbumViewModel viewModel)
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