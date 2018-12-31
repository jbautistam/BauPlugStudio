using System;
using System.Windows;

using Bau.Libraries.LibBlogReader.ViewModel.Blogs;

namespace Bau.Plugins.BlogReader.Views
{
	/// <summary>
	///		Formulario para mantenimiento de un <see cref="BlogModel"/>
	/// </summary>
	public partial class BlogView : Window
	{
		public BlogView(BlogViewModel viewModel)
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