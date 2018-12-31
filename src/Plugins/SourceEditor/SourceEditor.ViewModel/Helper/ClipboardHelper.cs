using System;
using System.IO;

using Bau.Libraries.ImageFilters.Helper;

namespace Bau.Libraries.SourceEditor.ViewModel.Helper
{
	/// <summary>
	///		Helper para el tratamiento del portapapeles
	/// </summary>
	internal class ClipboardHelper
	{
		/// <summary>
		///		Pega la imagen
		/// </summary>
		internal bool PasteImage(string path, int maxImageWidth, int thumbWidth, out string error)
		{
			return PasteImage(path, maxImageWidth, thumbWidth, out string fileName, out error);
		}

		/// <summary>
		///		Pega la imagen
		/// </summary>
		internal bool PasteImage(string path, int maxImageWidth, int thumbWidth, out string fileName, out string error)
		{
			bool paste = false;

				// Inicializa los argumentos de salida
				fileName = null;
				error = null;
				// Pega la imagen
				if (!System.Windows.Clipboard.ContainsImage())
					error = "No hay ninguna imagen en el portapapeles";
				else
				{
					// Obtiene el nombre de archivo inicial
					fileName = LibCommonHelper.Files.HelperFiles.GetConsecutiveFileName(path, Path.GetFileName(path) + ".jpg");
					// Obtiene el nombre de archivo real a partir de un cuadro de diálogo
					fileName = SourceEditorViewModel.Instance.DialogsController.OpenDialogSave
																(Path.GetDirectoryName(fileName), "*.jpg|*.jpg",
																	Path.GetFileName(fileName), "*.jpg");
					// Graba la imagen
					if (!string.IsNullOrEmpty(fileName))
						try
						{
							string fileClipBoard = fileName + ".bak";

								// Graba la imagen del portapapeles a un archivo
								SaveClipboardImageToFile(fileClipBoard);
								// Redimensiona la imagen y la graba junto con el thumb
								ConvertImage(fileClipBoard, fileName, maxImageWidth, thumbWidth);
								// Borra la imagen
								LibCommonHelper.Files.HelperFiles.KillFile(fileClipBoard);
								// Indica que se ha grabado correctamente
								paste = true;
						}
						catch (Exception exception)
						{
							error = "Error al grabar la imagen." + Environment.NewLine + exception.Message;
						}
				}
				// Devuelve el valor que indica si se ha grabado correctamente
				return paste;
		}

		/// <summary>
		///		Graba una imagen del clipBoard en un archivo
		/// </summary>
		private void SaveClipboardImageToFile(string fileName)
		{
			System.Windows.Media.Imaging.BitmapSource image = System.Windows.Clipboard.GetImage();

				// Graba la imagen
				using (FileStream stmFile = new FileStream(fileName, FileMode.Create))
				{
					System.Windows.Media.Imaging.BitmapEncoder encoder = new System.Windows.Media.Imaging.JpegBitmapEncoder();

						// Codifica la imagen
						encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(image));
						// Graba la imagen
						encoder.Save(stmFile);
				}
		}

		/// <summary>
		///		Copia una serie de imágenes
		/// </summary>
		internal string[] CopyImages(string[] filesSource, string pathTarget, int maxImageWidth, int thumbWidth, out string error)
		{
			string[] filesTarget = new string[filesSource.Length];

				// Inicializa los argumentos de salida
				error = null;
				// Copia las imágenes
				for (int index = 0; index < filesSource.Length; index++)
				{
					// Obtiene el nombre de archivo destino
					filesTarget[index] = LibCommonHelper.Files.HelperFiles.GetConsecutiveFileName(pathTarget,
																							Path.GetFileName(pathTarget) + Path.GetExtension(filesSource[index]));
					// Graba la imagen
					try
					{
						ConvertImage(filesSource[index], filesTarget[index], maxImageWidth, thumbWidth);
					}
					catch (Exception exception)
					{
						error = "Error al grabar la imagen." + Environment.NewLine + exception.Message;
						filesTarget[index] = null;
					}
				}
				// Devuelve los archivos de destino
				return filesTarget;
		}

		/// <summary>
		///		Graba una imagen redimensionada y su thumbnail
		/// </summary>
		private void ConvertImage(string fileSource, string fileTarget, int maxImageWidth, int thumbWidth)
		{
			System.Drawing.Image imageResized = null;

				// Graba el thumb de la imagen y la redimensiona si es necesario
				using (System.Drawing.Image image = FiltersHelpers.Load(fileSource))
				{
					// Graba el thumb
					FiltersHelpers.SaveThumb(image, thumbWidth, fileTarget);
					// Redimensiona la imagen (si es necesario) y la graba
					if (image.Width > maxImageWidth)
						imageResized = FiltersHelpers.Resize(image, maxImageWidth);
				}
				// Graba la imagen redimensionada
				if (imageResized != null)
					FiltersHelpers.Save(imageResized, fileTarget);
				else
					LibCommonHelper.Files.HelperFiles.CopyFile(fileSource, fileTarget);
		}
	}
}