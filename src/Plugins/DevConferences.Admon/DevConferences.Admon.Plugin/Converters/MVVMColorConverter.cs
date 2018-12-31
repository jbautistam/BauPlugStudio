using System;
using System.Windows.Data;
using System.Windows.Media;

using Bau.Libraries.BauMvvm.ViewModels.Media;

namespace Bau.Plugins.DevConferences.Admon.Converters
{
	/// <summary>
	///		Conversor para <see cref="MvvmColor"/>
	/// </summary>
	public class MVVMColorConverter : IValueConverter
	{
		/// <summary>
		///		Convierte un <see cref="MvvmColor"/> en un <see cref="System.Windows.Media.Color"/>
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{ 
			MvvmColor source = value as MvvmColor;

				// Convierte el color
				if (source == null)
					return new SolidColorBrush(Colors.Black);
				else
					return new SolidColorBrush(Color.FromArgb(source.A, source.R, source.G, source.B));
		}

		/// <summary>
		///		Convierte un <see cref="Color"/> en <see cref="MvvmColor"/>
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{	
			throw new NotImplementedException();
		}
	}
}
