using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Bau.Applications.BauPlugStudio.Views.Tools.Graphical
{
	/// <summary>
	///		Ventana para presentar una imagen
	/// </summary>
	public partial class ImageView : UserControl
	{
		public ImageView(string fileName)
		{
			// Inicializa los componentes
			InitializeComponent();
			// Carga el archivo
			LoadImage(fileName);
			// Cambia el zoom
			pnlZoom.ZoomMode = Controls.Graphical.ImageZoomBoxPanel.eZoomMode.ActualSize;
		}

		/// <summary>
		///		Carga una imagen
		/// </summary>
		private void LoadImage(string fileName)
		{
			BitmapImage image = new BitmapImage();

				// Lee el archivo sobre la imagen
				image.BeginInit();
				image.StreamSource = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				image.CacheOption = BitmapCacheOption.OnLoad;
				//bitmapImage.DecodePixelWidth = (int) _decodePixelWidth;
				//bitmapImage.DecodePixelHeight = (int) _decodePixelHeight;
				image.EndInit();
				// Libera el stream para evitar excepciones de acceso al archivo cuando se intenta borrar la imagen
				image.StreamSource.Dispose();
				// Asigna la imagen
				imgImage.Source = image;
				// Muestra las propiedades de la imagen
				lblStatus.Text = $"Dimensiones {image.PixelWidth} x {image.PixelHeight}";
		}
	}
}
