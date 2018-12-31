using System;
using System.Windows.Data;

using Bau.Libraries.LibMediaPlayer.Model;

namespace Bau.Plugins.MediaPlayer.Views.Converters
{
	/// <summary>
	///		Conversor para los iconos asociados a las entradas de un archivo (leído, no leído, interesante)
	/// </summary>
	public class MediaFileIconConverter : IValueConverter
	{
		/// <summary>
		///		Convierte un elemento <see cref="MediaFileModel"/> en el icono asociado
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string icon = null;

				// Convierte la entrada según el estado en el icono asociado
				if (value is MediaFileModel.StatusFile)
				{
					MediaFileModel.StatusFile status = (MediaFileModel.StatusFile) ((int) value);

						// Obtiene la dirección del icono
						switch (status)
						{
							case MediaFileModel.StatusFile.Interesting:
									icon = "/BauControls;component/Themes/Images/EntryInteresting.png";
								break;
							case MediaFileModel.StatusFile.NotPlayed:
									icon = "/BauControls;component/Themes/Images/EntryNotRead.png";
								break;
							case MediaFileModel.StatusFile.Played:
									icon = "/BauControls;component/Themes/Images/EntryRead.png";
								break;
						}
				}
				// Devuelve la dirección del icono
				return icon;
		}

		/// <summary>
		///		Convierte el valor devuelto (no hace nada, simplemente implementa la interface)
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value;
		}
	}
}
