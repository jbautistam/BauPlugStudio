using System;
using System.Windows.Data;

namespace Bau.Plugins.MediaPlayer.Views.Converters
{
	/// <summary>
	///		Conversor para un boolean en un valor de visibilidad
	/// </summary>
	public class BoolToVisibilityConverter : IValueConverter
	{
		/// <summary>
		///		Convierte un booleano a un valor de visibilidad
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is bool visible && !visible)
				return System.Windows.Visibility.Collapsed;
			else
				return System.Windows.Visibility.Visible;
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
