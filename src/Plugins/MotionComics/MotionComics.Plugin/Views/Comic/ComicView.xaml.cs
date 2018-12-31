using System;
using System.Windows.Controls;

namespace Bau.Plugins.MotionComics.Views.Comic
{
	/// <summary>
	///		Formulario para visualizar un cómic
	/// </summary>
	public partial class ComicView : UserControl
	{   
		// Variables privadas
		private bool _isLoadedComic = false;

		public ComicView(string pathComic)
		{
			InitializeComponent();
			PathComic = pathComic;
		}

		/// <summary>
		///		Carga el cómic
		/// </summary>
		private void LoadComic()
		{
			if (!_isLoadedComic)
			{
				string error;
				string fileName = null;

					// Muestra / oculta los adorners
					udtComic.ShowAdorners = MotionComicsPlugin.MainInstance.ViewModelManager.ShowAdorners;
					// Carga el cómic
					foreach (string file in System.IO.Directory.GetFiles(PathComic, "*.cml"))
						fileName = file;
					// Carga el cómic
					if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
						udtComic.LoadComic(fileName, out error);
					else
						error = "No se encuentra ningún archivo de cómic en el directorio especificado";
					// Muestra el error
					if (!string.IsNullOrEmpty(error))
						MotionComicsPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowMessage($"Error en la carga del cómic.{Environment.NewLine}{error}");
					// Indica que se ha cargado el cómic
					_isLoadedComic = true;
			}
		}

		/// <summary>
		///		Directorio del cómic
		/// </summary>
		public string PathComic { get; }

		private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			LoadComic();
		}
	}
}
