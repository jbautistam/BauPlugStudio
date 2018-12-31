using System;
using System.Windows.Data;

namespace Bau.Plugins.DevConferences.Admon.Converters
{
	/// <summary>
	///		Conversor para transformar un valor booleano en un enumerado "FontWeight" (para las negritas)
	/// </summary>
	public class BoolToFontWeightConverter : IValueConverter
	{
		/// <summary>
		///		Convierte un valor boolean en un valor de FontWeight
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{ 
			if ((value is bool) && (bool) value)
				return System.Windows.FontWeights.Bold;
			else
				return System.Windows.FontWeights.Normal;
		}

		/// <summary>
		///		Convierte un valor de vuelta
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{ 
			throw new NotImplementedException();
		}
	}
}
