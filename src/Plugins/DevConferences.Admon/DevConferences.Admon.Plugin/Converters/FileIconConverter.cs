using System;
using System.Windows.Data;

using Bau.Libraries.DevConferences.Admon.ViewModel.Controllers;

namespace Bau.Plugins.DevConferences.Admon.Converters
{
	/// <summary>
	///		Conversor para los iconos asociados a los archivos del árbol de secciones
	/// </summary>
	public class FileIconConverter : IValueConverter
	{
		/// <summary>
		///		Convierte un tipo de sección en un icono
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{ 
			if (value is GlobalEnums.NodeTypes)
				return GetIcon((GlobalEnums.NodeTypes) value);
			else
				return null;
		}

		/// <summary>
		///		Obtiene el icono asociado a un <see cref="GlobalEnums.NodeTypes"/>
		/// </summary>
		private string GetIcon(GlobalEnums.NodeTypes iconType)
		{	
			switch (iconType)
			{ 
				case GlobalEnums.NodeTypes.TrackManager:
					return "/DevConferences.Admon.Plugin;component/Resources/Images/Project.png";
				case GlobalEnums.NodeTypes.Track:
					return "/DevConferences.Admon.Plugin;component/Resources/Images/Reference.png";
				case GlobalEnums.NodeTypes.Category:
					return "/DevConferences.Admon.Plugin;component/Resources/Images/Folder.png";
				case GlobalEnums.NodeTypes.Entry:
					return "/DevConferences.Admon.Plugin;component/Resources/Images/contact.png";
				default:
					return null;
			}
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
